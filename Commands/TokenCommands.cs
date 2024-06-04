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
                if (tokenData.Key < tokenRewardRatio)
                {
                    ctx.Reply("You don't have enough tokens to redeem.");
                    return;
                }

                int rewards = tokenData.Key / tokenRewardRatio;
                int cost = rewards * tokenRewardRatio;
                
                if (Core.ServerGameManager.TryAddInventoryItem(ctx.Event.User.LocalCharacter._Entity, tokenReward, rewards))
                {
                    tokenData = new(tokenData.Key - cost, tokenData.Value);
                    Core.DataStructures.PlayerTokens[steamId] = tokenData;
                    Core.DataStructures.SavePlayerTokens();
                    string unformatted = tokenReward.LookupName();
                    ctx.Reply($"You've received <color=#00FFFF>{unformatted[..unformatted.IndexOf(' ')]}</color> for redeeming <color=<color=#FFC0CB>>{tokenData.Key}</color> tokens!");
                }
                else
                {
                    tokenData = new(tokenData.Key - cost, tokenData.Value);
                    Core.DataStructures.PlayerTokens[steamId] = tokenData;
                    Core.DataStructures.SavePlayerTokens();
                    Core.ServerGameManager.CreateDroppedItemEntity(ctx.Event.User.LocalCharacter._Entity, tokenReward, rewards);
                    string unformatted = tokenReward.LookupName();
                    ctx.Reply($"You've received <color=#00FFFF>{unformatted[..unformatted.IndexOf(' ')]}</color> for redeeming <color=<color=#FFC0CB>>{tokenData.Key}</color> tokens! It dropped on the ground because your inventory was full.");
                }
            }
        }
        [Command(name: ".getTokens", shortHand: "gt", adminOnly: false, usage: ".gt", description: "Shows earned tokens, also updates them.")]
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
                TimeSpan timeOnline = DateTime.Now - tokenData.Value.Key;
                tokenData = new(tokenData.Key + timeOnline.Minutes * tokensPerMinute, new(DateTime.Now, tokenData.Value.Value));
                Core.DataStructures.PlayerTokens[steamId] = tokenData;
                Core.DataStructures.SavePlayerTokens();
                ctx.Reply($"Tokens updated! You have <color=#FFC0CB>{tokenData.Key}</color> tokens.");
            }
        }
    }
}