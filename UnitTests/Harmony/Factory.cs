using System.Collections.Generic;
using ImmersiveCrosshair.Harmony;
using Moq;

namespace UnitTests.Harmony;

public static class Factory
{
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

        var hasHud = parameters.ContainsKey("HasHud") && (bool)parameters["HasHud"];
        var hasHitInfo = parameters.ContainsKey("HitInfo") && (bool)parameters["HitInfo"];
        var hasBHitValid = parameters.ContainsKey("bHitValid") && (bool)parameters["bHitValid"];
        var distanceSq = parameters.ContainsKey("hit.distanceSq") ? (float)parameters["hit.distanceSq"] : 1;
        var hasFirstPersonView =
            parameters.ContainsKey("HasFirstPersonView") ? (bool)parameters["HasFirstPersonView"] : true;

        holdingItemItemValue.Setup(p => p.ItemClass).Returns(itemClassMock.Object);
        inventory.Setup(p => p.holdingItemItemValue)
            .Returns(holdingItemItemValue.Object);
        playerUI.Setup(p => p.GetComponentInChildren<IGuiWdwInGameHUD>())
            .Returns(hasHud ? hudMock.Object : null);
        playerLocal.Setup(p => p.playerUI).Returns(playerUI.Object);
        playerLocal.Setup(p => p.inventory).Returns(inventory.Object);
        playerLocal.Setup(p => p.HitInfo)
            .Returns(hasHitInfo ? hitInfo.Object : null);
        playerLocal.Setup(p => p.bFirstPersonView).Returns(hasFirstPersonView);
        hitInfo.Setup(p => p.bHitValid).Returns(hasBHitValid);
        hitInfo.Setup(p => p.hit).Returns(hit.Object);
        hit.Setup(p => p.distanceSq).Returns(distanceSq);

        var actions = new List<IItemAction>();

        var itemActionRanged = new Mock<IItemActionRanged>();
        var holdingRanged = parameters.ContainsKey("holdingRanged") && (bool)parameters["holdingRanged"];
        itemActionRanged.Setup(p => p.IsRanged).Returns(holdingRanged);
        if (holdingRanged) actions.Add(itemActionRanged.Object);

        var itemActionRepair = new Mock<IItemActionRepair>();
        var holdingRepair = parameters.ContainsKey("holdingRepair") && (bool)parameters["holdingRepair"];
        itemActionRepair.Setup(p => p.IsRepair).Returns(holdingRepair);
        if (holdingRepair) actions.Add(itemActionRepair.Object);

        var itemActionHarvest = new Mock<IItemActionHarvest>();
        var holdingHarvest =
            parameters.ContainsKey("holdingHarvest") && (bool)parameters["holdingHarvest"];
        itemActionHarvest.Setup(p => p.IsHarvest).Returns(holdingHarvest);
        if (holdingHarvest) actions.Add(itemActionHarvest.Object);

        var itemActionSalvage = new Mock<IItemActionSalvage>();
        var holdingSalvage =
            parameters.ContainsKey("holdingSalvage") && (bool)parameters["holdingSalvage"];
        itemActionSalvage.Setup(p => p.IsSalvage).Returns(holdingSalvage);
        if (holdingSalvage) actions.Add(itemActionSalvage.Object);
        
        var itemActionKnife = new Mock<IItemActionBareHands>();
        var holdingKnife =
            parameters.ContainsKey("holdingKnife") && (bool)parameters["holdingKnife"];
        itemActionKnife.Setup(p => p.IsKnife).Returns(holdingKnife);
        if (holdingKnife) actions.Add(itemActionKnife.Object);

        var itemActionBareHands = new Mock<IItemActionBareHands>();
        var holdingBareHands =
            parameters.ContainsKey("holdingBareHands") && (bool)parameters["holdingBareHands"];
        itemActionBareHands.Setup(p => p.IsBareHands).Returns(holdingBareHands);
        if (holdingBareHands) actions.Add(itemActionBareHands.Object);

        itemClassMock.Setup(p => p.Actions).Returns(actions.ToArray());

        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.SetLogger(logger.Object);

        return (playerLocal, hudMock);
    }
}