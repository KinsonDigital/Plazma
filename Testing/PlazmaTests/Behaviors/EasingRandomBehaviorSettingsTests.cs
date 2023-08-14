// <copyright file="EasingRandomBehaviorSettingsTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable UseObjectOrCollectionInitializer
namespace PlazmaTests.Behaviors;

using FluentAssertions;
using Plazma;
using Plazma.Behaviors;
using Xunit;

/// <summary>
/// Tests the <see cref="EasingRandomBehaviorSettings"/> class.
/// </summary>
public class EasingRandomBehaviorSettingsTests
{
    #region Prop Tests
    [Fact]
    public void ApplyToAttribute_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var setting = new EasingRandomBehaviorSettings();

        // Act
        setting.ApplyToAttribute = ParticleAttribute.Angle;
        var actual = setting.ApplyToAttribute;

        // Assert
        actual.Should().Be(ParticleAttribute.Angle);
    }

    [Fact]
    public void StartMin_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var setting = new EasingRandomBehaviorSettings();

        // Act
        setting.RandomStartMin = 1234f;
        var actual = setting.RandomStartMin;

        // Assert
        actual.Should().Be(1234f);
    }

    [Fact]
    public void StartMax_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var setting = new EasingRandomBehaviorSettings();

        // Act
        setting.RandomStartMax = 1234f;
        var actual = setting.RandomStartMax;

        // Assert
        actual.Should().Be(1234f);
    }

    [Fact]
    public void RandomStopMin_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var setting = new EasingRandomBehaviorSettings();

        // Act
        setting.RandomStopMin = 1234f;
        var actual = setting.RandomStopMin;

        // Assert
        actual.Should().Be(1234f);
    }

    [Fact]
    public void RandomStopMax_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var setting = new EasingRandomBehaviorSettings();

        // Act
        setting.RandomStopMax = 1234f;
        var actual = setting.RandomStopMax;

        // Assert
        actual.Should().Be(1234f);
    }

    [Fact]
    public void LifeTimeMillisecondsMin_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var setting = new EasingRandomBehaviorSettings();

        // Act
        setting.LifeTimeMillisecondsMin = 1234f;
        var actual = setting.LifeTimeMillisecondsMin;

        // Assert
        actual.Should().Be(1234f);
    }

    [Fact]
    public void LifeTimeMillisecondsMax_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var setting = new EasingRandomBehaviorSettings();

        // Act
        setting.LifeTimeMillisecondsMax = 1234f;
        var actual = setting.LifeTimeMillisecondsMax;

        // Assert
        actual.Should().Be(1234f);
    }
    #endregion

    #region Method Tests
    [Fact]
    public void Equals_WithDifferentObjectTypes_ReturnsFalse()
    {
        // Arrange
        var setting = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = ParticleAttribute.Angle,
            RandomStopMin = 10,
            RandomStopMax = 20,
            RandomStartMin = 30,
            RandomStartMax = 40,
            LifeTimeMillisecondsMin = 50,
            LifeTimeMillisecondsMax = 60,
        };
        var otherObj = new object();

        // Act
        var actual = setting.Equals(otherObj);

        // Assert
        actual.Should().BeFalse();
    }

    [Fact]
    public void Equals_WithEqualObjects_ReturnsTrue()
    {
        // Arrange
        var settingA = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = ParticleAttribute.Angle,
            RandomStopMin = 10,
            RandomStopMax = 20,
            RandomStartMin = 30,
            RandomStartMax = 40,
            LifeTimeMillisecondsMin = 50,
            LifeTimeMillisecondsMax = 60,
        };

        var settingB = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = ParticleAttribute.Angle,
            RandomStopMin = 10,
            RandomStopMax = 20,
            RandomStartMin = 30,
            RandomStartMax = 40,
            LifeTimeMillisecondsMin = 50,
            LifeTimeMillisecondsMax = 60,
        };

        // Act & Assert
        settingA.ApplyToAttribute.Should().Be(settingB.ApplyToAttribute);
        settingA.RandomStopMin.Should().Be(settingB.RandomStopMin);
        settingA.RandomStopMax.Should().Be(settingB.RandomStopMax);
        settingA.RandomStartMin.Should().Be(settingB.RandomStartMin);
        settingA.RandomStartMax.Should().Be(settingB.RandomStartMax);
        settingA.LifeTimeMillisecondsMin.Should().Be(settingB.LifeTimeMillisecondsMin);
        settingA.LifeTimeMillisecondsMax.Should().Be(settingB.LifeTimeMillisecondsMax);
    }

    [Fact]
    public void Equals_WithNonEqualObjects_ReturnsFalse()
    {
        // Arrange
        var settingA = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = ParticleAttribute.BlueColorComponent,
            RandomStopMin = 100,
            RandomStopMax = 200,
            RandomStartMin = 300,
            RandomStartMax = 400,
            LifeTimeMillisecondsMin = 500,
            LifeTimeMillisecondsMax = 600,
        };

        var settingB = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = ParticleAttribute.Angle,
            RandomStopMin = 10,
            RandomStopMax = 20,
            RandomStartMin = 30,
            RandomStartMax = 40,
            LifeTimeMillisecondsMin = 50,
            LifeTimeMillisecondsMax = 60,
        };

        // Act
        var actual = settingA.Equals(settingB);

        // Assert
        actual.Should().BeFalse();
    }
    #endregion
}
