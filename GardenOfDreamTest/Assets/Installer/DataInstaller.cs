using Zenject;

public class DataInstaller : MonoInstaller
{
    private PersistentData _persistentData;
    private DataLocalProvider _dataProvider;

    public override void InstallBindings()
    {
        BindPersistentData();
        BindDataProvider();
    }

    private void BindDataProvider()
    {
        _dataProvider = new DataLocalProvider(_persistentData);
        LoadDataOrInit();
        Container.BindInterfacesAndSelfTo<DataLocalProvider>().FromInstance(_dataProvider).AsSingle();
    }

    private void BindPersistentData()
    {
        _persistentData = new PersistentData();
        Container.BindInterfacesAndSelfTo<PersistentData>().FromInstance(_persistentData).AsSingle();
    }

    private void LoadDataOrInit()
    {
        if (_dataProvider.TryLoad() == false)
            _persistentData.PlayerData = new PlayerData();
    }
}
