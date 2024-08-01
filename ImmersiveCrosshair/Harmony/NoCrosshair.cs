using HarmonyLib;
using UniLinq;
using UnityEngine;

public class ImmersiveCrosshair
{
    public class Init : IModApi
    {
        public void InitMod(Mod modInstance)
        {
            var name = modInstance.GetType().ToString();
            Log.Out("Loading Patch: " + name);
            var harmony = new Harmony(name);
            harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(EntityPlayerLocal), "Update")]
    public static class Update
    {
        /**
         * Double underscores for __instance are required by Harmony to inject the class instance.
         *
         * https://harmony.pardeike.net/index.html
         */
        public static void Postfix(EntityPlayerLocal __instance)
        {
            if (__instance == null) return;

            var hud = __instance.playerUI.GetComponentInChildren<NGuiWdwInGameHUD>();
            if (hud == null) return;

            var holdingItem = __instance.inventory.holdingItemItemValue;
            var holdingRangedWeapon =
                holdingItem?.ItemClass?.Actions?.Any(action => action is ItemActionRanged) ?? false;

            if (holdingRangedWeapon)
            {
                hud.showCrosshair = true;
                return;
            }

            var hitInfo = __instance.HitInfo;

            if (hitInfo == null) return;

            var hasInteractable = hitInfo.bHitValid && Mathf.Sqrt(hitInfo.hit.distanceSq) <= 2.3f;

            hud.showCrosshair = hasInteractable;
        }
    }
}