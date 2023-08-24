// <copyright file="ParticleEngineTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace PlazmaTests;

using System;
using Fakes;
using FluentAssertions;
using Plazma;
using Plazma.Behaviors;
using NSubstitute;
using Xunit;

/// <summary>
/// Tests the <see cref="ParticleEngine{TTexture}"/> class.
/// </summary>
public class ParticleEngineTests
{
    private readonly ITextureLoader<IDisposable> mockTextureLoader;

    /// <summary>
    /// Initializes a new instance of the <see cref="ParticleEngineTests"/> class.
    /// </summary>
    public ParticleEngineTests()
    {
        var fakeTexture = Substitute.For<IDisposable>();
        this.mockTextureLoader = Substitute.For<ITextureLoader<IDisposable>>();
        this.mockTextureLoader.LoadTexture(Arg.Any<string>()).Returns(fakeTexture);
    }

    #region Prop Tests
    [Fact]
    public void Enabled_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var sut = CreateSystemUnderTest();
        sut.Enabled = false;

        // Assert
        sut.Enabled.Should().BeFalse();
    }

    [Fact]
    public void ParticlePools_WhenGettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var mockPool = Substitute.For<IParticlePool<FakeTexture>>();

        var sut = CreateSystemUnderTest();
        sut.AddPool(mockPool);

        // Act
        var actual = sut.ParticlePools;

        // Assert
        actual.Should().ContainSingle();
    }

    [Fact]
    public void TexturesLoaded_WhenGettingValueAfterTexturesAreLoaded_ReturnsTrue()
    {
        // Arrange
        var mockPool = Substitute.For<IParticlePool<FakeTexture>>();
        mockPool.TextureLoaded.Returns(true);

        var sut = CreateSystemUnderTest();
        sut.AddPool(mockPool);
        sut.LoadTextures();

        // Act
        var actual = sut.TexturesLoaded;

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

                textureALoaded = true;
                return mockPool1Texture;
            });

        var sut = new ParticleEngine<IDisposable>();

        var mockPoolA = Substitute.For<IParticlePool<FakeTexture>>();
        var mockPoolB = Substitute.For<IParticlePool<FakeTexture>>();

        // Create 2 pools
        sut.AddPool(mockPoolA);
        sut.AddPool(mockPoolB);
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
        var mockPool = Substitute.For<IParticlePool<FakeTexture>>();
        mockPool.TextureLoaded.Returns(true);

        var sut = CreateSystemUnderTest();
        sut.AddPool(mockPool);

        // Act
        sut.LoadTextures();

        // Assert
        mockPool.Received(1).LoadTexture();
    }

    [Fact]
    public void Update_WithTexturesNotLoaded_ThrowsException()
    {
        // Arrange
        var mockPool = Substitute.For<IParticlePool<FakeTexture>>();

        var sut = CreateSystemUnderTest();
        sut.AddPool(mockPool);

        // Act
        var act = () => sut.Update(new TimeSpan(0, 0, 0, 0, 16));

        // Assert
        act.Should().Throw<Exception>()
            .WithMessage("The textures must be loaded first.");
    }

    [Fact]
    public void Update_WhenDisabled_DoesNotUpdateParticles()
    {
        // Arrange
        var mockBehavior = Substitute.For<IBehavior>();

        var mockPool = Substitute.For<IParticlePool<FakeTexture>>();
        mockPool.TextureLoaded.Returns(true);

        var timeElapsed = 16.ToMillisecondsTimeSpan();

        var sut = CreateSystemUnderTest();
        sut.Enabled = false;
        sut.AddPool(mockPool);
        sut.LoadTextures();

        // Act
        sut.Update(timeElapsed);

        // Assert
        mockBehavior.DidNotReceive().Update(Arg.Any<TimeSpan>());
    }

    [Fact]
    public void Update_WhenEnabled_UpdatesPools()
    {
        var mockPool = Substitute.For<IParticlePool<FakeTexture>>();
        mockPool.TextureLoaded.Returns(true);

        var sut = CreateSystemUnderTest();
        sut.Enabled = true;
        sut.AddPool(mockPool);

        var timeElapsed = 16.ToMillisecondsTimeSpan();

        // Act
        sut.Update(timeElapsed);

        // Assert
        mockPool.Received(1).Update(timeElapsed);
    }
    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="ParticleEngine{FakeTexture}"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private ParticleEngine<FakeTexture> CreateSystemUnderTest() => new ();
}
