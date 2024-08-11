using System.Collections.Generic;
using ImmersiveCrosshair.Harmony;
using ImmersiveCrosshair.Harmony.Interfaces;
using Moq;

namespace UnitTests.Harmony;

public static class Factory
{
    public static float MinimumInteractableDistance =
        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.MinimumInteractableDistance;

    public static Dictionary<string, object> Input => new()
    {
        { "HasHud", true },
        { "HitInfo", true },
        { "bHitValid", true },
        { "holdingToolTag", true },
        { "hit.distanceSq", MinimumInteractableDistance }
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
        var holdingToolTag =
            parameters.ContainsKey("holdingToolTag") && (bool)parameters["holdingToolTag"];
        itemActionMock.Setup(p => p.IsTool).Returns(holdingToolTag);
        if (holdingToolTag) actions.Add(itemActionMock.Object);

        itemClassMock.Setup(p => p.Actions).Returns(actions.ToArray());

        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.SetLogger(logger.Object);

        return (playerLocal, hudMock);
    }
}