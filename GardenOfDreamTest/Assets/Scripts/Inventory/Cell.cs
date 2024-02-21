using UnityEngine;

public class Cell : MonoBehaviour
{
    private InventoryItemView _currentItem;
    private bool _isOccupied;

    public bool IsOccupied => _isOccupied;
    public InventoryItemView CurrentItem => _currentItem;

    private void Awake()
    {
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
}
