using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Shooter : MonoBehaviour
{
    private const int PlayerDamage = 15;

    [SerializeField] private Button _shoot;
    [SerializeField] private Button _machineGun;
    [SerializeField] private Button _pistol;

    private Weapon _weapon;
    private Player _player;
    private Enemy _enemy;
    private PopUp _popUp;

    private int _machineGunPatrons;
    private int _pistolPatrons;

    private ConsumeOfConsumables _consumeOfConsumables;

    [Inject]
    private void Construct(PopUp popUp)
    {
        _popUp = popUp;

        _popUp.UseClicked += OnUseClicked;
    }

    public void Initialize(Player player, Enemy enemy)
    {
        _player = player;
        _enemy = enemy;
    }

    private void Awake()
    {
        UseMachineGun();
    }

    private void OnEnable()
    {
        _machineGun.onClick.AddListener(UseMachineGun);
        _pistol.onClick.AddListener(UsePistol);
        _shoot.onClick.AddListener(Shoot);
    }

    private void OnDisable()
    {
        _machineGun.onClick.RemoveListener(UseMachineGun);
        _pistol.onClick.RemoveListener(UsePistol);
        _shoot.onClick.RemoveListener(Shoot);

        _popUp.UseClicked -= OnUseClicked;
    }

    private void UseMachineGun()
    {
        if (_weapon != null)
            _weapon.Shooted -= SpendPistolAmmo;

        _weapon = new MachineGun(_machineGunPatrons);

        _weapon.Shooted += SpendMachineGunAmmo;
    }

    private void UsePistol()
    {
        if (_weapon != null)
            _weapon.Shooted -= SpendMachineGunAmmo;

        _weapon = new Pistol(_pistolPatrons);

        _weapon.Shooted += SpendPistolAmmo;
    }

    private void Shoot()
    {
        _weapon.Shoot();
    }

    private void SpendMachineGunAmmo(int patrons, int damage)
    {
        if (patrons <= 0)
            throw new ArgumentException(nameof(patrons));
        if (damage <= 0)
            throw new ArgumentException(nameof(damage));

        _machineGunPatrons -= patrons;

        if (_machineGunPatrons < 0)
            _machineGunPatrons = 0;

        _player.Reduce(PlayerDamage);
        _enemy.Reduce(damage);
    }

    private void SpendPistolAmmo(int patrons, int damage)
    {
        if (patrons <= 0)
            throw new ArgumentException(nameof(patrons));
        if (damage <= 0)
            throw new ArgumentException(nameof(damage));

        _pistolPatrons -= patrons;

        if (_pistolPatrons < 0)
            _pistolPatrons = 0;

        _player.Reduce(PlayerDamage);
        _enemy.Reduce(damage);
    }

    private void OnUseClicked(InventoryItemView view, Cell cell)
    {
        _consumeOfConsumables = new ConsumeOfConsumables(this, view, cell);

        _consumeOfConsumables.Visit(view.Item);
    }

    private class ConsumeOfConsumables: IInventoryItemVisitor
    {
        private Shooter _shooter;
        private InventoryItemView _view;
        private Cell _cell;

        public ConsumeOfConsumables(Shooter shooter, InventoryItemView view, Cell cell)
        {
            _shooter = shooter;
            _view = view;
            _cell = cell;
        }

        public void Visit(InventoryItem inventoryItem) => Visit((dynamic)inventoryItem);

        public void Visit(ConsumableSkinItem consumableSkinItem)
        {
            if (consumableSkinItem.SkinType == ConsumableTypes.FirstAidKit)
            {
                _shooter._player.Add(consumableSkinItem.HealAmount);
                _view.SpendQuantity();
                if (_view == null)
                    _cell.Clear();
            }
            else if (consumableSkinItem.SkinType == ConsumableTypes.Patrons5_45x39)
            {
                _shooter._machineGunPatrons = _view.Count;
                _view.Delete();
                _cell.Clear();
            }
            else
            {
                _shooter._pistolPatrons = _view.Count;
                _view.Delete();
                _cell.Clear();
            }
        }

        public void Visit(ClothesHeadSkinItem clothesHeadSkinItem)
        {
        }

        public void Visit(ClothesTorsSkinItem clothesTorsSkinItem)
        {
        }
    }
}
