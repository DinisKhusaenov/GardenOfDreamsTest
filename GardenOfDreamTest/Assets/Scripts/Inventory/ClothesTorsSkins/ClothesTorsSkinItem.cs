using UnityEngine;

[CreateAssetMenu(fileName = "ClothesTorsSkinItem", menuName = "Inventory/ClothesTorsSkinItem")]
public class ClothesTorsSkinItem : InventoryItem
{
    [field: SerializeField] public ClothesTorsType SkinType { get; private set; }
    [field: SerializeField, Range(0, 50)] public int TorsoProtection { get; private set; }
}
