using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PopUp : MonoBehaviour
{
    public event Action<InventoryItemView, Cell> UseClicked;

    [SerializeField] private TMP_Text _name;
    [SerializeField] private Image _item;
    [SerializeField] private TMP_Text _weight;
    [SerializeField] private TMP_Text _protect;
    [SerializeField] private TMP_Text _heal;
    [SerializeField] private Button _delete;
    [SerializeField] private Button _use;
    [SerializeField] private Button _anticlicker;

    private List<Cell> _cells;
    private PopUpInitializer _popUpInitializer;

    private InventoryItemView _currentItem;
    private Cell _currentCell;

    [Inject]
    private void Construct(List<Cell> cells)
    {
        _cells = cells;
        _popUpInitializer = new PopUpInitializer(this);
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

        _use.onClick.AddListener(OnUseClicked);
        _delete.onClick.AddListener(OnDeleteClicked);
        _anticlicker.onClick.AddListener(Hide);
    }

    private void OnDisable()
    {
        foreach (var cell in _cells)
            cell.Click -= OnCellClicked;

        _use.onClick.RemoveListener(OnUseClicked);
        _delete.onClick.RemoveListener(OnDeleteClicked);
        _anticlicker.onClick.RemoveListener(Hide);
    }

    public void Show() => gameObject.SetActive(true);

    private void Hide()
    {
        _heal.gameObject.SetActive(false);
        _protect.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void OnUseClicked()
    {
        UseClicked?.Invoke(_currentItem, _currentCell);
        Hide();
    }

    private void OnDeleteClicked()
    {
        _currentItem.Delete();
        _currentCell.Clear();
        Hide();
    }

    private void OnCellClicked(InventoryItemView view, Cell cell)
    {
        _name.text = view.Item.Name;
        _item.sprite = view.Item.Image;
        _weight.text = view.Item.Weight + " кг";

        _popUpInitializer.Visit(view.Item);

        _currentItem = view;
        _currentCell = cell;
    }

    private class PopUpInitializer : IInventoryItemVisitor
    {
        private const string Buy = " упить";
        private const string Heal = "Ћечить";
        private const string Equip = "Ёкипировать";

        private PopUp _popUp;

        public PopUpInitializer(PopUp popUp)
        {
            _popUp = popUp;
        }

        public void Visit(InventoryItem inventoryItem) => Visit((dynamic)inventoryItem);

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
