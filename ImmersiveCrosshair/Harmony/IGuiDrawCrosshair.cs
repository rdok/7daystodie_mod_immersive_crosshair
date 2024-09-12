namespace ImmersiveCrosshair.Harmony
{
    public interface IGuiDrawCrosshair
    {
        void Update(
            IEntityPlayerLocal entityPlayerLocal,
            IItemAction itemAction,
            IWorldRayHitInfo worldRayHitInfo
        );
    }
}