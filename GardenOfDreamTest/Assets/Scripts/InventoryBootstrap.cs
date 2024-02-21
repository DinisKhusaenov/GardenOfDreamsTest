using UnityEngine;

public class InventoryBootstrap : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;

    private IDataProvider _dataProvider;
    private IPersistentData _persistentData;

    private void Awake()
    {
        InitializeData();
        InitializeInventory();
    }

    private void InitializeData()
    {
        _persistentData = new PersistentData();
        _dataProvider = new DataLocalProvider(_persistentData);

        LoadDataOrInit();
    }

    private void InitializeInventory()
    {
        SkinRemover skinRemover = new SkinRemover(_persistentData);

        _inventory.Initialize(_dataProvider, _persistentData, skinRemover);
    }

    private void LoadDataOrInit()
    {
        if (_dataProvider.TryLoad() == false)
            _persistentData.PlayerData = new PlayerData();
    }
}
