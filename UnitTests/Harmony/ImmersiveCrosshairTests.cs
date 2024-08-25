using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;

namespace UnitTests.Harmony;

[TestFixture]
public class ImmersiveCrosshairTests
{
    [Test]
    public void it_does_not_change_crosshair_when_hud_is_not_loaded()
    {
        var (playerLocalMock, hudMock) = Factory.Create(
            new Dictionary<string, object>(Factory.Input) { ["HasHud"] = false }
        );
        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.ApplyPatch(playerLocalMock.Object);
        hudMock.VerifyNoOtherCalls();
    }


    [Test]
    public void it_hides_the_crosshair_having_no_interactable_in_distance()
    {
        var (playerLocalMock, hudMock) = Factory.Create(new Dictionary<string, object>(Factory.Input)
        {
            ["hit.distanceSq"] = (float)Math.Pow(Factory.MinimumInteractableDistance, 2) + .1f
        });
        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.ApplyPatch(playerLocalMock.Object);
        hudMock.VerifySet(h => h.showCrosshair = false, Times.Once);
    }

    [Test]
    public void it_enables_crosshair_having_an_interactable_in_distance_while_holding_tool()
    {
        var (playerLocalMock, hudMock) = Factory.Create(new Dictionary<string, object>(Factory.Input));
        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.ApplyPatch(playerLocalMock.Object);
        hudMock.VerifySet(h => h.showCrosshair = true, Times.Once);
    }

    [Test]
    public void it_enables_crosshair_while_holding_a_tool_and_enabled_setting()
    {
        var (playerLocalMock, hudMock) = Factory.Create(new Dictionary<string, object>(Factory.Input)
        {
            ["HitInfo"] = false,
            ["holdingToolTag"] = true,
            ["EnableCrosshairForToolSetting"] = true
        });
        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.ApplyPatch(playerLocalMock.Object);
        hudMock.VerifySet(h => h.showCrosshair = true, Times.Once);
    }
    
    [Test]
    public void it_enables_crosshair_while_holding_a_melee_and_enabled_setting()
    {
        var (playerLocalMock, hudMock) = Factory.Create(new Dictionary<string, object>(Factory.Input)
        {
            ["HitInfo"] = false,
            ["holdingToolTag"] = true,
            ["EnableCrosshairForMeleeSetting"] = true
        });
        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.ApplyPatch(playerLocalMock.Object);
        hudMock.VerifySet(h => h.showCrosshair = true, Times.Once);
    }

    [Test]
    public void it_hides_the_crosshair_holding_a_non_interactable_item()
    {
        var (playerLocalMock, hudMock) = Factory.Create(new Dictionary<string, object>(Factory.Input)
        {
            ["holdingToolTag"] = false
        });
        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.ApplyPatch(playerLocalMock.Object);
        hudMock.VerifySet(h => h.showCrosshair = false, Times.Once);
    }

    [Test]
    public void it_enables_crosshair_having_non_first_person_view()
    {
        var (playerLocalMock, hudMock) = Factory.Create(new Dictionary<string, object>(Factory.Input)
        {
            ["HasFirstPersonView"] = false
        });
        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.ApplyPatch(playerLocalMock.Object);
        hudMock.VerifySet(h => h.showCrosshair = true, Times.Once);
    }

    [Test]
    public void it_enables_crosshair_having_enabled_bows_with_no_sights_setting()
    {
        var (playerLocalMock, hudMock) = Factory.Create(new Dictionary<string, object>(Factory.Input)
        {
            ["BowsWithNoSightsSetting"] = true,
            ["HasBowWithNoSights"] = true,
            ["holdingToolTag"] = false
        });
        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.ApplyPatch(playerLocalMock.Object);
        hudMock.VerifySet(h => h.showCrosshair = true, Times.Once);
    }

    [Test]
    public void it_does_not_enable_crosshair_having_disabled_bows_with_no_sights_setting()
    {
        var (playerLocalMock, hudMock) = Factory.Create(new Dictionary<string, object>(Factory.Input)
        {
            ["BowsWithNoSightsSetting"] = false,
            ["HasBowWithNoSights"] = true,
            ["holdingToolTag"] = false
        });
        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.ApplyPatch(playerLocalMock.Object);
        hudMock.VerifySet(h => h.showCrosshair = true, Times.Never);
    }
}