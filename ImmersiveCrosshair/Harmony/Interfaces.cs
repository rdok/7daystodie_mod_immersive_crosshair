namespace ImmersiveCrosshair.Harmony
{
    public interface ILocalPlayerUI
    {
        T GetComponentInChildren<T>();
    }

    public interface IEntityPlayerLocal
    {
        ILocalPlayerUI playerUI { get; }
        IInventory inventory { get; }
        IWorldRayHitInfo HitInfo { get; }
    }

    public interface IInventory
    {
        IItemValue holdingItemItemValue { get; }
    }

    public interface IWorldRayHitInfo
    {
        bool bHitValid { get; }
        IHitInfoDetails hit { get; }
    }

    public interface IItemAction
    {
        bool IsRanged { get; }
    }

    public interface IItemClass
    {
        IItemAction[] Actions { get; }
    }

    public interface IHarmony
    {
        void PatchAll();
    }

    public interface IItemValue
    {
        IItemClass ItemClass { get; }
    }

    public interface IGuiWdwInGameHUD
    {
        bool showCrosshair { get; set; }
    }

    public interface IHitInfoDetails
    {
        float distanceSq { get; }
    }

    public interface IItemActionRanged: IItemAction
    {
    }
}