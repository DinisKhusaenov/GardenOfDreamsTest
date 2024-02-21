using UnityEngine;

public abstract class InventoryItem : ScriptableObject
{
    [field: SerializeField] public Sprite Image { get; private set; }
    [field: SerializeField, Range(0, 150)] public int MaxCount { get; private set; }
    [field: SerializeField, Range(0, 10)] public float Weight { get; private set; }
}
