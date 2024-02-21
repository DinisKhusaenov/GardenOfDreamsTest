using System;
using System.Collections.Generic;
using UnityEngine;

public class SkinsPanel : MonoBehaviour
{
    public event Action<InventoryItemView> ItemViewClicked;
    public event Action<InventoryItemView> ItemViewDestroyed;

    private List<InventoryItemView> _inventoryItems = new List<InventoryItemView>();

    [SerializeField] private Transform _draggingParent;
    [SerializeField] private InventoryItemViewFactory _inventoryItemViewFactory;
    [SerializeField] private List<Cell> _cells;

    private Cell _currentCell;

    public void Show(IEnumerable<InventoryItem> items, List<int> itemCount)
    {
        int i = 0;

        foreach (var item in items)
        {
            foreach (var cell in _cells)
            {
                if (!cell.IsOccupied)
                {
                    InventoryItemView spawnedItem = _inventoryItemViewFactory.Get(item, cell.transform, _draggingParent);
                    cell.TryAddToCell(spawnedItem);

                    spawnedItem.SetCount(itemCount[i]);

                    spawnedItem.Click += OnItemViewClick;
                    spawnedItem.Destroyed += OnItemViewDestroyed;
                    spawnedItem.DragBeginned += OnDragBeginned;
                    spawnedItem.DragEnded += OnDragEnded;

                    _inventoryItems.Add(spawnedItem);
                    break;
                }
            }
            i++;
        }
    }

    private void OnItemViewClick(InventoryItemView inventoryItemView)
    {
        ItemViewClicked?.Invoke(inventoryItemView);
    }

    private void OnItemViewDestroyed(InventoryItemView inventoryItemView)
    {
        _inventoryItems.Remove(inventoryItemView);
        ItemViewDestroyed?.Invoke(inventoryItemView);
    }

    private void OnDragBeginned(InventoryItemView view)
    {
        foreach (var cell in _cells)
        {
            if (cell.CurrentItem == view)
            {
                cell.Clear();
                _currentCell = cell;
            }
        }
    }

    private void OnDragEnded(InventoryItemView view)
    {
        float minEpmtyDistance = int.MaxValue;
        float minOccupiedDistance = int.MaxValue;

        Cell closestEmptyCell = null;
        Cell closestOccupiedCell = null;

        foreach (var cell in _cells)
        {
            var distance = Vector3.Distance(view.transform.position, cell.transform.position);
            if (distance < minEpmtyDistance && !cell.IsOccupied)
            {
                minEpmtyDistance = distance;
                closestEmptyCell = cell;
            }
            if (distance < minEpmtyDistance && cell.IsOccupied)
            {
                minOccupiedDistance = distance;
                closestOccupiedCell = cell;
            }
        }

        if (minEpmtyDistance < minOccupiedDistance)
            closestEmptyCell.TryAddToCell(view);
        else
            closestOccupiedCell.ChangeCurrentItem(view, _currentCell);
    }
}
