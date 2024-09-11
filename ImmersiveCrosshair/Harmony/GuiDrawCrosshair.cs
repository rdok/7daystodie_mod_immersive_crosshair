namespace ImmersiveCrosshair.Harmony
{
    public class GuiDrawCrosshair : IGuiDrawCrosshair
    {
        private readonly ILogger _logger;
        private readonly IModSettings _modSettings;

        public GuiDrawCrosshair(ILogger logger, IModSettings modSettings)
        {
            _logger = logger;
            _modSettings = modSettings;
        }

        public void Update(
            IEntityPlayerLocal entityPlayerLocal,
            IItemAction itemAction,
            IWorldRayHitInfo worldRayHitInfo
        )
        {
            if (entityPlayerLocal == null) return;
            if (itemAction == null) return;
            if (worldRayHitInfo == null) return;

            var playerUI = entityPlayerLocal.playerUI;
            var hud = playerUI.GetComponentInChildren<INGuiWdwInGameHUD>();
            if (hud == null) return;

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
                hud.showCrosshair = true;
                return;
            }

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

            if (itemAction.IsBowWithNoSights)
            {
                DrawBowCrosshair(worldRayHitInfo, hud);
                return;
            }

            if (_modSettings.RangedWeaponsSetting == "off")
            {
                hud.showCrosshair = false;
                return;
            }

            hud.showCrosshair = true;
        }

        private void DrawBowCrosshair(
            IWorldRayHitInfo worldRayHitInfo, INGuiWdwInGameHUD hud
        )
        {
            if (_modSettings.BowsSetting == "off")
            {
                hud.showCrosshair = false;
                return;
            }

            if (_modSettings.BowsSetting == "static")
            {
                hud.showCrosshair = true;
                return;
            }

            hud.showCrosshair = worldRayHitInfo.bHitValid;
        }

        private void DrawMeleeCrosshair(
            IWorldRayHitInfo worldRayHitInfo, INGuiWdwInGameHUD hud
        )
        {
            if (_modSettings.MeleeWeaponsSetting == "off")
            {
                hud.showCrosshair = false;
                return;
            }

            if (_modSettings.MeleeWeaponsSetting == "static")
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
            if (_modSettings.ToolsSetting == "off")
            {
                hud.showCrosshair = false;
                return;
            }

            if (_modSettings.ToolsSetting == "static")
            {
                hud.showCrosshair = true;
                return;
            }

            hud.showCrosshair = worldRayHitInfo.bHitValid;
        }
    }
}