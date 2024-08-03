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
        itemActionRanged.Setup(p => p.IsHarvest).Returns(true);
        var holdingRanged = parameters.ContainsKey("holdingRanged") && (bool)parameters["holdingRanged"];
        if (holdingRanged) actions.Add(itemActionRanged.Object);

        var itemActionHarvest = new Mock<IItemActionHarvest>();
        itemActionHarvest.Setup(p => p.IsHarvest).Returns(true);
        var holdingHarvest =
            parameters.ContainsKey("holdingHarvest") && (bool)parameters["holdingHarvest"];
        if (holdingHarvest) actions.Add(itemActionHarvest.Object);

        var itemActionRepair = new Mock<IItemActionRepair>();
        itemActionRepair.Setup(p => p.IsRepair).Returns(true);
        var holdingRepair = parameters.ContainsKey("holdingRepair") && (bool)parameters["holdingRepair"];
        if (holdingRepair) actions.Add(itemActionRepair.Object);
        
        var itemActionTerrain = new Mock<IItemActionTerrain>();
        itemActionTerrain.Setup(p => p.IsTerrain).Returns(true);
        var holdingTerrain = parameters.ContainsKey("holdingTerrain") && (bool)parameters["holdingTerrain"];
        if (holdingTerrain) actions.Add(itemActionTerrain.Object);

        itemClassMock.Setup(p => p.Actions).Returns(actions.ToArray());

        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.SetLogger(logger.Object);

        return (playerLocal, hudMock);
    }
}