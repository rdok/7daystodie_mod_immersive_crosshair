using System.Collections.Generic;
using Moq;
using NUnit.Framework;

namespace UnitTests.Harmony;

[TestFixture]
public class GuiDrawCrosshairPatchTests
{
    [Test]
    public void it_does_not_change_crosshair_when_hud_is_not_loaded()
    {
        var (guiDrawCrosshair, entityPlayerLocalMock, itemActionMock, worldRayHitInfoMock, hudMock) =
            Factory.Create(new Dictionary<string, object>(Factory.Input) { ["HasHud"] = false });

        guiDrawCrosshair.Update(entityPlayerLocalMock.Object, itemActionMock.Object, worldRayHitInfoMock.Object);

        hudMock.VerifyNoOtherCalls();
    }


    [Test]
    public void it_hides_the_crosshair_having_no_interactable_in_distance()
    {
        var (guiDrawCrosshair, entityPlayerLocalMock, itemActionMock, worldRayHitInfoMock, hudMock) =
            Factory.Create(new Dictionary<string, object>(Factory.Input) { ["bHitValid"] = false });

        guiDrawCrosshair.Update(entityPlayerLocalMock.Object, itemActionMock.Object, worldRayHitInfoMock.Object);

        hudMock.VerifySet(h => h.showCrosshair = false, Times.Once);
    }

    [Test]
    public void it_enables_crosshair_having_an_interactable_in_distance_while_holding_tool()
    {
        var (guiDrawCrosshair, entityPlayerLocalMock, itemActionMock, worldRayHitInfoMock, hudMock) =
            Factory.Create(new Dictionary<string, object>(Factory.Input));

        guiDrawCrosshair.Update(entityPlayerLocalMock.Object, itemActionMock.Object, worldRayHitInfoMock.Object);

        hudMock.VerifySet(h => h.showCrosshair = true, Times.Once);
    }

    [Test]
    public void it_enables_crosshair_while_holding_a_tool_and_enabled_setting()
    {
        var (guiDrawCrosshair, entityPlayerLocalMock, itemActionMock, worldRayHitInfoMock, hudMock) =
            Factory.Create(new Dictionary<string, object>(Factory.Input)
            {
                ["HitInfo"] = false,
                ["HoldsTool"] = true,
                ["ToolSetting"] = "dynamic"
            });

        guiDrawCrosshair.Update(entityPlayerLocalMock.Object, itemActionMock.Object, worldRayHitInfoMock.Object);

        hudMock.VerifySet(h => h.showCrosshair = true, Times.Once);
    }

    [Test]
    public void it_enables_crosshair_while_holding_a_melee_and_enabled_setting()
    {
        var (guiDrawCrosshair, entityPlayerLocalMock, itemActionMock, worldRayHitInfoMock, hudMock) =
            Factory.Create(new Dictionary<string, object>(Factory.Input)
            {
                ["HitInfo"] = false,
                ["HoldsTool"] = true,
                ["MeleeSetting"] = "dynamic"
            });

        guiDrawCrosshair.Update(entityPlayerLocalMock.Object, itemActionMock.Object, worldRayHitInfoMock.Object);

        hudMock.VerifySet(h => h.showCrosshair = true, Times.Once);
    }

    [Test]
    public void it_hides_the_crosshair_holding_a_non_interactable_item()
    {
        var (guiDrawCrosshair, entityPlayerLocalMock, itemActionMock, worldRayHitInfoMock, hudMock) =
            Factory.Create(new Dictionary<string, object>(Factory.Input)
            {
                ["HoldsTool"] = false, ["bHitValid"] = false, ["RangedWeaponSettings"] = "off"
            });

        guiDrawCrosshair.Update(entityPlayerLocalMock.Object, itemActionMock.Object, worldRayHitInfoMock.Object);

        hudMock.VerifySet(h => h.showCrosshair = false, Times.Once);
    }

    [Test]
    public void it_enables_crosshair_having_non_first_person_view()
    {
        var (guiDrawCrosshair, entityPlayerLocalMock, itemActionMock, worldRayHitInfoMock, hudMock) = Factory.Create(
            new Dictionary<string, object>(Factory.Input) { ["HasFirstPersonView"] = false });

        guiDrawCrosshair.Update(entityPlayerLocalMock.Object, itemActionMock.Object, worldRayHitInfoMock.Object);

        hudMock.VerifySet(h => h.showCrosshair = true, Times.Once);
    }


    [Test]
    public void it_enables_crosshair_having_enabled_bows_with_no_sights_setting()
    {
        var (guiDrawCrosshair, entityPlayerLocalMock, itemActionMock, worldRayHitInfoMock, hudMock)
            = Factory.Create(new Dictionary<string, object>(Factory.Input)
            {
                ["BowSetting"] = "static",
                ["HoldsBowWithNoSights"] = true,
                ["HoldsTool"] = false
            });

        guiDrawCrosshair.Update(entityPlayerLocalMock.Object, itemActionMock.Object, worldRayHitInfoMock.Object);

        hudMock.VerifySet(h => h.showCrosshair = true, Times.Once);
    }

    [Test]
    public void it_does_not_enable_crosshair_having_disabled_bows_and_disabled_bow_setting()
    {
        var (guiDrawCrosshair, entityPlayerLocalMock, itemActionMock, worldRayHitInfoMock, hudMock)
            = Factory.Create(new Dictionary<string, object>(Factory.Input)
            {
                ["BowSetting"] = "off",
                ["HoldsBowWithNoSights"] = true,
                ["HoldsTool"] = false
            });

        guiDrawCrosshair.Update(entityPlayerLocalMock.Object, itemActionMock.Object, worldRayHitInfoMock.Object);

        hudMock.VerifySet(h => h.showCrosshair = true, Times.Never);
    }
}