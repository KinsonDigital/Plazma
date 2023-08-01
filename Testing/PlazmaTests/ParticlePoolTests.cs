// <copyright file="ParticlePoolTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDParticleEngineTests;

using System;
using System.Drawing;
using Plazma;
using Plazma.Behaviors;
using Plazma.Services;
using KDParticleEngineTests.XUnitHelpers;
using Moq;
using Xunit;

/// <summary>
/// Tests the <see cref="ParticlePool{Texture}"/> class.
/// </summary>
public class ParticlePoolTests
{
    private const string ParticleTextureName = "particle-texture";
    private readonly Mock<IRandomizerService> mockRandomizerService;
    private readonly Mock<ITextureLoader<IDisposable>> mockTextureLoader;
    private readonly Mock<IBehaviorFactory> mockBehaviorFactory;
    private readonly Mock<IBehavior> mockBehavior;
    private readonly EasingRandomBehaviorSettings[] settings;
    private readonly ParticleEffect effect;

    /// <summary>
    /// Initializes a new instance of the <see cref="ParticlePoolTests"/> class.
    /// </summary>
    public ParticlePoolTests()
    {
        this.settings = new EasingRandomBehaviorSettings[]
        {
            new EasingRandomBehaviorSettings(),
        };
        this.effect = new ParticleEffect(ParticleTextureName, this.settings);

        this.mockRandomizerService = new Mock<IRandomizerService>();
        this.mockTextureLoader = new Mock<ITextureLoader<IDisposable>>();

        this.mockBehavior = new Mock<IBehavior>();
        this.mockBehavior.SetupGet(p => p.Value).Returns("0");
        this.mockBehavior.SetupGet(p => p.Enabled).Returns(true);

        this.mockBehaviorFactory = new Mock<IBehaviorFactory>();
        this.mockBehaviorFactory.Setup(m => m.CreateBehaviors(this.settings, this.mockRandomizerService.Object))
            .Returns(() =>
            {
                return new IBehavior[] { this.mockBehavior.Object };
            });
    }

    #region Constructor Tests
    [Fact]
    public void Ctor_WhenInvoked_SetsEffectProp()
    {
        // Act
        var pool = CreatePool();

        // Assert
        Assert.Equal(this.effect, pool.Effect);
    }

    [Fact]
    public void Ctor_WhenInvoked_CreatesParticles()
    {
        // Act
        this.effect.TotalParticlesAliveAtOnce = 10;

        var pool = CreatePool();

        // Assert
        Assert.Equal(10, pool.Particles.Count);
    }

    [Fact]
    public void Ctor_WithNullParticleEffect_ThrowsException()
    {
        // Act & Assert
        AssertHelpers.ThrowsWithMessage<ArgumentNullException>(() =>
        {
            var pool = new ParticlePool<IDisposable>(this.mockBehaviorFactory.Object, this.mockTextureLoader.Object, null, this.mockRandomizerService.Object);
        }, "The parameter must not be null. (Parameter 'effect')");
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
        Assert.Equal(1, actual);
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
        Assert.Equal(9, actual);
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
        this.mockRandomizerService.Setup(m => m.GetValue(It.IsAny<int>(), It.IsAny<int>())).Returns(32);
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
        Assert.True(totalLivingWithSpawnRateDisabled > totalLivingWithSpawnRateEnabled,
            $"Total living particles when spawn rate is disabled, is not greater then when spawn rate is enabled.\nTotal Living With Spawn Rate Enabled: {totalLivingWithSpawnRateEnabled}\nTotal Living With Spawn Rate Disabled: {totalLivingWithSpawnRateDisabled}");
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
        Assert.True(actual);
    }

    [Theory]
    [InlineData(true, true)]
    [InlineData(false, false)]
    public void IsCurrentlyBursting_WhenGettingValue_ReturnsCorrectResult(bool burstingEnabled, bool expected)
    {
        // Arrange
        this.effect.BurstOffTime = 10;
        this.effect.BurstOnTime = 10;
        this.effect.BurstEnabled = burstingEnabled;

        var pool = CreatePool();

        // Act
        pool.Update(new TimeSpan(0, 0, 0, 0, 16));
        var actual = pool.IsCurrentlyBursting;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TextureLoaded_WhenGettingValueAfterLoadingTexture_ReturnsTrue()
    {
        // Arrange
        var mockTexture = new Mock<IDisposable>();
        this.mockTextureLoader.Setup(m => m.LoadTexture(It.IsAny<string>())).Returns(mockTexture.Object);
        var pool = CreatePool();

        // Act
        pool.LoadTexture();

        // Assert
        Assert.True(pool.TextureLoaded);
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
        var pool = new ParticlePool<IDisposable>(this.mockBehaviorFactory.Object, It.IsAny<ITextureLoader<IDisposable>>(), this.effect, this.mockRandomizerService.Object);

        // Act
        pool.Update(new TimeSpan(0, 0, 0, 0, 16));

        // Assert
        this.mockRandomizerService.Verify(m => m.GetValue(rateMin < rateMax ? rateMin : rateMax, rateMax > rateMin ? rateMax : rateMin), Times.Exactly(2));
    }

    [Fact]
    public void Update_WhenInvoked_SpawnsNewParticle()
    {
        // Arrange
        var pool = new ParticlePool<IDisposable>(this.mockBehaviorFactory.Object, It.IsAny<ITextureLoader<IDisposable>>(), this.effect, this.mockRandomizerService.Object);

        // Act
        pool.Update(new TimeSpan(0, 0, 0, 0, 16));

        // Assert
        Assert.Equal(1, pool.TotalLivingParticles);
    }

    [Theory]
    [InlineData(false, 111, 222)]
    [InlineData(true, 333, 444)]
    public void Update_WhenCurrentlyBursting_ReturnsCorrectSpawnRate(bool burstingEnabled, int expectedRateMin, int expectedRateMax)
    {
        // Arrange
        this.effect.BurstOffTime = 10;
        this.effect.BurstOnTime = 10;
        this.effect.SpawnRateMin = 111;
        this.effect.SpawnRateMax = 222;
        this.effect.BurstSpawnRateMin = 333;
        this.effect.BurstSpawnRateMax = 444;
        this.effect.BurstEnabled = burstingEnabled;

        var pool = CreatePool();

        // Act
        pool.Update(new TimeSpan(0, 0, 0, 0, 16));
        var actual = pool.IsCurrentlyBursting;

        // Assert
        this.mockRandomizerService.Verify(m => m.GetValue(expectedRateMin, expectedRateMax), Times.AtLeast(1));
    }

    [Fact]
    public void KillAllParticles_WhenInvoked_KillsAllParticles()
    {
        var pool = new ParticlePool<IDisposable>(this.mockBehaviorFactory.Object, It.IsAny<ITextureLoader<IDisposable>>(), this.effect, this.mockRandomizerService.Object);
        pool.Update(new TimeSpan(0, 0, 0, 0, 16));

        // Act
        pool.KillAllParticles();

        // Assert
        Assert.Equal(0, pool.TotalLivingParticles);
    }

    [Fact]
    public void LoadTexture_WhenInvoked_LoadsTextureWithEffectTextureName()
    {
        // Arrange
        var pool = CreatePool();

        // Act
        pool.LoadTexture();

        // Assert
        this.mockTextureLoader.Verify(m => m.LoadTexture(ParticleTextureName), Times.Once());
    }

    [Fact]
    public void Equals_WithDifferentObjectTypes_ReturnsFalse()
    {
        // Arrange
        this.effect.SpawnLocation = new PointF(11, 22);
        this.effect.SpawnRateMin = 33;
        this.effect.SpawnRateMax = 44;
        this.effect.TotalParticlesAliveAtOnce = 99;
        this.effect.UseColorsFromList = true;

        var poolA = CreatePool();
        var otherObj = new object();

        // Act
        var actual = poolA.Equals(otherObj);

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void Equals_WithEqualObjects_ReturnsTrue()
    {
        // Arrange
        this.effect.SpawnLocation = new PointF(11, 22);
        this.effect.SpawnRateMin = 33;
        this.effect.SpawnRateMax = 44;
        this.effect.TotalParticlesAliveAtOnce = 99;
        this.effect.UseColorsFromList = true;

        var poolA = CreatePool();
        var poolB = CreatePool();

        // Act
        var actual = poolA.Equals(poolB);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void Equals_WithNonEqualObjects_ReturnsFalse()
    {
        // Arrange
        var effectA = new ParticleEffect("texture-name", this.settings)
        {
            SpawnLocation = new PointF(11, 22),
            SpawnRateMin = 33,
            SpawnRateMax = 44,
            TotalParticlesAliveAtOnce = 99,
            UseColorsFromList = true,
        };

        var effectB = new ParticleEffect("texture-name", this.settings)
        {
            SpawnLocation = new PointF(11, 22),
            SpawnRateMin = 33,
            SpawnRateMax = 44,
            TotalParticlesAliveAtOnce = 100,
            UseColorsFromList = true,
        };

        var poolA = new ParticlePool<IDisposable>(this.mockBehaviorFactory.Object, this.mockTextureLoader.Object, effectA, this.mockRandomizerService.Object);
        var poolB = new ParticlePool<IDisposable>(this.mockBehaviorFactory.Object, this.mockTextureLoader.Object, effectB, this.mockRandomizerService.Object);

        // Act
        var actual = poolA.Equals(poolB);

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void Dispose_WhenInvoked_ProperlyFreesManagedResources()
    {
        // Arrange
        var effect = new ParticleEffect(It.IsAny<string>(), Array.Empty<BehaviorSettings>());
        var mockTexture = new Mock<IDisposable>();

        this.mockTextureLoader.Setup(m => m.LoadTexture(It.IsAny<string>())).Returns<string>((textureName) =>
        {
            return mockTexture.Object;
        });

        var pool = new ParticlePool<IDisposable>(this.mockBehaviorFactory.Object,
            this.mockTextureLoader.Object,
            effect,
            this.mockRandomizerService.Object);

        pool.LoadTexture();

        // Call this twice to verify that the disposable pattern is implemented correctly.
        // You should be able to call this method twice and not throw an exception
        // Act
        pool.Dispose();
        pool.Dispose();

        // Assert
        mockTexture.Verify(m => m.Dispose(), Times.Once());
    }
    #endregion

    /// <summary>
    /// Creates an instance of <see cref="ParticlePool{T}"/> for the purpose of testing.
    /// </summary>
    /// <returns>The pool instance to return.</returns>
    private ParticlePool<IDisposable> CreatePool()
        => new ParticlePool<IDisposable>(this.mockBehaviorFactory.Object, this.mockTextureLoader.Object, this.effect, this.mockRandomizerService.Object);
}