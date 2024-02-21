using System;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItemViewFactory", menuName = "Inventory/InventoryItemViewFactory")]
public class InventoryItemViewFactory : ScriptableObject
{
    [SerializeField] private InventoryItemView _clothesHeadSkinItemPrefab;
    [SerializeField] private InventoryItemView _consumableSkinItemPrefab;
    [SerializeField] private InventoryItemView _colthesTorsSkinItemPrefab;

    public InventoryItemView Get(InventoryItem inventoryItem, Transform parent, Transform draggingParent)
    {
        InventoryItemView instance;

        switch (inventoryItem)
        {
            case ClothesHeadSkinItem clothesHeadSkinItem:
                instance = Instantiate(_clothesHeadSkinItemPrefab, parent);
                break;

            case ConsumableSkinItem consumableSkinItem:
                instance = Instantiate(_consumableSkinItemPrefab, parent);
                break;

            case ClothesTorsSkinItem clothesTorsSkinItem:
                instance = Instantiate(_colthesTorsSkinItemPrefab, parent);
                break;

            default:
                throw new ArgumentException(nameof(inventoryItem));
        }

        instance.Initialize(inventoryItem, draggingParent);
        return instance;
    }
}
