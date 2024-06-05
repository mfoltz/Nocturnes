using ProjectM;
using Stunlock.Core;
using Unity.Entities;
using VampireCommandFramework;

namespace Nocturnes.Commands
{
    public static class NocturneCommands
    {
        static readonly PrefabGUID tokenReward = new(Plugin.TokenReward);
        static readonly int tokenRewardRatio = Plugin.TokenRewardRatio;
        static readonly int tokensPerMinute = Plugin.TokensPerMinute;

        [Command(name: ".redeemNocturnes", shortHand: ".rn", adminOnly: false, usage: ".rn", description: "Redeems Nocturnes.")]
        public static void RedeemNocturnesCommand(ChatCommandContext ctx)
        {
            if (!Plugin.TokenSystem)
            {
                ctx.Reply("<color=#CBC3E3>Nocturnes</color> are currently disabled.");
                return;
            }

            ulong steamId = ctx.Event.User.PlatformId;

            if (Core.DataStructures.PlayerTokens.TryGetValue(steamId, out var tokenData))
            {
                if (tokenData.Tokens < tokenRewardRatio)
                {
                    ctx.Reply("You don't have enough <color=#CBC3E3>Nocturnes</color> to redeem.");
                    return;
                }

                int rewards = tokenData.Tokens / tokenRewardRatio;
                int cost = rewards * tokenRewardRatio;
                
                if (Core.ServerGameManager.TryAddInventoryItem(ctx.Event.SenderCharacterEntity, tokenReward, rewards))
                {
                    tokenData = new(tokenData.Tokens - cost, tokenData.TimeData);
                    Core.DataStructures.PlayerTokens[steamId] = tokenData;
                    Core.DataStructures.SavePlayerTokens();
                    ctx.Reply($"You've received <color=#00FFFF>{Core.ExtractName(tokenReward.LookupName())}</color>x<color=white>{rewards}</color> for redeeming <color=#FFC0CB>{cost}</color> <color=#CBC3E3>Nocturnes</color>!");
                }
                else
                {
                    EntityCommandBuffer entityCommandBuffer = Core.EntityCommandBufferSystem.CreateCommandBuffer();
                    if (InventoryUtilitiesServer.TryDropItem(Core.EntityManager, entityCommandBuffer, Core.GameDataSystem.ItemHashLookupMap, ctx.Event.SenderCharacterEntity, tokenReward, rewards))
                    {
                        tokenData = new(tokenData.Tokens - cost, tokenData.TimeData);
                        Core.DataStructures.PlayerTokens[steamId] = tokenData;
                        Core.DataStructures.SavePlayerTokens();
                        ctx.Reply($"You've received <color=#00FFFF>{Core.ExtractName(tokenReward.LookupName())}</color>x<color=white>{rewards}</color> for redeeming <color=#FFC0CB>{cost}</color> <color=#CBC3E3>Nocturnes</color>! It dropped on the ground because your inventory was full.");
                    }
                }
            }
        }
        [Command(name: ".getNocturnes", shortHand: "get n", adminOnly: false, usage: ".get n", description: "Shows earned Nocturnes, also updates them.")]
        public static void GetNocturnesCommand(ChatCommandContext ctx)
        {
            if (!Plugin.TokenSystem)
            {
                ctx.Reply("<color=#CBC3E3>Nocturnes</color> are currently disabled.");
                return;
            }

            ulong steamId = ctx.Event.User.PlatformId;

            if (Core.DataStructures.PlayerTokens.TryGetValue(steamId, out var tokenData))
            {
                TimeSpan timeOnline = DateTime.Now - tokenData.TimeData.Start;
                tokenData = new(tokenData.Tokens + timeOnline.Minutes * tokensPerMinute, new(DateTime.Now, tokenData.TimeData.DailyLogin));
                Core.DataStructures.PlayerTokens[steamId] = tokenData;
                Core.DataStructures.SavePlayerTokens();
                ctx.Reply($"You have <color=#FFC0CB>{tokenData.Tokens}</color> <color=#CBC3E3>Nocturnes</color>.");
            }
        }
    }
}