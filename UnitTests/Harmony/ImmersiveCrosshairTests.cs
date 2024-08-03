using System.Collections.Generic;
using Moq;
using NUnit.Framework;

namespace UnitTests.Harmony;

[TestFixture]
public class ImmersiveCrosshairInitTests
{
    [Test]
    public void it_does_not_change_crosshair_when_hud_is_not_loaded()
    {
        var (playerLocalMock, hudMock) = Factory.CreatePostfixFactory(
            new Dictionary<string, object> { { "HasHud", false } }
        );

        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.ApplyPatch(playerLocalMock.Object);

        hudMock.VerifyNoOtherCalls();
    }

    [Test]
    public void it_enables_crosshair_holding_a_ranged_weapon()
    {
        var (playerLocalMock, hudMock) = Factory.CreatePostfixFactory(new Dictionary<string, object>
            {
                { "HasHud", true },
                { "HoldingRanged", true }
            }
        );

        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.ApplyPatch(playerLocalMock.Object);

        hudMock.VerifySet(h => h.showCrosshair = true, Times.Once);
    }

    [Test]
    public void it_does_not_change_crosshair_having_no_hit_info()
    {
        var (playerLocal, hud) = Factory.CreatePostfixFactory(new Dictionary<string, object>
        {
            { "HasHud", true },
            { "HoldingRanged", false },
            { "HitInfo", false }
        });

        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.ApplyPatch(playerLocal.Object);

        hud.VerifyNoOtherCalls();
    }

    [Test]
    public void it_enables_crosshair_having_an_interactable_in_distance()
    {
        var (playerLocal, hud) = Factory.CreatePostfixFactory(new Dictionary<string, object>
        {
            { "HasHud", true },
            { "HoldingRanged", false },
            { "HitInfo", true },
            { "bHitValid", true },
            { "hit.distanceSq", (float)5.2 }
        });

        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.ApplyPatch(playerLocal.Object);

        hud.VerifySet(h => h.showCrosshair = true, Times.Once);
    }

    [Test]
    public void it_disables_crosshair_having_not_an_interactable_in_distance()
    {
        var (playerLocal, hud) = Factory.CreatePostfixFactory(new Dictionary<string, object>
        {
            { "HasHud", true },
            { "HoldingRanged", false },
            { "HitInfo", true },
            { "bHitValid", true },
            { "hit.distanceSq", (float)5.3 }
        });

        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.ApplyPatch(playerLocal.Object);

        hud.VerifySet(h => h.showCrosshair = false, Times.Once);
    }


    [Test]
    public void it_disables_crosshair_holding_a_non_repair_dynamic_melee()
    {
        var (playerLocal, hud) = Factory.CreatePostfixFactory(new Dictionary<string, object>
        {
            { "HasHud", true },
            { "HitInfo", true },
            { "bHitValid", true },
            { "HoldingDynamicMelee", true },
            { "HoldingRepair", false }
        });

        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.ApplyPatch(playerLocal.Object);

        hud.VerifySet(h => h.showCrosshair = false, Times.Once);
    }
}