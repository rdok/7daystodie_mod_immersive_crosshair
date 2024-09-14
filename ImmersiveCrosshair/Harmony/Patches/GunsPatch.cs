using HarmonyLib;
using ImmersiveCrosshair.Harmony.Adapters;

namespace ImmersiveCrosshair.Harmony.Patches
{
    [HarmonyPatch(typeof(ItemActionRanged), nameof(ItemActionRanged.GetExecuteActionTarget))]
    public static class GunsPatch
    {
        private static readonly IGuiDrawCrosshair GUIDrawCrosshair =
            Services.Get<IGuiDrawCrosshair>();

        public static void Postfix(
            ItemActionRanged __instance,
            ItemActionData _actionData,
            WorldRayHitInfo __result
        )
        {
            if (__result == null) return;

            var entityPlayerLocal = _actionData.invData.holdingEntity;
            if (entityPlayerLocal == null) return;

            var player = new EntityPlayerLocalAdapter(
                entityPlayerLocal as EntityPlayerLocal
            );

            var itemAction = new ItemActionAdapter(__instance);
            var worldRayHitInfo = new WorldHitInfoAdapter(__result);

            GUIDrawCrosshair.Update(player, itemAction, worldRayHitInfo);
        }
    }
}