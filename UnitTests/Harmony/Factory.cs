using System.Collections.Generic;
using ImmersiveCrosshair.Harmony;
using ImmersiveCrosshair.Harmony.Interfaces;
using Moq;

namespace UnitTests.Harmony;

public static class Factory
{
    public static Dictionary<string, object> Input => new()
    {
        { "HasHud", true },
        { "bHitValid", true },
        { "HoldsTool", true },
        { "HoldsBowWithNoSights", false },
        { "HoldsMelee", false },
        { "ToolSetting", "dynamic" },
        { "RangedWeaponSettings", "static" },
        { "MeleeSetting", "dynamic" },
        { "BowSetting", "static" },
        { "HasFirstPersonView", true },
        { "HasInteractionPromptOpened", false }
    };

    public static (GuiDrawCrosshair, Mock<IEntityPlayerLocal> entityPlayerLocalMock, Mock<IItemAction> itemActionMock,
        Mock<IWorldRayHitInfo> worldRayHitInfoMock, Mock<INGuiWdwInGameHUD>)
        Create(Dictionary<string, object> parameters)
    {
        var localPlayerUIMock = new Mock<ILocalPlayerUI>();
        var hudMock = new Mock<INGuiWdwInGameHUD>();
        var worldRayHitInfoMock = new Mock<IWorldRayHitInfo>();
        var entityPlayerLocalMock = new Mock<IEntityPlayerLocal>();
        var inventory = new Mock<ImmersiveCrosshair.Harmony.IInventory>();
        var itemClassMock = new Mock<IItemClass>();
        var holdingItemItemValue = new Mock<IItemValue>();
        var loggerMock = new Mock<ILogger>();
        var windowManagerMock = new Mock<IGUIWindowManager>();
        var modSettings = new Mock<IModSettings>();

        var hasHud = (bool)parameters["HasHud"];
        var hasFirstPersonView = (bool)parameters["HasFirstPersonView"];
        var hasInteractionPromptOpened = (bool)parameters["HasInteractionPromptOpened"];

        XUiC_InteractionPrompt.ID = hasInteractionPromptOpened ? "lorem-ipsum" : "";
        windowManagerMock.Setup(p => p.IsWindowOpen(XUiC_InteractionPrompt.ID))
            .Returns(hasInteractionPromptOpened);
        modSettings.Setup(p => p.ToolsSetting).Returns((string)parameters["ToolSetting"]);
        modSettings.Setup(p => p.MeleeWeaponsSetting).Returns((string)parameters["MeleeSetting"]);
        modSettings.Setup(p => p.BowsSetting).Returns((string)parameters["BowSetting"]);
        modSettings.Setup(p => p.RangedWeaponsSetting).Returns((string)parameters["RangedWeaponSettings"]);
        holdingItemItemValue.Setup(p => p.ItemClass).Returns(itemClassMock.Object);
        inventory.Setup(p => p.holdingItemItemValue).Returns(holdingItemItemValue.Object);
        localPlayerUIMock.Setup(p => p.GetComponentInChildren<INGuiWdwInGameHUD>())
            .Returns(hasHud ? hudMock.Object : null);
        localPlayerUIMock.Setup(p => p.windowManager).Returns(windowManagerMock.Object);
        entityPlayerLocalMock.Setup(p => p.playerUI).Returns(localPlayerUIMock.Object);
        entityPlayerLocalMock.Setup(p => p.inventory).Returns(inventory.Object);
        entityPlayerLocalMock.Setup(p => p.bFirstPersonView).Returns(hasFirstPersonView);
        worldRayHitInfoMock.Setup(p => p.bHitValid).Returns((bool)parameters["bHitValid"]);

        var itemActionMock = new Mock<IItemAction>();
        itemActionMock.Setup(p => p.IsTool).Returns((bool)parameters["HoldsTool"]);
        itemActionMock.Setup(p => p.IsBowWithNoSights).Returns((bool)parameters["HoldsBowWithNoSights"]);
        itemActionMock.Setup(p => p.IsMelee).Returns((bool)parameters["HoldsMelee"]);

        var guiDrawCrosshair = new GuiDrawCrosshair(loggerMock.Object, modSettings.Object);

        return (guiDrawCrosshair, entityPlayerLocalMock, itemActionMock, worldRayHitInfoMock, hudMock);
    }
}