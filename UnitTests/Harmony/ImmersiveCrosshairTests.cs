using System.Collections.Generic;
using Moq;
using NUnit.Framework;

namespace UnitTests.Harmony;

[TestFixture]
public class ImmersiveCrosshairInitTests
{
    private const float MinimumInteractableDistance = (float)5.2;

    [Test]
    public void it_does_not_change_crosshair_when_hud_is_not_loaded()
    {
        var (playerLocalMock, hudMock) = Factory.Create(
            new Dictionary<string, object> { { "HasHud", false } }
        );

        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.ApplyPatch(playerLocalMock.Object);

        hudMock.VerifyNoOtherCalls();
    }

    [Test]
    public void it_disables_crosshair_holding_a_ranged_weapon()
    {
        var (playerLocalMock, hudMock) = Factory.Create(new Dictionary<string, object>
            {
                { "HasHud", true },
                { "HitInfo", true },
                { "bHitValid", true },
                { "holdingRanged", true },
                { "holdingHarvest", false },
                { "holdingRepair", false },
            }
        );

        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.ApplyPatch(playerLocalMock.Object);

        hudMock.VerifySet(h => h.showCrosshair = false, Times.Once);
    }

    [Test]
    public void it_does_not_change_crosshair_having_no_hit_info()
    {
        var (playerLocal, hud) = Factory.Create(new Dictionary<string, object>
        {
            { "HasHud", true },
            { "holdingRanged", false },
            { "HitInfo", false },
            { "holdingHarvest", true },
            { "holdingRepair", false },
        });

        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.ApplyPatch(playerLocal.Object);

        hud.VerifyNoOtherCalls();
    }

    [Test]
    public void it_enables_crosshair_having_an_interactable_in_distance_while_holding_repair_tool()
    {
        var (playerLocal, hud) = Factory.Create(new Dictionary<string, object>
        {
            { "HasHud", true },
            { "holdingRanged", false },
            { "holdingRepair", true },
            { "holdingHarvest", false },
            { "HitInfo", true },
            { "bHitValid", true },
            { "hit.distanceSq", MinimumInteractableDistance }
        });

        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.ApplyPatch(playerLocal.Object);

        hud.VerifySet(h => h.showCrosshair = true, Times.Once);
    }

    [Test]
    public void it_enables_crosshair_having_an_interactable_in_distance_while_holding_harvest_tool()
    {
        var (playerLocal, hud) = Factory.Create(new Dictionary<string, object>
        {
            { "HasHud", true },
            { "holdingRanged", false },
            { "holdingHarvest", true },
            { "HitInfo", true },
            { "bHitValid", true },
            { "hit.distanceSq", MinimumInteractableDistance }
        });

        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.ApplyPatch(playerLocal.Object);

        hud.VerifySet(h => h.showCrosshair = true, Times.Once);
    }

    [Test]
    public void it_disables_crosshair_having_no_interactable_in_distance()
    {
        var (playerLocal, hud) = Factory.Create(new Dictionary<string, object>
        {
            { "HasHud", true },
            { "HitInfo", true },
            { "bHitValid", true },
            { "holdingRanged", false },
            { "holdingRepair", true },
            { "hit.distanceSq", MinimumInteractableDistance + .1f },
        });

        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.ApplyPatch(playerLocal.Object);

        hud.VerifySet(h => h.showCrosshair = false, Times.Once);
    }

    [Test]
    public void it_hides_the_crosshair_holding_a_non_interactable_item()
    {
        var (playerLocal, hud) = Factory.Create(new Dictionary<string, object>
        {
            { "HasHud", true },
            { "HitInfo", true },
            { "bHitValid", true },
            { "holdingRanged", false },
            { "holdingHarvest", false },
            { "holdingRepair", false },
            { "holdingSalvage", false }
        });

        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.ApplyPatch(playerLocal.Object);

        hud.VerifySet(h => h.showCrosshair = false, Times.Once);
    }
    
    [Test]
    public void it_enables_crosshair_having_an_interactable_in_distance_while_holding_salvage_tool()
    {
        var (playerLocal, hud) = Factory.Create(new Dictionary<string, object>
        {
            { "HasHud", true },
            { "holdingRanged", false },
            { "holdingSalvage", true },
            { "HitInfo", true },
            { "bHitValid", true },
            { "hit.distanceSq", MinimumInteractableDistance }
        });

        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.ApplyPatch(playerLocal.Object);

        hud.VerifySet(h => h.showCrosshair = true, Times.Once);
    }
    
    [Test]
    public void it_enables_crosshair_having_an_interactable_in_distance_while_bare_hands_tool()
    {
        var (playerLocal, hud) = Factory.Create(new Dictionary<string, object>
        {
            { "HasHud", true },
            { "holdingRanged", false },
            { "holdingSalvage", false },
            { "holdingBareHands", true },
            { "HitInfo", true },
            { "bHitValid", true },
            { "hit.distanceSq", MinimumInteractableDistance }
        });

        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.ApplyPatch(playerLocal.Object);

        hud.VerifySet(h => h.showCrosshair = true, Times.Once);
    }
    
    [Test]
    public void it_enables_crosshair_having_non_first_person_view()
    {
        var (playerLocal, hud) = Factory.Create(new Dictionary<string, object>
        {
            { "HasHud", true },
            { "HasFirstPersonView", false },
        });

        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.ApplyPatch(playerLocal.Object);

        hud.VerifySet(h => h.showCrosshair = true, Times.Once);
    }
}