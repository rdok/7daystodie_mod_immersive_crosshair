using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;

namespace UnitTests.Harmony;

[TestFixture]
public class InteractionPromptTests
{
    [Test]
    public void it_enables_the_crosshair_having_interaction_prompt_opened()
    {
        var (playerLocalMock, hudMock) = Factory.Create(new Dictionary<string, object>(Factory.Input)
            {
                ["HasInteractionPromptOpened"] = true,
            }
        );
        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.ApplyPatch(playerLocalMock.Object);
        hudMock.VerifySet(h => h.showCrosshair = true, Times.Once);
    }


    [Test]
    public void it_does_not_modify_the_crosshair_having_interaction_closed()
    {
        var (playerLocalMock, hudMock) = Factory.Create(new Dictionary<string, object>(Factory.Input)
            {
                ["HasInteractionPromptOpened"] = false,
                ["HitInfo"] = false,
            }
        );
        ImmersiveCrosshair.Harmony.ImmersiveCrosshair.ApplyPatch(playerLocalMock.Object);
        hudMock.VerifyNoOtherCalls();
    }
}