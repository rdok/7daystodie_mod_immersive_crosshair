using System;
using GearsAPI.Settings;
using GearsAPI.Settings.Global;
using GearsAPI.Settings.World;

namespace ImmersiveCrosshair.Harmony
{
    public class Main : IModApi, IGearsModApi
    {
        public static IGearsMod gearsMod;
        private static ILogger _logger;
        private static ISettings _settings;

        public Main()
        {
            Services.Initialise();
            _settings = Services.Get<ISettings>();
            _logger = Services.Get<ILogger>();
        }

        public void InitMod(IGearsMod modInstance)
        {
            gearsMod = modInstance;
        }

        public void OnGlobalSettingsLoaded(IModGlobalSettings modGlobalSettings)
        {
            var generalTab = modGlobalSettings.GetTab("General");
            var category = generalTab.GetCategory("ImmersiveCrosshair");

            var toolsSetting = category.GetSetting("Tools");
            toolsSetting.OnSettingChanged += ToolsSettingChanged;

            var meleeWeapons = category.GetSetting("MeleeWeapons");
            meleeWeapons.OnSettingChanged += MeleeWeaponsSettingChanged;

            var bowsSetting = category.GetSetting("Bows");
            bowsSetting.OnSettingChanged += BowsSettingChanged;

            var rangedWeaponsSetting = category.GetSetting("RangedWeapons");
            rangedWeaponsSetting.OnSettingChanged += RangedWeaponsSettingChanged;
        }

        public void OnWorldSettingsLoaded(IModWorldSettings worldSettings)
        {
        }

        public void InitMod(Mod modInstance)
        {
            var harmony = new HarmonyLib.Harmony("uk.co.rdok.7daystodie.mod.immersive_crosshair");
            harmony.PatchAll();
        }

        private static void ToolsSettingChanged(IGlobalModSetting globalModSetting, string value)
        {
            _logger.Debug($"setting.Name: {globalModSetting.Name}. New Value: {value}");
            _settings.ToolsSetting = value;
        }

        private static void MeleeWeaponsSettingChanged(IGlobalModSetting globalModSetting, string value)
        {
            _logger.Debug($"setting.Name: {globalModSetting.Name}. New Value: {value}");
            _settings.MeleeWeaponsSetting = value;
        }

        private static void BowsSettingChanged(IGlobalModSetting globalModSetting, string value)
        {
            _logger.Debug($"setting.Name: {globalModSetting.Name}. New Value: {value}");
            _settings.BowsSetting = value;
        }

        private static void RangedWeaponsSettingChanged(IGlobalModSetting globalModSetting, string value)
        {
            _logger.Debug($"setting.Name: {globalModSetting.Name}. New Value: {value}");
            _settings.RangedWeaponsSetting = value;
        }
    }
}