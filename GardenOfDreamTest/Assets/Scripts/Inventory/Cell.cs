using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class Cell : MonoBehaviour, IPointerClickHandler
{
    public event Action<InventoryItemView, Cell> Click;

    private InventoryItemView _currentItem;
    private bool _isOccupied;
    private PopUp _popUp;

    public bool IsOccupied => _isOccupied;

    public bool IsCanClick { get; set; }

    public InventoryItemView CurrentItem => _currentItem;

    [Inject]
    private void Construct(PopUp popUp)
    {
        _popUp = popUp;
        IsCanClick = true;
        _isOccupied = false;
    }

    public bool TryAddToCell(InventoryItemView item)
    {
        if (_isOccupied) return false;

        item.transform.SetParent(transform);
        _currentItem = item;
        _isOccupied = true;

        return true;
    }

    public void Clear()
    {
        _currentItem = null;
        _isOccupied = false;
    }

    public void ChangeCurrentItem(InventoryItemView view, Cell cell)
    {
        cell.TryAddToCell(_currentItem);
        Clear();
        TryAddToCell(view);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_isOccupied && IsCanClick)
        {
            _popUp.Show();
            Click?.Invoke(_currentItem, this);
        }
    }
}
