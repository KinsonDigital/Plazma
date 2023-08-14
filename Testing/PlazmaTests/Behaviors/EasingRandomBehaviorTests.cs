// <copyright file="EasingRandomBehaviorTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable UseObjectOrCollectionInitializer
namespace PlazmaTests.Behaviors;

using System;
using FluentAssertions;
using NSubstitute;
using Plazma;
using Plazma.Behaviors;
using Plazma.Services;
using Xunit;

/// <summary>
/// Tests testing the <see cref="EasingRandomBehavior"/> abstract class.
/// </summary>
public class EasingRandomBehaviorTests
{
    private readonly IRandomizerService mockRandomizerService;

    /// <summary>
    /// Initializes a new instance of the <see cref="EasingRandomBehaviorTests"/> class.
    /// </summary>
    public EasingRandomBehaviorTests() => this.mockRandomizerService = Substitute.For<IRandomizerService>();

    #region Constructor Tests
    [Fact]
    public void Ctor_WhenInvoked_SetsSetting()
    {
        // Arrange
        var setting = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = ParticleAttribute.Angle,
        };
        var behavior = new EasingRandomBehavior(setting, this.mockRandomizerService);

        // Act
        var actual = behavior.ApplyToAttribute;

        // Assert
        actual.Should().Be(ParticleAttribute.Angle);
    }
    #endregion

    #region Prop Tests
    [Fact]
    public void Start_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var settings = new EasingRandomBehaviorSettings();
        var behavior = new EasingRandomBehavior(settings, this.mockRandomizerService);

        // Act
        behavior.Start = 123;
        var actual = behavior.Start;

        // Assert
        actual.Should().Be(123);
    }

    [Fact]
    public void Change_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var settings = new EasingRandomBehaviorSettings();
        var behavior = new EasingRandomBehavior(settings, this.mockRandomizerService);

        // Act
        behavior.Change = 123;
        var actual = behavior.Change;

        // Assert
        actual.Should().Be(123);
    }
    #endregion

    #region Method Tests
    [Theory]
    //              easingFunction             start      change  lifeTime      elapsedTime    expected
    [InlineData(EasingFunction.EaseOutBounce,   13,         400,    800,            200,        "13")]
    [InlineData(EasingFunction.EaseIn,          100,        600,    1000,           113,        "100")]
    public void Update_WhenInvoked_CorrectlySetsBehaviorValue(EasingFunction easingFunction, int start, int change, int lifeTime, int elapsedTime, string expected)
    {
        // Arrange
        this.mockRandomizerService.GetValue(11f, 11f).Returns(start);
        this.mockRandomizerService.GetValue(22f, 22f).Returns(change);
        this.mockRandomizerService.GetValue(33f, 33f).Returns(lifeTime);

        var settings = new EasingRandomBehaviorSettings
        {
            EasingFunctionType = easingFunction,
            RandomStartMin = 11,
            RandomStartMax = 11,
            RandomStopMin = 22,
            RandomStopMax = 22,
            LifeTimeMillisecondsMin = 33,
            LifeTimeMillisecondsMax = 33,
        };
        var behavior = new EasingRandomBehavior(settings, this.mockRandomizerService);

        // Act
        behavior.Update(new TimeSpan(0, 0, 0, 0, elapsedTime));

        // Assert
        behavior.Value.Should().Be(expected);
    }

    [Fact]
    public void Update_WhenInvoked_UpdatesElapsedTime()
    {
        // Arrange
        var settings = new EasingRandomBehaviorSettings();
        var behavior = new EasingRandomBehavior(settings, this.mockRandomizerService);

        // Act
        behavior.Update(new TimeSpan(0, 0, 0, 0, 16));

        // Assert
        behavior.ElapsedTime.Should().Be(16);
    }

    [Theory]
    [InlineData(800, true)]
    [InlineData(1500, false)]
    public void Update_WhenInvoked_ProperlySetsEnabledState(int timeElapsed, bool expected)
    {
        // Arrange
        this.mockRandomizerService.GetValue(Arg.Any<float>(), Arg.Any<float>()).Returns(1000f);
        var settings = new EasingRandomBehaviorSettings();
        var behavior = new EasingRandomBehavior(settings, this.mockRandomizerService);

        // Act
        behavior.Update(new TimeSpan(0, 0, 0, 0, timeElapsed));

        // Assert
        behavior.Enabled.Should().Be(expected);
    }

    [Fact]
    public void Reset_WhenInvoked_ResetsStartProp()
    {
        // Arrange
        var setting = new EasingRandomBehaviorSettings();
        this.mockRandomizerService.GetValue(Arg.Any<float>(), Arg.Any<float>()).Returns(123);
        var behavior = new EasingRandomBehavior(setting, this.mockRandomizerService);

        // Act
        behavior.Reset();

        // Assert
        behavior.Start.Should().Be(123);
    }

    [Fact]
    public void Reset_WhenInvoked_ResetsChangeProp()
    {
        // Arrange
        var setting = new EasingRandomBehaviorSettings();
        this.mockRandomizerService.GetValue(Arg.Any<float>(), Arg.Any<float>()).Returns(123);
        var behavior = new EasingRandomBehavior(setting, this.mockRandomizerService);

        // Act
        behavior.Reset();

        // Assert
        behavior.Change.Should().Be(123);
    }

    [Fact]
    public void Reset_WhenInvoked_ResetsElapsedTimeProp()
    {
        // Arrange
        var setting = new EasingRandomBehaviorSettings();
        var behavior = new EasingRandomBehavior(setting, this.mockRandomizerService);

        // Act
        behavior.Update(new TimeSpan(0, 0, 0, 0, 16));
        behavior.Reset();

        // Assert
        behavior.ElapsedTime.Should().Be(0);
    }

    [Fact]
    public void Reset_WhenInvoked_ResetsEnabledProp()
    {
        // Arrange
        var setting = new EasingRandomBehaviorSettings();
        var behavior = new EasingRandomBehavior(setting, this.mockRandomizerService);

        // Act
        behavior.Update(new TimeSpan(0, 0, 0, 0, 45));
        behavior.Reset();

        // Assert
        behavior.Enabled.Should().BeTrue();
    }
    #endregion
}
