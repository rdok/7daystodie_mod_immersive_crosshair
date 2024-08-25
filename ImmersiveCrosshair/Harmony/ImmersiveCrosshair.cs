using HarmonyLib;
using UniLinq;
using UnityEngine;

namespace ImmersiveCrosshair.Harmony
{
    public class ImmersiveCrosshair
    {
        public const float MinimumInteractableDistance = 2.4f;
        private static ILogger _logger = new Logger();
        public static bool BowWithNoSightsSetting { get; set; }
        public static bool EnabledForToolsSetting { get; set; }
        public static bool EnabledForMeleeSetting { get; set; }

        public static void SetLogger(ILogger logger)
        {
            _logger = logger;
        }

        public static void ApplyPatch(IEntityPlayerLocal entityPlayerLocal)
        {
            if (entityPlayerLocal == null) return;

            var playerUI = entityPlayerLocal.playerUI;
            var hud = playerUI.GetComponentInChildren<IGuiWdwInGameHUD>();

            if (hud == null) return;

            var windowManager = playerUI.windowManager;
            var interactionPromptID = XUiC_InteractionPrompt.ID;
            var isInteractionPromptOpen = !string.IsNullOrEmpty(interactionPromptID)
                                          && windowManager.IsWindowOpen(interactionPromptID);
            if (isInteractionPromptOpen)
            {
                hud.showCrosshair = true;
                return;
            }

            var holdingItem = entityPlayerLocal.inventory.holdingItemItemValue;
            var actions = holdingItem?.ItemClass?.Actions;

            if (!entityPlayerLocal.bFirstPersonView)
            {
                hud.showCrosshair = true;
                return;
            }

            var holdsBowWithNoSights = actions?.Any(action => action.HasBowWithNoSights) ?? false;
            if (BowWithNoSightsSetting && holdsBowWithNoSights)
            {
                hud.showCrosshair = true;
                return;
            }

            var holdsMelee = actions?.Any(action => action.IsMelee) ?? false;
            if (holdsMelee && EnabledForMeleeSetting)
            {
                hud.showCrosshair = true;
                return;
            }
            
            var holdsTool = actions?.Any(action => action.IsTool) ?? false;
            
            if (!holdsTool)
            {
                hud.showCrosshair = false;
                return;
            }
            
            if (EnabledForToolsSetting)
            {
                hud.showCrosshair = true;
                return;
            }

            var hitInfo = entityPlayerLocal.HitInfo;

            if (hitInfo == null) return;

            var hasInteractable =
                hitInfo.bHitValid && Mathf.Sqrt(hitInfo.hit.distanceSq) <= MinimumInteractableDistance;

            hud.showCrosshair = hasInteractable;
        }


        [HarmonyPatch(typeof(EntityPlayerLocal), "Update")]
        public static class Update
        {
            public static void Postfix(EntityPlayerLocal __instance)
            {
                var entityPlayerLocalWrapper = new EntityPlayerLocalWrapper(__instance);
                ApplyPatch(entityPlayerLocalWrapper);
            }
        }
    }
}