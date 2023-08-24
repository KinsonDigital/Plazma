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
        var setting = new EasingRandomBehaviorSettings { ApplyToAttribute = BehaviorAttribute.Angle };

        // Assert
        setting.ApplyToAttribute.Should().Be(BehaviorAttribute.Angle);
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
        var setting = new EasingRandomBehaviorSettings { RandomChangeMin = 1234f };

        // Assert
        setting.RandomChangeMin.Should().Be(1234f);
    }

    [Fact]
    public void RandomStopMax_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange & Act
        var setting = new EasingRandomBehaviorSettings { RandomChangeMax = 1234f };

        // Assert
        setting.RandomChangeMax.Should().Be(1234f);
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
    public void UpdateRandomChangeMin_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var func = () => 123f;

        // Act
        var setting = new EasingRandomBehaviorSettings { UpdateRandomChangeMin = func };

        // Assert
        setting.UpdateRandomChangeMin.Should().BeSameAs(func);
    }

    [Fact]
    public void UpdateRandomChangeMax_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var func = () => 123f;

        // Act
        var setting = new EasingRandomBehaviorSettings { UpdateRandomChangeMax = func };

        // Assert
        setting.UpdateRandomChangeMax.Should().BeSameAs(func);
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
            ApplyToAttribute = BehaviorAttribute.Angle,
            RandomChangeMin = 10,
            RandomChangeMax = 20,
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
            ApplyToAttribute = BehaviorAttribute.Angle,
            RandomChangeMin = 10,
            RandomChangeMax = 20,
            RandomStartMin = 30,
            RandomStartMax = 40,
            LifeTimeMillisecondsMin = 50,
            LifeTimeMillisecondsMax = 60,
        };

        var settingB = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = BehaviorAttribute.Angle,
            RandomChangeMin = 10,
            RandomChangeMax = 20,
            RandomStartMin = 30,
            RandomStartMax = 40,
            LifeTimeMillisecondsMin = 50,
            LifeTimeMillisecondsMax = 60,
        };

        // Act & Assert
        settingA.ApplyToAttribute.Should().Be(settingB.ApplyToAttribute);
        settingA.RandomChangeMin.Should().Be(settingB.RandomChangeMin);
        settingA.RandomChangeMax.Should().Be(settingB.RandomChangeMax);
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
            ApplyToAttribute = BehaviorAttribute.BlueColorComponent,
            RandomChangeMin = 100,
            RandomChangeMax = 200,
            RandomStartMin = 300,
            RandomStartMax = 400,
            LifeTimeMillisecondsMin = 500,
            LifeTimeMillisecondsMax = 600,
        };

        var settingB = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = BehaviorAttribute.Angle,
            RandomChangeMin = 10,
            RandomChangeMax = 20,
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
