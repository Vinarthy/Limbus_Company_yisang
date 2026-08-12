using System.Collections.Generic;

public static class DecorateConfig
{
    private static readonly Dictionary<string, string> ResourcePaths = new Dictionary<string, string>
    {
        { "正常喵喵", "Decorate/prefab/cat1" },
        { "招财喵喵", "Decorate/prefab/cat2" }
    };

    public static bool TryGetResourcePath(string itemName, out string resourcePath)
    {
        return ResourcePaths.TryGetValue(itemName, out resourcePath);
    }
}