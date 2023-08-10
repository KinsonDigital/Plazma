// <copyright file="RandomColorBehaviorTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace PlazmaTests.Behaviors;

using System;
using System.Collections.ObjectModel;
using FluentAssertions;
using Plazma.Behaviors;
using Plazma.Services;
using NSubstitute;
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
        var mockRandomizerService = Substitute.For<IRandomizerService>();
        var settings = new RandomChoiceBehaviorSettings();
        var behavior = new RandomColorBehavior(settings, mockRandomizerService);

        // Act
        behavior.Update(TimeSpan.Zero);
        behavior.Update(TimeSpan.Zero);

        // Assert
        mockRandomizerService.Received(1).GetValue(0, 0);
    }

    [Fact]
    public void Update_WhenInvokedWithNullSettingsData_UsesDefaultWhiteColor()
    {
        // Arrange
        var mockRandomizerService = Substitute.For<IRandomizerService>();
        var settings = new RandomChoiceBehaviorSettings();
        var behavior = new RandomColorBehavior(settings, mockRandomizerService);

        // Act
        behavior.Update(TimeSpan.Zero);

        // Assert
        behavior.Value.Should().Be("clr:255,255,255,255");
    }

    [Fact]
    public void Update_WhenInvokedWithSettingsData_RandomlyChoosesSecondColor()
    {
        // Arrange
        var mockRandomizerService = Substitute.For<IRandomizerService>();
        mockRandomizerService.GetValue(0, 1).Returns(1);

        var settings = new RandomChoiceBehaviorSettings
        {
            Data = new ReadOnlyCollection<string>(new[] { "clr:255,255,0,0", "clr:255,0,255,0" }),
        };

        var behavior = new RandomColorBehavior(settings, mockRandomizerService);

        // Act
        behavior.Update(TimeSpan.Zero);

        // Assert
        behavior.Value.Should().Be("clr:255,0,255,0");
    }

    [Fact]
    public void Update_WhenResetAfterInvoked_ChoosesAnotherColor()
    {
        // Arrange
        var mockRandomizerService = Substitute.For<IRandomizerService>();
        var settings = new RandomChoiceBehaviorSettings();
        var behavior = new RandomColorBehavior(settings, mockRandomizerService);

        // Act
        behavior.Update(TimeSpan.Zero);
        behavior.Reset();
        behavior.Update(TimeSpan.Zero);

        // Assert
        mockRandomizerService.Received(2).GetValue(0, 0);
    }
    #endregion
}
