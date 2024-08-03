using System.Collections.Generic;
using ImmersiveCrosshair.Harmony;
using Moq;

namespace UnitTests.Harmony;

public static class Factory
{
    public static (Mock<IEntityPlayerLocal>, Mock<IGuiWdwInGameHUD>)
        CreatePostfixFactory(Dictionary<string, object> parameters)
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

        var hasHud = parameters.ContainsKey("HasHud") && (bool)parameters["HasHud"];
        var hasHitInfo = parameters.ContainsKey("HitInfo") && (bool)parameters["HitInfo"];
        var hasBHitValid = parameters.ContainsKey("bHitValid") && (bool)parameters["bHitValid"];
        var distanceSq = parameters.ContainsKey("hit.distanceSq") ? (float)parameters["hit.distanceSq"] : 1;

        holdingItemItemValue.Setup(p => p.ItemClass).Returns(itemClassMock.Object);
        inventory.Setup(p => p.holdingItemItemValue)
            .Returns(holdingItemItemValue.Object);
        playerUI.Setup(p => p.GetComponentInChildren<IGuiWdwInGameHUD>())
            .Returns(hasHud ? hudMock.Object : null);
        playerLocal.Setup(p => p.playerUI).Returns(playerUI.Object);
        playerLocal.Setup(p => p.inventory).Returns(inventory.Object);
        playerLocal.Setup(p => p.HitInfo)
            .Returns(hasHitInfo ? hitInfo.Object : null);
        hitInfo.Setup(p => p.bHitValid).Returns(hasBHitValid);
        hitInfo.Setup(p => p.hit).Returns(hit.Object);
        hit.Setup(p => p.distanceSq).Returns(distanceSq);

        var actions = new List<IItemAction>();

        var itemActionRanged = new Mock<IItemActionRanged>();
        var holdingRanged = parameters.ContainsKey("HoldingRanged") && (bool)parameters["HoldingRanged"];
        if (holdingRanged) actions.Add(itemActionRanged.Object);

        var itemActionDynamicMelee = new Mock<IItemActionDynamicMelee>();
        itemActionDynamicMelee.Setup(p => p.IsDynamicMelee).Returns(true);
        var holdingDynamicMelee =
            parameters.ContainsKey("HoldingDynamicMelee") && (bool)parameters["HoldingDynamicMelee"];
        if (holdingDynamicMelee) actions.Add(itemActionDynamicMelee.Object);

        var itemActionRepair = new Mock<IItemActionRepair>();
        var holdingRepair = parameters.ContainsKey("HoldingRepair") && (bool)parameters["HoldingRepair"];
        if (holdingRepair) actions.Add(itemActionRepair.Object);

        itemClassMock.Setup(p => p.Actions).Returns(actions.ToArray());

        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.SetLogger(logger.Object);

        return (playerLocal, hudMock);
    }
}