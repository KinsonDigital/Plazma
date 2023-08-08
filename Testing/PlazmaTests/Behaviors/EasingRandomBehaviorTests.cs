// <copyright file="EasingRandomBehaviorTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace PlazmaTests.Behaviors;

using System;
using Plazma;
using Plazma.Behaviors;
using Plazma.Services;
using Moq;
using Xunit;

/// <summary>
/// Tests testing the <see cref="EasingRandomBehavior"/> abstract class.
/// </summary>
public class EasingRandomBehaviorTests : IDisposable
{
    private Mock<IRandomizerService> mockRandomizerService;

    /// <summary>
    /// Initializes a new instance of the <see cref="EasingRandomBehaviorTests"/> class.
    /// </summary>
    public EasingRandomBehaviorTests() => this.mockRandomizerService = new Mock<IRandomizerService>();

    #region Constructor Tests
    [Fact]
    public void Ctor_WhenInvoked_SetsSetting()
    {
        // Arrange
        var setting = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = ParticleAttribute.Angle,
        };
        var behavior = new EasingRandomBehavior(setting, this.mockRandomizerService.Object);

        // Act
        var actual = behavior.ApplyToAttribute;

        // Assert
        Assert.Equal(ParticleAttribute.Angle, actual);
    }
    #endregion

    #region Prop Tests
    [Fact]
    public void Start_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var settings = new EasingRandomBehaviorSettings();
        var behavior = new EasingRandomBehavior(settings, this.mockRandomizerService.Object);

        // Act
        behavior.Start = 123;
        var actual = behavior.Start;

        // Assert
        Assert.Equal(123, actual);
    }

    [Fact]
    public void Change_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var settings = new EasingRandomBehaviorSettings();
        var behavior = new EasingRandomBehavior(settings, this.mockRandomizerService.Object);

        // Act
        behavior.Change = 123;
        var actual = behavior.Change;

        // Assert
        Assert.Equal(123, actual);
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
        this.mockRandomizerService.Setup(m => m.GetValue(11f, 11f)).Returns(start);
        this.mockRandomizerService.Setup(m => m.GetValue(22f, 22f)).Returns(change);
        this.mockRandomizerService.Setup(m => m.GetValue(33f, 33f)).Returns(lifeTime);

        var settings = new EasingRandomBehaviorSettings
        {
            EasingFunctionType = easingFunction,
            StartMin = 11,
            StartMax = 11,
            ChangeMin = 22,
            ChangeMax = 22,
            TotalTimeMin = 33,
            TotalTimeMax = 33,
        };
        var behavior = new EasingRandomBehavior(settings, this.mockRandomizerService.Object);

        // Act
        behavior.Update(new TimeSpan(0, 0, 0, 0, elapsedTime));

        // Assert
        Assert.Equal(expected, behavior.Value);
    }

    [Fact]
    public void Update_WhenInvoked_UpdatesElapsedTime()
    {
        // Arrange
        var settings = new EasingRandomBehaviorSettings();
        var behavior = new EasingRandomBehavior(settings, this.mockRandomizerService.Object);

        // Act
        behavior.Update(new TimeSpan(0, 0, 0, 0, 16));

        // Assert
        Assert.Equal(16, behavior.ElapsedTime);
    }

    [Theory]
    [InlineData(800, true)]
    [InlineData(1500, false)]
    public void Update_WhenInvoked_ProperlySetsEnabledState(int timeElapsed, bool expected)
    {
        // Arrange
        this.mockRandomizerService.Setup(m => m.GetValue(It.IsAny<float>(), It.IsAny<float>())).Returns(1000f);
        var settings = new EasingRandomBehaviorSettings();
        var behavior = new EasingRandomBehavior(settings, this.mockRandomizerService.Object);

        // Act
        behavior.Update(new TimeSpan(0, 0, 0, 0, timeElapsed));

        // Assert
        Assert.Equal(expected, behavior.Enabled);
    }

    [Fact]
    public void Reset_WhenInvoked_ResetsStartProp()
    {
        // Arrange
        var setting = new EasingRandomBehaviorSettings();
        this.mockRandomizerService.Setup(m => m.GetValue(It.IsAny<float>(), It.IsAny<float>())).Returns(123);
        var behavior = new EasingRandomBehavior(setting, this.mockRandomizerService.Object);

        // Act
        behavior.Reset();

        // Assert
        Assert.Equal(123, behavior.Start);
    }

    [Fact]
    public void Reset_WhenInvoked_ResetsChangeProp()
    {
        // Arrange
        var setting = new EasingRandomBehaviorSettings();
        this.mockRandomizerService.Setup(m => m.GetValue(It.IsAny<float>(), It.IsAny<float>())).Returns(123);
        var behavior = new EasingRandomBehavior(setting, this.mockRandomizerService.Object);

        // Act
        behavior.Reset();

        // Assert
        Assert.Equal(123, behavior.Change);
    }

    [Fact]
    public void Reset_WhenInvoked_ResetsElapsedTimeProp()
    {
        // Arrange
        var setting = new EasingRandomBehaviorSettings();
        var behavior = new EasingRandomBehavior(setting, this.mockRandomizerService.Object);

        // Act
        behavior.Update(new TimeSpan(0, 0, 0, 0, 16));
        behavior.Reset();

        // Assert
        Assert.Equal(0, behavior.ElapsedTime);
    }

    [Fact]
    public void Reset_WhenInvoked_ResetsEnabledProp()
    {
        // Arrange
        var setting = new EasingRandomBehaviorSettings();
        var behavior = new EasingRandomBehavior(setting, this.mockRandomizerService.Object);

        // Act
        behavior.Update(new TimeSpan(0, 0, 0, 0, 45));
        behavior.Reset();

        // Assert
        Assert.True(behavior.Enabled);
    }
    #endregion

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public void Dispose()
    {
        this.mockRandomizerService = null;
        GC.SuppressFinalize(this);
    }
}