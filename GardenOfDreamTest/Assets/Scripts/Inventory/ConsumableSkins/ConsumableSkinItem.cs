using UnityEngine;

[CreateAssetMenu(fileName = "ConsumableSkinItem", menuName = "Inventory/ConsumableSkinItem")]
public class ConsumableSkinItem : InventoryItem
{
    [field: SerializeField] public ConsumableTypes SkinType { get; private set; }
    [field: SerializeField] public int HealAmount { get; private set; }
}
