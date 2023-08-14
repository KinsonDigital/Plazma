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
    public void ChangeMin_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var setting = new EasingRandomBehaviorSettings();

        // Act
        setting.RandomChangeMin = 1234f;
        var actual = setting.RandomChangeMin;

        // Assert
        actual.Should().Be(1234f);
    }

    [Fact]
    public void ChangeMax_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var setting = new EasingRandomBehaviorSettings();

        // Act
        setting.RandomChangeMax = 1234f;
        var actual = setting.RandomChangeMax;

        // Assert
        actual.Should().Be(1234f);
    }

    [Fact]
    public void TotalTimeMin_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var setting = new EasingRandomBehaviorSettings();

        // Act
        setting.LifeTimeMinMilliseconds = 1234f;
        var actual = setting.LifeTimeMinMilliseconds;

        // Assert
        actual.Should().Be(1234f);
    }

    [Fact]
    public void TotalTimeMax_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var setting = new EasingRandomBehaviorSettings();

        // Act
        setting.LifeTimeMaxMilliseconds = 1234f;
        var actual = setting.LifeTimeMaxMilliseconds;

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
            RandomChangeMin = 10,
            RandomChangeMax = 20,
            RandomStartMin = 30,
            RandomStartMax = 40,
            LifeTimeMinMilliseconds = 50,
            LifeTimeMaxMilliseconds = 60,
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
            RandomChangeMin = 10,
            RandomChangeMax = 20,
            RandomStartMin = 30,
            RandomStartMax = 40,
            LifeTimeMinMilliseconds = 50,
            LifeTimeMaxMilliseconds = 60,
        };

        var settingB = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = ParticleAttribute.Angle,
            RandomChangeMin = 10,
            RandomChangeMax = 20,
            RandomStartMin = 30,
            RandomStartMax = 40,
            LifeTimeMinMilliseconds = 50,
            LifeTimeMaxMilliseconds = 60,
        };

        // Act & Assert
        settingA.ApplyToAttribute.Should().Be(settingB.ApplyToAttribute);
        settingA.RandomChangeMin.Should().Be(settingB.RandomChangeMin);
        settingA.RandomChangeMax.Should().Be(settingB.RandomChangeMax);
        settingA.RandomStartMin.Should().Be(settingB.RandomStartMin);
        settingA.RandomStartMax.Should().Be(settingB.RandomStartMax);
        settingA.LifeTimeMinMilliseconds.Should().Be(settingB.LifeTimeMinMilliseconds);
        settingA.LifeTimeMaxMilliseconds.Should().Be(settingB.LifeTimeMaxMilliseconds);
    }

    [Fact]
    public void Equals_WithNonEqualObjects_ReturnsFalse()
    {
        // Arrange
        var settingA = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = ParticleAttribute.BlueColorComponent,
            RandomChangeMin = 100,
            RandomChangeMax = 200,
            RandomStartMin = 300,
            RandomStartMax = 400,
            LifeTimeMinMilliseconds = 500,
            LifeTimeMaxMilliseconds = 600,
        };

        var settingB = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = ParticleAttribute.Angle,
            RandomChangeMin = 10,
            RandomChangeMax = 20,
            RandomStartMin = 30,
            RandomStartMax = 40,
            LifeTimeMinMilliseconds = 50,
            LifeTimeMaxMilliseconds = 60,
        };

        // Act
        var actual = settingA.Equals(settingB);

        // Assert
        actual.Should().BeFalse();
    }
    #endregion
}
