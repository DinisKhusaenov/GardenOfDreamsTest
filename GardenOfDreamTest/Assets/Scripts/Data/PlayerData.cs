using Newtonsoft.Json;
using System.Collections.Generic;

public class PlayerData
{
    private Dictionary<ClothesHeadTypes, int> _inventoryClothesHeadSkins;
    private Dictionary<ConsumableTypes, int> _inventoryConsumableSkins;
    private Dictionary<ClothesTorsType, int> _inventoryClothesTorsSkins;

    public bool IsStartDataLoaded { get; set; }

    public PlayerData()
    {
        _inventoryClothesHeadSkins = new Dictionary<ClothesHeadTypes, int>();
        _inventoryConsumableSkins = new Dictionary<ConsumableTypes, int>();
        _inventoryClothesTorsSkins = new Dictionary<ClothesTorsType, int>();

        IsStartDataLoaded = false;
    }

    [JsonConstructor]
    public PlayerData(Dictionary<ClothesHeadTypes, int> inventoryClothesHeadSkins, Dictionary<ConsumableTypes, int> inventoryConsumableSkins, 
        Dictionary<ClothesTorsType, int> inventoryClothesTorsSkins, bool isStartDataLoaded)
    {
        _inventoryClothesHeadSkins = new Dictionary<ClothesHeadTypes, int>(inventoryClothesHeadSkins);
        _inventoryConsumableSkins = new Dictionary<ConsumableTypes, int>(inventoryConsumableSkins);
        _inventoryClothesTorsSkins = new Dictionary<ClothesTorsType, int>(inventoryClothesTorsSkins);
        IsStartDataLoaded = isStartDataLoaded;
    }

    public IReadOnlyDictionary<ClothesHeadTypes, int> InventoryClothesHeadSkins => _inventoryClothesHeadSkins;

    public IReadOnlyDictionary<ConsumableTypes, int> InventoryConsumableSkins => _inventoryConsumableSkins;

    public IReadOnlyDictionary<ClothesTorsType, int> InventoryClothesTorsSkins => _inventoryClothesTorsSkins;

    public void AddClothesHeadSkin(ClothesHeadTypes clothesHeadSkin)
    {
        if (_inventoryClothesHeadSkins.ContainsKey(clothesHeadSkin)) 
        {
            var count = _inventoryClothesHeadSkins[clothesHeadSkin];
            _inventoryClothesHeadSkins[clothesHeadSkin] = count + 1;
        }
        else
        {
            _inventoryClothesHeadSkins.Add(clothesHeadSkin, 1);
        }
    }

    public void AddConsumableSkin(ConsumableTypes consumableSkin, int amount)
    {
        if (_inventoryConsumableSkins.ContainsKey(consumableSkin))
        {
            var count = _inventoryConsumableSkins[consumableSkin];
            _inventoryConsumableSkins[consumableSkin] = count + amount;
        }
        else
        {
            _inventoryConsumableSkins.Add(consumableSkin, amount);
        }
    }

    public void AddClothesTorsSkin(ClothesTorsType clothesTorsSkin)
    {
        if (_inventoryClothesTorsSkins.ContainsKey(clothesTorsSkin))
        {
            var count = _inventoryClothesTorsSkins[clothesTorsSkin];
            _inventoryClothesTorsSkins[clothesTorsSkin] = count + 1;
        }
        else
        {
            _inventoryClothesTorsSkins.Add(clothesTorsSkin, 1);
        }
    }

    public void RemoveClothesHeadSkin(ClothesHeadTypes clothesHeadSkin, int count)
    {
        if (_inventoryClothesHeadSkins.ContainsKey(clothesHeadSkin))
        {
            _inventoryClothesHeadSkins[clothesHeadSkin] -= count;

            if (_inventoryClothesHeadSkins[clothesHeadSkin] <= 0)
                _inventoryClothesHeadSkins.Remove(clothesHeadSkin);
        }
    }

    public void RemoveConsumableSkin(ConsumableTypes consumableSkin, int count)
    {
        if (_inventoryConsumableSkins.ContainsKey(consumableSkin))
        {
            _inventoryConsumableSkins[consumableSkin] -= count;

            if (_inventoryConsumableSkins[consumableSkin] <= 0)
                _inventoryConsumableSkins.Remove(consumableSkin);
        }
    }

    public void RemoveClothesTorsSkin(ClothesTorsType clothesTorsSkin, int count)
    {
        if (_inventoryClothesTorsSkins.ContainsKey(clothesTorsSkin))
        {
            _inventoryClothesTorsSkins[clothesTorsSkin] -= count;

            if (_inventoryClothesTorsSkins[clothesTorsSkin] <= 0)
                _inventoryClothesTorsSkins.Remove(clothesTorsSkin);
        }
    }
}
