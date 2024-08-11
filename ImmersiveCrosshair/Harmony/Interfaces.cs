using ImmersiveCrosshair.Harmony.Interfaces;
using UnityEngine;

namespace ImmersiveCrosshair.Harmony
{
    public interface ILocalPlayerUI
    {
        T GetComponentInChildren<T>();
        XUi xui { get; }
        IGUIWindowManager windowManager { get; }
    }

    public interface IEntityPlayerLocal
    {
        ILocalPlayerUI playerUI { get; }
        IInventory inventory { get; }
        IWorldRayHitInfo HitInfo { get; }
        bool bFirstPersonView { get; set; }
    }

    public interface IInventory
    {
        IItemValue holdingItemItemValue { get; }
    }

    public interface IWorldRayHitInfo
    {
        bool bHitValid { get; }
        IHitInfoDetails hit { get; }
        string tag { get; set; }
        HitInfoDetails fmcHit { get; set; }
        Transform transform { get; set; }
        Collider hitCollider { get; set; }
    }

    public interface IItemAction
    {
        bool IsTool { get; }
    }

    public interface IItemClass
    {
        IItemAction[] Actions { get; }
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
}