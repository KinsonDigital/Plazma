// <copyright file="ParticlePoolTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace PlazmaTests;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
    public void Ctor_WithNullTextureLoaderWhenUsing2ParamCtor_ThrowsException()
    {
        // Arrange & Act
        var act = () => new ParticlePool<IDisposable>(default, null);

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
            default);

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
            default);

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
            default);

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
            default);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'particleFactory')");
    }
    #endregion

    #region Prop Tests
    [Fact]
    [Trait(Category, Props)]
    public void TotalLivingParticles_WhenDisposed_ReturnsZer0o()
    {
        // Arrange
        var settings = new EasingRandomBehaviorSettings[] { new (), };
        var effect = new ParticleEffect(ParticleTextureName, settings)
        {
            SpawnLocation = new Vector2(11, 22),
            UseColorsFromList = true,
            TotalParticles = 3,
        };

        var createLiving = true;

        this.mockParticleFactory.Create(Arg.Any<IBehavior[]>())
            .Returns((_) =>
            {
                var newParticle = new Particle { IsAlive = createLiving };

                createLiving = !createLiving;

                return newParticle;
            });

        var sut = CreateSystemUnderTest(effect);
        sut.Dispose();

        // Act
        var actual = sut.TotalLivingParticles;

        // Assert
        actual.Should().Be(0);
    }

    [Fact]
    [Trait(Category, Props)]
    public void TotalLivingParticles_WhenGettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var settings = new EasingRandomBehaviorSettings[] { new (), };
        var effect = new ParticleEffect(ParticleTextureName, settings)
        {
            SpawnLocation = new Vector2(11, 22),
            UseColorsFromList = true,
            TotalParticles = 3,
        };

        var createLiving = true;

        this.mockParticleFactory.Create(Arg.Any<IBehavior[]>())
            .Returns((_) =>
            {
                var newParticle = new Particle { IsAlive = createLiving };

                createLiving = !createLiving;

                return newParticle;
            });

        var sut = CreateSystemUnderTest(effect);

        // Act
        var actual = sut.TotalLivingParticles;

        // Assert
        actual.Should().Be(2);
    }

    [Fact]
    [Trait(Category, Props)]
    public void TotalDeadParticles_WhenDisposed_ReturnsZer0o()
    {
        // Arrange
        var settings = new EasingRandomBehaviorSettings[] { new (), };
        var effect = new ParticleEffect(ParticleTextureName, settings)
        {
            SpawnLocation = new Vector2(11, 22),
            UseColorsFromList = true,
            TotalParticles = 3,
        };

        var createLiving = true;

        this.mockParticleFactory.Create(Arg.Any<IBehavior[]>())
            .Returns((_) =>
            {
                var newParticle = new Particle { IsAlive = createLiving };

                createLiving = !createLiving;

                return newParticle;
            });

        var sut = CreateSystemUnderTest(effect);
        sut.Dispose();

        // Act
        var actual = sut.TotalDeadParticles;

        // Assert
        actual.Should().Be(0);
    }

    [Fact]
    [Trait(Category, Props)]
    public void TotalDeadParticles_WhenGettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var settings = new EasingRandomBehaviorSettings[] { new (), };
        var effect = new ParticleEffect(ParticleTextureName, settings)
        {
            SpawnLocation = new Vector2(11, 22),
            UseColorsFromList = true,
            TotalParticles = 3,
        };

        var particleA = new Particle { IsAlive = true };
        var particleB = new Particle { IsAlive = false };
        var particleC = new Particle { IsAlive = true };

        var particles = new List<Particle>
        {
            particleA,
            particleB,
            particleC,
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

        // Act
        var actual = sut.TotalDeadParticles;

        // Assert
        actual.Should().Be(1);
    }

    [Fact]
    [Trait(Category, Props)]
    public void LimitSpawnRate_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var effect = default(ParticleEffect);

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
    [Trait(Category, Props)]
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
    public void Particles_WhenGettingValueWhileDisposed_ReturnsZero()
    {
        // Arrange
        var effect = new ParticleEffect { TotalParticles = 4, };
        var sut = CreateSystemUnderTest(effect);
        sut.Dispose();

        // Act
        var actual = sut.Particles;

        // Assert
        actual.Should().BeEmpty();
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
        sut.LoadTexture();

        // Act
        var actual = sut.TextureLoaded;

        // Assert
        actual.Should().BeTrue();
    }

    [Fact]
    [Trait(Category, Props)]
    public void TextureLoaded_WhenGettingValueWhileDisposed_ReturnsFalse()
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
        sut.LoadTexture();
        sut.Dispose();

        // Act
        var actual = sut.TextureLoaded;

        // Assert
        actual.Should().BeFalse();
    }
    #endregion

    #region Method Tests
    [Fact]
    [Trait(Category, Methods)]
    public void Update_WhileDisposed_ThrowsException()
    {
        // Arrange
        var effect = new ParticleEffect(ParticleTextureName, [new ()]);
        var sut = CreateSystemUnderTest(effect);
        sut.Dispose();

        // Act
        var act = () => sut.Update(100.ToMillisecondsTimeSpan());

        // Assert
        act.Should().Throw<ObjectDisposedException>()
            .WithMessage("Cannot access a disposed object.\nObject name: 'ParticlePool'.");
    }

    [Theory]
    [InlineData(true, 0)]
    [InlineData(false, 1)]
    [Trait(Category, Methods)]
    public void Update_WithSpawnRateIsLimiting_GeneratesCorrectNumberOfLivingParticles(bool limitSpawnRate, int expectedLivingParticles)
    {
        // Arrange
        var effect = new ParticleEffect(ParticleTextureName, [new ()])
        {
            SpawnLocation = new Vector2(11f, 22f),
            UseColorsFromList = true,
            TotalParticles = 4,
            LimitSpawnRate = limitSpawnRate,
        };

        this.mockRandomizerService.GetValue(Arg.Any<float>(), Arg.Any<float>()).Returns(1000);
        this.mockBehaviorFactory.CreateEasingRandomBehavior(Arg.Any<EasingRandomBehaviorSettings>()).Returns(callInfo =>
            {
                var settings = callInfo.Arg<EasingRandomBehaviorSettings>();

                return new EasingRandomBehavior(settings, this.mockRandomizerService);
            });

        this.mockParticleFactory
            .Create(Arg.Any<IBehavior[]>()).Returns(callInfo =>
            {
                var behaviors = callInfo.Arg<IBehavior[]>();

                var newParticle = new Particle(behaviors) { IsAlive = false };

                return newParticle;
            });

        var sut = CreateSystemUnderTest(effect);

        var timeElapsed = 100.ToMillisecondsTimeSpan();

        // Act
        sut.Update(timeElapsed);

        // Assert
        sut.TotalLivingParticles.Should().Be(expectedLivingParticles);
    }

    [Theory]
    [InlineData(10f, 20f)]
    [InlineData(20f, 10f)]
    [Trait(Category, Methods)]
    public void Update_WhenGeneratingRandomSpawnRate_CorrectlyUsesMinAndMax(float rateMin, float rateMax)
    {
        // Arrange
        var settings = new EasingRandomBehaviorSettings[]
        {
            new (),
        };
        var effect = new ParticleEffect(ParticleTextureName, settings)
        {
            SpawnLocation = new Vector2(11f, 22f),
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
    [InlineData(false, 111f, 222f, 2)]
    [InlineData(true, 333f, 444f, 1)]
    [Trait(Category, Methods)]
    public void Update_IfBursting_ReturnsCorrectSpawnRate(
        bool burstingEnabled,
        float expectedRateMin,
        float expectedRateMax,
        int expectedCallCount)
    {
        // Arrange
        var settings = new EasingRandomBehaviorSettings[]
        {
            new (),
        };
        var effect = new ParticleEffect(ParticleTextureName, settings)
        {
            BurstOffMilliseconds = 10f,
            BurstOnMilliseconds = 10f,
            SpawnRateMin = 111f,
            SpawnRateMax = 222f,
            BurstSpawnRateMin = 333f,
            BurstSpawnRateMax = 444f,
            BurstEnabled = burstingEnabled,
        };

        var sut = CreateSystemUnderTest(effect);

        // Act
        sut.Update(new TimeSpan(0, 0, 0, 0, 16));
        // var unused = sut.InBurstMode;

        // Assert
        this.mockRandomizerService.Received(expectedCallCount).GetValue(expectedRateMin, expectedRateMax);
    }

    [Fact]
    [Trait(Category, Methods)]
    public void KillAllParticles_WhileDisposed_ThrowsException()
    {
        // Arrange
        var effect = new ParticleEffect(ParticleTextureName, [new ()]);
        var sut = CreateSystemUnderTest(effect);
        sut.Dispose();

        // Act
        var act = () => sut.KillAllParticles();

        // Assert
        act.Should().Throw<ObjectDisposedException>()
            .WithMessage("Cannot access a disposed object.\nObject name: 'ParticlePool'.");
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
    public void LoadTexture_WhileDisposed_ThrowsException()
    {
        // Arrange
        var effect = new ParticleEffect(ParticleTextureName, [new ()]);
        var sut = CreateSystemUnderTest(effect);
        sut.Dispose();

        // Act
        var act = () => sut.LoadTexture();

        // Assert
        act.Should().Throw<ObjectDisposedException>()
            .WithMessage("Cannot access a disposed object.\nObject name: 'ParticlePool'.");
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
    [Trait(Category, Methods)]
    public void AddBehavior_WhileDisposed_ThrowsException()
    {
        // Arrange
        var effect = new ParticleEffect(ParticleTextureName, [new ()]);
        var sut = CreateSystemUnderTest(effect);
        sut.Dispose();

        // Act
        var act = () => sut.AddBehavior(default);

        // Assert
        act.Should().Throw<ObjectDisposedException>()
            .WithMessage("Cannot access a disposed object.\nObject name: 'ParticlePool'.");
    }

    [Fact]
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules",
        "SA1129:Do not use default value type constructor",
        Justification = "Required for proper testing.")]
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

        var particleA = new Particle();
        var particleB = new Particle();

        this.mockParticleFactory.Create(Arg.Any<IBehavior[]>())
            .ReturnsForAnyArgs(particleA, particleB);

        var sut = CreateSystemUnderTest(effect);

        // Act
        sut.AddBehavior(settings);

        // Assert
        particleA.Behaviors.Should().ContainSingle();
        particleB.Behaviors.Should().ContainSingle();

        this.mockBehaviorFactory.Received(2).CreateEasingRandomBehavior(settings);
    }

    [Fact]
    [Trait(Category, Methods)]
    public void RemoveBehavior_WhileDisposed_ThrowsException()
    {
        // Arrange
        var effect = new ParticleEffect(ParticleTextureName, [new ()]);
        var sut = CreateSystemUnderTest(effect);
        sut.Dispose();

        // Act
        var act = () => sut.RemoveBehavior(BehaviorAttribute.Angle);

        // Assert
        act.Should().Throw<ObjectDisposedException>()
            .WithMessage("Cannot access a disposed object.\nObject name: 'ParticlePool'.");
    }

    [Fact]
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules",
        "SA1129:Do not use default value type constructor",
        Justification = "Required for proper testing.")]
    public void RemoveBehavior_WhenInvoked_AddsNewBehavior()
    {
        // Arrange
        var particleA = new Particle();
        var particleB = new Particle();

        this.mockParticleFactory.Create(Arg.Any<IBehavior[]>())
            .Returns(particleA, particleB);

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

        this.mockBehaviorFactory.CreateEasingRandomBehavior(Arg.Any<EasingRandomBehaviorSettings>())
            .Returns(callInfo =>
                {
                    var settings = callInfo.Arg<EasingRandomBehaviorSettings>();

                    if (settings.ApplyToAttribute == BehaviorAttribute.Angle)
                    {
                        return new EasingRandomBehavior(angleSettings, this.mockRandomizerService);
                    }

                    if (settings.ApplyToAttribute == BehaviorAttribute.BlueColorComponent)
                    {
                        return new EasingRandomBehavior(blueSettings, this.mockRandomizerService);
                    }

                    throw new InvalidOperationException("The current settings are not supported for this unit test.");
                });

        var sut = CreateSystemUnderTest(effect);
        sut.AddBehavior(angleSettings);
        sut.AddBehavior(blueSettings);

        // Act
        sut.RemoveBehavior(BehaviorAttribute.BlueColorComponent);

        // Assert
        particleA.Behaviors.Should().NotBeEmpty();
        // Check that the particle does not contain a blue color component behavior
        particleA.Behaviors
            .Should()
            .Match(behavior => behavior.Any(b => b.BehaviorType != BehaviorAttribute.BlueColorComponent));
        particleB.Behaviors.Should().NotBeEmpty();

        // Check that the particle does not contain a blue color component behavior
        particleB.Behaviors
            .Should()
            .Match(behavior => behavior.Any(b => b.BehaviorType != BehaviorAttribute.BlueColorComponent));
    }

    [Fact]
    [SuppressMessage("csharpsquid", "S3966", Justification = "Redundant intentional for testing.")]
    public void Dispose_WhenInvoked2Times_DisposesOfPoolOneTime()
    {
        // Arrange
        var sut = CreateSystemUnderTest(default);

        // Act
        sut.Dispose();
        sut.Dispose();

        // Assert
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
