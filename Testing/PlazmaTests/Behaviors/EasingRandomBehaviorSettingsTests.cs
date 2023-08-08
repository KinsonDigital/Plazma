// <copyright file="EasingRandomBehaviorSettingsTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace PlazmaTests.Behaviors;

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
        Assert.Equal(ParticleAttribute.Angle, actual);
    }

    [Fact]
    public void StartMin_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var setting = new EasingRandomBehaviorSettings();

        // Act
        setting.StartMin = 1234f;
        var actual = setting.StartMin;

        // Assert
        Assert.Equal(1234f, actual);
    }

    [Fact]
    public void StartMax_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var setting = new EasingRandomBehaviorSettings();

        // Act
        setting.StartMax = 1234f;
        var actual = setting.StartMax;

        // Assert
        Assert.Equal(1234f, actual);
    }

    [Fact]
    public void ChangeMin_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var setting = new EasingRandomBehaviorSettings();

        // Act
        setting.ChangeMin = 1234f;
        var actual = setting.ChangeMin;

        // Assert
        Assert.Equal(1234f, actual);
    }

    [Fact]
    public void ChangeMax_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var setting = new EasingRandomBehaviorSettings();

        // Act
        setting.ChangeMax = 1234f;
        var actual = setting.ChangeMax;

        // Assert
        Assert.Equal(1234f, actual);
    }

    [Fact]
    public void TotalTimeMin_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var setting = new EasingRandomBehaviorSettings();

        // Act
        setting.TotalTimeMin = 1234f;
        var actual = setting.TotalTimeMin;

        // Assert
        Assert.Equal(1234f, actual);
    }

    [Fact]
    public void TotalTimeMax_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var setting = new EasingRandomBehaviorSettings();

        // Act
        setting.TotalTimeMax = 1234f;
        var actual = setting.TotalTimeMax;

        // Assert
        Assert.Equal(1234f, actual);
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
            ChangeMin = 10,
            ChangeMax = 20,
            StartMin = 30,
            StartMax = 40,
            TotalTimeMin = 50,
            TotalTimeMax = 60,
        };
        var otherObj = new object();

        // Act
        var actual = setting.Equals(otherObj);

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void Equals_WithEqualObjects_ReturnsTrue()
    {
        // Arrange
        var settingA = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = ParticleAttribute.Angle,
            ChangeMin = 10,
            ChangeMax = 20,
            StartMin = 30,
            StartMax = 40,
            TotalTimeMin = 50,
            TotalTimeMax = 60,
        };

        var settingB = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = ParticleAttribute.Angle,
            ChangeMin = 10,
            ChangeMax = 20,
            StartMin = 30,
            StartMax = 40,
            TotalTimeMin = 50,
            TotalTimeMax = 60,
        };

        // Act
        var actual = settingA.Equals(settingB);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void Equals_WithNonEqualObjects_ReturnsFalse()
    {
        // Arrange
        var settingA = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = ParticleAttribute.BlueColorComponent,
            ChangeMin = 100,
            ChangeMax = 200,
            StartMin = 300,
            StartMax = 400,
            TotalTimeMin = 500,
            TotalTimeMax = 600,
        };

        var settingB = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = ParticleAttribute.Angle,
            ChangeMin = 10,
            ChangeMax = 20,
            StartMin = 30,
            StartMax = 40,
            TotalTimeMin = 50,
            TotalTimeMax = 60,
        };

        // Act
        var actual = settingA.Equals(settingB);

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void GetHashCode_WhenInvoked_ReturnsCorrectResult()
    {
        // Arrange
        var settingA = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = ParticleAttribute.Angle,
            ChangeMin = 10,
            ChangeMax = 20,
            StartMin = 30,
            StartMax = 40,
            TotalTimeMin = 50,
            TotalTimeMax = 60,
        };

        var settingB = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = ParticleAttribute.Angle,
            ChangeMin = 10,
            ChangeMax = 20,
            StartMin = 30,
            StartMax = 40,
            TotalTimeMin = 50,
            TotalTimeMax = 60,
        };

        // Act
        var settingAHashCode = settingA.GetHashCode();
        var settingBHashCode = settingB.GetHashCode();

        // Assert
        Assert.Equal(settingAHashCode, settingBHashCode);
    }
    #endregion
}