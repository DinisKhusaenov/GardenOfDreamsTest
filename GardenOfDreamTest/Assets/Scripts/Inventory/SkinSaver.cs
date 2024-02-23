public class SkinSaver : IInventoryItemVisitor
{
    private IPersistentData _data;

    public SkinSaver(IPersistentData data)
    {
        _data = data;
    }

    public void Visit(InventoryItem inventoryItem) => Visit((dynamic)inventoryItem);

    public void Visit(ClothesHeadSkinItem clothesHeadSkinItem)
    {
        _data.PlayerData.AddClothesHeadSkin(clothesHeadSkinItem.SkinType);
    }

    public void Visit(ConsumableSkinItem consumableSkinItem)
    {
        _data.PlayerData.AddConsumableSkin(consumableSkinItem.SkinType, consumableSkinItem.MaxCount);
    }

    public void Visit(ClothesTorsSkinItem clothesTorsSkinItem)
    {
        _data.PlayerData.AddClothesTorsSkin(clothesTorsSkinItem.SkinType);
    }
}
