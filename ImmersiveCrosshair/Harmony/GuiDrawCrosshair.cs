namespace ImmersiveCrosshair.Harmony
{
    public class GuiDrawCrosshair : IGuiDrawCrosshair
    {
        private readonly ILogger _logger;
        private readonly ISettings _settings;

        public GuiDrawCrosshair(ILogger logger, ISettings settings)
        {
            _logger = logger;
            _settings = settings;
        }

        public void Update(
            IEntityPlayerLocal entityPlayerLocal,
            IItemAction itemAction,
            IWorldRayHitInfo worldRayHitInfo
        )
        {
            if (entityPlayerLocal == null) return;
            if (itemAction == null) return;

            var playerUI = entityPlayerLocal.playerUI;
            var hud = playerUI.GetComponentInChildren<INGuiWdwInGameHUD>();
            if (hud == null) return;

            _logger.Debug("GuiDrawCrosshair.Update: start");

            if (!entityPlayerLocal.bFirstPersonView)
            {
                hud.showCrosshair = true;
                return;
            }

            var windowManager = entityPlayerLocal.playerUI.windowManager;
            var interactionPromptID = XUiC_InteractionPrompt.ID;
            var isInteractionPromptOpen =
                !string.IsNullOrEmpty(interactionPromptID)
                && windowManager.IsWindowOpen(interactionPromptID);

            if (isInteractionPromptOpen)
            {
                _logger.Debug("IntegrationPromptOpen: showing crosshair");
                hud.showCrosshair = true;
                return;
            }

            if (itemAction.IsBowWithNoSights)
            {
                DrawBowCrosshair(hud);
                return;
            }

            if (worldRayHitInfo == null) return;

            if (itemAction.IsTool)
            {
                DrawToolCrosshair(worldRayHitInfo, hud);
                return;
            }

            if (itemAction.IsMelee)
            {
                DrawMeleeCrosshair(worldRayHitInfo, hud);
                return;
            }

            if (_settings.RangedWeaponsSetting == "off")
            {
                hud.showCrosshair = false;
                return;
            }

            _logger.Debug("Fallback to ranged weapons.");
            hud.showCrosshair = true;
        }

        private void DrawBowCrosshair(INGuiWdwInGameHUD hud)
        {
            _logger.Debug($"_settings.BowsSetting: {_settings.BowsSetting}");

            if (_settings.BowsSetting == "off")
            {
                hud.showCrosshair = false;
                return;
            }

            if (_settings.BowsSetting == "static")
            {
                hud.showCrosshair = true;
            }
        }

        private void DrawMeleeCrosshair(
            IWorldRayHitInfo worldRayHitInfo, INGuiWdwInGameHUD hud
        )
        {
            _logger.Debug(
                $"_settings.MeleeWeaponsSetting: {_settings.MeleeWeaponsSetting}"
            );

            if (_settings.MeleeWeaponsSetting == "off")
            {
                hud.showCrosshair = false;
                return;
            }

            if (_settings.MeleeWeaponsSetting == "static")
            {
                hud.showCrosshair = true;
                return;
            }

            hud.showCrosshair = worldRayHitInfo.bHitValid;
        }

        private void DrawToolCrosshair(
            IWorldRayHitInfo worldRayHitInfo, INGuiWdwInGameHUD hud
        )
        {
            _logger.Debug($"_settings.ToolsSetting: {_settings.ToolsSetting}");

            if (_settings.ToolsSetting == "off")
            {
                hud.showCrosshair = false;
                return;
            }

            if (_settings.ToolsSetting == "static")
            {
                hud.showCrosshair = true;
                return;
            }

            hud.showCrosshair = worldRayHitInfo.bHitValid;
        }
    }
}