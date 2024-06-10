using HarmonyLib;
using ProjectM;
using ProjectM.Network;
using Sanguis.Services;
using Stunlock.Core;
using Stunlock.Network;
using Unity.Entities;

namespace Sanguis.Patches;

[HarmonyPatch]
public class ServerBootstrapPatches
{
    static readonly PrefabGUID dailyReward = new(Plugin.DailyReward);
    static readonly int dailyQuantity = Plugin.DailyQuantity;
    static readonly bool dailyLogin = Plugin.DailyLogin;
    static readonly int tokensPerMinute = Plugin.TokensPerMinute;
    static readonly bool tokenSystem = Plugin.TokenSystem;

    [HarmonyPatch(typeof(ServerBootstrapSystem), nameof(ServerBootstrapSystem.OnUserConnected))]
    [HarmonyPostfix]
    static void OnUserConnectedPostix(ServerBootstrapSystem __instance, NetConnectionId netConnectionId)
    {
        int userIndex = __instance._NetEndPointToApprovedUserIndex[netConnectionId];
        ServerBootstrapSystem.ServerClient serverClient = __instance._ApprovedUsersLookup[userIndex];
        Entity userEntity = serverClient.UserEntity;
        User user = __instance.EntityManager.GetComponentData<User>(userEntity);
        ulong steamId = user.PlatformId;
        if (tokenSystem)
        {
            if (!Core.DataStructures.PlayerTokens.TryGetValue(steamId, out var tokenData))
            {
                DateTime firstLogin = DateTime.Now;
                tokenData = new(0, new(firstLogin, firstLogin));
                Core.DataStructures.PlayerTokens.Add(steamId, tokenData);
                Core.DataStructures.SavePlayerTokens();
                if (dailyLogin && Core.EntityManager.Exists(user.LocalCharacter._Entity))
                {
                    if (Core.ServerGameManager.TryAddInventoryItem(user.LocalCharacter._Entity, dailyReward, dailyQuantity))
                    {
                        //string message = $"You've received <color=#00FFFF>{Core.ExtractName(dailyReward.LookupName())}</color>x<color=white>{dailyQuantity}</color> for logging in today!";
                        string message = $"You've received <color=#00FFFF>{SanguisService.dailyReward}</color>x<color=white>{dailyQuantity}</color> for logging in today!";

                        ServerChatUtils.SendSystemMessageToClient(__instance.EntityManager, user, message);
                    }
                    else
                    {
                        InventoryUtilitiesServer.CreateDropItem(Core.EntityManager, user.LocalCharacter._Entity, dailyReward, dailyQuantity, new Entity());
                        //string message = $"You've received <color=#00FFFF>{Core.ExtractName(dailyReward.LookupName())}</color>x<color=white>{dailyQuantity}</color> for logging in today! It dropped on the ground because your inventory was full.";
                        string message = $"You've received <color=#00FFFF>{SanguisService.dailyReward}</color>x<color=white>{dailyQuantity}</color> for logging in today! It dropped on the ground because your inventory was full.";

                        ServerChatUtils.SendSystemMessageToClient(__instance.EntityManager, user, message);
                    }
                }                
            }
            else
            {
                if (dailyLogin && DateTime.Now.Subtract(tokenData.TimeData.DailyLogin).Days >= 1)
                {
                    if (Core.ServerGameManager.TryAddInventoryItem(user.LocalCharacter._Entity, dailyReward, dailyQuantity))
                    {
                        //string message = $"You've received <color=#00FFFF>{Core.ExtractName(dailyReward.LookupName())}</color>x<color=white>{dailyQuantity}</color> for logging in today!";
                        string message = $"You've received <color=#00FFFF>{SanguisService.dailyReward}</color>x<color=white>{dailyQuantity}</color> for logging in today!";

                        ServerChatUtils.SendSystemMessageToClient(__instance.EntityManager, user, message);
                    }
                    else
                    {
                        InventoryUtilitiesServer.CreateDropItem(Core.EntityManager, user.LocalCharacter._Entity, dailyReward, dailyQuantity, new Entity());
                        //string message = $"You've received <color=#00FFFF>{Core.ExtractName(dailyReward.LookupName())}</color>x<color=white>{dailyQuantity}</color> for logging in today! It dropped on the ground because your inventory was full.";
                        string message = $"You've received <color=#00FFFF>{SanguisService.dailyReward}</color>x<color=white>{dailyQuantity}</color> for logging in today!";

                        ServerChatUtils.SendSystemMessageToClient(__instance.EntityManager, user, message);
                    }
                    tokenData = new(tokenData.Tokens, new(tokenData.TimeData.Start, DateTime.Now));
                    Core.DataStructures.PlayerTokens[steamId] = tokenData;
                    Core.DataStructures.SavePlayerTokens();
                }
                tokenData = new(tokenData.Tokens, new(DateTime.Now, tokenData.TimeData.DailyLogin));
                Core.DataStructures.PlayerTokens[steamId] = tokenData;
                Core.DataStructures.SavePlayerTokens();
            }
        }
        
    }

    [HarmonyPatch(typeof(ServerBootstrapSystem), nameof(ServerBootstrapSystem.OnUserDisconnected))]
    [HarmonyPrefix]
    static void OnUserDisconnectedPreix(ServerBootstrapSystem __instance, NetConnectionId netConnectionId)
    {
        int userIndex = __instance._NetEndPointToApprovedUserIndex[netConnectionId];
        ServerBootstrapSystem.ServerClient serverClient = __instance._ApprovedUsersLookup[userIndex];
        Entity userEntity = serverClient.UserEntity;
        User user = __instance.EntityManager.GetComponentData<User>(userEntity);
        ulong steamId = user.PlatformId;

        if (tokenSystem && Core.DataStructures.PlayerTokens.TryGetValue(steamId, out var tokenData))
        {
            TimeSpan timeOnline = DateTime.Now - tokenData.TimeData.Start;
            tokenData = new(tokenData.Tokens + timeOnline.Minutes * tokensPerMinute, new(DateTime.Now, tokenData.TimeData.DailyLogin));
            Core.DataStructures.PlayerTokens[steamId] = tokenData;
            Core.DataStructures.SavePlayerTokens();
        }
    }
}