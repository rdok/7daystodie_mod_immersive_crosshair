using HarmonyLib;
using ImmersiveCrosshair.Harmony.Adapters;

namespace ImmersiveCrosshair.Harmony.Patches
{
    [HarmonyPatch(typeof(ItemActionDynamic), nameof(ItemActionDynamic.GetExecuteActionTarget))]
    public static class ToolsPatch
    {
        private static readonly IGuiDrawCrosshair GUIDrawCrosshair =
            Services.Get<IGuiDrawCrosshair>();

        public static void Postfix(
            ItemActionDynamic __instance,
            ItemActionData _actionData,
            WorldRayHitInfo __result
        )
        {
            if (__result == null) return;

            var player = new EntityPlayerLocalAdapter(
                _actionData.invData.holdingEntity as EntityPlayerLocal
            );

            var itemAction = new ItemActionAdapter(__instance);
            var worldRayHitInfo = new WorldHitInfoAdapter(__result);

            GUIDrawCrosshair.Update(player, itemAction, worldRayHitInfo);
        }
    }
}