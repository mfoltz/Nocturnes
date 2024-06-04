using Stunlock.Core;
using VampireCommandFramework;

namespace Tokens.Commands
{
    public static class TokenCommands
    {
        static readonly PrefabGUID tokenReward = new(Plugin.TokenReward);
        static readonly int tokenRewardRatio = Plugin.TokenRewardRatio;
        static readonly int tokensPerMinute = Plugin.TokensPerMinute;

        [Command(name: ".redeemTokens", shortHand: "tokens", adminOnly: false, usage: ".tokens", description: "Redeems tokens for rewards.")]
        public static void TokenRedeemCommand(ChatCommandContext ctx)
        {
            if (!Plugin.TokenSystem)
            {
                ctx.Reply("The token system is currently disabled.");
                return;
            }

            ulong steamId = ctx.Event.User.PlatformId;

            if (Core.DataStructures.PlayerTokens.TryGetValue(steamId, out var tokenData))
            {
                if (tokenData.Tokens < tokenRewardRatio)
                {
                    ctx.Reply("You don't have enough tokens to redeem.");
                    return;
                }

                int rewards = tokenData.Tokens / tokenRewardRatio;
                int cost = rewards * tokenRewardRatio;
                
                if (Core.ServerGameManager.TryAddInventoryItem(ctx.Event.User.LocalCharacter._Entity, tokenReward, rewards))
                {
                    tokenData = new(tokenData.Tokens - cost, tokenData.TimeData);
                    Core.DataStructures.PlayerTokens[steamId] = tokenData;
                    Core.DataStructures.SavePlayerTokens();
                    string unformatted = tokenReward.LookupName();
                    ctx.Reply($"You've received <color=#00FFFF>{unformatted[..unformatted.IndexOf(' ')]}</color>x<color=white>{rewards}</color> for redeeming <color=#FFC0CB>>{tokenData.Tokens}</color> tokens!");
                }
                else
                {
                    tokenData = new(tokenData.Tokens - cost, tokenData.TimeData);
                    Core.DataStructures.PlayerTokens[steamId] = tokenData;
                    Core.DataStructures.SavePlayerTokens();
                    Core.ServerGameManager.CreateDroppedItemEntity(ctx.Event.User.LocalCharacter._Entity, tokenReward, rewards);
                    string unformatted = tokenReward.LookupName();
                    ctx.Reply($"You've received <color=#00FFFF>{unformatted[..unformatted.IndexOf(' ')]}</color>x<color=white>{rewards}</color> for redeeming <color=#FFC0CB>>{tokenData.Tokens}</color> tokens! It dropped on the ground because your inventory was full.");
                }
            }
        }
        [Command(name: ".getTokens", shortHand: "get t", adminOnly: false, usage: ".get t", description: "Shows earned tokens, also updates them.")]
        public static void GetTokensCommand(ChatCommandContext ctx)
        {
            if (!Plugin.TokenSystem)
            {
                ctx.Reply("The token system is currently disabled.");
                return;
            }

            ulong steamId = ctx.Event.User.PlatformId;

            if (Core.DataStructures.PlayerTokens.TryGetValue(steamId, out var tokenData))
            {
                TimeSpan timeOnline = DateTime.Now - tokenData.TimeData.Start;
                tokenData = new(tokenData.Tokens + timeOnline.Minutes * tokensPerMinute, new(DateTime.Now, tokenData.TimeData.DailyLogin));
                Core.DataStructures.PlayerTokens[steamId] = tokenData;
                Core.DataStructures.SavePlayerTokens();
                ctx.Reply($"Tokens updated! You have <color=#FFC0CB>{tokenData.Tokens}</color> tokens.");
            }
        }
    }
}