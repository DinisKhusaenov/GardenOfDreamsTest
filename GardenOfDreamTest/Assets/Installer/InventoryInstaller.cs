using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InventoryInstaller : MonoInstaller
{
    [SerializeField] private InventoryContent _inventoryContent;
    [SerializeField] private InitialInventoryData _initialInventoryData;
    [SerializeField] private PopUp popUp;
    [SerializeField] private InventoryItemViewFactory _inventoryItemViewFactory;
    [SerializeField] private List<Cell> _cells;

    public override void InstallBindings()
    {
        BindInventoryContent();
        BindInitialData();
        BindPopUp();
        BindSkinRemover();
        BindItemFactory();
        BindCells();
    }

    private void BindCells()
    {
        Container.Bind<List<Cell>>().FromInstance(_cells).AsSingle();
    }

    private void BindItemFactory()
    {
        Container.Bind<InventoryItemViewFactory>().FromInstance(_inventoryItemViewFactory).AsSingle();
    }

    private void BindInitialData()
    {
        Container.Bind<InitialInventoryData>().FromInstance(_initialInventoryData).AsSingle();
    }

    private void BindInventoryContent()
    {
        Container.Bind<InventoryContent>().FromInstance(_inventoryContent).AsSingle();
    }

    private void BindSkinRemover()
    {
        Container.BindInterfacesAndSelfTo<SkinRemover>().AsSingle();
    }

    private void BindPopUp()
    {
        Container.Bind<PopUp>().FromInstance(popUp).AsSingle();
    }
}
