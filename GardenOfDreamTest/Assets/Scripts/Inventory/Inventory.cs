using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private InventoryContent _inventoryContent;
    [SerializeField] private InitialInventoryData _initialInventoryData;

    [SerializeField] private SkinsPanel _skinsPanel;

    private List<int> _weaponSkinCount;
    private List<int> _armorSkinCount;
    private List<int> _ammoSkinCount;

    private List<ClothesTorsSkinItem> _weaponSkinItems;
    private List<ConsumableSkinItem> _ammoSkinItems;
    private List<ClothesHeadSkinItem> _armorSkinItems;

    private IDataProvider _dataProvider;
    private IPersistentData _persistentData;

    private SkinRemover _skinRemover;

    public void Initialize(IDataProvider dataProvider, IPersistentData persistentData, SkinRemover skinRemover)
    {
        _dataProvider = dataProvider;
        _persistentData = persistentData;
        _skinRemover = skinRemover;

        _weaponSkinCount = new List<int>();
        _armorSkinCount = new List<int>();
        _ammoSkinCount = new List<int>();

        _weaponSkinItems = new List<ClothesTorsSkinItem>();
        _armorSkinItems = new List<ClothesHeadSkinItem>();
        _ammoSkinItems = new List<ConsumableSkinItem>();

        _skinsPanel.Show(_initialInventoryData.ClosthesHeadSkinItems, _initialInventoryData.ClosthesHeadSkinItemsCount.ToList());
        _skinsPanel.Show(_initialInventoryData.ClothesTorsSkinItems, _initialInventoryData.ClothesTorsSkinItemsCount.ToList());
        _skinsPanel.Show(_initialInventoryData.ConsumableSkinItems, _initialInventoryData.ConsumableSkinItemsCount.ToList());
    }

    private void OnEnable()
    {
        _skinsPanel.ItemViewDestroyed += OnItemViewDestroyed;
    }

    private void OnDisable()
    {
        _skinsPanel.ItemViewDestroyed -= OnItemViewDestroyed;
    }

    private void OnItemViewDestroyed(InventoryItemView item)
    {
        _skinRemover.Visit(item.Item);

        _dataProvider.Save();
    }

    private List<ClothesTorsSkinItem> GetWeaponData()
    {
        _weaponSkinItems.Clear();
        _weaponSkinCount.Clear();

        foreach (ClothesTorsSkinItem item in _inventoryContent.ClothesTorsSkinItems)
        {
            if (_persistentData.PlayerData.InventoryClothesTorsSkins.ContainsKey(item.SkinType))
            {
                _weaponSkinItems.Add(item);
                _weaponSkinCount.Add(_persistentData.PlayerData.InventoryClothesTorsSkins[item.SkinType]);
            }
        }

        return _weaponSkinItems;
    }

    private List<ConsumableSkinItem> GetAmmoData()
    {
        _ammoSkinItems.Clear();
        _ammoSkinCount.Clear();

        foreach (ConsumableSkinItem item in _inventoryContent.ConsumableSkinItems)
        {
            if (_persistentData.PlayerData.InventoryConsumableSkins.ContainsKey(item.SkinType))
            {
                _ammoSkinItems.Add(item);
                _ammoSkinCount.Add(_persistentData.PlayerData.InventoryConsumableSkins[item.SkinType]);
            }
        }

        return _ammoSkinItems;
    }

    private List<ClothesHeadSkinItem> GetArmorData()
    {
        _armorSkinItems.Clear();
        _armorSkinCount.Clear();

        foreach (ClothesHeadSkinItem item in _inventoryContent.ClothesHeadSkinItems)
        {
            if (_persistentData.PlayerData.InventoryClothesHeadSkins.ContainsKey(item.SkinType))
            {
                _armorSkinItems.Add(item);
                _armorSkinCount.Add(_persistentData.PlayerData.InventoryClothesHeadSkins[item.SkinType]);
            }
        }

        return _armorSkinItems;
    }
}
