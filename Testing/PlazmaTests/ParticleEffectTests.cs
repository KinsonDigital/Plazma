// <copyright file="ParticleEffectTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace PlazmaTests;

using System;
using System.Drawing;
using FluentAssertions;
using Plazma;
using Plazma.Behaviors;
using Xunit;

/// <summary>
/// Tests the <see cref="ParticleEffect"/> class.
/// </summary>
public class ParticleEffectTests
{
    #region Constructor Tests
    [Fact]
    public void Ctor_WhenInvoked_SetsParticleTextureName()
    {
        // Act
        var effect = new ParticleEffect("effect-name", Array.Empty<BehaviorSettings>());

        // Assert
        effect.ParticleTextureName.Should().Be("effect-name");
    }

    [Fact]
    public void Ctor_WhenInvoked_SetsBehaviorSettings()
    {
        // Arrange
        var settings = new BehaviorSettings[]
        {
            new EasingRandomBehaviorSettings
            {
                ApplyToAttribute = ParticleAttribute.Angle,
                ChangeMin = 11,
                ChangeMax = 22,
                StartMin = 33,
                StartMax = 44,
                TotalTimeMin = 55,
                TotalTimeMax = 66,
            },
        };
        var effect = new ParticleEffect(null, settings);

        // Act
        var actual = effect.BehaviorSettings;

        // Assert
        settings[0].Should().Be(actual[0]);
    }
    #endregion

    #region Prop Tests
    [Fact]
    public void SpawnLocation_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var effect = CreateEffect();

        // Act
        effect.SpawnLocation = new PointF(11, 22);
        var actual = effect.SpawnLocation;

        // Assert
        actual.Should().Be(new PointF(11, 22));
    }

    [Fact]
    public void TotalParticlesAliveAtOnce_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var effect = CreateEffect();

        // Act
        effect.TotalParticlesAliveAtOnce = 1234;
        var actual = effect.TotalParticlesAliveAtOnce;

        // Assert
        actual.Should().Be(1234);
    }

    [Fact]
    public void SpawnRateMin_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var effect = CreateEffect();

        // Act
        effect.SpawnRateMin = 1234;
        var actual = effect.SpawnRateMin;

        // Assert
        actual.Should().Be(1234);
    }

    [Fact]
    public void SpawnRateMax_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var effect = CreateEffect();

        // Act
        effect.SpawnRateMax = 1234;
        var actual = effect.SpawnRateMax;

        // Assert
        actual.Should().Be(1234);
    }

    [Fact]
    public void UseColorsFromList_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var effect = CreateEffect();

        // Act
        effect.UseColorsFromList = true;
        var actual = effect.UseColorsFromList;

        // Assert
        actual.Should().BeTrue();
    }
    #endregion

    #region Method Tests
    [Fact]
    public void Equals_WithDifferentObjects_ReturnsFalse()
    {
        // Arrange
        var settings = new BehaviorSettings[]
        {
            new EasingRandomBehaviorSettings
            {
                ApplyToAttribute = ParticleAttribute.Angle,
                ChangeMin = 11,
                ChangeMax = 22,
                StartMin = 33,
                StartMax = 44,
                TotalTimeMin = 55,
                TotalTimeMax = 66,
            },
        };

        var effect = new ParticleEffect("test-name", settings)
        {
            SpawnLocation = new PointF(11, 22),
            SpawnRateMin = 11,
            SpawnRateMax = 22,
            TotalParticlesAliveAtOnce = 33,
            UseColorsFromList = true,
        };
        var otherObj = new object();

        // Act
        var actual = effect.Equals(otherObj);

        // Assert
        actual.Should().BeFalse();
    }

    [Fact]
    public void Equals_WithEqualObjects_ReturnsTrue()
    {
        // Arrange
        var settings = new BehaviorSettings[]
        {
            new EasingRandomBehaviorSettings
            {
                ApplyToAttribute = ParticleAttribute.Angle,
                ChangeMin = 11,
                ChangeMax = 22,
                StartMin = 33,
                StartMax = 44,
                TotalTimeMin = 55,
                TotalTimeMax = 66,
            },
        };

        var effectA = new ParticleEffect("test-name", settings)
        {
            SpawnLocation = new PointF(11, 22),
            SpawnRateMin = 11,
            SpawnRateMax = 22,
            TotalParticlesAliveAtOnce = 33,
            UseColorsFromList = true,
        };

        var effectB = new ParticleEffect("test-name", settings)
        {
            SpawnLocation = new PointF(11, 22),
            SpawnRateMin = 11,
            SpawnRateMax = 22,
            TotalParticlesAliveAtOnce = 33,
            UseColorsFromList = true,
        };

        // Act
        var actual = effectA.Equals(effectB);

        // Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public void Equals_WithNonEqualObjects_ReturnsFalse()
    {
        // Arrange
        var settings = new BehaviorSettings[]
        {
            new EasingRandomBehaviorSettings
            {
                ApplyToAttribute = ParticleAttribute.Angle,
                ChangeMin = 11,
                ChangeMax = 22,
                StartMin = 33,
                StartMax = 44,
                TotalTimeMin = 55,
                TotalTimeMax = 66,
            },
        };

        var effectA = new ParticleEffect("test-name", settings)
        {
            SpawnLocation = new PointF(11, 22),
            SpawnRateMin = 11,
            SpawnRateMax = 22,
            TotalParticlesAliveAtOnce = 33,
            UseColorsFromList = true,
        };

        var effectB = new ParticleEffect("effect-bee", settings)
        {
            SpawnLocation = new PointF(99, 88),
            SpawnRateMin = 77,
            SpawnRateMax = 66,
            TotalParticlesAliveAtOnce = 55,
            UseColorsFromList = false,
        };

        // Act
        var actual = effectA.Equals(effectB);

        // Assert
        actual.Should().BeFalse();
    }

    [Fact]
    public void Equals_WithDifferentTintColorTotals_ReturnsFalse()
    {
        // Arrange
        var settings = new BehaviorSettings[]
        {
            new EasingRandomBehaviorSettings
            {
                ApplyToAttribute = ParticleAttribute.Angle,
                ChangeMin = 11,
                ChangeMax = 22,
                StartMin = 33,
                StartMax = 44,
                TotalTimeMin = 55,
                TotalTimeMax = 66,
            },
        };

        var effectA = new ParticleEffect("test-name", settings)
        {
            SpawnLocation = new PointF(11, 22),
            SpawnRateMin = 11,
            SpawnRateMax = 22,
            TotalParticlesAliveAtOnce = 33,
            UseColorsFromList = true,
        };

        var effectB = new ParticleEffect("effect-name", settings)
        {
            SpawnLocation = new PointF(11, 22),
            SpawnRateMin = 11,
            SpawnRateMax = 22,
            TotalParticlesAliveAtOnce = 33,
            UseColorsFromList = true,
        };

        // Act
        var actual = effectA.Equals(effectB);

        // Assert
        actual.Should().BeFalse();
    }
    #endregion

    /// <summary>
    /// Creates a <see cref="ParticleEffect"/> instance for the purpose of testing.
    /// </summary>
    /// <returns>The instance to return.</returns>
    private static ParticleEffect CreateEffect() => new ("test-texture", Array.Empty<BehaviorSettings>());
}
