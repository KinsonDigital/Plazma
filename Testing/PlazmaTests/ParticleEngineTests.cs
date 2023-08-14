// <copyright file="ParticleEngineTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace PlazmaTests;

using System;
using FluentAssertions;
using Plazma;
using Plazma.Behaviors;
using Plazma.Services;
using NSubstitute;
using Xunit;

/// <summary>
/// Tests the <see cref="ParticleEngine{TTexture}"/> class.
/// </summary>
public class ParticleEngineTests
{
    private readonly ITextureLoader<IDisposable> mockTextureLoader;
    private readonly IBehaviorFactory mockBehaviorFactory;
    private readonly IRandomizerService mockRandomizerService;
    private readonly ParticleEngine<IDisposable> engine;

    /// <summary>
    /// Initializes a new instance of the <see cref="ParticleEngineTests"/> class.
    /// </summary>
    public ParticleEngineTests()
    {
        this.mockRandomizerService = Substitute.For<IRandomizerService>();

        var fakeTexture = Substitute.For<IDisposable>();
        this.mockTextureLoader = Substitute.For<ITextureLoader<IDisposable>>();
        this.mockTextureLoader.LoadTexture(Arg.Any<string>()).Returns(fakeTexture);

        this.mockBehaviorFactory = Substitute.For<IBehaviorFactory>();

        this.engine = new ParticleEngine<IDisposable>(this.mockTextureLoader, this.mockRandomizerService);
    }

    #region Prop Tests
    [Fact]
    public void Enabled_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        this.engine.Enabled = false;

        // Assert
        this.engine.Enabled.Should().BeFalse();
    }

    [Fact]
    public void ParticlePools_WhenGettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var settings = new BehaviorSettings[] { new EasingRandomBehaviorSettings(), };
        var effect = new ParticleEffect(null, settings);
        this.engine.CreatePool(effect, this.mockBehaviorFactory);
        this.engine.LoadTextures();
        this.engine.Update(new TimeSpan(0, 0, 0, 0, 16));

        // Act
        var actual = this.engine.ParticlePools;

        // Assert
        actual.Should().ContainSingle();
        this.engine.ParticlePools[0].Effect.Should().Be(effect);
    }

    [Fact]
    public void TexturesLoaded_WhenGettingValueAfterTexturesAreLoaded_ReturnsTrue()
    {
        // Arrange
        var settings = new BehaviorSettings[] { new EasingRandomBehaviorSettings(), };
        var effect = new ParticleEffect(null, settings);
        this.engine.CreatePool(effect, this.mockBehaviorFactory);
        this.engine.LoadTextures();

        // Act
        var actual = this.engine.TexturesLoaded;

        // Assert
        actual.Should().BeTrue();
    }
    #endregion

    #region Method Tests
    [Fact]
    public void ClearPools_WhenInvoked_DisposesOfManagedResources()
    {
        // Arrange
        var mockPool1Texture = Substitute.For<IDisposable>();
        var mockPool2Texture = Substitute.For<IDisposable>();
        var textureALoaded = false;

        this.mockTextureLoader.LoadTexture(Arg.Any<string>()).Returns((_) =>
            {
                // Load the correct texture depending on the pool.
                // All pools use the same instance of texture loader so we have
                // to mock out the correct texture to go with the correct pool,
                // so we can verify that each pool is disposing of there textures
                if (textureALoaded)
                {
                    return mockPool2Texture;
                }
                else
                {
                    textureALoaded = true;
                    return mockPool1Texture;
                }
            });

        var effect = new ParticleEffect(null, Array.Empty<BehaviorSettings>());
        var sut = new ParticleEngine<IDisposable>(this.mockTextureLoader, this.mockRandomizerService);

        // Create 2 pools
        sut.CreatePool(effect, this.mockBehaviorFactory);
        sut.CreatePool(effect, this.mockBehaviorFactory);
        sut.LoadTextures();

        // Act
        sut.ClearPools();

        // Assert
        sut.ParticlePools.Should().BeEmpty();
    }

    [Fact]
    public void LoadTextures_WhenInvoked_LoadsParticlePoolTextures()
    {
        // Arrange
        var settings = new BehaviorSettings[] { new EasingRandomBehaviorSettings(), };
        var effect = new ParticleEffect("texture-name", settings);
        this.engine.CreatePool(effect, this.mockBehaviorFactory);
        this.engine.LoadTextures();
        this.engine.Update(new TimeSpan(0, 0, 0, 0, 16));

        // Act
        var unused = this.engine.ParticlePools;

        // Assert
        this.mockTextureLoader.Received(1).LoadTexture("texture-name");
    }

    [Fact]
    public void Update_WithTexturesNotLoaded_ThrowsException()
    {
        // Arrange
        var effect = new ParticleEffect();
        this.engine.CreatePool(effect, this.mockBehaviorFactory);

        // Act
        var act = () => this.engine.Update(new TimeSpan(0, 0, 0, 0, 16));

        // Assert
        act.Should().Throw<Exception>()
            .WithMessage("The textures must be loaded first.");
    }

    [Fact]
    public void Update_WhenDisabled_DoesNotUpdateParticles()
    {
        // Arrange
        var settings = new BehaviorSettings[] { new EasingRandomBehaviorSettings(), };
        var effect = new ParticleEffect(null, settings);
        var mockBehavior = Substitute.For<IBehavior>();

        this.mockBehaviorFactory.CreateBehaviors(settings, this.mockRandomizerService).Returns(new[] { mockBehavior });
        this.engine.Enabled = false;
        this.engine.CreatePool(effect, this.mockBehaviorFactory);
        this.engine.LoadTextures();

        // Act
        this.engine.Update(new TimeSpan(0, 0, 0, 0, 16));

        // Assert
        mockBehavior.DidNotReceive().Update(Arg.Any<TimeSpan>());
    }

    [Fact]
    public void Update_WhenEnabled_UpdatesAllParticles()
    {
        // Arrange
        var settings = new BehaviorSettings[] { new EasingRandomBehaviorSettings(), };
        var effect = new ParticleEffect(null, settings) { TotalParticlesAliveAtOnce = 2, };
        var mockBehavior = Substitute.For<IBehavior>();
        mockBehavior.Enabled.Returns(true);
        mockBehavior.Value.Returns("0");

        this.mockRandomizerService.GetValue(Arg.Any<int>(), Arg.Any<int>()).Returns(16);
        this.mockBehaviorFactory.CreateBehaviors(Arg.Any<BehaviorSettings[]>(), this.mockRandomizerService)
            .Returns(new[] { mockBehavior });

        this.engine.CreatePool(effect, this.mockBehaviorFactory);
        this.engine.LoadTextures();

        // Act
        this.engine.Update(new TimeSpan(0, 0, 0, 0, 16));
        this.engine.Update(new TimeSpan(0, 0, 0, 0, 16));

        // Assert
        mockBehavior.Received(3).Update(Arg.Any<TimeSpan>());
    }
    #endregion
}
