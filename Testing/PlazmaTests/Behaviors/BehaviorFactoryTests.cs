// <copyright file="BehaviorFactoryTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace PlazmaTests.Behaviors;

using System;
using FluentAssertions;
using Plazma.Behaviors;
using Plazma.Services;
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
        // Arrange
        var sut = new BehaviorFactory();

        // Act
        var act = () => sut.CreateBehaviors(null, new Mock<IRandomizerService>().Object);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'settings')");
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
        var sut = new BehaviorFactory();

        // Act
        var actual = sut.CreateBehaviors(settings, mockRandomizerService.Object);

        // Assert
        actual.Should().ContainSingle();
        actual[0].Should().BeOfType<EasingRandomBehavior>();
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
        var sut = new BehaviorFactory();

        // Act
        var actual = sut.CreateBehaviors(settings, mockRandomizerService.Object);

        // Assert
        actual.Should().ContainSingle();
        actual[0].Should().BeOfType<ColorTransitionBehavior>();
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
        var sut = new BehaviorFactory();

        // Act
        var actual = sut.CreateBehaviors(settings, mockRandomizerService.Object);

        // Assert
        actual.Should().ContainSingle();
        actual[0].Should().BeOfType<RandomColorBehavior>();
    }
    #endregion
}
