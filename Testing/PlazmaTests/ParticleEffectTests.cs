// <copyright file="ParticleEffectTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace PlazmaTests;

using System;
using System.Diagnostics.CodeAnalysis;
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
    [SuppressMessage(
        "StyleCop.CSharp.ReadabilityRules",
        "SA1129:Do not use default value type constructor",
        Justification = "Required for testing purposes.")]
    public void Ctor_WhenInvokingParameterlessCtor_DoesNotChangeDefaultValues()
    {
        // Arrange & Act
        var sut = new ParticleEffect();

        // Assert
        sut.ParticleTextureName.Should().Be(string.Empty);
        sut.SpawnLocation.Should().Be(new Vector2(0, 0));
        sut.TotalParticles.Should().Be(1);
        sut.SpawnRateMin.Should().Be(250);
        sut.SpawnRateMax.Should().Be(1000);
        sut.LimitSpawnRate.Should().Be(true);
        sut.BurstEnabled.Should().Be(false);
        sut.BurstSpawnRateMin.Should().Be(0);
        sut.BurstSpawnRateMax.Should().Be(250);
        sut.BurstOnMilliseconds.Should().Be(3000);
        sut.BurstOffMilliseconds.Should().Be(1000);
        sut.UseColorsFromList.Should().Be(false);
        sut.BehaviorSettings.Should().NotBeNull();
        sut.BehaviorSettings.Should().BeEmpty();
    }

    [Fact]
    public void Ctor_WhenInvoked_SetsParticleTextureName()
    {
        // Act
        var effect = new ParticleEffect("effect-name", []);

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
                ApplyToAttribute = BehaviorAttribute.Angle,
                RandomChangeMin = 11,
                RandomChangeMax = 22,
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
    public void SpawnLocation_WhenSettingInitValue_ReturnsCorrectResult()
    {
        // Arrange
        var effect = CreateEffect();

        // Act
        effect = effect with { SpawnLocation = new Vector2(11, 22) };

        // Assert
        effect.SpawnLocation.Should().Be(new Vector2(11, 22));
    }

    [Fact]
    public void TotalParticles_WhenSettingInitValue_ReturnsCorrectResult()
    {
        // Arrange
        var effect = CreateEffect();

        // Act
        effect = effect with { TotalParticles = 1234 };

        // Assert
        effect.TotalParticles.Should().Be(1234);
    }

    [Fact]
    public void SpawnRateMin_WhenSettingInitValue_ReturnsCorrectResult()
    {
        // Arrange
        var effect = CreateEffect();

        // Act
        effect = effect with { SpawnRateMin = 1234 };

        // Assert
        effect.SpawnRateMin.Should().Be(1234);
    }

    [Fact]
    public void SpawnRateMax_WhenSettingInitValue_ReturnsCorrectResult()
    {
        // Arrange
        var effect = CreateEffect();

        // Act
        effect = effect with { SpawnRateMax = 1234 };

        // Assert
        effect.SpawnRateMax.Should().Be(1234);
    }

    [Fact]
    public void LimitSpawnRate_WhenSettingInitValue_ReturnsCorrectResult()
    {
        // Arrange
        var effect = CreateEffect();

        // Act
        var expected = !effect.LimitSpawnRate;
        effect = effect with { LimitSpawnRate = !effect.LimitSpawnRate };

        // Assert
        effect.LimitSpawnRate.Should().Be(expected);
    }

    [Fact]
    public void BurstEnabled_WhenSettingInitValue_ReturnsCorrectResult()
    {
        // Arrange
        var effect = CreateEffect();

        // Act
        var expected = !effect.BurstEnabled;
        effect = effect with { BurstEnabled = !effect.BurstEnabled };

        // Assert
        effect.BurstEnabled.Should().Be(expected);
    }

    [Fact]
    public void BurstSpawnRateMin_WhenSettingInitValue_ReturnsCorrectResult()
    {
        // Arrange
        var effect = CreateEffect();

        // Act
        effect = effect with { BurstSpawnRateMin = 123f };

        // Assert
        effect.BurstSpawnRateMin.Should().Be(123f);
    }

    [Fact]
    public void BurstSpawnRateMax_WhenSettingInitValue_ReturnsCorrectResult()
    {
        // Arrange
        var effect = CreateEffect();

        // Act
        effect = effect with { BurstSpawnRateMax = 123f };

        // Assert
        effect.BurstSpawnRateMax.Should().Be(123f);
    }

    [Fact]
    public void BurstOnMilliseconds_WhenSettingInitValue_ReturnsCorrectResult()
    {
        // Arrange
        var effect = CreateEffect();

        // Act
        effect = effect with { BurstOnMilliseconds = 123f };

        // Assert
        effect.BurstOnMilliseconds.Should().Be(123f);
    }

    [Fact]
    public void BurstOffMilliseconds_WhenSettingInitValue_ReturnsCorrectResult()
    {
        // Arrange
        var effect = CreateEffect();

        // Act
        effect = effect with { BurstOffMilliseconds = 123f };

        // Assert
        effect.BurstOffMilliseconds.Should().Be(123f);
    }

    [Fact]
    public void BehaviorSettings_WhenSettingInitValue_ReturnsCorrectResult()
    {
        // Arrange
        var expected = new[]
        {
            new EasingRandomBehaviorSettings
            {
                ApplyToAttribute = BehaviorAttribute.Angle,
                RandomChangeMin = 1,
                RandomChangeMax = 2,
                RandomStartMin = 3,
                RandomStartMax = 4,
                LifeTimeMillisecondsMin = 5,
                LifeTimeMillisecondsMax = 6,
                EasingFunctionType = EasingFunction.EaseIn,
            },
            new EasingRandomBehaviorSettings
            {
                ApplyToAttribute = BehaviorAttribute.Size,
                RandomChangeMin = 11,
                RandomChangeMax = 22,
                RandomStartMin = 33,
                RandomStartMax = 44,
                LifeTimeMillisecondsMin = 55,
                LifeTimeMillisecondsMax = 66,
                EasingFunctionType = EasingFunction.EaseOutBounce,
            },
        };
        var effect = CreateEffect();

        // Act
        effect = effect with { BehaviorSettings = Array.AsReadOnly(expected) };

        // Assert
        effect.BehaviorSettings.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void UseColorsFromList_WhenSettingInitValue_ReturnsCorrectResult()
    {
        // Arrange
        var effect = CreateEffect();

        // Act
        effect = effect with { UseColorsFromList = true };

        // Assert
        effect.UseColorsFromList.Should().BeTrue();
    }
    #endregion

    /// <summary>
    /// Creates a <see cref="ParticleEffect"/> instance for the purpose of testing.
    /// </summary>
    /// <returns>The instance to return.</returns>
    private static ParticleEffect CreateEffect() => new ("test-texture", []);
}
