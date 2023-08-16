// <copyright file="ParticleTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace PlazmaTests;

using System;
using System.ComponentModel;
using System.Numerics;
using FluentAssertions;
using Plazma;
using Plazma.Behaviors;
using NSubstitute;
using Xunit;

/// <summary>
/// Tests the <see cref="Particle"/> class.
/// </summary>
public class ParticleTests : Tests
{
    private readonly TimeSpan frameTime;

    /// <summary>
    /// Initializes a new instance of the <see cref="ParticleTests"/> class.
    /// </summary>
    public ParticleTests() => this.frameTime = 16.ToMillisecondsTimeSpan();

    #region Prop Tests
    [Fact]
    [Trait(Category, Props)]
    public void Position_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var sut = new Particle(Array.Empty<IBehavior>())
        {
            Position = new Vector2(11, 22),
        };

        // Act
        var actual = sut.Position;

        // Assert
        actual.Should().Be(new Vector2(11, 22));
    }

    [Fact]
    [Trait(Category, Props)]
    public void Angle_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var sut = new Particle(Array.Empty<IBehavior>())
        {
            Angle = 1234f,
        };

        // Act
        var actual = sut.Angle;

        // Assert
        actual.Should().Be(1234f);
    }

    [Fact]
    [Trait(Category, Props)]
    public void Size_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var sut = new Particle(Array.Empty<IBehavior>())
        {
            Size = 1019f,
        };

        // Act
        var actual = sut.Size;

        // Assert
        actual.Should().Be(1019f);
    }

    [Fact]
    [Trait(Category, Props)]
    public void IsAlive_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var sut = new Particle(Array.Empty<IBehavior>())
        {
            IsAlive = true,
        };

        // Assert
        sut.IsAlive.Should().BeTrue();
    }
    #endregion

    #region Method Tests
    [Fact]
    [Trait(Category, Methods)]
    public void Update_WithInvalidParticleAttribute_ThrowsException()
    {
        // Arrange
        var mockBehavior = Substitute.For<IBehavior>();
        mockBehavior.BehaviorType.Returns((BehaviorAttribute)999);
        mockBehavior.Enabled.Returns(true);

        var sut = new Particle(new[] { mockBehavior });

        // Act
        var act = () => sut.Update(16.ToMillisecondsTimeSpan());

        // Assert
        var expected = $"The value of argument '{nameof(BehaviorAttribute)}' (999) is invalid for Enum type '{nameof(BehaviorAttribute)}'.";
        expected += " (Parameter 'ParticleAttribute')";
        act.Should().Throw<InvalidEnumArgumentException>()
            .WithMessage(expected);
    }

    [Fact]
    [Trait(Category, Methods)]
    public void Update_WithDisabledBehavior_BehaviorShouldNotUpdate()
    {
        // Arrange
        var mockBehavior = Substitute.For<IBehavior>();
        mockBehavior.Value.Returns(123);
        mockBehavior.Enabled.Returns(false);
        var sut = new Particle(new[] { mockBehavior });

        // Act
        sut.Update(this.frameTime);

        // Assert
        mockBehavior.DidNotReceive().Update(Arg.Any<TimeSpan>());
    }

    [Fact]
    [Trait(Category, Methods)]
    public void Update_WithEnabledBehavior_BehaviorShouldUpdate()
    {
        // Arrange
        var mockBehavior = Substitute.For<IBehavior>();
        mockBehavior.Enabled.Returns(true);
        mockBehavior.Value.Returns(123);

        var sut = new Particle(new[] { mockBehavior });

        // Act
        sut.Update(this.frameTime);

        // Assert
        mockBehavior.Received(1).Update(Arg.Any<TimeSpan>());
    }

    [Fact]
    [Trait(Category, Methods)]
    public void Update_WhenApplyingToXAttribute_UpdatesPositionX()
    {
        // Arrange
        var mockBehavior = Substitute.For<IBehavior>();
        mockBehavior.Enabled.Returns(true);
        mockBehavior.Value.Returns(123);
        mockBehavior.BehaviorType.Returns(BehaviorAttribute.X);

        var sut = new Particle(new[] { mockBehavior });

        // Act
        sut.Update(this.frameTime);

        // Assert
        sut.Position.X.Should().Be(123);
    }

    [Fact]
    [Trait(Category, Methods)]
    public void Update_WhenApplyingToYAttribute_UpdatesPositionY()
    {
        // Arrange
        var mockBehavior = Substitute.For<IBehavior>();
        mockBehavior.Enabled.Returns(true);
        mockBehavior.Value.Returns(123);
        mockBehavior.BehaviorType.Returns(BehaviorAttribute.Y);

        var sut = new Particle(new[] { mockBehavior });

        // Act
        sut.Update(this.frameTime);

        // Assert
        sut.Position.Y.Should().Be(123);
    }

    [Fact]
    [Trait(Category, Methods)]
    public void Update_WhenApplyingToAngleAttribute_UpdatesAngle()
    {
        // Arrange
        var mockBehavior = Substitute.For<IBehavior>();
        mockBehavior.Enabled.Returns(true);
        mockBehavior.Value.Returns(123);
        mockBehavior.BehaviorType.Returns(BehaviorAttribute.Angle);

        var sut = new Particle(new[] { mockBehavior });

        // Act
        sut.Update(this.frameTime);

        // Assert
        sut.Angle.Should().Be(123);
    }

    [Fact]
    [Trait(Category, Methods)]
    public void Update_WhenApplyingToSizeAttribute_UpdatesSize()
    {
        // Arrange
        var mockBehavior = Substitute.For<IBehavior>();
        mockBehavior.Enabled.Returns(true);
        mockBehavior.Value.Returns(123);
        mockBehavior.BehaviorType.Returns(BehaviorAttribute.Size);

        var sut = new Particle(new[] { mockBehavior });

        // Act
        sut.Update(this.frameTime);

        // Assert
        sut.Size.Should().Be(123);
    }

    [Fact]
    [Trait(Category, Methods)]
    public void Update_WhenApplyingToRedColorComponentAttribute_UpdatesRedColorComponent()
    {
        // Arrange
        var mockBehavior = Substitute.For<IBehavior>();
        mockBehavior.Enabled.Returns(true);
        mockBehavior.Value.Returns(123);
        mockBehavior.BehaviorType.Returns(BehaviorAttribute.RedColorComponent);

        var sut = new Particle(new[] { mockBehavior });

        // Act
        sut.Update(this.frameTime);

        // Assert
        sut.TintColor.R.Should().Be(123);
    }

    [Fact]
    [Trait(Category, Methods)]
    public void Update_WhenApplyingToGreenColorComponentAttribute_UpdatesGreenColorComponent()
    {
        // Arrange
        var mockBehavior = Substitute.For<IBehavior>();
        mockBehavior.Enabled.Returns(true);
        mockBehavior.Value.Returns(123);
        mockBehavior.BehaviorType.Returns(BehaviorAttribute.GreenColorComponent);

        var sut = new Particle(new[] { mockBehavior });

        // Act
        sut.Update(this.frameTime);

        // Assert
        sut.TintColor.G.Should().Be(123);
    }

    [Fact]
    [Trait(Category, Methods)]
    public void Update_WhenApplyingToBlueColorComponentAttribute_UpdatesBlueColorComponent()
    {
        // Arrange
        var mockBehavior = Substitute.For<IBehavior>();
        mockBehavior.Enabled.Returns(true);
        mockBehavior.Value.Returns(123);
        mockBehavior.BehaviorType.Returns(BehaviorAttribute.BlueColorComponent);

        var sut = new Particle(new[] { mockBehavior });

        // Act
        sut.Update(this.frameTime);

        // Assert
        sut.TintColor.B.Should().Be(123);
    }

    [Fact]
    [Trait(Category, Methods)]
    public void Update_WhenApplyingToAlphaColorComponentAttribute_UpdatesAlphaColorComponent()
    {
        // Arrange
        var mockBehavior = Substitute.For<IBehavior>();
        mockBehavior.Enabled.Returns(true);
        mockBehavior.Value.Returns(123);
        mockBehavior.BehaviorType.Returns(BehaviorAttribute.AlphaColorComponent);

        var sut = new Particle(new[] { mockBehavior });

        // Act
        sut.Update(this.frameTime);

        // Assert
        sut.TintColor.A.Should().Be(123);
    }

    [Fact]
    [Trait(Category, Methods)]
    public void Reset_WhenInvoked_ResetsAllBehaviors()
    {
        // Arrange
        var mockBehavior = Substitute.For<IBehavior>();
        var sut = new Particle(new[] { mockBehavior });

        // Act
        sut.Reset();

        // Assert
        mockBehavior.Received(1).Reset();
    }

    [Fact]
    [Trait(Category, Methods)]
    public void Reset_WhenInvoked_ResetsAngle()
    {
        // Arrange
        var mockBehavior = Substitute.For<IBehavior>();
        mockBehavior.Value.Returns(123);
        mockBehavior.BehaviorType.Returns(BehaviorAttribute.Angle);
        var sut = new Particle(new[] { mockBehavior });

        // Act
        sut.Update(this.frameTime);
        sut.Reset();

        // Assert
        sut.Angle.Should().Be(0);
    }

    [Fact]
    [Trait(Category, Methods)]
    public void Reset_WhenInvoked_ResetsTintColor()
    {
        // Arrange
        var mockBehavior = Substitute.For<IBehavior>();
        mockBehavior.Value.Returns(123);
        mockBehavior.BehaviorType.Returns(BehaviorAttribute.RedColorComponent);
        var sut = new Particle(new[] { mockBehavior });

        // Act
        sut.Update(this.frameTime);
        sut.Reset();

        // Assert
        sut.TintColor.R.Should().Be(255);
    }

    [Fact]
    [Trait(Category, Methods)]
    public void Reset_WhenInvoked_ResetsIsAlive()
    {
        // Arrange
        var mockBehavior = Substitute.For<IBehavior>();
        mockBehavior.Enabled.Returns(false);
        var sut = new Particle(new[] { mockBehavior });

        // Act
        sut.Update(new TimeSpan(0, 0, 0, 10, 0));
        sut.Reset();

        // Assert
        sut.IsAlive.Should().BeTrue();
    }
    #endregion
}
