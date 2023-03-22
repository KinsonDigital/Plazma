// <copyright file="PartithystTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

#pragma warning disable CA1724 // The type name conflicts in whole or in part with the namespace name
#pragma warning disable CA1031 // Do not catch general exception types
namespace KDParticleEngineTests;

using System;
using Partithyst;
using Partithyst.Behaviors;
using Partithyst.Services;
using KDParticleEngineTests.XUnitHelpers;
using Moq;
using Xunit;

/// <summary>
/// Tests the <see cref="ParticleEngine"/> class.
/// </summary>
public class ParticleEngineTests : IDisposable
{
    private readonly Mock<ITextureLoader<IDisposable>> mockTextureLoader;
    private readonly Mock<IBehaviorFactory> mockBehaviorFactory;
    private readonly Mock<IDisposable> fakeTexture;
    private Mock<IRandomizerService> mockRandomizerService;
    private ParticleEngine<IDisposable> engine;

    /// <summary>
    /// Initializes a new instance of the <see cref="ParticleEngineTests"/> class.
    /// </summary>
    public ParticleEngineTests()
    {
        this.mockRandomizerService = new Mock<IRandomizerService>();

        this.fakeTexture = new Mock<IDisposable>();
        this.mockTextureLoader = new Mock<ITextureLoader<IDisposable>>();
        this.mockTextureLoader.Setup(m => m.LoadTexture(It.IsAny<string>())).Returns(this.fakeTexture.Object);

        this.mockBehaviorFactory = new Mock<IBehaviorFactory>();

        this.engine = new ParticleEngine<IDisposable>(this.mockTextureLoader.Object, this.mockRandomizerService.Object);
    }

    #region Constructor Tests
    [Fact]
    public void Ctor_WhenInvokedWithNullFactory_ThrowsException()
    {
        // Act & Assert
        AssertHelpers.ThrowsWithMessage<ArgumentNullException>(() =>
        {
            var pool = new ParticlePool<IDisposable>(null, null, null, null);
        }, "The parameter must not be null. (Parameter 'behaviorFactory')");
    }
    #endregion

    #region Prop Tests
    [Fact]
    public void Enabled_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        this.engine.Enabled = false;

        // Assert
        Assert.False(this.engine.Enabled);
    }

    [Fact]
    public void ParticlePools_WhenGettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var settings = new EasingRandomBehaviorSettings[]
        {
            new EasingRandomBehaviorSettings(),
        };
        var effect = new ParticleEffect(It.IsAny<string>(), settings);
        this.engine.CreatePool(effect, this.mockBehaviorFactory.Object);
        this.engine.LoadTextures();
        this.engine.Update(new TimeSpan(0, 0, 0, 0, 16));

        // Act
        var actual = this.engine.ParticlePools;

        // Assert
        Assert.Single(actual);
        Assert.Equal(effect, this.engine.ParticlePools[0].Effect);
    }

    [Fact]
    public void TexturesLoaded_WhenGettingValueAfterTexturesAreLoaded_ReturnsTrue()
    {
        // Arrange
        var settings = new EasingRandomBehaviorSettings[]
        {
            new EasingRandomBehaviorSettings(),
        };
        var effect = new ParticleEffect(It.IsAny<string>(), settings);
        this.engine.CreatePool(effect, this.mockBehaviorFactory.Object);
        this.engine.LoadTextures();

        // Act
        var actual = this.engine.TexturesLoaded;

        // Assert
        Assert.True(actual);
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

        this.mockTextureLoader.Setup(m => m.LoadTexture(It.IsAny<string>())).Returns<string>((textureName) =>
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
        var engine = new ParticleEngine<IDisposable>(this.mockTextureLoader.Object, this.mockRandomizerService.Object);

        // Create 2 pools
        engine.CreatePool(effect, this.mockBehaviorFactory.Object);
        engine.CreatePool(effect, this.mockBehaviorFactory.Object);
        engine.LoadTextures();

        // Act
        engine.ClearPools();

        // Assert
        mockPool1Texture.Verify(m => m.Dispose(), Times.Once());
        mockPool2Texture.Verify(m => m.Dispose(), Times.Once());
        Assert.Empty(engine.ParticlePools);
    }

    [Fact]
    public void LoadTextures_WhenInvoked_LoadsParticlePoolTextures()
    {
        // Arrange
        var settings = new EasingRandomBehaviorSettings[]
        {
            new EasingRandomBehaviorSettings(),
        };
        var effect = new ParticleEffect("texture-name", settings);
        this.engine.CreatePool(effect, this.mockBehaviorFactory.Object);
        this.engine.LoadTextures();
        this.engine.Update(new TimeSpan(0, 0, 0, 0, 16));

        // Act
        var actual = this.engine.ParticlePools;

        // Assert
        this.mockTextureLoader.Verify(m => m.LoadTexture("texture-name"), Times.Once());
    }

    [Fact]
    public void Update_WithTexturesNotLoaded_ThrowsException()
    {
        // Arrange
        this.mockTextureLoader.Setup(m => m.LoadTexture(It.IsAny<string>())).Returns(() => null);

        // Act & Assert
        AssertHelpers.ThrowsWithMessage<Exception>(() =>
        {
            this.engine.Update(new TimeSpan(0, 0, 0, 0, 16));
        }, "The textures must be loaded first.");
    }

    [Fact]
    public void Update_WhenDisabled_DoesNotUpdateParticles()
    {
        // Arrange
        var settings = new EasingRandomBehaviorSettings[]
        {
            new EasingRandomBehaviorSettings(),
        };
        var effect = new ParticleEffect(It.IsAny<string>(), settings);
        var mockBehavior = new Mock<IBehavior>();

        this.mockBehaviorFactory.Setup(m => m.CreateBehaviors(settings, this.mockRandomizerService.Object))
            .Returns(new IBehavior[] { mockBehavior.Object });
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
        var settings = new EasingRandomBehaviorSettings[]
        {
            new EasingRandomBehaviorSettings(),
        };
        var effect = new ParticleEffect(It.IsAny<string>(), settings)
        {
            TotalParticlesAliveAtOnce = 2,
        };
        var mockBehavior = new Mock<IBehavior>();
        mockBehavior.SetupGet(p => p.Enabled).Returns(true);
        mockBehavior.SetupGet(p => p.Value).Returns("0");

        this.mockRandomizerService.Setup(m => m.GetValue(It.IsAny<int>(), It.IsAny<int>())).Returns(16);
        this.mockBehaviorFactory.Setup(m => m.CreateBehaviors(settings, this.mockRandomizerService.Object))
            .Returns(new IBehavior[] { mockBehavior.Object });

        this.engine.CreatePool(effect, this.mockBehaviorFactory.Object);
        this.engine.LoadTextures();

        // Act
        this.engine.Update(new TimeSpan(0, 0, 0, 0, 16));
        this.engine.Update(new TimeSpan(0, 0, 0, 0, 16));

        // Assert
        mockBehavior.Verify(m => m.Update(It.IsAny<TimeSpan>()), Times.Exactly(3));
    }

    [Fact]
    public void Dispose_WhenInvoked_DisposesOfManagedResources()
    {
        // Arrange
        var mockPool1Texture = new Mock<IDisposable>();
        var mockPool2Texture = new Mock<IDisposable>();
        var textureALoaded = false;

        this.mockTextureLoader.Setup(m => m.LoadTexture(It.IsAny<string>())).Returns<string>((textureName) =>
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
        var engine = new ParticleEngine<IDisposable>(this.mockTextureLoader.Object, this.mockRandomizerService.Object);

        // Create 2 pools
        engine.CreatePool(effect, this.mockBehaviorFactory.Object);
        engine.CreatePool(effect, this.mockBehaviorFactory.Object);
        engine.LoadTextures();

        // Act
        engine.Dispose();
        engine.Dispose();

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

    /// <summary>
    /// Asserts if an action does not throw a null reference exception.
    /// </summary>
    /// <param name="action">The action to catch the exception against.</param>
    private static void DoesNotThrowNullReference(Action action)
    {
        try
        {
            action();
        }
        catch (Exception ex)
        {
            if (ex.GetType() == typeof(NullReferenceException))
            {
                Assert.True(false, $"Expected not to raise a {nameof(NullReferenceException)} exception.");
            }
            else
            {
                Assert.True(true);
            }
        }
    }
}
