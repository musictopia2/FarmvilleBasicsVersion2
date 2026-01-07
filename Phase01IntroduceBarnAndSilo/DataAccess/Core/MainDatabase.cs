namespace Phase01IntroduceBarnAndSilo.DataAccess.Core;
public static class MainDatabase
{
    public static string DatabasePath =>
       RepoDatabasePath.Get("FarmvilleV4.db");
    public const string DatabaseName = "Farmville";
    public static void Prep()
    {
        SqliteCreateDocumentDatabaseClass.RegisterCreatingDocumentDatabase();
        dd1.SQLiteConnector = new CustomSQLiteConnectionClass();
        bb1.SetupIConfiguration(); //hopefully good enough.
    }
}