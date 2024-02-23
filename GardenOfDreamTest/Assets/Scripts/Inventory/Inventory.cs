using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class Inventory : MonoBehaviour
{
    [SerializeField] private SkinsPanel _skinsPanel;

    private InitialInventoryData _initialInventoryData;
    private InventoryContent _inventoryContent;

    private List<int> _clothesTorsSkinCount;
    private List<int> _consumableSkinCount;
    private List<int> _clothesHeadSkinCount;

    private List<ClothesTorsSkinItem> _clothesTorsSkinItems;
    private List<ConsumableSkinItem> _consumableSkinItems;
    private List<ClothesHeadSkinItem> _clothesHeadSkinItems;

    private IDataProvider _dataProvider;
    private IPersistentData _persistentData;

    private SkinRemover _skinRemover;
    private SkinSaver _skinSaver;

    [Inject]
    private void Construct(IDataProvider dataProvider, IPersistentData persistentData, SkinRemover skinRemover, 
        InitialInventoryData initialInventoryData, InventoryContent inventoryContent, SkinSaver skinSaver)
    {
        _dataProvider = dataProvider;
        _persistentData = persistentData;
        _skinRemover = skinRemover;
        _initialInventoryData = initialInventoryData;
        _inventoryContent = inventoryContent;
        _skinSaver = skinSaver;

        _clothesTorsSkinCount = new List<int>();
        _consumableSkinCount = new List<int>();
        _clothesHeadSkinCount = new List<int>();

        _clothesTorsSkinItems = new List<ClothesTorsSkinItem>();
        _clothesHeadSkinItems = new List<ClothesHeadSkinItem>();
        _consumableSkinItems = new List<ConsumableSkinItem>();

        if (!_persistentData.PlayerData.IsStartDataLoaded)
        {
            LoadStartData();
        }
        else
        {
            LoadClothesHeadData();
            LoadClothesTorsData();
            LoadConsumableData();

            _skinsPanel.Show(_clothesTorsSkinItems, _clothesTorsSkinCount);
            _skinsPanel.Show(_consumableSkinItems, _consumableSkinCount);
            _skinsPanel.Show(_clothesHeadSkinItems, _clothesHeadSkinCount);
        }
    }

    private void OnEnable()
    {
        _skinsPanel.ItemViewDestroyed += OnItemViewDestroyed;
    }

    private void OnDisable()
    {
        _skinsPanel.ItemViewDestroyed -= OnItemViewDestroyed;

        _dataProvider.Save();
    }

    public void AddRandomItem()
    {
        List<InventoryItem> allItems = new List<InventoryItem>();
        allItems.AddRange(_inventoryContent.ClothesHeadSkinItems);
        allItems.AddRange(_inventoryContent.ClothesTorsSkinItems);
        allItems.AddRange(_inventoryContent.ConsumableSkinItems);

        var randomItem = allItems[Random.Range(0, allItems.Count)];
        IEnumerable<InventoryItem> item = new List<InventoryItem>() {randomItem};
        _skinsPanel.Show(item, new List<int>() { randomItem.MaxCount});

        _skinSaver.Visit(randomItem);
        _dataProvider.Save();
    }

    private void LoadStartData()
    {
        _skinsPanel.Show(_initialInventoryData.ClosthesHeadSkinItems, _initialInventoryData.ClosthesHeadSkinItemsCount.ToList());
        _skinsPanel.Show(_initialInventoryData.ClothesTorsSkinItems, _initialInventoryData.ClothesTorsSkinItemsCount.ToList());
        _skinsPanel.Show(_initialInventoryData.ConsumableSkinItems, _initialInventoryData.ConsumableSkinItemsCount.ToList());

        for (int i = 0; i < _initialInventoryData.ConsumableSkinItems.Count(); i++)
        {
            _persistentData.PlayerData.AddConsumableSkin(_initialInventoryData.ConsumableSkinItems.ElementAt(i).SkinType, 
                _initialInventoryData.ConsumableSkinItemsCount.ElementAt(i));
        }

        foreach (var item in _initialInventoryData.ClothesTorsSkinItems)
            _persistentData.PlayerData.AddClothesTorsSkin(item.SkinType);

        foreach (var item in _initialInventoryData.ClosthesHeadSkinItems)
            _persistentData.PlayerData.AddClothesHeadSkin(item.SkinType);

        _persistentData.PlayerData.IsStartDataLoaded = true;
        _dataProvider.Save();
    }

    private void OnItemViewDestroyed(InventoryItemView item)
    {
        _skinRemover.Visit(item.Item, item.Count);

        _dataProvider.Save();
    }

    private void LoadClothesTorsData()
    {
        foreach (ClothesTorsSkinItem item in _inventoryContent.ClothesTorsSkinItems)
        {
            if (_persistentData.PlayerData.InventoryClothesTorsSkins.ContainsKey(item.SkinType))
            {
                _clothesTorsSkinItems.Add(item);
                _clothesTorsSkinCount.Add(_persistentData.PlayerData.InventoryClothesTorsSkins[item.SkinType]);
            }
        }
    }

    private void LoadConsumableData()
    {
        foreach (ConsumableSkinItem item in _inventoryContent.ConsumableSkinItems)
        {
            if (_persistentData.PlayerData.InventoryConsumableSkins.ContainsKey(item.SkinType))
            {
                _consumableSkinItems.Add(item);
                _consumableSkinCount.Add(_persistentData.PlayerData.InventoryConsumableSkins[item.SkinType]);
            }
        }
    }

    private void LoadClothesHeadData()
    {
        _clothesHeadSkinItems.Clear();
        _consumableSkinCount.Clear();

        foreach (ClothesHeadSkinItem item in _inventoryContent.ClothesHeadSkinItems)
        {
            if (_persistentData.PlayerData.InventoryClothesHeadSkins.ContainsKey(item.SkinType))
            {
                _clothesHeadSkinItems.Add(item);
                _clothesHeadSkinCount.Add(_persistentData.PlayerData.InventoryClothesHeadSkins[item.SkinType]);
            }
        }
    }
}
