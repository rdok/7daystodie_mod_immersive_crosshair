using System.Collections.Generic;
using Moq;
using NUnit.Framework;

namespace UnitTests.Harmony.GuiDrawCrosshairPatch;

[TestFixture]
public class ToolsTest
{
    [Test]
    public void it_hides_crosshair_having_tool_setting_off()
    {
        var (guiDrawCrosshair, entityPlayerLocalMock, itemActionMock, worldRayHitInfoMock, hudMock) =
            Factory.Create(new Dictionary<string, object>(Factory.Input)
            {
                ["HoldsTool"] = true,
                ["ToolSetting"] = "off"
            });

        guiDrawCrosshair.Update(entityPlayerLocalMock.Object, itemActionMock.Object, worldRayHitInfoMock.Object);

        hudMock.VerifySet(h => h.showCrosshair = false, Times.Once);
    }
}