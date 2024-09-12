using System.Collections.Generic;
using Moq;
using NUnit.Framework;

namespace UnitTests.Harmony.GuiDrawCrosshairPatch;

[TestFixture]
public class InteractionPromptTests
{
    [Test]
    public void it_enables_the_crosshair_having_interaction_prompt_opened()
    {
        var (guiDrawCrosshair, entityPlayerLocalMock, itemActionMock, worldRayHitInfoMock, hudMock) =
            Factory.Create(new Dictionary<string, object>(Factory.Input) { ["HasInteractionPromptOpened"] = true });

        guiDrawCrosshair.Update(entityPlayerLocalMock.Object, itemActionMock.Object, worldRayHitInfoMock.Object);

        hudMock.VerifySet(h => h.showCrosshair = true, Times.Once);
    }
}