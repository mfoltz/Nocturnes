using HarmonyLib;
using ProjectM;
using Tokens;


namespace Tokens.Patches;

[HarmonyPatch(typeof(SpawnTeamSystem_OnPersistenceLoad), nameof(SpawnTeamSystem_OnPersistenceLoad.OnUpdate))]
public static class InitializationPatch
{
	[HarmonyPostfix]
	public static void OnUpdatePostfix()
	{
		Core.Initialize();
	}
}
