// <copyright file="BehaviorFactoryTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDParticleEngineTests.Behaviors;

using System;
using Plazma.Behaviors;
using Plazma.Services;
using KDParticleEngineTests.XUnitHelpers;
using Moq;
using Xunit;

/// <summary>
/// Tests the <see cref="BehaviorFactory"/> class.
/// </summary>
public class BehaviorFactoryTests
{
    #region Method Tests
    [Fact]
    public void CreateBehaviors_WhenSettingsParamIsNull_ThrowsException()
    {
        // Act & Assert
        var factory = new BehaviorFactory();

        AssertHelpers.ThrowsWithMessage<ArgumentNullException>(() =>
        {
            factory.CreateBehaviors(null, new Mock<IRandomizerService>().Object);
        }, "The parameter must not be null. (Parameter 'settings')");
    }

    [Fact]
    public void CreateBehaviors_WhenUsingEasingRandomBehaviorSettings_CreatesCorrectBehavior()
    {
        // Arrange
        var mockRandomizerService = new Mock<IRandomizerService>();
        var settings = new BehaviorSettings[]
        {
            new EasingRandomBehaviorSettings(),
        };
        var factory = new BehaviorFactory();

        // Act
        var actual = factory.CreateBehaviors(settings, mockRandomizerService.Object);

        // Assert
        Assert.Single(actual);
        Assert.Equal(typeof(EasingRandomBehavior), actual[0].GetType());
    }

    [Fact]
    public void CreateBehaviors_WhenUsingColorTransitionBehaviorSettings_CreatesCorrectBehavior()
    {
        // Arrange
        var mockRandomizerService = new Mock<IRandomizerService>();
        var settings = new BehaviorSettings[]
        {
            new ColorTransitionBehaviorSettings(),
        };
        var factory = new BehaviorFactory();

        // Act
        var actual = factory.CreateBehaviors(settings, mockRandomizerService.Object);

        // Assert
        Assert.Single(actual);
        Assert.Equal(typeof(ColorTransitionBehavior), actual[0].GetType());
    }

    [Fact]
    public void CreateBehaviors_WhenUsingRandomBehaviorSettings_CreatesCorrectBehavior()
    {
        // Arrange
        var mockRandomizerService = new Mock<IRandomizerService>();
        var settings = new BehaviorSettings[]
        {
            new RandomChoiceBehaviorSettings(),
        };
        var factory = new BehaviorFactory();

        // Act
        var actual = factory.CreateBehaviors(settings, mockRandomizerService.Object);

        // Assert
        Assert.Single(actual);
        Assert.Equal(typeof(RandomColorBehavior), actual[0].GetType());
    }
    #endregion
}