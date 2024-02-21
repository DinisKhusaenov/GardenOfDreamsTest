using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PopUp : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private Image _item;
    [SerializeField] private TMP_Text _weight;
    [SerializeField] private TMP_Text _protect;
    [SerializeField] private TMP_Text _heal;
    [SerializeField] private Button _delete;
    [SerializeField] private Button _use;

    private List<Cell> _cells;
    private PopUpInitializer _popUpInitializer;

    [Inject]
    private void Construct(List<Cell> cells)
    {
        _cells = cells;
    }

    public void Initialize(string heal, string protect, string useButtonText)
    {
        if (heal != "") 
        {
            _heal.gameObject.SetActive(true);
            _heal.text = "+ " + heal + " h";
        }
        else if (protect != "")
        {
            _protect.gameObject.SetActive(true);
            _protect.text = "+ " + protect + " pr";
        }

        _use.GetComponentInChildren<TMP_Text>().text = useButtonText;  
    }

    private void Awake()
    {
        Hide();
    }

    private void OnEnable()
    {
        foreach (var cell in _cells)
            cell.Click += OnCellClicked;
    }

    private void OnDisable()
    {
        foreach (var cell in _cells)
            cell.Click -= OnCellClicked;
    }

    public void Show() => gameObject.SetActive(true);

    private void Hide()
    {
        _heal.gameObject.SetActive(false);
        _protect.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void OnCellClicked(InventoryItemView view)
    {
        _name.text = view.Item.Name;
        _item.sprite = view.Item.Image;
        _weight.text = view.Item.Weight + " кг";

        _popUpInitializer = new PopUpInitializer(view.Item, this);
        _popUpInitializer.Visit(view.Item);
    }

    private class PopUpInitializer : IInventoryItemVisitor
    {
        private const string Buy = " упить";
        private const string Heal = "Ћечить";
        private const string Equip = "Ёкипировать";

        private InventoryItem _inventoryItem;
        private PopUp _popUp;

        public PopUpInitializer(InventoryItem inventoryItem, PopUp popUp)
        {
            _inventoryItem = inventoryItem;
            _popUp = popUp;
        }

        public void Visit(InventoryItem inventoryItem) => Visit((dynamic)_inventoryItem);

        public void Visit(ClothesHeadSkinItem clothesHeadSkinItem)
            => _popUp.Initialize("", clothesHeadSkinItem.HeadProtection.ToString(), Equip);

        public void Visit(ConsumableSkinItem consumableSkinItem)
        {
            if (consumableSkinItem.SkinType == ConsumableTypes.FirstAidKit)
            {
                _popUp.Initialize(consumableSkinItem.HealAmount.ToString(), "", Heal);
            }
            else
            {
                _popUp.Initialize("", "", Buy);
            }
        }

        public void Visit(ClothesTorsSkinItem clothesTorsSkinItem)
            => _popUp.Initialize("", clothesTorsSkinItem.TorsoProtection.ToString(), Equip);
    }
}
