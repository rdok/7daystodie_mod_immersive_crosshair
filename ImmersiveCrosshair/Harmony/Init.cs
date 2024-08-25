using System;
using GearsAPI.Settings;
using GearsAPI.Settings.Global;
using GearsAPI.Settings.World;

namespace ImmersiveCrosshair.Harmony
{
    public class Init : IModApi, IGearsModApi
    {
        private static readonly ILogger Logger = new Logger();
        public static IGearsMod gearsMod;

        public void InitMod(IGearsMod modInstance)
        {
            gearsMod = modInstance;
        }

        public void OnGlobalSettingsLoaded(IModGlobalSettings modSettings)
        {
            Logger.Info("OnGlobalSettingsLoaded");
            var generalTab = modSettings.GetTab("General");
            var category = generalTab.GetCategory("ImmersiveCrosshair");

            var enabledForBowsWithNoSights = category.GetSetting("EnabledForBowsWithNoSights");
            enabledForBowsWithNoSights.OnSettingChanged += BowsWithNoSightsChanged;
            BowsWithNoSightsChanged(enabledForBowsWithNoSights,
                (enabledForBowsWithNoSights as IGlobalValueSetting)?.CurrentValue);

            var enabledForTools = category.GetSetting("EnabledForTools");
            enabledForTools.OnSettingChanged += EnabledForToolsChanged;
            EnabledForToolsChanged(enabledForTools,
                (enabledForTools as IGlobalValueSetting)?.CurrentValue);
        }

        public void OnWorldSettingsLoaded(IModWorldSettings worldSettings)
        {
        }

        public void InitMod(Mod modInstance)
        {
            var harmony = new HarmonyLib.Harmony("uk.co.rdok.7daystodie.mod.immersive_crosshair");
            harmony.PatchAll();
        }

        private static void BowsWithNoSightsChanged(IGlobalModSetting setting, string value)
        {
            Logger.Info($"setting.Name: {setting.Name}. New Value: {value}");
            bool.TryParse(value, out var bowsWithNoSightsSetting);
            ImmersiveCrosshair.BowWithNoSightsSetting = bowsWithNoSightsSetting;
        }

        private static void EnabledForToolsChanged(IGlobalModSetting setting, string value)
        {
            Logger.Info($"setting.Name: {setting.Name}. New Value: {value}");
            bool.TryParse(value, out var toolsSetting);
            ImmersiveCrosshair.EnabledForToolsSetting = toolsSetting;
        }
    }
}