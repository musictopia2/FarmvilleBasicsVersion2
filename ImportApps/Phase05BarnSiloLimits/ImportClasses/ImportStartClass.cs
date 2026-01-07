namespace Phase05BarnSiloLimits.ImportClasses;
public static class ImportStartClass
{
    public static async Task ImportStartAsync()
    {
        
        StartFarmDatabase db = new();
        await db.ImportAsync(FarmHelperClass.GetAllFarms());
    }
}