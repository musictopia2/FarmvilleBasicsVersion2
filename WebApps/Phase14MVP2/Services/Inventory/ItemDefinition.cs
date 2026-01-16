namespace Phase14MVP2.Services.Inventory;
public readonly record struct ItemDefinition(string ItemName, EnumInventoryStorageCategory Storage, EnumInventoryItemCategory ItemCategory);