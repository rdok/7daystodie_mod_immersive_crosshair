using System.Collections.Generic;
using ImmersiveCrosshair.Harmony;
using ImmersiveCrosshair.Harmony.Interfaces;
using Moq;

namespace UnitTests.Harmony;

public static class Factory
{
    public const float MinimumInteractableDistance =
        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.MinimumInteractableDistance;

    public static Dictionary<string, object> Input => new()
    {
        { "HasHud", true },
        { "HitInfo", true },
        { "bHitValid", true },
        { "holdingToolTag", true },
        { "hit.distanceSq", MinimumInteractableDistance },
        { "BowsWithNoSightsSetting", false },
        { "HasBowWithNoSights", false },
        { "EnableCrosshairForToolSetting", false },
        { "EnableCrosshairForMeleeSetting", false },
        
    };

    public static (Mock<IEntityPlayerLocal>, Mock<IGuiWdwInGameHUD>)
        Create(Dictionary<string, object> parameters)
    {
        var playerUI = new Mock<ILocalPlayerUI>();
        var hudMock = new Mock<IGuiWdwInGameHUD>();
        var hitInfo = new Mock<IWorldRayHitInfo>();
        var playerLocal = new Mock<IEntityPlayerLocal>();
        var inventory = new Mock<ImmersiveCrosshair.Harmony.IInventory>();
        var itemClassMock = new Mock<IItemClass>();
        var hit = new Mock<IHitInfoDetails>();
        var holdingItemItemValue = new Mock<IItemValue>();
        var logger = new Mock<ILogger>();
        var windowManagerMock = new Mock<IGUIWindowManager>();

        var hasHud = parameters.ContainsKey("HasHud") && (bool)parameters["HasHud"];
        var hasHitInfo = parameters.ContainsKey("HitInfo") && (bool)parameters["HitInfo"];
        var hasBHitValid = parameters.ContainsKey("bHitValid") && (bool)parameters["bHitValid"];
        var distanceSq = parameters.ContainsKey("hit.distanceSq") ? (float)parameters["hit.distanceSq"] : 99;
        var hasFirstPersonView =
            parameters.ContainsKey("HasFirstPersonView") ? (bool)parameters["HasFirstPersonView"] : true;
        var hasInteractionPromptOpened =
            parameters.ContainsKey("HasInteractionPromptOpened")
                ? (bool)parameters["HasInteractionPromptOpened"]
                : false;
        var bowsWithNoSightsSetting = (bool)parameters["BowsWithNoSightsSetting"];
        var enableCrosshairForToolSetting = (bool)parameters["EnableCrosshairForToolSetting"];
        var enableCrosshairForMeleeSetting = (bool)parameters["EnableCrosshairForMeleeSetting"];
        var hasBowWithNoSights = (bool)parameters["HasBowWithNoSights"];

        XUiC_InteractionPrompt.ID = hasInteractionPromptOpened ? "lorem-ipsum" : "";
        windowManagerMock.Setup(p => p.IsWindowOpen(XUiC_InteractionPrompt.ID))
            .Returns(hasInteractionPromptOpened);
        holdingItemItemValue.Setup(p => p.ItemClass).Returns(itemClassMock.Object);
        inventory.Setup(p => p.holdingItemItemValue).Returns(holdingItemItemValue.Object);
        playerUI.Setup(p => p.GetComponentInChildren<IGuiWdwInGameHUD>())
            .Returns(hasHud ? hudMock.Object : null);
        playerUI.Setup(p => p.windowManager).Returns(windowManagerMock.Object);
        playerLocal.Setup(p => p.playerUI).Returns(playerUI.Object);
        playerLocal.Setup(p => p.inventory).Returns(inventory.Object);
        playerLocal.Setup(p => p.HitInfo)
            .Returns(hasHitInfo ? hitInfo.Object : null);
        playerLocal.Setup(p => p.bFirstPersonView).Returns(hasFirstPersonView);
        hitInfo.Setup(p => p.bHitValid).Returns(hasBHitValid);
        hitInfo.Setup(p => p.hit).Returns(hit.Object);
        hit.Setup(p => p.distanceSq).Returns(distanceSq);

        var actions = new List<IItemAction>();
        var itemActionMock = new Mock<IItemAction>();
        var holdingToolTag = (bool)parameters["holdingToolTag"];
        itemActionMock.Setup(p => p.IsTool).Returns(holdingToolTag);
        itemActionMock.Setup(p => p.HasBowWithNoSights).Returns(hasBowWithNoSights);
        itemActionMock.Setup(p => p.IsMelee).Returns(enableCrosshairForMeleeSetting);
        actions.Add(itemActionMock.Object);

        itemClassMock.Setup(p => p.Actions).Returns(actions.ToArray());

        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.SetLogger(logger.Object);
        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.BowWithNoSightsSetting = bowsWithNoSightsSetting;
        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.EnabledForToolsSetting = enableCrosshairForToolSetting;
        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.EnabledForMeleeSetting = enableCrosshairForMeleeSetting;


        return (playerLocal, hudMock);
    }
}