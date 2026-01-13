namespace Phase11StoreWithBasicPurchases.ImportClasses;
public static class ImportStoreUiStateClass
{
    public static async Task ImportUiStoreStateAsync()
    {
        BasicList<FarmKey> farms = FarmHelperClass.GetAllFarms();
        BasicList<StoreUiStateDocument> list = [];
        foreach (var item in farms)
        {
            StoreUiStateDocument document = new()
            {
                Farm = item,
            };
            list.Add(document);
        }
        StoreUiStateDatabase db = new();
        await db.ImportAsync(list);
    }
}