namespace Phase12QuestsBasedOnLevel.ImportClasses;
public static class ImportStartClass
{
    public static async Task ImportStartAsync()
    {
        
        StartFarmDatabase db = new();
        await db.ImportAsync(FarmHelperClass.GetAllFarms());
    }
}