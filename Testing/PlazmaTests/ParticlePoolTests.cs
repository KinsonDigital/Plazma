// <copyright file="ParticlePoolTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace PlazmaTests;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
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

#pragma warning disable SA1514
    #region Test Data
    /// <summary>
    /// Gets the data used to test the change of a particle's color component.
    /// </summary>
    public static TheoryData<BehaviorAttribute, Color> ColorAttrData =>
        new ()
        {
            { BehaviorAttribute.AlphaColorComponent, Color.FromArgb(100, 20, 30, 40) },
            { BehaviorAttribute.RedColorComponent, Color.FromArgb(10, 100, 30, 40) },
            { BehaviorAttribute.GreenColorComponent, Color.FromArgb(10, 20, 100, 40) },
            { BehaviorAttribute.BlueColorComponent, Color.FromArgb(10, 20, 30, 100) },
        };
    #endregion
#pragma warning restore SA1514

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
            SpawnLocation = new Vector2(11, 22), UseColorsFromList = true, TotalParticles = 3,
        };

        var createLiving = true;

        this.mockParticleFactory.Create(Arg.Any<IBehavior[]>())
            .Returns(
                (_) =>
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
            SpawnLocation = new Vector2(11, 22), UseColorsFromList = true, TotalParticles = 3,
        };

        var createLiving = true;

        this.mockParticleFactory.Create(Arg.Any<IBehavior[]>())
            .Returns(
                (_) =>
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
            SpawnLocation = new Vector2(11, 22), UseColorsFromList = true, TotalParticles = 3,
        };

        var createLiving = true;

        this.mockParticleFactory.Create(Arg.Any<IBehavior[]>())
            .Returns(
                (_) =>
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
            SpawnLocation = new Vector2(11, 22), UseColorsFromList = true, TotalParticles = 3,
        };

        var particleA = new Particle { IsAlive = true };
        var particleB = new Particle { IsAlive = false };
        var particleC = new Particle { IsAlive = true };

        var particles = new List<Particle> { particleA, particleB, particleC, };

        this.mockParticleFactory.Create(Arg.Any<IBehavior[]>())
            .Returns(
                (_) =>
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
        var settings = new EasingRandomBehaviorSettings[] { new (), };
        var effect = new ParticleEffect(ParticleTextureName, settings)
        {
            SpawnLocation = new Vector2(11, 22), TotalParticles = 99, UseColorsFromList = true,
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
            SpawnLocation = new Vector2(11, 22), BurstOffMilliseconds = 10, BurstOnMilliseconds = 10, BurstEnabled = burstingEnabled,
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
            SpawnLocation = new Vector2(11, 22), BurstOffMilliseconds = 10, BurstOnMilliseconds = 10,
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
            SpawnLocation = new Vector2(11, 22), BurstOffMilliseconds = 10, BurstOnMilliseconds = 10,
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

    [Fact]
    public void Update_WithInvalidBehaviorType_ThrowsException()
    {
        // Arrange
        const string expected = "The value of argument 'BehaviorAttribute' (900) is invalid for Enum " +
                                "type 'BehaviorAttribute'. (Parameter 'BehaviorAttribute')";
        var mockBehavior = Substitute.For<IBehavior>();
        mockBehavior.BehaviorType.Returns((BehaviorAttribute)900); // Force an invalid value
        mockBehavior.Enabled.Returns(true);

        this.mockBehaviorFactory.CreateEasingRandomBehavior(Arg.Any<EasingRandomBehaviorSettings>())
            .Returns(mockBehavior);

        this.mockParticleFactory.Create(Arg.Any<IBehavior[]>())
            .Returns(_ => new Particle([mockBehavior]));

        var effect = new ParticleEffect
        {
            BurstEnabled = false,
            LimitSpawnRate = false,
            SpawnRateMin = 10,
            SpawnRateMax = 20,
        };

        var sut = CreateSystemUnderTest(effect);

        // Act
        var act = () => sut.Update(0.ToMillisecondsTimeSpan());

        // Assert
        act.Should().Throw<InvalidEnumArgumentException>()
            .WithMessage(expected);
    }

    [Fact]
    public void Update_WhenParticleHasNoBehaviors_DoesNotThrowException()
    {
        // Arrange
        this.mockParticleFactory.Create(Arg.Any<IBehavior[]>())
            .Returns(_ => default);

        var effect = new ParticleEffect
        {
            BurstEnabled = false,
            LimitSpawnRate = false,
            SpawnRateMin = 10,
            SpawnRateMax = 20,
        };

        var sut = CreateSystemUnderTest(effect);

        // Act
        var act = () => sut.Update(0.ToMillisecondsTimeSpan());

        // Assert
        act.Should().NotThrow<NullReferenceException>();
    }

    [Theory]
    [InlineData(BehaviorAttribute.X, 10, 100, 30, 30)]
    [InlineData(BehaviorAttribute.Y, 10, 10, 30, 100)]
    public void Update_WhenInvokedWithXAndYBehaviorAttribute_UpdatesParticleXAndY(
        BehaviorAttribute attribute,
        float startX,
        float endX,
        float startY,
        float endY)
    {
        // Arrange
        var mockBehavior = Substitute.For<IBehavior>();
        mockBehavior.BehaviorType.Returns(attribute);
        mockBehavior.Enabled.Returns(true);
        mockBehavior.Value.Returns(100);

        this.mockBehaviorFactory.CreateEasingRandomBehavior(Arg.Any<EasingRandomBehaviorSettings>())
            .Returns(mockBehavior);

        var particle = new Particle([mockBehavior])
        {
            Position = new Vector2(startX, startY),
            IsAlive = true,
        };

        this.mockParticleFactory.Create(Arg.Any<IBehavior[]>())
            .Returns(_ => particle);

        var effect = new ParticleEffect
        {
            BurstEnabled = false,
            LimitSpawnRate = false,
            SpawnRateMin = 10,
            SpawnRateMax = 20,
        };

        var sut = CreateSystemUnderTest(effect);

        // Act
        sut.Update(0.ToMillisecondsTimeSpan());

        // Assert
        sut.Particles[0].Position.Should().Be(new Vector2(endX, endY));
    }

    [Theory]
    [MemberData(nameof(ColorAttrData))]
    public void Update_WhenInvokedWithColorBehaviorAttribute_UpdatesParticleColorComponent(
        BehaviorAttribute attribute,
        Color expected)
    {
        // Arrange
        var mockBehavior = Substitute.For<IBehavior>();
        mockBehavior.BehaviorType.Returns(attribute);
        mockBehavior.Enabled.Returns(true);
        mockBehavior.Value.Returns(100);

        this.mockBehaviorFactory.CreateEasingRandomBehavior(Arg.Any<EasingRandomBehaviorSettings>())
            .Returns(mockBehavior);

        var particle = new Particle([mockBehavior])
        {
            IsAlive = true,
            TintColor = Color.FromArgb(10, 20, 30, 40),
        };

        this.mockParticleFactory.Create(Arg.Any<IBehavior[]>())
            .Returns(_ => particle);

        var effect = new ParticleEffect
        {
            BurstEnabled = false,
            LimitSpawnRate = false,
            SpawnRateMin = 10,
            SpawnRateMax = 20,
        };

        var sut = CreateSystemUnderTest(effect);

        // Act
        sut.Update(0.ToMillisecondsTimeSpan());

        // Assert
        sut.Particles[0].TintColor.Should().Be(expected);
    }

    // [Fact]
    [Theory]
    [InlineData(BehaviorAttribute.Angle, 10, 100, 30, 30)]
    [InlineData(BehaviorAttribute.Size, 10, 10, 30, 100)]
    public void Update_WhenInvokedWithAngleAndSizeBehaviorAttribute_UpdatesParticleAngleAndSize(
        BehaviorAttribute attribute,
        float startAngle,
        float endAngle,
        float startSize,
        float endSize)
    {
        // Arrange
        var mockBehavior = Substitute.For<IBehavior>();
        mockBehavior.BehaviorType.Returns(attribute);
        mockBehavior.Enabled.Returns(true);
        mockBehavior.Value.Returns(100);

        this.mockBehaviorFactory.CreateEasingRandomBehavior(Arg.Any<EasingRandomBehaviorSettings>())
            .Returns(mockBehavior);

        var particle = new Particle([mockBehavior])
        {
            IsAlive = true,
            Angle = startAngle,
            Size = startSize,
        };

        this.mockParticleFactory.Create(Arg.Any<IBehavior[]>())
            .Returns(_ => particle);

        var effect = new ParticleEffect
        {
            BurstEnabled = false,
            LimitSpawnRate = false,
            SpawnRateMin = 10,
            SpawnRateMax = 20,
        };

        var sut = CreateSystemUnderTest(effect);

        // Act
        sut.Update(0.ToMillisecondsTimeSpan());

        // Assert
        sut.Particles[0].Angle.Should().Be(endAngle);
        sut.Particles[0].Size.Should().Be(endSize);
    }

    [Theory]
    [InlineData(true, 0, false)]
    [InlineData(false, 2, true)]
    [Trait(Category, Methods)]
    public void Update_WithSpawnRateIsLimiting_GeneratesCorrectNumberOfLivingParticles(
        bool limitSpawnRate,
        int expectedLivingParticles,
        bool expectedIsAlive)
    {
        // Arrange
        var effect = new ParticleEffect(ParticleTextureName, [new ()])
        {
            SpawnLocation = new Vector2(11f, 22f), UseColorsFromList = true, TotalParticles = 4, LimitSpawnRate = limitSpawnRate,
        };

        this.mockRandomizerService.GetValue(Arg.Any<float>(), Arg.Any<float>()).Returns(1000);
        this.mockBehaviorFactory.CreateEasingRandomBehavior(Arg.Any<EasingRandomBehaviorSettings>()).Returns(
            callInfo =>
            {
                var settings = callInfo.Arg<EasingRandomBehaviorSettings>();

                return new EasingRandomBehavior(settings, this.mockRandomizerService);
            });

        this.mockParticleFactory
            .Create(Arg.Any<IBehavior[]>()).Returns(
                callInfo =>
                {
                    var behaviors = callInfo.Arg<IBehavior[]>();

                    var newParticle = new Particle(behaviors) { IsAlive = false };

                    return newParticle;
                });

        var sut = CreateSystemUnderTest(effect);

        var timeElapsed = 100.ToMillisecondsTimeSpan();

        // Act
        sut.Update(timeElapsed);
        sut.Update(timeElapsed);

        // Assert
        sut.Particles.Should().HaveCount(4);
        sut.TotalLivingParticles.Should().Be(expectedLivingParticles);
        sut.Particles[0].IsAlive.Should().Be(expectedIsAlive);
        sut.Particles[1].IsAlive.Should().Be(expectedIsAlive);
        sut.Particles[2].IsAlive.Should().BeFalse();
        sut.Particles[3].IsAlive.Should().BeFalse();
    }

    [Theory]
    [InlineData(10f, 20f)]
    [InlineData(20f, 10f)]
    [Trait(Category, Methods)]
    public void Update_WhenGeneratingRandomSpawnRate_CorrectlyUsesMinAndMax(float rateMin, float rateMax)
    {
        // Arrange
        var settings = new EasingRandomBehaviorSettings[] { new (), };
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
    public void Update_WithBurstingEnabled_ReturnsCorrectSpawnRate(
        bool burstingEnabled,
        float expectedRateMin,
        float expectedRateMax,
        int expectedCallCount)
    {
        // Arrange
        var settings = new EasingRandomBehaviorSettings[] { new (), };
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
        sut.Update(16.ToMillisecondsTimeSpan());

        // Assert
        this.mockRandomizerService.Received(expectedCallCount).GetValue(expectedRateMin, expectedRateMax);
    }

    [Theory]
    [InlineData(14, true, false)]
    [InlineData(100, false, true)]
    public void Update_WithBurstingEnabled_SetsInBurstMode(int updateTime, bool startBurstMode, bool expectedBurstMode)
    {
        // Arrange
        var settings = new EasingRandomBehaviorSettings[] { new (), };
        var effect = new ParticleEffect(ParticleTextureName, settings)
        {
            BurstOffMilliseconds = 16f,
            BurstOnMilliseconds = 32f,
            SpawnRateMin = 111f,
            SpawnRateMax = 222f,
            BurstSpawnRateMin = 333f,
            BurstSpawnRateMax = 444f,
            BurstEnabled = true,
        };

        var sut = CreateSystemUnderTest(effect);
        sut.InBurstMode = startBurstMode;

        // Act
        sut.Update(updateTime.ToMillisecondsTimeSpan());
        sut.Update(updateTime.ToMillisecondsTimeSpan());

        // Assert
        sut.InBurstMode.Should().Be(expectedBurstMode);
    }

    [Fact]
    public void Update_WithDisabledBehavior_BehaviorUpdateIsNotInvoked()
    {
        // Arrange
        var mockBehavior = Substitute.For<IBehavior>();

        this.mockBehaviorFactory.CreateEasingRandomBehavior(Arg.Any<EasingRandomBehaviorSettings>())
            .Returns(mockBehavior);

        this.mockParticleFactory.Create(Arg.Any<IBehavior[]>())
            .Returns(_ => new Particle([mockBehavior]));

        var effect = new ParticleEffect
        {
            BurstEnabled = false,
            LimitSpawnRate = false,
            SpawnRateMin = 10,
            SpawnRateMax = 20,
        };

        var sut = CreateSystemUnderTest(effect);

        // Act
        sut.Update(0.ToMillisecondsTimeSpan());

        // Assert
        mockBehavior.DidNotReceive().Update(Arg.Any<TimeSpan>());
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
        var settings = new EasingRandomBehaviorSettings[] { new (), };
        var effect = new ParticleEffect(ParticleTextureName, settings)
        {
            SpawnLocation = new Vector2(11, 22),
            SpawnRateMin = 33,
            SpawnRateMax = 44,
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
        var settings = new EasingRandomBehaviorSettings[] { new (), };
        var effect = new ParticleEffect(ParticleTextureName, settings)
        {
            SpawnLocation = new Vector2(11, 22),
            SpawnRateMin = 33,
            SpawnRateMax = 44,
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
    [SuppressMessage(
        "StyleCop.CSharp.ReadabilityRules",
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

        MockCreateParticles(particleA, particleB);

        var sut = CreateSystemUnderTest(effect);

        // Act
        sut.AddBehavior(settings);

        // Assert
        particleA.Behaviors.Should().ContainSingle();
        particleB.Behaviors.Should().ContainSingle();

        this.mockBehaviorFactory.Received(2).CreateEasingRandomBehavior(settings);
    }

    [Fact]
    public void AddBehavior_WhenAddingBehaviorWithTypeThatAlreadyExists_DoesNotAddBehavior()
    {
        // Arrange
        var effect = new ParticleEffect { TotalParticles = 1 };

        var sut = CreateSystemUnderTest(effect);

        var particleA = default(Particle);

        this.mockRandomizerService.GetValue(Arg.Any<float>(), Arg.Any<float>()).Returns(0f);
        var settings = new EasingRandomBehaviorSettings { ApplyToAttribute = BehaviorAttribute.Angle, };
        var behavior = new EasingRandomBehavior(settings, this.mockRandomizerService);
        this.mockBehaviorFactory.CreateEasingRandomBehavior(Arg.Any<EasingRandomBehaviorSettings>())
            .Returns(behavior);
        MockCreateParticles(particleA);
        sut.AddBehavior(settings);

        // Act
        sut.AddBehavior(settings);

        // Assert
        sut.Particles.Should().ContainSingle();
        sut.Particles[0].Behaviors.Should().ContainSingle();
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
    [SuppressMessage(
        "StyleCop.CSharp.ReadabilityRules",
        "SA1129:Do not use default value type constructor",
        Justification = "Required for proper testing.")]
    public void RemoveBehavior_WhenInvoked_AddsNewBehavior()
    {
        // Arrange
        var particleA = new Particle();
        var particleB = new Particle();

        MockCreateParticles(particleA, particleB);

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
            .Returns(
                callInfo =>
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
        => new (
            this.mockTextureLoader,
            this.mockRandomizerService,
            this.mockBehaviorFactory,
            this.mockParticleFactory,
            effect);

    /// <summary>
    /// Mocks the creation of the given particle.
    /// </summary>
    /// <param name="p">A first particle to mock.</param>
    private void MockCreateParticles(Particle p) =>
        this.mockParticleFactory.Create(Arg.Any<IBehavior[]>())
            .Returns(p);

    /// <summary>
    /// Mocks the creation of the given particles.
    /// </summary>
    /// <param name="p1">A first particle to mock.</param>
    /// <param name="p2">A second particle to mock.</param>
    private void MockCreateParticles(Particle p1, Particle p2) =>
        this.mockParticleFactory.Create(Arg.Any<IBehavior[]>())
            .Returns(p1, p2);
}
