using System;
using TMPro;
using UnityEngine;
using Zenject;

public class PlayerCells : MonoBehaviour
{
    [SerializeField] private Cell _headCell;
    [SerializeField] private Cell _torsCell;
    [SerializeField] private TMP_Text _headProtect;
    [SerializeField] private TMP_Text _torsProtect;

    private PopUp _popUp;
    private PlayerCellInitializer _playerCellInitializer;

    public Cell HeadCell => _headCell;
    public Cell TorsCell => _torsCell;

    public int HeadProtect => int.Parse(_headProtect.text);
    public int TorsProtect => int.Parse(_torsProtect.text);

    [Inject]
    private void Construct(PopUp popUp)
    {
        _popUp = popUp;
        _headCell.IsCanClick = false;
        _torsCell.IsCanClick = false;
    }

    private void OnEnable()
    {
        _popUp.UseClicked += AddItemToCell;
    }

    private void OnDisable()
    {
        _popUp.UseClicked -= AddItemToCell;
    }

    public void ChangeHeadText(int headProtect)
    {
        if (headProtect < 0)
            throw new ArgumentException(nameof(headProtect));

        _headProtect.text = headProtect.ToString();
    }

    public void ChangeTorsText(int torsProtect)
    {
        if (torsProtect < 0)
            throw new ArgumentException(nameof(torsProtect));

        _torsProtect.text = torsProtect.ToString();
    }

    private void AddItemToCell(InventoryItemView view, Cell cell)
    {
        _playerCellInitializer = new PlayerCellInitializer(this, view, cell);
        _playerCellInitializer.Visit(view.Item);
    }

    private class PlayerCellInitializer : IInventoryItemVisitor
    {
        private PlayerCells _playerCells;
        private InventoryItemView _view;
        private Cell _cell;

        public PlayerCellInitializer(PlayerCells playerCells, InventoryItemView view, Cell cell)
        {
            _playerCells = playerCells;
            _view = view;
            _cell = cell;
        }

        public Cell HeadCell => _playerCells.HeadCell;
        public Cell TorsCell => _playerCells.TorsCell;

        public void Visit(InventoryItem inventoryItem) => Visit((dynamic)inventoryItem);

        public void Visit(ClothesHeadSkinItem clothesHeadSkinItem)
        {
            if (HeadCell.IsOccupied)
            {
                _cell.Clear();
                _cell.TryAddToCell(HeadCell.CurrentItem);
                HeadCell.CurrentItem.IsCanDragging = true;
                HeadCell.Clear();
                HeadCell.TryAddToCell(_view);
            }
            else
            {
                _cell.Clear();
                HeadCell.TryAddToCell(_view);
            }

            _view.IsCanDragging = false;
            _playerCells.ChangeHeadText(clothesHeadSkinItem.HeadProtection);
        }

        public void Visit(ClothesTorsSkinItem clothesTorsSkinItem)
        {
            if (TorsCell.IsOccupied)
            {
                _cell.Clear();
                _cell.TryAddToCell(TorsCell.CurrentItem);
                TorsCell.CurrentItem.IsCanDragging = true;
                TorsCell.Clear();
                TorsCell.TryAddToCell(_view);
            }
            else
            {
                _cell.Clear();
                TorsCell.TryAddToCell(_view);
            }

            _view.IsCanDragging = false;
            _playerCells.ChangeTorsText(clothesTorsSkinItem.TorsoProtection);
        }

        public void Visit(ConsumableSkinItem consumableSkinItem)
        {
        }
    }
}
