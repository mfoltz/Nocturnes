using BepInEx.Logging;
using BepInEx.Unity.IL2CPP.Utils.Collections;
using ProjectM;
using ProjectM.Physics;
using ProjectM.Scripting;
using Stunlock.Core;
using System.Collections;
using System.Text.Json;
using Unity.Entities;
using UnityEngine;

namespace Cobalt;

internal static class Core
{
    public static World Server { get; } = GetWorld("Server") ?? throw new System.Exception("There is no Server world (yet). Did you install a server mod on the client?");

    // V Rising systems
    public static EntityManager EntityManager { get; } = Server.EntityManager;

    public static PrefabCollectionSystem PrefabCollectionSystem { get; internal set; }
    public static ServerGameSettingsSystem ServerGameSettingsSystem { get; internal set; }
    public static ServerScriptMapper ServerScriptMapper { get; internal set; }
    public static DebugEventsSystem DebugEventsSystem { get; internal set; }
    public static double ServerTime => ServerGameManager.ServerTime;
    public static ServerGameManager ServerGameManager => ServerScriptMapper.GetServerGameManager();

    // BepInEx services
    public static ManualLogSource Log => Plugin.LogInstance;

    private static bool hasInitialized;

    public static void Initialize()
    {
        if (hasInitialized) return;

        PrefabCollectionSystem = Server.GetExistingSystemManaged<PrefabCollectionSystem>();
        ServerGameSettingsSystem = Server.GetExistingSystemManaged<ServerGameSettingsSystem>();
        DebugEventsSystem = Server.GetExistingSystemManaged<DebugEventsSystem>();
        ServerScriptMapper = Server.GetExistingSystemManaged<ServerScriptMapper>();

        // Initialize utility services

        Core.Log.LogInfo("Cobalt initialized");

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

    public class DataStructures
    {
        // Encapsulated fields with properties

        private static readonly JsonSerializerOptions prettyJsonOptions = new()
        {
            WriteIndented = true,
            IncludeFields = true
        };

        // structures to write to json for permanence

        private static Dictionary<ulong, KeyValuePair<int, float>> playerExperience = [];
        private static Dictionary<ulong, KeyValuePair<int, float>> playerPrestige = [];
        private static Dictionary<ulong, Dictionary<string, bool>> playerBools = [];

        private static Dictionary<ulong, KeyValuePair<int, float>> playerWoodcutting = [];
        private static Dictionary<ulong, KeyValuePair<int, float>> playerMining = [];
        private static Dictionary<ulong, KeyValuePair<int, float>> playerFishing = [];
        private static Dictionary<ulong, KeyValuePair<int, float>> playerBlacksmithing = [];
        private static Dictionary<ulong, KeyValuePair<int, float>> playerTailoring = [];
        private static Dictionary<ulong, KeyValuePair<int, float>> playerJewelcrafting = [];
        private static Dictionary<ulong, KeyValuePair<int, float>> playerAlchemy = [];
        private static Dictionary<ulong, KeyValuePair<int, float>> playerHarvesting = [];

        private static Dictionary<ulong, KeyValuePair<int, float>> playerSwordMastery = [];
        private static Dictionary<ulong, KeyValuePair<int, float>> playerAxeMastery = [];
        private static Dictionary<ulong, KeyValuePair<int, float>> playerMaceMastery = [];
        private static Dictionary<ulong, KeyValuePair<int, float>> playerSpearMastery = [];
        private static Dictionary<ulong, KeyValuePair<int, float>> playerCrossbowMastery = [];
        private static Dictionary<ulong, KeyValuePair<int, float>> playerGreatSwordMastery = [];
        private static Dictionary<ulong, KeyValuePair<int, float>> playerSlashersMastery = [];
        private static Dictionary<ulong, KeyValuePair<int, float>> playerPistolsMastery = [];
        private static Dictionary<ulong, KeyValuePair<int, float>> playerReaperMastery = [];
        private static Dictionary<ulong, KeyValuePair<int, float>> playerLongbowMastery = [];
        private static Dictionary<ulong, KeyValuePair<int, float>> playerUnarmedMastery = [];
        private static Dictionary<ulong, KeyValuePair<int, float>> playerWhipMastery = [];
        private static Dictionary<ulong, Dictionary<string, List<string>>> playerWeaponChoices = [];
        private static Dictionary<ulong, Dictionary<string, bool>> playerEquippedWeapon = [];

        private static Dictionary<ulong, KeyValuePair<int, float>> playerSanguimancy = [];
        private static Dictionary<ulong, List<string>> playerBloodChoices = [];

        public static Dictionary<ulong, KeyValuePair<int, float>> PlayerExperience
        {
            get => playerExperience;
            set => playerExperience = value;
        }

        public static Dictionary<ulong, KeyValuePair<int, float>> PlayerPrestige
        {
            get => playerPrestige;
            set => playerPrestige = value;
        }

        public static Dictionary<ulong, Dictionary<string, bool>> PlayerBools
        {
            get => playerBools;
            set => playerBools = value;
        }

        public static Dictionary<ulong, KeyValuePair<int, float>> PlayerWoodcutting
        {
            get => playerWoodcutting;
            set => playerWoodcutting = value;
        }

        public static Dictionary<ulong, KeyValuePair<int, float>> PlayerMining
        {
            get => playerMining;
            set => playerMining = value;
        }

        public static Dictionary<ulong, KeyValuePair<int, float>> PlayerFishing
        {
            get => playerFishing;
            set => playerFishing = value;
        }

        public static Dictionary<ulong, KeyValuePair<int, float>> PlayerBlacksmithing
        {
            get => playerBlacksmithing;
            set => playerBlacksmithing = value;
        }

        public static Dictionary<ulong, KeyValuePair<int, float>> PlayerTailoring
        {
            get => playerTailoring;
            set => playerTailoring = value;
        }

        public static Dictionary<ulong, KeyValuePair<int, float>> PlayerJewelcrafting
        {
            get => playerJewelcrafting;
            set => playerJewelcrafting = value;
        }

        public static Dictionary<ulong, KeyValuePair<int, float>> PlayerAlchemy
        {
            get => playerAlchemy;
            set => playerAlchemy = value;
        }

        public static Dictionary<ulong, KeyValuePair<int, float>> PlayerHarvesting
        {
            get => playerHarvesting;
            set => playerHarvesting = value;
        }

        public static Dictionary<ulong, KeyValuePair<int, float>> PlayerSwordMastery
        {
            get => playerSwordMastery;
            set => playerSwordMastery = value;
        }

        public static Dictionary<ulong, KeyValuePair<int, float>> PlayerAxeMastery
        {
            get => playerAxeMastery;
            set => playerAxeMastery = value;
        }

        public static Dictionary<ulong, KeyValuePair<int, float>> PlayerMaceMastery
        {
            get => playerMaceMastery;
            set => playerMaceMastery = value;
        }

        public static Dictionary<ulong, KeyValuePair<int, float>> PlayerSpearMastery
        {
            get => playerSpearMastery;
            set => playerSpearMastery = value;
        }

        public static Dictionary<ulong, KeyValuePair<int, float>> PlayerCrossbowMastery
        {
            get => playerCrossbowMastery;
            set => playerCrossbowMastery = value;
        }

        public static Dictionary<ulong, KeyValuePair<int, float>> PlayerGreatSwordMastery
        {
            get => playerGreatSwordMastery;
            set => playerGreatSwordMastery = value;
        }

        public static Dictionary<ulong, KeyValuePair<int, float>> PlayerSlashersMastery
        {
            get => playerSlashersMastery;
            set => playerSlashersMastery = value;
        }

        public static Dictionary<ulong, KeyValuePair<int, float>> PlayerPistolsMastery
        {
            get => playerPistolsMastery;
            set => playerPistolsMastery = value;
        }

        public static Dictionary<ulong, KeyValuePair<int, float>> PlayerReaperMastery
        {
            get => playerReaperMastery;
            set => playerReaperMastery = value;
        }

        public static Dictionary<ulong, KeyValuePair<int, float>> PlayerLongbowMastery
        {
            get => playerLongbowMastery;
            set => playerLongbowMastery = value;
        }

        public static Dictionary<ulong, KeyValuePair<int, float>> PlayerWhipMastery
        {
            get => playerWhipMastery;
            set => playerWhipMastery = value;
        }

        public static Dictionary<ulong, KeyValuePair<int, float>> PlayerUnarmedMastery
        {
            get => playerUnarmedMastery;
            set => playerUnarmedMastery = value;
        }

        public static Dictionary<ulong, KeyValuePair<int, float>> PlayerSanguimancy
        {
            get => playerSanguimancy;
            set => playerSanguimancy = value;
        }

        public static Dictionary<ulong, Dictionary<string, List<string>>> PlayerWeaponChoices // weapon, then list of stats for the weapon in string form
        {
            get => playerWeaponChoices;
            set => playerWeaponChoices = value;
        }

        public static Dictionary<ulong, Dictionary<string, bool>> PlayerEquippedWeapon
        {
            get => playerEquippedWeapon;
            set => playerEquippedWeapon = value;
        }

        public static Dictionary<ulong, List<string>> PlayerBloodChoices
        {
            get => playerBloodChoices;
            set => playerBloodChoices = value;
        }

        // cache-only

        private static Dictionary<ulong, Dictionary<PrefabGUID, bool>> playerCraftingJobs = [];

        public static Dictionary<ulong, Dictionary<PrefabGUID, bool>> PlayerCraftingJobs
        {
            get => playerCraftingJobs;
            set => playerCraftingJobs = value;
        }

        // file paths dictionary
        private static readonly Dictionary<string, string> filePaths = new()
        {
            {"Experience", Core.JsonFiles.PlayerExperienceJson},
            {"Prestige", Core.JsonFiles.PlayerPrestigeJson },
            {"PlayerBools", Core.JsonFiles.PlayerBoolsJson},
            {"Woodcutting", Core.JsonFiles.PlayerWoodcuttingJson},
            {"Mining", Core.JsonFiles.PlayerMiningJson},
            {"Fishing", Core.JsonFiles.PlayerFishingJson},
            {"Blacksmithing", Core.JsonFiles.PlayerBlacksmithingJson},
            {"Tailoring", Core.JsonFiles.PlayerTailoringJson},
            {"Jewelcrafting", Core.JsonFiles.PlayerJewelcraftingJson},
            {"Alchemy", Core.JsonFiles.PlayerAlchemyJson},
            {"Harvesting", Core.JsonFiles.PlayerHarvestingJson},
            {"SwordMastery", Core.JsonFiles.PlayerSwordMasteryJson },
            {"AxeMastery", Core.JsonFiles.PlayerAxeMasteryJson},
            {"MaceMastery", Core.JsonFiles.PlayerMaceMasteryJson},
            {"SpearMastery", Core.JsonFiles.PlayerSpearMasteryJson},
            {"CrossbowMastery", Core.JsonFiles.PlayerCrossbowMasteryJson},
            {"GreatSwordMastery", Core.JsonFiles.PlayerGreatSwordMastery},
            {"SlashersMastery", Core.JsonFiles.PlayerSlashersMasteryJson},
            {"PistolsMastery", Core.JsonFiles.PlayerPistolsMasteryJson},
            {"ReaperMastery", Core.JsonFiles.PlayerReaperMastery},
            {"LongbowMastery", Core.JsonFiles.PlayerLongbowMasteryJson},
            {"WhipMastery", Core.JsonFiles.PlayerWhipMasteryJson},
            {"UnarmedMastery", Core.JsonFiles.PlayerUnarmedMasteryJson},
            {"Sanguimancy", Core.JsonFiles.PlayerSanguimancyJson},
            {"EquippedWeapon", Core.JsonFiles.PlayerEquippedWeaponJson},
            {"WeaponChoices", Core.JsonFiles.PlayerWeaponChoicesJson},
            {"BloodChoices", Core.JsonFiles.PlayerBloodChoicesJson}
        };

        public static readonly Dictionary<string, Dictionary<ulong, KeyValuePair<int, float>>> professionMap = new()
        {
            {"Woodcutting", PlayerWoodcutting},
            {"Mining", PlayerMining},
            {"Fishing", PlayerFishing},
            {"Blacksmithing", PlayerBlacksmithing},
            {"Tailoring", PlayerTailoring},
            {"Jewelcrafting", PlayerJewelcrafting},
            {"Alchemy", PlayerAlchemy},
            {"Harvesting", PlayerHarvesting }
        };

        public static readonly Dictionary<string, Dictionary<ulong, KeyValuePair<int, float>>> weaponMasteryMap = new()
        {
            {"Sword", PlayerSwordMastery},
            {"Axe", PlayerAxeMastery},
            {"Mace", PlayerMaceMastery},
            {"Spear", PlayerSpearMastery},
            {"Crossbow", PlayerCrossbowMastery},
            {"GreatSword", PlayerGreatSwordMastery},
            {"Slashers", PlayerSlashersMastery},
            {"Pistols", PlayerPistolsMastery},
            {"Reaper", PlayerReaperMastery},
            {"Longbow", PlayerLongbowMastery},
            {"Whip", PlayerWhipMastery},
            {"Unarmed", PlayerUnarmedMastery},
        };

        // Generic method to save any type of dictionary.
        public static readonly Dictionary<string, Action> saveActions = new()
        {
            {"Sword", SavePlayerSwordMastery},
            {"Axe", SavePlayerAxeMastery},
            {"Mace", SavePlayerMaceMastery},
            {"Spear", SavePlayerSpearMastery},
            {"Crossbow", SavePlayerCrossbowMastery},
            {"GreatSword", SavePlayerGreatSwordMastery},
            {"Slashers", SavePlayerSlashersMastery},
            {"Pistols", SavePlayerPistolsMastery},
            {"Reaper", SavePlayerReaperMastery},
            {"Longbow", SavePlayerLongbowMastery},
            {"Whip", SavePlayerWhipMastery},
            {"Unarmed", SavePlayerUnarmedMastery},
            // Add other mastery types as needed
        };

        public static void LoadData<T>(ref Dictionary<ulong, T> dataStructure, string key)
        {
            string path = filePaths[key];
            if (!File.Exists(path))
            {
                // If the file does not exist, create a new empty file to avoid errors on initial load.
                File.Create(path).Dispose();
                dataStructure = []; // Initialize as empty if file does not exist.
                Core.Log.LogInfo($"{key} file created as it did not exist.");
                return;
            }

            try
            {
                string json = File.ReadAllText(path);
                var data = JsonSerializer.Deserialize<Dictionary<ulong, T>>(json, prettyJsonOptions);
                dataStructure = data ?? []; // Ensure non-null assignment.
                Core.Log.LogInfo($"{key} data loaded successfully.");
            }
            catch (IOException ex)
            {
                Core.Log.LogError($"Error reading {key} data from file: {ex.Message}");
                dataStructure = []; // Provide default empty dictionary on error.
            }
            catch (JsonException ex)
            {
                Core.Log.LogError($"JSON deserialization error when loading {key} data: {ex.Message}");
                dataStructure = []; // Provide default empty dictionary on error.
            }
        }

        public static void LoadPlayerExperience() => LoadData(ref playerExperience, "Experience");

        public static void LoadPlayerPrestige() => LoadData(ref playerPrestige, "Prestige");

        public static void LoadPlayerBools() => LoadData(ref playerBools, "PlayerBools");

        public static void LoadPlayerWoodcutting() => LoadData(ref playerWoodcutting, "Woodcutting");

        public static void LoadPlayerMining() => LoadData(ref playerMining, "Mining");

        public static void LoadPlayerFishing() => LoadData(ref playerFishing, "Fishing");

        public static void LoadPlayerBlacksmithing() => LoadData(ref playerBlacksmithing, "Blacksmithing");

        public static void LoadPlayerTailoring() => LoadData(ref playerTailoring, "Tailoring");

        public static void LoadPlayerJewelcrafting() => LoadData(ref playerJewelcrafting, "Jewelcrafting");

        public static void LoadPlayerAlchemy() => LoadData(ref playerAlchemy, "Alchemy");

        public static void LoadPlayerHarvesting() => LoadData(ref playerHarvesting, "Harvesting");

        public static void LoadPlayerSwordMastery() => LoadData(ref playerSwordMastery, "SwordMastery");

        public static void LoadPlayerAxeMastery() => LoadData(ref playerAxeMastery, "AxeMastery");

        public static void LoadPlayerMaceMastery() => LoadData(ref playerMaceMastery, "MaceMastery");

        public static void LoadPlayerSpearMastery() => LoadData(ref playerSpearMastery, "SpearMastery");

        public static void LoadPlayerCrossbowMastery() => LoadData(ref playerCrossbowMastery, "CrossbowMastery");

        public static void LoadPlayerGreatSwordMastery() => LoadData(ref playerGreatSwordMastery, "GreatSwordMastery");

        public static void LoadPlayerSlashersMastery() => LoadData(ref playerSlashersMastery, "SlashersMastery");

        public static void LoadPlayerPistolsMastery() => LoadData(ref playerPistolsMastery, "PistolsMastery");

        public static void LoadPlayerReaperMastery() => LoadData(ref playerReaperMastery, "ReaperMastery");

        public static void LoadPlayerLongbowMastery() => LoadData(ref playerLongbowMastery, "LongbowMastery");

        public static void LoadPlayerWhipMastery() => LoadData(ref playerWhipMastery, "WhipMastery");

        public static void LoadPlayerUnarmedMastery() => LoadData(ref playerUnarmedMastery, "UnarmedMastery");

        public static void LoadPlayerSanguimancy() => LoadData(ref playerSanguimancy, "Sanguimancy");

        public static void LoadPlayerEquippedWeapon() => LoadData(ref playerEquippedWeapon, "EquippedWeapon");

        public static void LoadPlayerWeaponChoices() => LoadData(ref playerWeaponChoices, "WeaponChoices");

        public static void LoadPlayerBloodStats() => LoadData(ref playerBloodChoices, "BloodChoices");

        public static void SaveData<T>(Dictionary<ulong, T> data, string key)
        {
            string path = filePaths[key];
            try
            {
                string json = JsonSerializer.Serialize(data, prettyJsonOptions);
                File.WriteAllText(path, json);
                //Core.Log.LogInfo($"{key} data saved successfully.");
            }
            catch (IOException ex)
            {
                Core.Log.LogError($"Failed to write {key} data to file: {ex.Message}");
            }
            catch (JsonException ex)
            {
                Core.Log.LogError($"JSON serialization error when saving {key} data: {ex.Message}");
            }
        }

        public static void SavePlayerExperience() => SaveData(PlayerExperience, "Experience");

        public static void SavePlayerPrestige() => SaveData(PlayerPrestige, "Prestige");

        public static void SavePlayerBools() => SaveData(PlayerBools, "PlayerBools");

        public static void SavePlayerWoodcutting() => SaveData(PlayerWoodcutting, "Woodcutting");

        public static void SavePlayerMining() => SaveData(PlayerMining, "Mining");

        public static void SavePlayerFishing() => SaveData(PlayerFishing, "Fishing");

        public static void SavePlayerBlacksmithing() => SaveData(PlayerBlacksmithing, "Blacksmithing");

        public static void SavePlayerTailoring() => SaveData(PlayerTailoring, "Tailoring");

        public static void SavePlayerJewelcrafting() => SaveData(PlayerJewelcrafting, "Jewelcrafting");

        public static void SavePlayerAlchemy() => SaveData(PlayerAlchemy, "Alchemy");

        public static void SavePlayerHarvesting() => SaveData(PlayerHarvesting, "Harvesting");

        public static void SavePlayerSwordMastery() => SaveData(PlayerSwordMastery, "SwordMastery");

        public static void SavePlayerAxeMastery() => SaveData(PlayerAxeMastery, "AxeMastery");

        public static void SavePlayerMaceMastery() => SaveData(PlayerMaceMastery, "MaceMastery");

        public static void SavePlayerSpearMastery() => SaveData(PlayerSpearMastery, "SpearMastery");

        public static void SavePlayerCrossbowMastery() => SaveData(PlayerCrossbowMastery, "CrossbowMastery");

        public static void SavePlayerGreatSwordMastery() => SaveData(PlayerGreatSwordMastery, "GreatSwordMastery");

        public static void SavePlayerSlashersMastery() => SaveData(PlayerSlashersMastery, "SlashersMastery");

        public static void SavePlayerPistolsMastery() => SaveData(PlayerPistolsMastery, "PistolsMastery");

        public static void SavePlayerReaperMastery() => SaveData(PlayerReaperMastery, "ReaperMastery");

        public static void SavePlayerLongbowMastery() => SaveData(PlayerLongbowMastery, "LongbowMastery");

        public static void SavePlayerWhipMastery() => SaveData(PlayerWhipMastery, "WhipMastery");

        public static void SavePlayerUnarmedMastery() => SaveData(PlayerUnarmedMastery, "UnarmedMastery");

        public static void SavePlayerSanguimancy() => SaveData(PlayerSanguimancy, "Sanguimancy");

        public static void SavePlayerEquippedWeapon() => SaveData(PlayerEquippedWeapon, "EquippedWeapon");

        public static void SavePlayerWeaponChoices() => SaveData(PlayerWeaponChoices, "WeaponChoices");

        public static void SavePlayerBloodChoices() => SaveData(PlayerBloodChoices, "BloodChoices");
    }

    public class JsonFiles
    {
        public static readonly string PlayerExperienceJson = Path.Combine(Plugin.ConfigPath, "player_experience.json");
        public static readonly string PlayerPrestigeJson = Path.Combine(Plugin.ConfigPath, "player_prestige.json");
        public static readonly string PlayerBoolsJson = Path.Combine(Plugin.ConfigPath, "player_bools.json");
        public static readonly string PlayerWoodcuttingJson = Path.Combine(Plugin.ConfigPath, "player_woodcutting.json");
        public static readonly string PlayerMiningJson = Path.Combine(Plugin.ConfigPath, "player_mining.json");
        public static readonly string PlayerFishingJson = Path.Combine(Plugin.ConfigPath, "player_fishing.json");
        public static readonly string PlayerBlacksmithingJson = Path.Combine(Plugin.ConfigPath, "player_blacksmithing.json");
        public static readonly string PlayerTailoringJson = Path.Combine(Plugin.ConfigPath, "player_tailoring.json");
        public static readonly string PlayerJewelcraftingJson = Path.Combine(Plugin.ConfigPath, "player_jewelcrafting.json");
        public static readonly string PlayerAlchemyJson = Path.Combine(Plugin.ConfigPath, "player_alchemy.json");
        public static readonly string PlayerHarvestingJson = Path.Combine(Plugin.ConfigPath, "player_harvesting.json");
        public static readonly string PlayerSwordMasteryJson = Path.Combine(Plugin.ConfigPath, "player_sword.json");
        public static readonly string PlayerAxeMasteryJson = Path.Combine(Plugin.ConfigPath, "player_axe.json");
        public static readonly string PlayerMaceMasteryJson = Path.Combine(Plugin.ConfigPath, "player_mace.json");
        public static readonly string PlayerSpearMasteryJson = Path.Combine(Plugin.ConfigPath, "player_spear.json");
        public static readonly string PlayerCrossbowMasteryJson = Path.Combine(Plugin.ConfigPath, "player_crossbow.json");
        public static readonly string PlayerGreatSwordMastery = Path.Combine(Plugin.ConfigPath, "player_greatsword.json");
        public static readonly string PlayerSlashersMasteryJson = Path.Combine(Plugin.ConfigPath, "player_slashers.json");
        public static readonly string PlayerPistolsMasteryJson = Path.Combine(Plugin.ConfigPath, "player_pistols.json");
        public static readonly string PlayerReaperMastery = Path.Combine(Plugin.ConfigPath, "player_reaper.json");
        public static readonly string PlayerLongbowMasteryJson = Path.Combine(Plugin.ConfigPath, "player_longbow.json");
        public static readonly string PlayerUnarmedMasteryJson = Path.Combine(Plugin.ConfigPath, "player_unarmed.json");
        public static readonly string PlayerWhipMasteryJson = Path.Combine(Plugin.ConfigPath, "player_whip.json");
        public static readonly string PlayerSanguimancyJson = Path.Combine(Plugin.ConfigPath, "player_sanguimancy.json");

        //public static readonly string PlayerWeaponStatsJson = Path.Combine(Plugin.ConfigPath, "player_weapon_stats.json");
        public static readonly string PlayerEquippedWeaponJson = Path.Combine(Plugin.ConfigPath, "player_equipped_weapon.json");

        public static readonly string PlayerWeaponChoicesJson = Path.Combine(Plugin.ConfigPath, "player_weapon_choices.json");
        public static readonly string PlayerBloodChoicesJson = Path.Combine(Plugin.ConfigPath, "player_blood_choices.json");
    }
}