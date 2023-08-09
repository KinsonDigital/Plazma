// <copyright file="ParticleEngineTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

#pragma warning disable CA1724 // The type name conflicts in whole or in part with the namespace name
#pragma warning disable CA1031 // Do not catch general exception types
namespace PlazmaTests;

using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Plazma;
using Plazma.Behaviors;
using Plazma.Services;
using Moq;
using Xunit;

/// <summary>
/// Tests the <see cref="ParticleEngine{TTexture}"/> class.
/// </summary>
public class ParticleEngineTests : IDisposable
{
    private readonly Mock<ITextureLoader<IDisposable>> mockTextureLoader;
    private readonly Mock<IBehaviorFactory> mockBehaviorFactory;
    private Mock<IRandomizerService> mockRandomizerService;
    private ParticleEngine<IDisposable> engine;

    /// <summary>
    /// Initializes a new instance of the <see cref="ParticleEngineTests"/> class.
    /// </summary>
    public ParticleEngineTests()
    {
        this.mockRandomizerService = new Mock<IRandomizerService>();

        var fakeTexture = new Mock<IDisposable>();
        this.mockTextureLoader = new Mock<ITextureLoader<IDisposable>>();
        this.mockTextureLoader.Setup(m => m.LoadTexture(It.IsAny<string>())).Returns(fakeTexture.Object);

        this.mockBehaviorFactory = new Mock<IBehaviorFactory>();

        this.engine = new ParticleEngine<IDisposable>(this.mockTextureLoader.Object, this.mockRandomizerService.Object);
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
        var effect = new ParticleEffect(It.IsAny<string>(), settings);
        this.engine.CreatePool(effect, this.mockBehaviorFactory.Object);
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
        var effect = new ParticleEffect(It.IsAny<string>(), settings);
        this.engine.CreatePool(effect, this.mockBehaviorFactory.Object);
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
        var mockPool1Texture = new Mock<IDisposable>();
        var mockPool2Texture = new Mock<IDisposable>();
        var textureALoaded = false;

        this.mockTextureLoader
            .Setup(m =>
                m.LoadTexture(It.IsAny<string>())).Returns<string>((_) =>
            {
                // Load the correct texture depending on the pool.
                // All pools use the same instance of texture loader so we have
                // to mock out the correct texture to go with the correct pool,
                // so we can verify that each pool is disposing of there textures
                if (textureALoaded)
                {
                    return mockPool2Texture.Object;
                }
                else
                {
                    textureALoaded = true;
                    return mockPool1Texture.Object;
                }
            });

        var effect = new ParticleEffect(It.IsAny<string>(), Array.Empty<BehaviorSettings>());
        var sut = new ParticleEngine<IDisposable>(this.mockTextureLoader.Object, this.mockRandomizerService.Object);

        // Create 2 pools
        sut.CreatePool(effect, this.mockBehaviorFactory.Object);
        sut.CreatePool(effect, this.mockBehaviorFactory.Object);
        sut.LoadTextures();

        // Act
        sut.ClearPools();

        // Assert
        mockPool1Texture.Verify(m => m.Dispose(), Times.Once());
        mockPool2Texture.Verify(m => m.Dispose(), Times.Once());
        sut.ParticlePools.Should().BeEmpty();
    }

    [Fact]
    public void LoadTextures_WhenInvoked_LoadsParticlePoolTextures()
    {
        // Arrange
        var settings = new BehaviorSettings[] { new EasingRandomBehaviorSettings(), };
        var effect = new ParticleEffect("texture-name", settings);
        this.engine.CreatePool(effect, this.mockBehaviorFactory.Object);
        this.engine.LoadTextures();
        this.engine.Update(new TimeSpan(0, 0, 0, 0, 16));

        // Act
        var unused = this.engine.ParticlePools;

        // Assert
        this.mockTextureLoader.Verify(m => m.LoadTexture("texture-name"), Times.Once());
    }

    [Fact]
    public void Update_WithTexturesNotLoaded_ThrowsException()
    {
        // Arrange & Act
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
        var effect = new ParticleEffect(It.IsAny<string>(), settings);
        var mockBehavior = new Mock<IBehavior>();

        this.mockBehaviorFactory.Setup(m => m.CreateBehaviors(settings, this.mockRandomizerService.Object))
            .Returns(new[] { mockBehavior.Object });
        this.engine.Enabled = false;
        this.engine.CreatePool(effect, this.mockBehaviorFactory.Object);
        this.engine.LoadTextures();

        // Act
        this.engine.Update(new TimeSpan(0, 0, 0, 0, 16));

        // Assert
        mockBehavior.Verify(m => m.Update(It.IsAny<TimeSpan>()), Times.Never());
    }

    [Fact]
    public void Update_WhenEnabled_UpdatesAllParticles()
    {
        // Arrange
        var settings = new BehaviorSettings[] { new EasingRandomBehaviorSettings(), };
        var effect = new ParticleEffect(It.IsAny<string>(), settings) { TotalParticlesAliveAtOnce = 2, };
        var mockBehavior = new Mock<IBehavior>();
        mockBehavior.SetupGet(p => p.Enabled).Returns(true);
        mockBehavior.SetupGet(p => p.Value).Returns("0");

        this.mockRandomizerService.Setup(m => m.GetValue(It.IsAny<int>(), It.IsAny<int>())).Returns(16);
        this.mockBehaviorFactory.Setup(m => m.CreateBehaviors(It.IsAny<BehaviorSettings[]>(), this.mockRandomizerService.Object))
            .Returns(new[] { mockBehavior.Object });

        this.engine.CreatePool(effect, this.mockBehaviorFactory.Object);
        this.engine.LoadTextures();

        // Act
        this.engine.Update(new TimeSpan(0, 0, 0, 0, 16));
        this.engine.Update(new TimeSpan(0, 0, 0, 0, 16));

        // Assert
        mockBehavior.Verify(m => m.Update(It.IsAny<TimeSpan>()), Times.Exactly(3));
    }

    [Fact]
    [SuppressMessage("csharpsquid", "S3966", Justification = "Double invoke intended.")]
    public void Dispose_WhenInvoked_DisposesOfManagedResources()
    {
        // Arrange
        var mockPool1Texture = new Mock<IDisposable>();
        var mockPool2Texture = new Mock<IDisposable>();
        var textureALoaded = false;

        this.mockTextureLoader
            .Setup(m =>
                m.LoadTexture(It.IsAny<string>())).Returns<string>((_) =>
            {
                // Load the correct texture depending on the pool.
                // All pools use the same instance of texture loader so we have
                // to mock out the correct texture to go with the correct pool,
                // so we can verify that each pool is disposing of there textures
                if (textureALoaded)
                {
                    return mockPool2Texture.Object;
                }
                else
                {
                    textureALoaded = true;
                    return mockPool1Texture.Object;
                }
            });

        var effect = new ParticleEffect(It.IsAny<string>(), Array.Empty<BehaviorSettings>());
        var sut = new ParticleEngine<IDisposable>(this.mockTextureLoader.Object, this.mockRandomizerService.Object);

        // Create 2 pools
        sut.CreatePool(effect, this.mockBehaviorFactory.Object);
        sut.CreatePool(effect, this.mockBehaviorFactory.Object);
        sut.LoadTextures();

        // Act
        sut.Dispose();

        sut.Dispose();

        // Assert
        mockPool1Texture.Verify(m => m.Dispose(), Times.Once());
        mockPool2Texture.Verify(m => m.Dispose(), Times.Once());
    }

    #endregion

    /// <inheritdoc/>
    public void Dispose()
    {
        this.mockRandomizerService = null;
        this.engine.Dispose();
        this.engine = null;
        GC.SuppressFinalize(this);
    }
}
