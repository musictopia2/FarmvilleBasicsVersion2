namespace Phase09LevelProgression.Utilities;

public static class RepoImagesPath
{
    public static string GetImagesRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);

        while (dir != null)
        {
            // Marker: Images folder at repo root
            var imagesDir = Path.Combine(dir.FullName, "Images");
            if (Directory.Exists(imagesDir))
            {
                return imagesDir;
            }

            dir = dir.Parent;
        }

        throw new InvalidOperationException(
            "Could not locate the repository Images folder (no 'Images' folder found above the app base directory). " +
            "Make sure you are running from inside the cloned repo and that the repo includes a top-level 'Images' folder.");
    }

    // Optional convenience: build a physical file path for checks (not required for <img>)
    public static string GetPhysicalImagePath(string relativePathFromImagesRoot)
    {
        if (string.IsNullOrWhiteSpace(relativePathFromImagesRoot))
            throw new ArgumentException("Relative path is required.", nameof(relativePathFromImagesRoot));

        // Normalize slashes
        relativePathFromImagesRoot = relativePathFromImagesRoot
            .Replace('\\', Path.DirectorySeparatorChar)
            .Replace('/', Path.DirectorySeparatorChar);

        return Path.Combine(GetImagesRoot(), relativePathFromImagesRoot);
    }
}