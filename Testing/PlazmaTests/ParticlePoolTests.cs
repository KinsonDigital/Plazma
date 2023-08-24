// <copyright file="ParticlePoolTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace PlazmaTests;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using FluentAssertions;
using FluentAssertions.Execution;
using Plazma;
using Plazma.Behaviors;
using Plazma.Services;
using NSubstitute;
using Plazma.Factories;
using Xunit;

/// <summary>
/// Tests the <see cref="ParticlePool{Texture}"/> class.
/// </summary>
public class ParticlePoolTests : Tests
{
    private const string ParticleTextureName = "particle-texture";
    private readonly IRandomizerService mockRandomizerService;
    private readonly ITextureLoader<IDisposable> mockTextureLoader;
    private readonly IBehaviorFactory mockBehaviorFactory;
    private readonly IParticleFactory mockParticleFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="ParticlePoolTests"/> class.
    /// </summary>
    public ParticlePoolTests()
    {
        this.mockRandomizerService = Substitute.For<IRandomizerService>();
        this.mockTextureLoader = Substitute.For<ITextureLoader<IDisposable>>();
        this.mockBehaviorFactory = Substitute.For<IBehaviorFactory>();
        this.mockParticleFactory = Substitute.For<IParticleFactory>();
    }

    #region Constructor Tests
    [Fact]
    [Trait(Category, IoCConstructors)]
    public void Ctor_WithNullParticleEffectWhenUsing2ParamCtor_ThrowsException()
    {
        // Arrange & Act
        var act = () => new ParticlePool<IDisposable>(null, this.mockTextureLoader);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'effect')");
    }

    [Fact]
    [Trait(Category, IoCConstructors)]
    public void Ctor_WithNullTextureLoaderWhenUsing2ParamCtor_ThrowsException()
    {
        // Arrange & Act
        var act = () => new ParticlePool<IDisposable>(new ParticleEffect(), null);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'textureLoader')");
    }

    [Fact]
    [Trait(Category, InternalConstructors)]
    public void Ctor_WithNullTextureLoaderParamAndWithInternalCtor_SetsEffectProp()
    {
        // Arrange & Act
        var act = () => new ParticlePool<IDisposable>(
            null,
            this.mockRandomizerService,
            this.mockBehaviorFactory,
            this.mockParticleFactory,
            new ParticleEffect());

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'textureLoader')");
    }

    [Fact]
    [Trait(Category, InternalConstructors)]
    public void Ctor_WithNullRandomizerParamAndWithInternalCtor_CreatesParticles()
    {
        // Arrange & Act
        var act = () => new ParticlePool<IDisposable>(
            this.mockTextureLoader,
            null,
            this.mockBehaviorFactory,
            this.mockParticleFactory,
            new ParticleEffect());

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'randomizer')");
    }

    [Fact]
    [Trait(Category, InternalConstructors)]
    public void Ctor_WithNullBehaviorFactoryParamAndWithInternalCtor_CreatesParticles()
    {
        // Arrange & Act
        var act = () => new ParticlePool<IDisposable>(
            this.mockTextureLoader,
            this.mockRandomizerService,
            null,
            this.mockParticleFactory,
            new ParticleEffect());

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'behaviorFactory')");
    }

    [Fact]
    [Trait(Category, InternalConstructors)]
    public void Ctor_WithNullParticleFactoryParamAndWithInternalCtor_CreatesParticles()
    {
        // Arrange & Act
        var act = () => new ParticlePool<IDisposable>(
            this.mockTextureLoader,
            this.mockRandomizerService,
            this.mockBehaviorFactory,
            null,
            new ParticleEffect());

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'particleFactory')");
    }

    [Fact]
    [Trait(Category, InternalConstructors)]
    public void Ctor_WithNullEffectParamAndWithInternalCtor_CreatesParticles()
    {
        // Arrange & Act
        var act = () => new ParticlePool<IDisposable>(
            this.mockTextureLoader,
            this.mockRandomizerService,
            this.mockBehaviorFactory,
            this.mockParticleFactory,
            null);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'effect')");
    }
    #endregion

    #region Prop Tests
    [Fact]
    [Trait(Category, Props)]
    public void TotalLivingParticles_WhenGettingValue_ReturnsCorrectResult()
    {
        // Arrange & Act
        var settings = new EasingRandomBehaviorSettings[] { new (), };
        var effect = new ParticleEffect(ParticleTextureName, settings)
        {
            SpawnLocation = new Vector2(11, 22),
            UseColorsFromList = true,
            TotalParticles = 3,
        };

        var mockParticleA = Substitute.For<IParticle>();
        mockParticleA.IsAlive.Returns(true);

        var mockParticleB = Substitute.For<IParticle>();
        mockParticleB.IsAlive.Returns(false);

        var mockParticleC = Substitute.For<IParticle>();
        mockParticleC.IsAlive.Returns(true);

        var particles = new List<IParticle>
        {
            mockParticleA,
            mockParticleB,
            mockParticleC,
        };

        this.mockParticleFactory.Create(Arg.Any<IBehavior[]>())
            .Returns((_) =>
            {
                if (particles.Count <= 0)
                {
                    throw new AssertionFailedException("Attempting to use a test particle that does not exist.");
                }

                var newParticle = particles[0];
                particles.RemoveAt(0);

                return newParticle;
            });

        var sut = CreateSystemUnderTest(effect);

        // Assert
        sut.TotalLivingParticles.Should().Be(2);
    }

    [Fact]
    [Trait(Category, Props)]
    public void TotalDeadParticles_WhenGettingValue_ReturnsCorrectResult()
    {
        // Arrange & Act
        var settings = new EasingRandomBehaviorSettings[] { new (), };
        var effect = new ParticleEffect(ParticleTextureName, settings)
        {
            SpawnLocation = new Vector2(11, 22),
            UseColorsFromList = true,
            TotalParticles = 3,
        };

        var mockParticleA = Substitute.For<IParticle>();
        mockParticleA.IsAlive.Returns(true);

        var mockParticleB = Substitute.For<IParticle>();
        mockParticleB.IsAlive.Returns(false);

        var mockParticleC = Substitute.For<IParticle>();
        mockParticleC.IsAlive.Returns(true);

        var particles = new List<IParticle>
        {
            mockParticleA,
            mockParticleB,
            mockParticleC,
        };

        this.mockParticleFactory.Create(Arg.Any<IBehavior[]>())
            .Returns((_) =>
            {
                if (particles.Count <= 0)
                {
                    throw new AssertionFailedException("Attempting to use a test particle that does not exist.");
                }

                var newParticle = particles[0];
                particles.RemoveAt(0);

                return newParticle;
            });

        var sut = CreateSystemUnderTest(effect);

        // Assert
        sut.TotalDeadParticles.Should().Be(1);
    }

    [Fact]
    [Trait(Category, Props)]
    public void LimitSpawnRate_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var effect = new ParticleEffect();

        var sut = CreateSystemUnderTest(effect);

        // Act
        sut.LimitSpawnRate = true;

        // Assert
        sut.LimitSpawnRate.Should().BeTrue();
    }

    [Fact]
    [Trait(Category, Props)]
    public void BurstEnabled_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var settings = new EasingRandomBehaviorSettings[]
        {
            new (),
        };
        var effect = new ParticleEffect(ParticleTextureName, settings)
        {
            SpawnLocation = new Vector2(11, 22),
            TotalParticles = 99,
            UseColorsFromList = true,
        };
        var sut = CreateSystemUnderTest(effect);

        // Act
        sut.BurstEnabled = true;
        var actual = sut.BurstEnabled;

        // Assert
        actual.Should().BeTrue();
    }

    [Theory]
    [InlineData(true, true)]
    [InlineData(false, false)]
    [Trait(Category, Props)]
    public void InBurstMode_WhenGettingValue_ReturnsCorrectResult(bool burstingEnabled, bool expected)
    {
        // Arrange
        var settings = new EasingRandomBehaviorSettings[] { new (), };
        var effect = new ParticleEffect(ParticleTextureName, settings)
        {
            SpawnLocation = new Vector2(11, 22),
            BurstOffMilliseconds = 10,
            BurstOnMilliseconds = 10,
            BurstEnabled = burstingEnabled,
        };

        var sut = CreateSystemUnderTest(effect);

        // Act
        sut.Update(new TimeSpan(0, 0, 0, 0, 16));
        var actual = sut.InBurstMode;

        // Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public void Particles_WhenGettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var effect = new ParticleEffect { TotalParticles = 4, };

        // Act
        var sut = CreateSystemUnderTest(effect);

        // Assert
        sut.Particles.Should().HaveCount(4);
    }

    [Fact]
    [Trait(Category, Props)]
    public void TextureLoaded_WhenGettingValueAfterLoadingTexture_ReturnsTrue()
    {
        // Arrange
        var settings = new EasingRandomBehaviorSettings[] { new (), };
        var effect = new ParticleEffect(ParticleTextureName, settings)
        {
            SpawnLocation = new Vector2(11, 22),
            BurstOffMilliseconds = 10,
            BurstOnMilliseconds = 10,
        };
        var mockTexture = Substitute.For<IDisposable>();
        this.mockTextureLoader.LoadTexture(Arg.Any<string>()).Returns(mockTexture);

        var sut = CreateSystemUnderTest(effect);

        // Act
        sut.LoadTexture();

        // Assert
        sut.TextureLoaded.Should().BeTrue();
    }
    #endregion

    #region Method Tests
    [Fact]
    [Trait(Category, Methods)]
    public void Update_WhenSpawnRateIsLimited_GeneratesCorrectNumberOfLivingParticles()
    {
        // Arrange
        var settings = new EasingRandomBehaviorSettings[] { new (), };
        var effect = new ParticleEffect(ParticleTextureName, settings)
        {
            SpawnLocation = new Vector2(11, 22),
            UseColorsFromList = true,
            TotalParticles = 4,
            LimitSpawnRate = true,
        };

        var livingParticleUsed = false;

        var mockAliveParticle = Substitute.For<IParticle>();
        mockAliveParticle.IsAlive.Returns(true);

        var mockDeadParticle = Substitute.For<IParticle>();
        mockDeadParticle.IsAlive.Returns(false);

        this.mockParticleFactory
            .Create(Arg.Any<IBehavior[]>()).Returns((_) =>
            {
                var newParticle = livingParticleUsed ? mockDeadParticle : mockAliveParticle;

                livingParticleUsed = true;

                return newParticle;
            });

        var sut = CreateSystemUnderTest(effect);

        var timeElapsed = 100.ToMillisecondsTimeSpan();

        // Act
        sut.Update(timeElapsed);

        // Assert
        sut.TotalLivingParticles.Should().Be(1);
    }

    [Fact]
    [Trait(Category, Methods)]
    public void Update_WhenSpawnRateIsNotLimited_GeneratesCorrectNumberOfLivingParticles()
    {
        // Arrange
        var settings = new EasingRandomBehaviorSettings[] { new (), };
        var effect = new ParticleEffect(ParticleTextureName, settings)
        {
            SpawnLocation = new Vector2(11, 22),
            UseColorsFromList = true,
            TotalParticles = 4,
            LimitSpawnRate = false,
        };

        // Make sure that the spawn rate will never be greater then the time elapsed
        this.mockRandomizerService.GetValue(Arg.Any<double>(), Arg.Any<double>()).Returns(100);

        var livingParticleUsed = false;

        var mockAliveParticle = Substitute.For<IParticle>();
        mockAliveParticle.IsAlive.Returns(true);

        var mockDeadParticle = Substitute.For<IParticle>();
        mockDeadParticle.IsAlive.Returns(false);

        this.mockParticleFactory
            .Create(Arg.Any<IBehavior[]>()).Returns((_) =>
            {
                var newParticle = livingParticleUsed ? mockDeadParticle : mockAliveParticle;

                livingParticleUsed = true;

                return newParticle;
            });

        var sut = CreateSystemUnderTest(effect);

        var timeElapsed = 0.ToMillisecondsTimeSpan();

        // Act
        sut.Update(timeElapsed);

        // Assert
        sut.TotalLivingParticles.Should().Be(1);
    }

    [Theory]
    [InlineData(10, 20)]
    [InlineData(20, 10)]
    [Trait(Category, Methods)]
    public void Update_WhenGeneratingRandomSpawnRate_CorrectlyUsesMinAndMax(int rateMin, int rateMax)
    {
        // Arrange
        var settings = new EasingRandomBehaviorSettings[]
        {
            new (),
        };
        var effect = new ParticleEffect(ParticleTextureName, settings)
        {
            SpawnLocation = new Vector2(11, 22),
            TotalParticles = 99,
            UseColorsFromList = true,
            SpawnRateMin = rateMin,
            SpawnRateMax = rateMax,
        };
        var sut = CreateSystemUnderTest(effect);

        // Act
        sut.Update(new TimeSpan(0, 0, 0, 0, 16));

        // Assert
        this.mockRandomizerService.Received(2).GetValue(rateMin < rateMax ? rateMin : rateMax, rateMax > rateMin ? rateMax : rateMin);
    }

    [Theory]
    [InlineData(false, 111, 222)]
    [InlineData(true, 333, 444)]
    [Trait(Category, Methods)]
    public void Update_WhenCurrentlyBursting_ReturnsCorrectSpawnRate(bool burstingEnabled, int expectedRateMin, int expectedRateMax)
    {
        // Arrange
        var settings = new EasingRandomBehaviorSettings[]
        {
            new (),
        };
        var effect = new ParticleEffect(ParticleTextureName, settings)
        {
            BurstOffMilliseconds = 10,
            BurstOnMilliseconds = 10,
            SpawnRateMin = 111,
            SpawnRateMax = 222,
            BurstSpawnRateMin = 333,
            BurstSpawnRateMax = 444,
            BurstEnabled = burstingEnabled,
        };

        var sut = CreateSystemUnderTest(effect);

        // Act
        sut.Update(new TimeSpan(0, 0, 0, 0, 16));
        var unused = sut.InBurstMode;

        // Assert
        this.mockRandomizerService.Received().GetValue(expectedRateMin, expectedRateMax);
    }

    [Fact]
    [Trait(Category, Methods)]
    public void KillAllParticles_WhenInvoked_KillsAllParticles()
    {
        // Arrange
        var settings = new EasingRandomBehaviorSettings[]
        {
            new (),
        };
        var effect = new ParticleEffect(ParticleTextureName, settings)
        {
            SpawnLocation = new Vector2(11, 22), SpawnRateMin = 33, SpawnRateMax = 44,
            TotalParticles = 99,
            UseColorsFromList = true,
        };

        var sut = CreateSystemUnderTest(effect);
        sut.Update(new TimeSpan(0, 0, 0, 0, 16));

        // Act
        sut.KillAllParticles();

        // Assert
        sut.TotalLivingParticles.Should().Be(0);
    }

    [Fact]
    [Trait(Category, Methods)]
    public void LoadTexture_WhenInvoked_LoadsTextureWithEffectTextureName()
    {
        // Arrange
        var settings = new EasingRandomBehaviorSettings[]
        {
            new (),
        };
        var effect = new ParticleEffect(ParticleTextureName, settings)
        {
            SpawnLocation = new Vector2(11, 22), SpawnRateMin = 33, SpawnRateMax = 44,
            TotalParticles = 99,
            UseColorsFromList = true,
        };

        var sut = CreateSystemUnderTest(effect);

        // Act
        sut.LoadTexture();

        // Assert
        this.mockTextureLoader.Received(1).LoadTexture(ParticleTextureName);
    }

    [Fact]
    public void AddBehavior_WhenInvoked_AddsNewBehavior()
    {
        // Arrange
        var effect = new ParticleEffect { TotalParticles = 2 };

        var settings = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = BehaviorAttribute.Angle,
            LifeTimeMillisecondsMin = 2000,
            LifeTimeMillisecondsMax = 2000,
            RandomStartMin = 0,
            RandomStartMax = 0,
            RandomChangeMin = 360,
            RandomChangeMax = 360,
        };

        var behavior = new EasingRandomBehavior(settings, Substitute.For<IRandomizerService>());

        this.mockBehaviorFactory.CreateEasingRandomBehavior(Arg.Any<EasingRandomBehaviorSettings>()).Returns(behavior);

        var mockParticleA = Substitute.For<IParticle>();
        var mockParticleB = Substitute.For<IParticle>();

        this.mockParticleFactory.Create(Arg.Any<IBehavior[]>())
            .ReturnsForAnyArgs(mockParticleA, mockParticleB);

        var sut = CreateSystemUnderTest(effect);

        // Act
        sut.AddBehavior(settings);

        // Assert
        mockParticleA.Received(1).AddBehavior(Arg.Any<EasingRandomBehavior>());
        mockParticleB.Received(1).AddBehavior(Arg.Any<EasingRandomBehavior>());
        this.mockBehaviorFactory.Received(2).CreateEasingRandomBehavior(settings);
    }

    [Fact]
    public void RemoveBehavior_WhenInvoked_AddsNewBehavior()
    {
        // Arrange
        var mockParticleA = Substitute.For<IParticle>();
        var mockParticleB = Substitute.For<IParticle>();

        this.mockParticleFactory.Create(Arg.Any<IBehavior[]>())
            .Returns(mockParticleA, mockParticleB);

        var effect = new ParticleEffect { TotalParticles = 2 };
        var angleSettings = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = BehaviorAttribute.Angle,
            LifeTimeMillisecondsMin = 2000,
            LifeTimeMillisecondsMax = 2000,
            RandomStartMin = 0,
            RandomStartMax = 0,
            RandomChangeMin = 360,
            RandomChangeMax = 360,
        };

        var blueSettings = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = BehaviorAttribute.BlueColorComponent,
            LifeTimeMillisecondsMin = 4000,
            LifeTimeMillisecondsMax = 4000,
            RandomStartMin = 0,
            RandomStartMax = 0,
            RandomChangeMin = 255,
            RandomChangeMax = 255,
        };

        var sut = CreateSystemUnderTest(effect);
        sut.AddBehavior(angleSettings);
        sut.AddBehavior(blueSettings);

        // Act
        sut.RemoveBehavior(BehaviorAttribute.BlueColorComponent);

        // Assert
        mockParticleA.Received(1).RemoveBehavior(BehaviorAttribute.BlueColorComponent);
        mockParticleB.Received(1).RemoveBehavior(BehaviorAttribute.BlueColorComponent);
    }

    [Fact]
    [SuppressMessage("csharpsquid", "S3966", Justification = "Redundant intentional for testing.")]
    public void Dispose_WhenInvoked2Times_DisposesOfPoolOneTime()
    {
        // Arrange
        var sut = CreateSystemUnderTest(new ParticleEffect());

        // Act
        sut.Dispose();
        sut.Dispose();

        // Assert
        this.mockRandomizerService.Received(1).Dispose();
        this.mockTextureLoader.Received(1).Dispose();
    }
    #endregion

    /// <summary>
    /// Creates an instance of <see cref="ParticlePool{T}"/> for the purpose of testing.
    /// </summary>
    /// <returns>The pool instance to return.</returns>
    private ParticlePool<IDisposable> CreateSystemUnderTest(ParticleEffect effect)
        => new (this.mockTextureLoader,
            this.mockRandomizerService,
            this.mockBehaviorFactory,
            this.mockParticleFactory,
            effect);
}
