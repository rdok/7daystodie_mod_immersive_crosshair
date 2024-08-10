using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;

namespace UnitTests.Harmony;

[TestFixture]
public class ImmersiveCrosshairInitTests
{
    private const float MinimumInteractableDistance =
        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.MinimumInteractableDistance;

    protected virtual Dictionary<string, object> InteractableInput => new()
    {
        { "HasHud", true },
        { "HitInfo", true },
        { "bHitValid", true },
        { "holdingToolTag", true },
        { "hit.distanceSq", MinimumInteractableDistance }
    };

    [Test]
    public void it_does_not_change_crosshair_when_hud_is_not_loaded()
    {
        var (playerLocalMock, hudMock) = Factory.Create(
            new Dictionary<string, object>(InteractableInput) { ["HasHud"] = false }
        );
        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.ApplyPatch(playerLocalMock.Object);
        hudMock.VerifyNoOtherCalls();
    }


    [Test]
    public void it_hides_the_crosshair_having_no_interactable_in_distance()
    {
        var (playerLocalMock, hudMock) = Factory.Create(new Dictionary<string, object>(InteractableInput)
        {
            ["hit.distanceSq"] = (float)Math.Pow(MinimumInteractableDistance, 2) + .1f
        });
        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.ApplyPatch(playerLocalMock.Object);
        hudMock.VerifySet(h => h.showCrosshair = false, Times.Once);
    }

    [Test]
    public void it_enables_crosshair_having_an_interactable_in_distance_while_holding_tool()
    {
        var (playerLocalMock, hudMock) = Factory.Create(new Dictionary<string, object>(InteractableInput));
        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.ApplyPatch(playerLocalMock.Object);
        hudMock.VerifySet(h => h.showCrosshair = true, Times.Once);
    }

    [Test]
    public void it_hides_the_crosshair_holding_a_non_interactable_item()
    {
        var (playerLocalMock, hudMock) = Factory.Create(new Dictionary<string, object>(InteractableInput)
        {
            ["holdingToolTag"] = false
        });
        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.ApplyPatch(playerLocalMock.Object);
        hudMock.VerifySet(h => h.showCrosshair = false, Times.Once);
    }

    [Test]
    public void it_enables_crosshair_having_non_first_person_view()
    {
        var (playerLocalMock, hudMock) = Factory.Create(new Dictionary<string, object>(InteractableInput)
        {
            { "HasFirstPersonView", false },
        });
        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.ApplyPatch(playerLocalMock.Object);
        hudMock.VerifySet(h => h.showCrosshair = true, Times.Once);
    }
}