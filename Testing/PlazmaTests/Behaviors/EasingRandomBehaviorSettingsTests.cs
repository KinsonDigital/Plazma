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
        // Arrange & Act
        var setting = new EasingRandomBehaviorSettings { ApplyToAttribute = ParticleAttribute.Angle };

        // Assert
        setting.ApplyToAttribute.Should().Be(ParticleAttribute.Angle);
    }

    [Fact]
    public void EasingFunctionType_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange & Act
        var setting = new EasingRandomBehaviorSettings { EasingFunctionType = EasingFunction.EaseIn };

        // Assert
        setting.EasingFunctionType.Should().Be(EasingFunction.EaseIn);
    }

    [Fact]
    public void RandomStartMin_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange & Act
        var setting = new EasingRandomBehaviorSettings { RandomStartMin = 1234f };

        // Assert
        setting.RandomStartMin.Should().Be(1234f);
    }

    [Fact]
    public void RandomStartMax_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange & Act
        var setting = new EasingRandomBehaviorSettings { RandomStartMax = 1234f };

        // Assert
        setting.RandomStartMax.Should().Be(1234f);
    }

    [Fact]
    public void RandomStopMin_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange & Act
        var setting = new EasingRandomBehaviorSettings { RandomStopMin = 1234f };

        // Assert
        setting.RandomStopMin.Should().Be(1234f);
    }

    [Fact]
    public void RandomStopMax_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange & Act
        var setting = new EasingRandomBehaviorSettings { RandomStopMax = 1234f };

        // Assert
        setting.RandomStopMax.Should().Be(1234f);
    }

    [Fact]
    public void UpdateRandomStartMin_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var func = () => 123f;

        // Act
        var setting = new EasingRandomBehaviorSettings { UpdateRandomStartMin = func };

        // Assert
        setting.UpdateRandomStartMin.Should().BeSameAs(func);
    }

    [Fact]
    public void UpdateRandomStartMax_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var func = () => 123f;

        // Act
        var setting = new EasingRandomBehaviorSettings { UpdateRandomStartMax = func };

        // Assert
        setting.UpdateRandomStartMax.Should().BeSameAs(func);
    }

    [Fact]
    public void UpdateRandomStopMin_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var func = () => 123f;

        // Act
        var setting = new EasingRandomBehaviorSettings { UpdateRandomStopMin = func };

        // Assert
        setting.UpdateRandomStopMin.Should().BeSameAs(func);
    }

    [Fact]
    public void UpdateRandomStopMax_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var func = () => 123f;

        // Act
        var setting = new EasingRandomBehaviorSettings { UpdateRandomStopMax = func };

        // Assert
        setting.UpdateRandomStopMax.Should().BeSameAs(func);
    }

    [Fact]
    public void LifeTimeMillisecondsMin_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange & Act
        var setting = new EasingRandomBehaviorSettings { LifeTimeMillisecondsMin = 1234f };

        // Assert
        setting.LifeTimeMillisecondsMin.Should().Be(1234f);
    }

    [Fact]
    public void LifeTimeMillisecondsMax_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange & Act
        var setting = new EasingRandomBehaviorSettings { LifeTimeMillisecondsMax = 1234f };

        // Assert
        setting.LifeTimeMillisecondsMax.Should().Be(1234f);
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
