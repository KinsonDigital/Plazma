// <copyright file="RandomColorBehaviorTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace PlazmaTests.Behaviors;

using System;
using System.Collections.ObjectModel;
using Plazma.Behaviors;
using Plazma.Services;
using Moq;
using Xunit;

/// <summary>
/// Holds the tests for the <see cref="RandomColorBehavior"/> class.
/// </summary>
public class RandomColorBehaviorTests
{
    #region Constructor Tests
    [Fact]
    public void Update_WhenInvokedAfterFirstInvoke_OnlyUpdatesValueOnFirstInvoke()
    {
        // Arrange
        var mockRandomizerService = new Mock<IRandomizerService>();
        var settings = new RandomChoiceBehaviorSettings();
        var behavior = new RandomColorBehavior(settings, mockRandomizerService.Object);

        // Act
        behavior.Update(It.IsAny<TimeSpan>());
        behavior.Update(It.IsAny<TimeSpan>());

        // Assert
        mockRandomizerService.Verify(m => m.GetValue(0, 0), Times.Once());
    }

    [Fact]
    public void Update_WhenInvokedWithNullSettingsData_UsesDefaultWhiteColor()
    {
        // Arrange
        var mockRandomizerService = new Mock<IRandomizerService>();
        var settings = new RandomChoiceBehaviorSettings();
        var behavior = new RandomColorBehavior(settings, mockRandomizerService.Object);

        // Act
        behavior.Update(It.IsAny<TimeSpan>());

        // Assert
        Assert.Equal("clr:255,255,255,255", behavior.Value);
    }

    [Fact]
    public void Update_WhenInvokedWithSettingsData_RandomlyChoosesSecondColor()
    {
        // Arrange
        var mockRandomizerService = new Mock<IRandomizerService>();
        mockRandomizerService.Setup(m => m.GetValue(0, 1)).Returns(1);

        var settings = new RandomChoiceBehaviorSettings
        {
            Data = new ReadOnlyCollection<string>(new[] { "clr:255,255,0,0", "clr:255,0,255,0" }),
        };

        var behavior = new RandomColorBehavior(settings, mockRandomizerService.Object);

        // Act
        behavior.Update(It.IsAny<TimeSpan>());

        // Assert
        Assert.Equal("clr:255,0,255,0", behavior.Value);
    }

    [Fact]
    public void Update_WhenResetAfterInvoked_ChoosesAnotherColor()
    {
        // Arrange
        var mockRandomizerService = new Mock<IRandomizerService>();
        var settings = new RandomChoiceBehaviorSettings();
        var behavior = new RandomColorBehavior(settings, mockRandomizerService.Object);

        // Act
        behavior.Update(It.IsAny<TimeSpan>());
        behavior.Reset();
        behavior.Update(It.IsAny<TimeSpan>());

        // Assert
        mockRandomizerService.Verify(m => m.GetValue(0, 0), Times.Exactly(2));
    }
    #endregion
}