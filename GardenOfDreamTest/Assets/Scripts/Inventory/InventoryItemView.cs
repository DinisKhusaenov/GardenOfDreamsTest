using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemView : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public event Action<InventoryItemView> Click;
    public event Action<InventoryItemView> Destroyed;
    public event Action<InventoryItemView> DragBeginned;
    public event Action<InventoryItemView> DragEnded;

    [SerializeField] private Image _contentImage;
    [SerializeField] private IntValueView _count;

    private Transform _draggingParent;

    public InventoryItem Item { get; private set; }

    public void Initialize(InventoryItem inventoryItem, Transform draggingParent)
    {
        Item = inventoryItem;
        _draggingParent = draggingParent;

        _contentImage.sprite = Item.Image;
    }

    public void SetCount(int value)
    {
        if (value <= 0)
            throw new ArgumentException(nameof(value));

        if (value > 1)
            _count.Show(value);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(_draggingParent);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        DragBeginned?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragEnded?.Invoke(this);
    }
}
