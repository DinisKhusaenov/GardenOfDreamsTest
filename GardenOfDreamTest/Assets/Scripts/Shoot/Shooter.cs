using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Shooter : MonoBehaviour
{
    private const int PlayerDamage = 15;

    [SerializeField] private Button _shoot;
    [SerializeField] private Button _machineGun;
    [SerializeField] private Button _pistol;
    [SerializeField] private Color _selectedColor;

    private Weapon _weapon;
    private Player _player;
    private Enemy _enemy;
    private PopUp _popUp;
    private IPersistentData _persistentData;

    private int _machineGunPatrons;
    private int _pistolPatrons;

    private ConsumeOfConsumables _consumeOfConsumables;

    [Inject]
    private void Construct(PopUp popUp, IPersistentData persistentData)
    {
        _popUp = popUp;
        _persistentData = persistentData;

        _popUp.UseClicked += OnUseClicked;

        UseMachineGun();
        ChangePatronsCountText();
    }

    public void Initialize(Player player, Enemy enemy)
    {
        _player = player;
        _enemy = enemy;
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

        _machineGun.image.color = _selectedColor;
        _pistol.image.color = Color.white;

        _weapon.Shooted += SpendMachineGunAmmo;
    }

    private void UsePistol()
    {
        if (_weapon != null)
            _weapon.Shooted -= SpendMachineGunAmmo;

        _weapon = new Pistol(_pistolPatrons);

        _pistol.image.color = _selectedColor;
        _machineGun.image.color = Color.white;

        _weapon.Shooted += SpendPistolAmmo;
    }

    private void ChangePatronsCountText()
    {
        _machineGun.GetComponentInChildren<TMP_Text>().text = _machineGunPatrons.ToString();
        _pistol.GetComponentInChildren<TMP_Text>().text = _pistolPatrons.ToString();
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

        ChangePatronsCountText();

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

        ChangePatronsCountText();

        _player.Reduce(PlayerDamage);
        _enemy.Reduce(damage);
    }

    private void OnUseClicked(InventoryItemView view, Cell cell)
    {
        _consumeOfConsumables = new ConsumeOfConsumables(this, view, cell, _persistentData);

        _consumeOfConsumables.Visit(view.Item);
    }

    private class ConsumeOfConsumables: IInventoryItemVisitor
    {
        private Shooter _shooter;
        private InventoryItemView _view;
        private Cell _cell;
        private IPersistentData _persistentData;

        public ConsumeOfConsumables(Shooter shooter, InventoryItemView view, Cell cell, IPersistentData persistentData)
        {
            _shooter = shooter;
            _view = view;
            _cell = cell;
            _persistentData = persistentData;
        }

        public void Visit(InventoryItem inventoryItem) => Visit((dynamic)inventoryItem);

        public void Visit(ConsumableSkinItem consumableSkinItem)
        {
            if (consumableSkinItem.SkinType == ConsumableTypes.FirstAidKit)
            {
                _shooter._player.Add(consumableSkinItem.HealAmount);
                _view.SpendQuantity();
                _persistentData.PlayerData.RemoveConsumableSkin(consumableSkinItem.SkinType, 1);

                if (_view == null)
                    _cell.Clear();
            }
            else if (consumableSkinItem.SkinType == ConsumableTypes.Patrons5_45x39)
            {
                _shooter._machineGunPatrons = _view.Count;
                _view.Delete();
                _cell.Clear();
                _shooter.ChangePatronsCountText();
            }
            else
            {
                _shooter._pistolPatrons = _view.Count;
                _view.Delete();
                _cell.Clear();
                _shooter.ChangePatronsCountText();
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
