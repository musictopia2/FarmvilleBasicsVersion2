namespace Phase04PrepareForMVP1.DataAccess;

public static class RepoDatabasePath
{
    public static string Get(string dbFileName = "FarmvilleV4.db")
    {
        // Start from the running app folder (bin/Debug/netX.Y/...)
        var dir = new DirectoryInfo(AppContext.BaseDirectory);

        while (dir != null)
        {
            // Marker #1: Databases folder at repo root
            var databasesDir = Path.Combine(dir.FullName, "Databases");
            if (Directory.Exists(databasesDir))
            {
                var dbPath = Path.Combine(databasesDir, dbFileName);

                // No creating folders. If file doesn't exist yet, that's fine if you intend SQLite to create it.
                // But the folder must exist (it does, because Directory.Exists succeeded).
                return dbPath;
            }

            // Optional marker #2 (extra safety): solution file at repo root
            // if (File.Exists(Path.Combine(dir.FullName, "FarmvilleBasicsVersion1.sln")))
            // {
            //     var dbPath = Path.Combine(dir.FullName, "Databases", dbFileName);
            //     if (!Directory.Exists(Path.Combine(dir.FullName, "Databases")))
            //         throw new InvalidOperationException("Found solution root but no Databases folder.");
            //     return dbPath;
            // }

            dir = dir.Parent;
        }

        throw new InvalidOperationException(
            "Could not locate the repository root (no 'Databases' folder found above the app base directory). " +
            "Make sure you are running from inside the cloned repo and that the repo includes a top-level 'Databases' folder.");
    }
}