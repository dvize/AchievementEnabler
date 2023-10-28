﻿using System.Runtime.Remoting.Contexts;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.Mono;
using Colossal.PSI.Common;
using Game.Achievements;
using HarmonyLib;

namespace AE
{
    [BepInPlugin("com.dvize.AchievementEnabler", "dvize.AchievementEnabler", "1.0.0")]
    public class AILimitPlugin : BaseUnityPlugin
    {
        public static ConfigEntry<bool> PluginEnabled;

        private void Awake()
        {
            PluginEnabled = Config.Bind(
                "Main Settings",
                "Plugin on/off",
                true,
                "");

            Logger.LogInfo("Patching Achievements");

            Harmony harmony = new Harmony("com.dvize.AchievementEnabler");
            harmony.PatchAll();

        }

    }

    [HarmonyPatch(typeof(AchievementTriggerSystem))]
    [HarmonyPatch("OnGameLoaded")]
    class EnableAchievementsPatch
    {
        static void Postfix(ref AchievementTriggerSystem __instance, Context serializationContext)
        {
            //use accesstools to invoke private method Reset from instance
            AccessTools.Method(typeof(AchievementTriggerSystem), "Reset").Invoke(__instance, null);
            PlatformManager.instance.achievementsEnabled = true;
        }
    }
}