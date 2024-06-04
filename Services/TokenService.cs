using BepInEx.Unity.IL2CPP.Utils.Collections;
using Il2CppInterop.Runtime;
using ProjectM;
using ProjectM.Network;
using ProjectM.Physics;
using System.Collections;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Tokens.Services
{
    internal class TokenService
    {
        static readonly int intervalMinutes = Plugin.UpdateInterval;
        static readonly int tokensPerMinute = Plugin.TokensPerMinute;
        static readonly bool tokenSystem = Plugin.TokenSystem;

        EntityQuery userQuery;

        readonly IgnorePhysicsDebugSystem tokenMonoBehaviour;

        public TokenService()
        {
            var queryDesc = new EntityQueryDesc
            {
                All = new ComponentType[] { new(Il2CppType.Of<User>(), ComponentType.AccessMode.ReadOnly) },
                Options = EntityQueryOptions.Default
            };

            userQuery = Core.EntityManager.CreateEntityQuery(queryDesc);

            tokenMonoBehaviour = (new GameObject("TokenService")).AddComponent<IgnorePhysicsDebugSystem>();
            if (tokenSystem) tokenMonoBehaviour.StartCoroutine(UpdateLoop().WrapToIl2Cpp());
        }

        // Iterate through each entity in the query
        IEnumerator UpdateLoop()
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(intervalMinutes * 60);
            
            while (true)
            {
                yield return waitForSeconds;

                NativeArray<Entity> userEntities = userQuery.ToEntityArray(Allocator.TempJob);
                try
                {
                    foreach (Entity userEntity in userEntities)
                    {
                        User user = userEntity.Read<User>();
                        if (!user.IsConnected) continue;
                        ulong steamId = user.PlatformId;
                        if (Core.DataStructures.PlayerTokens.TryGetValue(steamId, out var tokenData))
                        {
                            TimeSpan timeOnline = DateTime.Now - tokenData.Value.Key;
                            tokenData = new(tokenData.Key + timeOnline.Minutes * tokensPerMinute, new(DateTime.Now, tokenData.Value.Value));
                            Core.DataStructures.PlayerTokens[steamId] = tokenData;
                            Core.DataStructures.SavePlayerTokens();
                        }
                    }
                }
                finally
                {
                    userEntities.Dispose();
                }
            }
        }
    }
}