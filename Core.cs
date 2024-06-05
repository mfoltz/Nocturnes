using BepInEx.Logging;
using BepInEx.Unity.IL2CPP.Utils.Collections;
using Nocturnes.Services;
using ProjectM.Physics;
using ProjectM.Scripting;
using System.Collections;
using System.Text.Json;
using Unity.Entities;
using UnityEngine;

namespace Nocturnes;

internal static class Core
{
    public static World Server { get; } = GetWorld("Server") ?? throw new System.Exception("There is no Server world (yet)...");
    public static EntityManager EntityManager { get; } = Server.EntityManager;
    public static ServerScriptMapper ServerScriptMapper { get; internal set; }
    public static ServerGameManager ServerGameManager => ServerScriptMapper.GetServerGameManager();
    public static ManualLogSource Log => Plugin.LogInstance;

    private static bool hasInitialized;
    public static NocturneService NocturneService { get; internal set; }

    static MonoBehaviour monoBehaviour;

    public static void Initialize()
    {
        if (hasInitialized) return;

        ServerScriptMapper = Server.GetExistingSystemManaged<ServerScriptMapper>();
        NocturneService = new(); 
        // Initialize utility services
        Log.LogInfo($"{MyPluginInfo.PLUGIN_NAME}[{MyPluginInfo.PLUGIN_VERSION}] initialized!");
        hasInitialized = true;
    }
    private static World GetWorld(string name)
    {
        foreach (var world in World.s_AllWorlds)
        {
            if (world.Name == name)
            {
                return world;
            }
        }
        return null;
    }
    public static void StartCoroutine(IEnumerator routine)
    {
        if (monoBehaviour == null)
        {
            var go = new GameObject("Nocturnes");
            monoBehaviour = go.AddComponent<IgnorePhysicsDebugSystem>();
            UnityEngine.Object.DontDestroyOnLoad(go);
        }
        monoBehaviour.StartCoroutine(routine.WrapToIl2Cpp());
    }
    public static string ExtractName(string input)
    {
        // Split the input string by spaces
        string[] parts = input.Split(' ');

        // Check if the first part contains underscores
        if (parts.Length > 0 && parts[0].Contains('_'))
        {
            // Split the first part by underscores and take the last part
            string[] nameParts = parts[0].Split('_');
            return nameParts[^1];
        }

        return string.Empty;
    }
    public class DataStructures
    {
        // Encapsulated fields with properties

        static readonly JsonSerializerOptions prettyJsonOptions = new()
        {
            WriteIndented = true,
            IncludeFields = true
        };

        // structures to write to json for permanence

        static Dictionary<ulong, (int Tokens, (DateTime Start, DateTime DailyLogin) TimeData)> playerTokens = [];

        public static Dictionary<ulong, (int Tokens, (DateTime Start, DateTime DailyLogin) TimeData)> PlayerTokens
        {
            get => playerTokens;
            set => playerTokens = value;
        }

        // file paths dictionary
        static readonly Dictionary<string, string> filePaths = new()
        {
            {"Tokens", JsonFiles.PlayerTokenJsons},
        };

        // Generic method to save any type of dictionary.
        public static void LoadData<T>(ref Dictionary<ulong, T> dataStructure, string key)
        {
            string path = filePaths[key];
            if (!File.Exists(path))
            {
                // If the file does not exist, create a new empty file to avoid errors on initial load.
                File.Create(path).Dispose();
                dataStructure = []; // Initialize as empty if file does not exist.
                Log.LogInfo($"{key} file created as it did not exist.");
                return;
            }
            try
            {
                string json = File.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(json))
                {
                    // Handle the empty file case
                    dataStructure = []; // Provide default empty dictionary
                }
                else
                {
                    var data = System.Text.Json.JsonSerializer.Deserialize<Dictionary<ulong, T>>(json, prettyJsonOptions);
                    dataStructure = data ?? []; // Ensure non-null assignment
                }
            }
            catch (IOException ex)
            {
                Log.LogError($"Error reading {key} data from file: {ex.Message}");
                dataStructure = []; // Provide default empty dictionary on error.
            }
            catch (System.Text.Json.JsonException ex)
            {
                Log.LogError($"JSON deserialization error when loading {key} data: {ex.Message}");
                dataStructure = []; // Provide default empty dictionary on error.
            }
        }
        public static void LoadPlayerTokens() => LoadData(ref playerTokens, "Tokens");
        public static void SaveData<T>(Dictionary<ulong, T> data, string key)
        {
            string path = filePaths[key];
            try
            {
                string json = System.Text.Json.JsonSerializer.Serialize(data, prettyJsonOptions);
                File.WriteAllText(path, json);
            }
            catch (IOException ex)
            {
                Log.LogError($"Failed to write {key} data to file: {ex.Message}");
            }
            catch (System.Text.Json.JsonException ex)
            {
                Log.LogError($"JSON serialization error when saving {key} data: {ex.Message}");
            }
        }
        public static void SavePlayerTokens() => SaveData(PlayerTokens, "Tokens");
    }
    static class JsonFiles
    {
        public static readonly string PlayerTokenJsons = Plugin.PlayerTokensPath;
    }
}