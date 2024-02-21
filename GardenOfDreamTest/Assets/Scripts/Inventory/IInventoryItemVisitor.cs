public interface IInventoryItemVisitor 
{
    void Visit(InventoryItem inventoryItem);
    void Visit(ClothesHeadSkinItem clothesHeadSkinItem);
    void Visit(ConsumableSkinItem consumableSkinItem);
    void Visit(ClothesTorsSkinItem clothesTorsSkinItem);
}
