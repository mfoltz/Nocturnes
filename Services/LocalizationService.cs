using ProjectM;
using Stunlock.Core;
using Stunlock.Localization;
using System.Reflection;
using System.Text.Json;

namespace Sanguis.Services;

internal class LocalizationService
{
    struct Code
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }

    struct Node
    {
        public string Guid { get; set; }
        public string Text { get; set; }
    }

    struct LocalizationFile
    {
        public Code[] Codes { get; set; }
        public Node[] Nodes { get; set; }
    }

    Dictionary<string, string> localization = [];
    Dictionary<int, string> prefabNames = [];

    public LocalizationService()
    {
        LoadLocalization();
        LoadPrefabNames();
    }

    void LoadLocalization()
    {
        var resourceName = "Sanguis.Localization.English.json";
        var assembly = Assembly.GetExecutingAssembly();
        var stream = assembly.GetManifestResourceStream(resourceName);
        
        using StreamReader reader = new(stream);
        string jsonContent = reader.ReadToEnd();
        var localizationFile = JsonSerializer.Deserialize<LocalizationFile>(jsonContent);
        localization = localizationFile.Nodes.ToDictionary(x => x.Guid, x => x.Text);   
    }

    void LoadPrefabNames()
    {
        var resourceName = "Sanguis.Localization.Prefabs.json";
        var assembly = Assembly.GetExecutingAssembly();
        var stream = assembly.GetManifestResourceStream(resourceName);

        using StreamReader reader = new(stream);
        string jsonContent = reader.ReadToEnd();
        prefabNames = JsonSerializer.Deserialize<Dictionary<int, string>>(jsonContent);
    }

    public string GetLocalization(string guid)
    {
        if (localization.TryGetValue(guid, out var text))
        {
            return text;
        }
        return $"<Localization not found for {guid}>";
    }
    public string GetLocalization(LocalizationKey key)
    {
        var guid = key.Key.ToGuid().ToString();
        return GetLocalization(guid);
    }
    public string GetPrefabName(PrefabGUID itemPrefabGUID)
    {
        if(!prefabNames.TryGetValue(itemPrefabGUID._Value, out var itemLocalizationHash))
        {
            return null;
        }

        string name = GetLocalization(itemLocalizationHash);

        if(Core.PrefabCollectionSystem._PrefabLookupMap.TryGetValue(itemPrefabGUID, out var prefab))
        {
            if (prefab.Has<ItemData>())
            {
                var itemData = prefab.Read<ItemData>();
                if (itemData.ItemType == ItemType.Tech)
                {
                    name = "Book " + name;
                }
            }
            
        }
        return name;
    }

}
