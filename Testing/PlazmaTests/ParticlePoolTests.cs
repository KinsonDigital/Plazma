// <copyright file="ParticlePoolTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace PlazmaTests;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Numerics;
using FluentAssertions;
using Plazma;
using Plazma.Behaviors;
using Plazma.Services;
using NSubstitute;
using Xunit;

/// <summary>
/// Tests the <see cref="ParticlePool{Texture}"/> class.
/// </summary>
public class ParticlePoolTests
{
    private const string ParticleTextureName = "particle-texture";
    private readonly IRandomizerService mockRandomizerService;
    private readonly ITextureLoader<IDisposable> mockTextureLoader;
    private readonly IBehaviorFactory mockBehaviorFactory;
    private readonly BehaviorSettings[] settings;
    private readonly ParticleEffect effect;

    /// <summary>
    /// Initializes a new instance of the <see cref="ParticlePoolTests"/> class.
    /// </summary>
    public ParticlePoolTests()
    {
        this.settings = new BehaviorSettings[]
        {
            new EasingRandomBehaviorSettings(),
        };
        this.effect = new ParticleEffect(ParticleTextureName, this.settings);

        this.mockRandomizerService = Substitute.For<IRandomizerService>();
        this.mockTextureLoader = Substitute.For<ITextureLoader<IDisposable>>();

        var mockBehavior = Substitute.For<IBehavior>();
        mockBehavior.Value.Returns("0");
        mockBehavior.Enabled.Returns(true);

        this.mockBehaviorFactory = Substitute.For<IBehaviorFactory>();
        this.mockBehaviorFactory
            .CreateBehaviors(Arg.Any<BehaviorSettings[]>(), this.mockRandomizerService)
            .Returns(new[] { mockBehavior });
    }

    #region Constructor Tests
    [Fact]
    public void Ctor_WhenInvoked_SetsEffectProp()
    {
        // Act
        var pool = CreatePool();

        // Assert
        pool.Effect.Should().Be(this.effect);
    }

    [Fact]
    public void Ctor_WhenInvoked_CreatesParticles()
    {
        // Act
        this.effect.TotalParticlesAliveAtOnce = 10;

        var pool = CreatePool();

        // Assert
        pool.Particles.Count.Should().Be(10);
    }

    [Fact]
    public void Ctor_WithNullParticleEffect_ThrowsException()
    {
        // Act
        var act = () => new ParticlePool<IDisposable>(
            this.mockBehaviorFactory,
            this.mockTextureLoader,
            null,
            this.mockRandomizerService);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'effect')");
    }
    #endregion

    #region Prop Tests
    [Fact]
    public void TotalLivingParticles_WhenGettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var pool = CreatePool();

        // Act
        pool.Update(new TimeSpan(0, 0, 0, 0, 16));
        var actual = pool.TotalLivingParticles;

        // Assert
        actual.Should().Be(1);
    }

    [Fact]
    public void TotalDeadParticles_WhenGettingValue_ReturnsCorrectResult()
    {
        // Arrange
        this.effect.TotalParticlesAliveAtOnce = 10;
        var pool = CreatePool();

        // Act
        pool.Update(new TimeSpan(0, 0, 0, 0, 16));
        var actual = pool.TotalDeadParticles;

        // Assert
        actual.Should().Be(9);
    }

    [Fact]
    public void SpawnRateEnabled_WhenSettingValue_GeneratesCorrectNumberOfParticles()
    {
        /*Explanation:
         * What we are testing for here is that if the spawn rate is enabled, then
         * less particles will be generated because the spawn rate acts like a "throttle"
         * which controls the rate of how many particles are generated.  If the spawn rate
         * is disabled, then this means there is no "throttle" and particles will be
         * generated as fast as possible.
         */
        // Arrange
        // Generate a spawn rate
        this.mockRandomizerService.GetValue(Arg.Any<int>(), Arg.Any<int>()).Returns(32);
        this.effect.TotalParticlesAliveAtOnce = 100;
        var pool = CreatePool();
        pool.SpawnRateEnabled = true;

        // Act
        pool.Update(new TimeSpan(0, 0, 0, 0, 16));
        pool.Update(new TimeSpan(0, 0, 0, 0, 16));
        pool.Update(new TimeSpan(0, 0, 0, 0, 16));
        pool.Update(new TimeSpan(0, 0, 0, 0, 16));

        var totalLivingWithSpawnRateEnabled = pool.TotalLivingParticles;

        pool.KillAllParticles();
        pool.SpawnRateEnabled = false;

        pool.Update(new TimeSpan(0, 0, 0, 0, 16));
        pool.Update(new TimeSpan(0, 0, 0, 0, 16));
        pool.Update(new TimeSpan(0, 0, 0, 0, 16));
        pool.Update(new TimeSpan(0, 0, 0, 0, 16));

        var totalLivingWithSpawnRateDisabled = pool.TotalLivingParticles;

        // Assert
        var becauseMsg = "Total living particles when spawn rate is disabled, is not greater then when spawn rate is enabled.\n";
        becauseMsg += $"Total Living With Spawn Rate Enabled: {totalLivingWithSpawnRateEnabled}\n";
        becauseMsg += "Total Living With Spawn Rate Disabled: {totalLivingWithSpawnRateDisabled}";
        totalLivingWithSpawnRateDisabled.Should()
            .BeGreaterThan(totalLivingWithSpawnRateEnabled, becauseMsg);
    }

    [Fact]
    public void BurstEnabled_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var pool = CreatePool();

        // Act
        pool.BurstEnabled = true;
        var actual = pool.BurstEnabled;

        // Assert
        actual.Should().BeTrue();
    }

    [Theory]
    [InlineData(true, true)]
    [InlineData(false, false)]
    public void IsCurrentlyBursting_WhenGettingValue_ReturnsCorrectResult(bool burstingEnabled, bool expected)
    {
        // Arrange
        this.effect.BurstOffMilliseconds = 10;
        this.effect.BurstOnMilliseconds = 10;
        this.effect.BurstEnabled = burstingEnabled;

        var pool = CreatePool();

        // Act
        pool.Update(new TimeSpan(0, 0, 0, 0, 16));
        var actual = pool.IsCurrentlyBursting;

        // Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public void TextureLoaded_WhenGettingValueAfterLoadingTexture_ReturnsTrue()
    {
        // Arrange
        var mockTexture = Substitute.For<IDisposable>();
        this.mockTextureLoader.LoadTexture(Arg.Any<string>()).Returns(mockTexture);
        var pool = CreatePool();

        // Act
        pool.LoadTexture();

        // Assert
        pool.TextureLoaded.Should().BeTrue();
    }
    #endregion

    #region Method Tests
    [Theory]
    [InlineData(10, 20)]
    [InlineData(20, 10)]
    public void Update_WhenGeneratingRandomSpawnRate_CorrectlyUsesMinAndMax(int rateMin, int rateMax)
    {
        // Arrange
        this.effect.SpawnRateMin = rateMin;
        this.effect.SpawnRateMax = rateMax;
        var pool = new ParticlePool<IDisposable>(this.mockBehaviorFactory, this.mockTextureLoader, this.effect, this.mockRandomizerService);

        // Act
        pool.Update(new TimeSpan(0, 0, 0, 0, 16));

        // Assert
        this.mockRandomizerService.Received(2).GetValue(rateMin < rateMax ? rateMin : rateMax, rateMax > rateMin ? rateMax : rateMin);
    }

    [Fact]
    public void Update_WhenInvoked_SpawnsNewParticle()
    {
        // Arrange
        var pool = new ParticlePool<IDisposable>(this.mockBehaviorFactory, this.mockTextureLoader, this.effect, this.mockRandomizerService);

        // Act
        pool.Update(new TimeSpan(0, 0, 0, 0, 16));

        // Assert
        pool.TotalLivingParticles.Should().Be(1);
    }

    [Theory]
    [InlineData(false, 111, 222)]
    [InlineData(true, 333, 444)]
    public void Update_WhenCurrentlyBursting_ReturnsCorrectSpawnRate(bool burstingEnabled, int expectedRateMin, int expectedRateMax)
    {
        // Arrange
        this.effect.BurstOffMilliseconds = 10;
        this.effect.BurstOnMilliseconds = 10;
        this.effect.SpawnRateMin = 111;
        this.effect.SpawnRateMax = 222;
        this.effect.BurstSpawnRateMin = 333;
        this.effect.BurstSpawnRateMax = 444;
        this.effect.BurstEnabled = burstingEnabled;

        var pool = CreatePool();

        // Act
        pool.Update(new TimeSpan(0, 0, 0, 0, 16));
        var unused = pool.IsCurrentlyBursting;

        // Assert
        this.mockRandomizerService.Received().GetValue(expectedRateMin, expectedRateMax);
    }

    [Fact]
    public void KillAllParticles_WhenInvoked_KillsAllParticles()
    {
        var pool = new ParticlePool<IDisposable>(this.mockBehaviorFactory, this.mockTextureLoader, this.effect, this.mockRandomizerService);
        pool.Update(new TimeSpan(0, 0, 0, 0, 16));

        // Act
        pool.KillAllParticles();

        // Assert
        pool.TotalLivingParticles.Should().Be(0);
    }

    [Fact]
    public void LoadTexture_WhenInvoked_LoadsTextureWithEffectTextureName()
    {
        // Arrange
        var pool = CreatePool();

        // Act
        pool.LoadTexture();

        // Assert
        this.mockTextureLoader.Received(1).LoadTexture(ParticleTextureName);
    }

    [Fact]
    public void Equals_WithDifferentObjectTypes_ReturnsFalse()
    {
        // Arrange
        this.effect.SpawnLocation = new Vector2(11, 22);
        this.effect.SpawnRateMin = 33;
        this.effect.SpawnRateMax = 44;
        this.effect.TotalParticlesAliveAtOnce = 99;
        this.effect.UseColorsFromList = true;

        var poolA = CreatePool();
        var otherObj = new object();

        // Act
        var actual = poolA.Equals(otherObj);

        // Assert
        actual.Should().BeFalse();
    }

    [Fact]
    public void Equals_WithNonEqualObjects_ReturnsFalse()
    {
        // Arrange
        var effectA = new ParticleEffect("texture-name", this.settings)
        {
            SpawnLocation = new Vector2(11, 22),
            SpawnRateMin = 33,
            SpawnRateMax = 44,
            TotalParticlesAliveAtOnce = 99,
            UseColorsFromList = true,
        };

        var effectB = new ParticleEffect("texture-name", this.settings)
        {
            SpawnLocation = new Vector2(11, 22),
            SpawnRateMin = 33,
            SpawnRateMax = 44,
            TotalParticlesAliveAtOnce = 100,
            UseColorsFromList = true,
        };

        var poolA = new ParticlePool<IDisposable>(this.mockBehaviorFactory, this.mockTextureLoader, effectA, this.mockRandomizerService);
        var poolB = new ParticlePool<IDisposable>(this.mockBehaviorFactory, this.mockTextureLoader, effectB, this.mockRandomizerService);

        // Act
        var actual = poolA.Equals(poolB);

        // Assert
        actual.Should().BeFalse();
    }
    #endregion

    /// <summary>
    /// Creates an instance of <see cref="ParticlePool{T}"/> for the purpose of testing.
    /// </summary>
    /// <returns>The pool instance to return.</returns>
    private ParticlePool<IDisposable> CreatePool()
        => new (this.mockBehaviorFactory, this.mockTextureLoader, this.effect, this.mockRandomizerService);
}
