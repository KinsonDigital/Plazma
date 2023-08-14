// <copyright file="BehaviorFactoryTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace PlazmaTests.Behaviors;

using System;
using Fakes;
using FluentAssertions;
using Plazma.Behaviors;
using Plazma.Services;
using NSubstitute;
using Xunit;

/// <summary>
/// Tests the <see cref="BehaviorFactory"/> class.
/// </summary>
public class BehaviorFactoryTests
{
    #region Method Tests
    [Fact]
    public void CreateBehaviors_WithNullSettingsArgument_ThrowsException()
    {
        // Arrange
        var sut = new BehaviorFactory();

        // Act
        var act = () => { sut.CreateBehaviors(null, null); };

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'settings')");
    }

    [Fact]
    public void CreateBehaviors_WhenUsingEasingRandomBehaviorSettings_CreatesCorrectBehavior()
    {
        // Arrange
        var mockRandomizerService = Substitute.For<IRandomizerService>();
        var settings = new IBehaviorSettings[]
        {
            new EasingRandomBehaviorSettings(),
        };
        var sut = new BehaviorFactory();

        // Act
        var actual = sut.CreateBehaviors(settings, mockRandomizerService);

        // Assert
        actual.Should().ContainSingle();
        actual[0].Should().BeOfType<EasingRandomBehavior>();
    }

    [Fact]
    public void CreateBehaviors_WhenUsingColorTransitionBehaviorSettings_CreatesCorrectBehavior()
    {
        // Arrange
        var mockRandomizerService = Substitute.For<IRandomizerService>();
        var settings = new IBehaviorSettings[]
        {
            new ColorTransitionBehaviorSettings(),
        };
        var sut = new BehaviorFactory();

        // Act
        var actual = sut.CreateBehaviors(settings, mockRandomizerService);

        // Assert
        actual.Should().ContainSingle();
        actual[0].Should().BeOfType<ColorTransitionBehavior>();
    }

    [Fact]
    public void CreateBehaviors_WhenUsingRandomBehaviorSettings_CreatesCorrectBehavior()
    {
        // Arrange
        var mockRandomizerService = Substitute.For<IRandomizerService>();
        var settings = new IBehaviorSettings[]
        {
            new RandomChoiceBehaviorSettings(),
        };
        var sut = new BehaviorFactory();

        // Act
        var actual = sut.CreateBehaviors(settings, mockRandomizerService);

        // Assert
        actual.Should().ContainSingle();
        actual[0].Should().BeOfType<RandomColorBehavior>();
    }

    [Fact]
    public void CreateBehaviors_WithNullRandomizerServiceArgument_ThrowsException()
    {
        // Arrange
        var sut = new BehaviorFactory();

        // Act
        var act = () =>
        {
            var settings = new IBehaviorSettings[] { new ColorTransitionBehaviorSettings(), };
            sut.CreateBehaviors(settings, null);
        };

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'randomService')");
    }

    [Fact]
    public void CreateBehaviors_WhenUsingUnknownBehaviorSettingsType_ThrowsException()
    {
        // Arrange
        var mockRandomizerService = Substitute.For<IRandomizerService>();
        var unknownSettingsType = new IBehaviorSettings[]
        {
            new FakeBehaviorSettings(),
        };
        var sut = new BehaviorFactory();

        // Act
        var act = () => sut.CreateBehaviors(unknownSettingsType, mockRandomizerService);

        // Assert
        act.Should().Throw<Exception>()
            .WithMessage($"Unknown behavior settings of type '{nameof(FakeBehaviorSettings)}'.");
    }
    #endregion
}
