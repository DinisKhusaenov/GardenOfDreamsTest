public class SkinRemover : IInventoryItemVisitor
{
    private IPersistentData _persistentData;

    public SkinRemover(IPersistentData persistentData)
    {
        _persistentData = persistentData;
    }

    public void Visit(InventoryItem inventoryItem) => Visit((dynamic) inventoryItem);

    public void Visit(ClothesHeadSkinItem armorSkinItem)
        => _persistentData.PlayerData.RemoveClothesHeadSkin(armorSkinItem.SkinType);

    public void Visit(ConsumableSkinItem ammoSkinItem)
        => _persistentData.PlayerData.RemoveConsumableSkin(ammoSkinItem.SkinType);

    public void Visit(ClothesTorsSkinItem weaponSkinItem)
        => _persistentData.PlayerData.RemoveClothesTorsSkin(weaponSkinItem.SkinType);
}
