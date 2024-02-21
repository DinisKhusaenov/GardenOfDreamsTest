using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryContent", menuName = "Inventory/InventoryContent")]
public class InventoryContent : ScriptableObject
{
    [SerializeField] private List<ClothesTorsSkinItem> _clothesTorsSkinItems;
    [SerializeField] private List<ClothesHeadSkinItem> _clothesHeadSkinItems;
    [SerializeField] private List<ConsumableSkinItem> _consumableSkinItems;

    public IEnumerable<ClothesTorsSkinItem> ClothesTorsSkinItems => _clothesTorsSkinItems;
    public IEnumerable<ClothesHeadSkinItem> ClothesHeadSkinItems => _clothesHeadSkinItems;
    public IEnumerable<ConsumableSkinItem> ConsumableSkinItems => _consumableSkinItems;

    private void OnValidate()
    {
        var clothesTorsSkinDublicates = _clothesTorsSkinItems.GroupBy(item => item.SkinType)
            .Where(array => array.Count() > 1);

        if (clothesTorsSkinDublicates.Count() > 0)
            throw new InvalidOperationException(nameof(_clothesTorsSkinItems));

        var clothesHeadSkinDublicates = _clothesHeadSkinItems.GroupBy(item => item.SkinType)
            .Where(array => array.Count() > 1);

        if (clothesHeadSkinDublicates.Count() > 0)
            throw new InvalidOperationException(nameof(_clothesHeadSkinItems));

        var consumableSkinDublicates = _consumableSkinItems.GroupBy(item => item.SkinType)
            .Where(array => array.Count() > 1);

        if (consumableSkinDublicates.Count() > 0)
            throw new InvalidOperationException(nameof(_consumableSkinItems));
    }
}
