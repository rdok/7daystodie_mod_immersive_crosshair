namespace ImmersiveCrosshair.Harmony
{
    public interface ISettings
    {
        string ToolsSetting { get; set; }
        string MeleeWeaponsSetting { get; set; }
        string BowsSetting { get; set; }
        string RangedWeaponsSetting { get; set; }

        string GetString(string tab, string category, string settingName);
    }
}