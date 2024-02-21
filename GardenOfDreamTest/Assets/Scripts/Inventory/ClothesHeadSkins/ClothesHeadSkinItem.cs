using UnityEngine;

[CreateAssetMenu(fileName = "ClothesHeadSkinItem", menuName = "Inventory/ClothesHeadSkinItem")]
public class ClothesHeadSkinItem : InventoryItem
{
    [field: SerializeField] public ClothesHeadTypes SkinType { get; private set; }
    [field: SerializeField, Range(0, 50)] public int HeadProtection { get; private set; }
}
