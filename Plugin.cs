using System.Runtime.Remoting.Contexts;
using BepInEx;
using BepInEx.Configuration;
using Colossal.PSI.Common;
using Game.Achievements;
using HarmonyLib;

namespace AE
{
    [BepInPlugin("com.dvize.AchievementEnabler", "dvize.AchievementEnabler", "1.0.0")]
    public class AEPlugin : BaseUnityPlugin
    {
        private void Awake()
        {
            Logger.LogInfo("Patching Achievements");

            Harmony harmony = new Harmony("com.dvize.AchievementEnabler");
            harmony.PatchAll();

        }

    }

    [HarmonyPatch(typeof(AchievementTriggerSystem))]
    [HarmonyPatch("OnGameLoaded")]
    internal class EnableAchievementsPatch
    {
        private static void Postfix(ref AchievementTriggerSystem __instance, Context serializationContext)
        {
            //use accesstools to invoke private method Reset from instance
            AccessTools.Method(typeof(AchievementTriggerSystem), "Reset").Invoke(__instance, null);
            PlatformManager.instance.achievementsEnabled = true;
        }
    }
}
