using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class SkinsPanel : MonoBehaviour
{
    public event Action<InventoryItemView> ItemViewDestroyed;

    [SerializeField] private Transform _draggingParent;

    private InventoryItemViewFactory _inventoryItemViewFactory;
    private List<Cell> _allCells;
    private List<Cell> _emptyCells;

    private Cell _currentCell;

    [Inject]
    private void Construct(InventoryItemViewFactory factory, List<Cell> cells)
    {
        _inventoryItemViewFactory = factory;
        _allCells = cells;
        _emptyCells = new List<Cell>(_allCells);
    }

    public void Show(IEnumerable<InventoryItem> items, List<int> itemCount)
    {
        int i = 0;

        foreach (var item in items)
        {
            var cell = GetEmptyCell();
            InventoryItemView spawnedItem = null;

            if (cell == null) break;

            int counter = 0;

            for (int k = item.MaxCount; k < itemCount[i]; k += item.MaxCount)
            {
                spawnedItem = _inventoryItemViewFactory.Get(item, cell.transform, _draggingParent);
                _emptyCells.Remove(cell);
                cell.TryAddToCell(spawnedItem);

                spawnedItem.SetCount(item.MaxCount);
                SubscribeToTheEvent(spawnedItem);
                counter++;

                cell = GetEmptyCell();
                if (cell == null) break;
            }

            if (counter * item.MaxCount < itemCount[i])
            {
                cell = GetEmptyCell();
                if (cell == null) break;

                spawnedItem = _inventoryItemViewFactory.Get(item, cell.transform, _draggingParent);
                _emptyCells.Remove(cell);
                cell.TryAddToCell(spawnedItem);
                SubscribeToTheEvent(spawnedItem);

                spawnedItem.SetCount(itemCount[i] - (counter * item.MaxCount));
            }
            i++;
        }
    }

    private Cell GetEmptyCell()
    {
        return _emptyCells.FirstOrDefault<Cell>();
    }

    private void SubscribeToTheEvent(InventoryItemView spawnedItem)
    {
        spawnedItem.Deleted += OnItemViewDestroyed;
        spawnedItem.DragBeginned += OnDragBeginned;
        spawnedItem.DragEnded += OnDragEnded;
    }

    private void OnItemViewDestroyed(InventoryItemView inventoryItemView)
    {
        ItemViewDestroyed?.Invoke(inventoryItemView);
    }

    private void OnDragBeginned(InventoryItemView view)
    {
        foreach (var cell in _allCells)
        {
            if (cell.CurrentItem == view)
            {
                cell.Clear();
                _currentCell = cell;
                _emptyCells.Add(_currentCell);
            }
        }
    }

    private void OnDragEnded(InventoryItemView view)
    {
        float minEpmtyDistance = int.MaxValue;
        float minOccupiedDistance = int.MaxValue;

        Cell closestEmptyCell = null;
        Cell closestOccupiedCell = null;

        foreach (var cell in _allCells)
        {
            var distance = Vector3.Distance(view.transform.position, cell.transform.position);
            if (distance < minEpmtyDistance && !cell.IsOccupied)
            {
                minEpmtyDistance = distance;
                closestEmptyCell = cell;
            }
            if (distance < minOccupiedDistance && cell.IsOccupied)
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
