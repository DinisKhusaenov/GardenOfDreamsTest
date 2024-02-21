using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InitialInventoryData", menuName = "Inventory/InitialInventoryData")]
public class InitialInventoryData : ScriptableObject
{
    [SerializeField] private List<ClothesHeadSkinItem> _clothesHeadSkinItems;
    [SerializeField] private List<ClothesTorsSkinItem> _clothesTorsSkinItems;
    [SerializeField] private List<ConsumableSkinItem> _consumableSkinItems;
    [SerializeField, Range(0, 10)] private List<int> _clothesHeadSkinItemsCount;
    [SerializeField, Range(0, 10)] private List<int> _clothesTorsSkinItemsCount;
    [SerializeField, Range(0, 150)] private List<int> _consumableSkinItemsCount;

    public IEnumerable<ClothesHeadSkinItem> ClosthesHeadSkinItems => _clothesHeadSkinItems;
    public IEnumerable<int> ClosthesHeadSkinItemsCount => _clothesHeadSkinItemsCount;

    public IEnumerable<ClothesTorsSkinItem> ClothesTorsSkinItems => _clothesTorsSkinItems;
    public IEnumerable<int> ClothesTorsSkinItemsCount => _clothesTorsSkinItemsCount;

    public IEnumerable<ConsumableSkinItem> ConsumableSkinItems => _consumableSkinItems;
    public IEnumerable<int> ConsumableSkinItemsCount => _consumableSkinItemsCount;
}
