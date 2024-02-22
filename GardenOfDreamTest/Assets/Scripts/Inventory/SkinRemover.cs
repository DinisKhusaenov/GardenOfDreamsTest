public class SkinRemover
{
    private IPersistentData _persistentData;

    public SkinRemover(IPersistentData persistentData)
    {
        _persistentData = persistentData;
    }

    public void Visit(InventoryItem inventoryItem, int count) => Visit((dynamic) inventoryItem, count);

    public void Visit(ClothesHeadSkinItem armorSkinItem, int count)
        => _persistentData.PlayerData.RemoveClothesHeadSkin(armorSkinItem.SkinType, count);

    public void Visit(ConsumableSkinItem ammoSkinItem, int count)
        => _persistentData.PlayerData.RemoveConsumableSkin(ammoSkinItem.SkinType, count);

    public void Visit(ClothesTorsSkinItem weaponSkinItem, int count)
        => _persistentData.PlayerData.RemoveClothesTorsSkin(weaponSkinItem.SkinType, count);
}
