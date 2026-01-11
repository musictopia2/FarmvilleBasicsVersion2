namespace Phase10ProgressionUnlocks.DataAccess;
public static class MainDatabase
{
    public static string DatabasePath =>
       RepoDatabasePath.Get("FarmvilleV10.db");
    public const string DatabaseName = "Farmville";
    public static void Prep()
    {
        SqliteCreateDocumentDatabaseClass.RegisterCreatingDocumentDatabase();
        dd1.SQLiteConnector = new CustomSQLiteConnectionClass();
        bb1.SetupIConfiguration(); //hopefully good enough.
    }
}