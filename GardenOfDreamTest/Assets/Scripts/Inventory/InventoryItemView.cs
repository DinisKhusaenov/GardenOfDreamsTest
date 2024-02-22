using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemView : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public const int SpendCount = 1;

    public event Action<InventoryItemView> Deleted;
    public event Action<InventoryItemView> DragBeginned;
    public event Action<InventoryItemView> DragEnded;

    [SerializeField] private Image _contentImage;
    [SerializeField] private IntValueView _count;

    private Transform _draggingParent;
    private int _itemCount;

    public InventoryItem Item { get; private set; }

    public int Count => _itemCount;


    public bool IsCanDragging;

    public void Initialize(InventoryItem inventoryItem, Transform draggingParent)
    {
        Item = inventoryItem;
        _draggingParent = draggingParent;

        _contentImage.sprite = Item.Image;
        IsCanDragging = true;
    }

    public void SetCount(int value)
    {
        if (value <= 0)
            Delete();

        _itemCount = value;

        if (value > 1)
            _count.Show(value);
    }

    public void Delete()
    {
        Deleted?.Invoke(this);
        Destroy(gameObject);
    }

    public void SpendQuantity()
    {
        _itemCount -= SpendCount;
        SetCount(_itemCount);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (IsCanDragging)
            transform.SetParent(_draggingParent);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (IsCanDragging)
        {
            transform.position = Input.mousePosition;
            DragBeginned?.Invoke(this);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (IsCanDragging)
            DragEnded?.Invoke(this);
    }
}
