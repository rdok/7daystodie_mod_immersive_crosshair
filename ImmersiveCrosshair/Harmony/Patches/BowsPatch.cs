using HarmonyLib;
using ImmersiveCrosshair.Harmony.Adapters;

namespace ImmersiveCrosshair.Harmony.Patches
{
    [HarmonyPatch(typeof(ItemActionCatapult), nameof(ItemActionCatapult.OnScreenOverlay))]
    public static class BowsPatch
    {
        private static readonly IGuiDrawCrosshair GUIDrawCrosshair =
            Services.Get<IGuiDrawCrosshair>();

        private static readonly ILogger Logger = new Logger();

        public static void Prefix(
            ItemActionCatapult __instance,
            ItemActionData _actionData
        )
        {
            var entityPlayerLocal = _actionData.invData.holdingEntity
                as EntityPlayerLocal;
            if (entityPlayerLocal == null) return;

            var player = new EntityPlayerLocalAdapter(entityPlayerLocal);

            var itemAction = new ItemActionAdapter(__instance);

            GUIDrawCrosshair.Update(player, itemAction);
        }
    }
}