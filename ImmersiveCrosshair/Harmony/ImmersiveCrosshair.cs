using HarmonyLib;
using UniLinq;
using UnityEngine;

namespace ImmersiveCrosshair.Harmony
{
    public class ImmersiveCrosshair
    {
        private static ILogger _logger = new Logger();

        public static void SetLogger(ILogger logger)
        {
            _logger = logger;
        }

        public static void ApplyPatch(IEntityPlayerLocal entityPlayerLocal)
        {
            _logger.Info("ApplyPatch: Starting patch application.");

            if (entityPlayerLocal == null)
            {
                _logger.Info("ApplyPatch: entityPlayerLocal is null. Exiting.");
                return;
            }

            _logger.Info("ApplyPatch: Retrieving playerUI from entityPlayerLocal.");
            var playerUI = entityPlayerLocal.playerUI;

            _logger.Info("ApplyPatch: Attempting to retrieve IGuiWdwInGameHUD from playerUI.");
            var hud = playerUI.GetComponentInChildren<IGuiWdwInGameHUD>();

            if (hud == null)
            {
                _logger.Info("ApplyPatch: IGuiWdwInGameHUD is null. Exiting.");
                return;
            }

            _logger.Info("ApplyPatch: IGuiWdwInGameHUD retrieved successfully.");


            _logger.Info("ApplyPatch: Checking currently held item.");
            var holdingItem = entityPlayerLocal.inventory.holdingItemItemValue;
            var actions = holdingItem?.ItemClass?.Actions;

            var holdingRanged = actions?.Any(action => action.IsRanged) ?? false;
            if (holdingRanged)
            {
                _logger.Info("ApplyPatch: Holding a ranged weapon with iron sights. Disabling crosshair.");
                hud.showCrosshair = false;
                return;
            }

            var holdsInteractable = actions?.Any(
                action => action.IsHarvest || action.IsRepair || action.IsSalvage || action.IsBareHands
            ) ?? false;

            if (!holdsInteractable)
            {
                _logger.Info(
                    "ApplyPatch: Holding item is neither a harvest, repair, or terrain tool. Hiding crosshair.");
                hud.showCrosshair = false;
                return;
            }

            _logger.Info("ApplyPatch: Retrieving hit information from entityPlayerLocal.");
            var hitInfo = entityPlayerLocal.HitInfo;

            if (hitInfo == null)
            {
                _logger.Info("ApplyPatch: HitInfo is null. Exiting.");
                return;
            }

            _logger.Info(
                $"ApplyPatch: HitInfo is valid: {hitInfo.bHitValid}, distance squared: {hitInfo.hit.distanceSq}.");

            var hasInteractable = hitInfo.bHitValid && Mathf.Sqrt(hitInfo.hit.distanceSq) <= 2.3f;

            _logger.Info(hasInteractable
                ? "ApplyPatch: Interactable object detected within range. Enabling crosshair."
                : "ApplyPatch: No interactable object within range. Disabling crosshair.");

            hud.showCrosshair = hasInteractable;

            _logger.Info("ApplyPatch: Patch application completed.");
        }


        public class Init : IModApi
        {
            public void InitMod(Mod modInstance)
            {
                var type = GetType();
                var message = type.ToString();
                _logger.Info("Loading Patch: " + message);
                var harmony = new HarmonyLib.Harmony(message);
                harmony.PatchAll();
            }
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