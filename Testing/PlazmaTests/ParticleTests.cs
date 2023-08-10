﻿// <copyright file="ParticleTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace PlazmaTests;

using System;
using System.Drawing;
using System.Globalization;
using System.Numerics;
using FluentAssertions;
using Plazma;
using Plazma.Behaviors;
using NSubstitute;
using Xunit;

/// <summary>
/// Tests the <see cref="Particle"/> class.
/// </summary>
public class ParticleTests
{
    private readonly TimeSpan frameTime;

    /// <summary>
    /// Initializes a new instance of the <see cref="ParticleTests"/> class.
    /// </summary>
    public ParticleTests() => this.frameTime = new TimeSpan(0, 0, 0, 0, 16);

    #region Prop Tests
    [Fact]
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

    [Fact]
    public void IsDead_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var sut = new Particle(Array.Empty<IBehavior>())
        {
            IsDead = false,
        };

        // Assert
        sut.IsDead.Should().BeFalse();
    }
    #endregion

    #region Method Tests
    [Fact]
    public void Update_WithFailedParse_ThrowsException()
    {
        // Arrange
        var mockBehavior = Substitute.For<IBehavior>();
        mockBehavior.Value.Returns("12z3");
        mockBehavior.Enabled.Returns(true);
        var sut = new Particle(new[] { mockBehavior });

        // Act
        var act = () => sut.Update(this.frameTime);

        // Assert
        act.Should().Throw<Exception>()
            .WithMessage($"Particle.Update Exception:\n\tParsing the behavior value '12z3' failed.\n\tValue must be a number.");
    }

    [Theory]
    [InlineData(ParticleAttribute.Color, "clr:300,0,0,255", "Particle.Update Exception: Error #1500. Invalid Syntax. Alpha color component out of range.")]
    [InlineData(ParticleAttribute.Color, "clr:-300,0,0,255", "Particle.Update Exception: Error #1500. Invalid Syntax. Alpha color component out of range.")] // Do negative values
    [InlineData(ParticleAttribute.Color, "clr:255,301,0,255", "Particle.Update Exception: Error #1600. Invalid Syntax. Red color component out of range.")]
    [InlineData(ParticleAttribute.Color, "clr:255,0,302,255", "Particle.Update Exception: Error #1700. Invalid Syntax. Green color component out of range.")]
    [InlineData(ParticleAttribute.Color, "clr:255,0,0,303", "Particle.Update Exception: Error #1800. Invalid Syntax. Blue color component out of range.")]
    [InlineData(ParticleAttribute.Color, "clr:1z0,0,0,255", "Particle.Update Exception: Error #1200. Invalid Syntax. Alpha color component must only contain numbers.")]
    [InlineData(ParticleAttribute.Color, "clr:255,1z1,0,255", "Particle.Update Exception: Error #1300. Invalid Syntax. Alpha color component must only contain numbers.")]
    [InlineData(ParticleAttribute.Color, "clr:255,0,1z2,255", "Particle.Update Exception: Error #1400. Invalid Syntax. Alpha color component must only contain numbers.")]
    [InlineData(ParticleAttribute.Color, "clr:255,0,0,1z3", "Particle.Update Exception: Error #1500. Invalid Syntax. Alpha color component must only contain numbers.")]
    [InlineData(ParticleAttribute.Color, "clr:,0,0,255",        "Particle.Update Exception: Error #1100. Invalid Syntax. Missing color component.\n\tSyntax is as follows: clr:<alpha>,<red>,<green>,<blue>")]
    [InlineData(ParticleAttribute.Color, "clr:255,,0,255",      "Particle.Update Exception: Error #1100. Invalid Syntax. Missing color component.\n\tSyntax is as follows: clr:<alpha>,<red>,<green>,<blue>")]
    [InlineData(ParticleAttribute.Color, "clr:255,0,,255",      "Particle.Update Exception: Error #1100. Invalid Syntax. Missing color component.\n\tSyntax is as follows: clr:<alpha>,<red>,<green>,<blue>")]
    [InlineData(ParticleAttribute.Color, "clr:255,0,0,",        "Particle.Update Exception: Error #1100. Invalid Syntax. Missing color component.\n\tSyntax is as follows: clr:<alpha>,<red>,<green>,<blue>")]
    [InlineData(ParticleAttribute.Color, "clr255,0,0,0",        "Particle.Update Exception: Error #1000. Invalid Syntax. Missing ':'.\n\tSyntax is as follows: clr:<alpha>,<red>,<green>,<blue>")]
    [InlineData(ParticleAttribute.Color, ":255,0,0,0",          "Particle.Update Exception: Error #900. Invalid Syntax. Missing 'clr' prefix.\n\tSyntax is as follows: clr:<alpha>,<red>,<green>,<blue>")]
    [InlineData(ParticleAttribute.Angle, "invalid-num",         "Particle.Update Exception:\n\tParsing the behavior value 'invalid-num' failed.\n\tValue must be a number.")]
    public void Update_WhenUsingWrongRandomColorValue_ThrowsException(ParticleAttribute attribute, string value, string expectedMessage)
    {
        // Arrange
        var mockBehavior = Substitute.For<IBehavior>();
        mockBehavior.Value.Returns(value);
        mockBehavior.Enabled.Returns(true);
        mockBehavior.ApplyToAttribute.Returns(attribute);

        var sut = new Particle(new[] { mockBehavior });

        // Act
        var act = () => sut.Update(this.frameTime);

        // Assert
        act.Should().Throw<Exception>()
            .WithMessage(expectedMessage);
    }

    [Fact]
    public void Update_WhenUsingRandomColorBehavior_SetsTintColor()
    {
        // Arrange
        var expected = Color.FromArgb(255, 0, 0, 255);

        var mockBehavior = Substitute.For<IBehavior>();
        mockBehavior.Value.Returns("clr:255,0,0,255");
        mockBehavior.Enabled.Returns(true);
        mockBehavior.ApplyToAttribute.Returns(ParticleAttribute.Color);

        var sut = new Particle(new[] { mockBehavior });

        // Act
        sut.Update(this.frameTime);

        // Assert
        sut.TintColor.Should().Be(expected);
    }

    [Fact]
    public void Update_WithDisabledBehavior_BehaviorShouldNotUpdate()
    {
        // Arrange
        var mockBehavior = Substitute.For<IBehavior>();
        mockBehavior.Value.Returns("123");
        mockBehavior.Enabled.Returns(false);
        var sut = new Particle(new[] { mockBehavior });

        // Act
        sut.Update(this.frameTime);

        // Assert
        mockBehavior.DidNotReceive().Update(Arg.Any<TimeSpan>());
    }

    [Fact]
    public void Update_WithEnabledBehavior_BehaviorShouldUpdate()
    {
        // Arrange
        var mockBehavior = Substitute.For<IBehavior>();
        mockBehavior.Enabled.Returns(true);
        mockBehavior.Value.Returns("123");

        var sut = new Particle(new[] { mockBehavior });

        // Act
        sut.Update(this.frameTime);

        // Assert
        mockBehavior.Received(1).Update(Arg.Any<TimeSpan>());
    }

    [Fact]
    public void Update_WhenApplyingToXAttribute_UpdatesPositionX()
    {
        // Arrange
        var mockBehavior = Substitute.For<IBehavior>();
        mockBehavior.Enabled.Returns(true);
        mockBehavior.Value.Returns("123");
        mockBehavior.ApplyToAttribute.Returns(ParticleAttribute.X);

        var sut = new Particle(new[] { mockBehavior });

        // Act
        sut.Update(this.frameTime);

        // Assert
        sut.Position.X.ToString(CultureInfo.InvariantCulture).Should().Be("123");
    }

    [Fact]
    public void Update_WhenApplyingToYAttribute_UpdatesPositionY()
    {
        // Arrange
        var mockBehavior = Substitute.For<IBehavior>();
        mockBehavior.Enabled.Returns(true);
        mockBehavior.Value.Returns("123");
        mockBehavior.ApplyToAttribute.Returns(ParticleAttribute.Y);

        var sut = new Particle(new[] { mockBehavior });

        // Act
        sut.Update(this.frameTime);

        // Assert
        sut.Position.Y.ToString(CultureInfo.InvariantCulture).Should().Be("123");
    }

    [Fact]
    public void Update_WhenApplyingToAngleAttribute_UpdatesAngle()
    {
        // Arrange
        var mockBehavior = Substitute.For<IBehavior>();
        mockBehavior.Enabled.Returns(true);
        mockBehavior.Value.Returns("123");
        mockBehavior.ApplyToAttribute.Returns(ParticleAttribute.Angle);

        var sut = new Particle(new[] { mockBehavior });

        // Act
        sut.Update(this.frameTime);

        // Assert
        sut.Angle.ToString(CultureInfo.InvariantCulture).Should().Be("123");
    }

    [Fact]
    public void Update_WhenApplyingToSizeAttribute_UpdatesSize()
    {
        // Arrange
        var mockBehavior = Substitute.For<IBehavior>();
        mockBehavior.Enabled.Returns(true);
        mockBehavior.Value.Returns("123");
        mockBehavior.ApplyToAttribute.Returns(ParticleAttribute.Size);

        var sut = new Particle(new[] { mockBehavior });

        // Act
        sut.Update(this.frameTime);

        // Assert
        sut.Size.ToString(CultureInfo.InvariantCulture).Should().Be("123");
    }

    [Fact]
    public void Update_WhenApplyingToRedColorComponentAttribute_UpdatesRedColorComponent()
    {
        // Arrange
        var mockBehavior = Substitute.For<IBehavior>();
        mockBehavior.Enabled.Returns(true);
        mockBehavior.Value.Returns("123");
        mockBehavior.ApplyToAttribute.Returns(ParticleAttribute.RedColorComponent);

        var sut = new Particle(new[] { mockBehavior });

        // Act
        sut.Update(this.frameTime);

        // Assert
        sut.TintColor.R.ToString(CultureInfo.InvariantCulture).Should().Be("123");
    }

    [Fact]
    public void Update_WhenApplyingToGreenColorComponentAttribute_UpdatesGreenColorComponent()
    {
        // Arrange
        var mockBehavior = Substitute.For<IBehavior>();
        mockBehavior.Enabled.Returns(true);
        mockBehavior.Value.Returns("123");
        mockBehavior.ApplyToAttribute.Returns(ParticleAttribute.GreenColorComponent);

        var sut = new Particle(new[] { mockBehavior });

        // Act
        sut.Update(this.frameTime);

        // Assert
        sut.TintColor.G.ToString(CultureInfo.InvariantCulture).Should().Be("123");
    }

    [Fact]
    public void Update_WhenApplyingToBlueColorComponentAttribute_UpdatesBlueColorComponent()
    {
        // Arrange
        var mockBehavior = Substitute.For<IBehavior>();
        mockBehavior.Enabled.Returns(true);
        mockBehavior.Value.Returns("123");
        mockBehavior.ApplyToAttribute.Returns(ParticleAttribute.BlueColorComponent);

        var sut = new Particle(new[] { mockBehavior });

        // Act
        sut.Update(this.frameTime);

        // Assert
        sut.TintColor.B.ToString(CultureInfo.InvariantCulture).Should().Be("123");
    }

    [Fact]
    public void Update_WhenApplyingToAlphaColorComponentAttribute_UpdatesAlphaColorComponent()
    {
        // Arrange
        var mockBehavior = Substitute.For<IBehavior>();
        mockBehavior.Enabled.Returns(true);
        mockBehavior.Value.Returns("123");
        mockBehavior.ApplyToAttribute.Returns(ParticleAttribute.AlphaColorComponent);

        var sut = new Particle(new[] { mockBehavior });

        // Act
        sut.Update(this.frameTime);

        // Assert
        sut.TintColor.A.ToString(CultureInfo.InvariantCulture).Should().Be("123");
    }

    [Fact]
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
    public void Reset_WhenInvoked_ResetsAngle()
    {
        // Arrange
        var mockBehavior = Substitute.For<IBehavior>();
        mockBehavior.Value.Returns("123");
        mockBehavior.ApplyToAttribute.Returns(ParticleAttribute.Angle);
        var sut = new Particle(new[] { mockBehavior });

        // Act
        sut.Update(this.frameTime);
        sut.Reset();

        // Assert
        sut.Angle.Should().Be(0);
    }

    [Fact]
    public void Reset_WhenInvoked_ResetsTintColor()
    {
        // Arrange
        var mockBehavior = Substitute.For<IBehavior>();
        mockBehavior.Value.Returns("123");
        mockBehavior.ApplyToAttribute.Returns(ParticleAttribute.RedColorComponent);
        var sut = new Particle(new[] { mockBehavior });

        // Act
        sut.Update(this.frameTime);
        sut.Reset();

        // Assert
        sut.TintColor.R.Should().Be(255);
    }

    [Fact]
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

    [Fact]
    public void Equals_WithDifferentObjects_ReturnsFalse()
    {
        // Arrange
        var sut = new Particle(null);
        var obj = new object();

        // Act
        var actual = sut.Equals(obj);

        // Assert
        actual.Should().BeFalse();
    }

    [Fact]
    public void Equals_WithNonEqualObjects_ReturnsFalse()
    {
        // Arrange
        var sutA = new Particle(null)
        {
            Size = 123f,
        };
        var sutB = new Particle(null);

        // Act
        var actual = sutA.Equals(sutB);

        // Assert
        actual.Should().BeFalse();
    }
    #endregion
}
