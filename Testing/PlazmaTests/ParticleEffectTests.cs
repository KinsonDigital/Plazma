// <copyright file="ParticleEffectTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace PlazmaTests;

using System;
using System.Numerics;
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
        var effect = new ParticleEffect("effect-name", Array.Empty<EasingRandomBehaviorSettings>());

        // Assert
        effect.ParticleTextureName.Should().Be("effect-name");
    }

    [Fact]
    public void Ctor_WhenInvoked_SetsBehaviorSettings()
    {
        // Arrange
        var settings = new EasingRandomBehaviorSettings[]
        {
            new ()
            {
                ApplyToAttribute = ParticleAttribute.Angle,
                RandomStopMin = 11,
                RandomStopMax = 22,
                RandomStartMin = 33,
                RandomStartMax = 44,
                LifeTimeMillisecondsMin = 55,
                LifeTimeMillisecondsMax = 66,
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
        effect.SpawnLocation = new Vector2(11, 22);
        var actual = effect.SpawnLocation;

        // Assert
        actual.Should().Be(new Vector2(11, 22));
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

    /// <summary>
    /// Creates a <see cref="ParticleEffect"/> instance for the purpose of testing.
    /// </summary>
    /// <returns>The instance to return.</returns>
    private static ParticleEffect CreateEffect() => new ("test-texture", Array.Empty<EasingRandomBehaviorSettings>());
}
