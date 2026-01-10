namespace Phase09LevelProgression.ImportClasses;
public static class ImportProgressionProfileClass
{
    public static async Task ImportProgressionAsync()
    {
        var farms = FarmHelperClass.GetAllFarms();
        ProgressionProfileDatabase db = new();
        BasicList<ProgressionProfileDocument> list = [];
        foreach (var farm in farms)
        {
            list.Add(new()
            { 
                Farm = farm 
            }
            );
        }
        await db.ImportAsync(list);
    }
}