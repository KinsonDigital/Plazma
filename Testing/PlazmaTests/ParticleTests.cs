// <copyright file="ParticleTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDParticleEngineTests;

using System;
using System.Drawing;
using System.Globalization;
using Plazma;
using Plazma.Behaviors;
using KDParticleEngineTests.XUnitHelpers;
using Moq;
using Xunit;

/// <summary>
/// Tests the <see cref="Particle"/> class.
/// </summary>
public class ParticleTests
{
    private readonly TimeSpan frameTime;
    private readonly Mock<IBehavior> mockBehavior;

    /// <summary>
    /// Initializes a new instance of the <see cref="ParticleTests"/> class.
    /// </summary>
    public ParticleTests()
    {
        this.frameTime = new TimeSpan(0, 0, 0, 0, 16);
        this.mockBehavior = new Mock<IBehavior>();
        this.mockBehavior.SetupGet(p => p.Enabled).Returns(true);
    }

    #region Prop Tests
    [Fact]
    public void Position_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var particle = new Particle(Array.Empty<IBehavior>())
        {
            Position = new PointF(11, 22),
        };

        // Act
        var actual = particle.Position;

        // Assert
        Assert.Equal(new PointF(11, 22), actual);
    }

    [Fact]
    public void Angle_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var particle = new Particle(Array.Empty<IBehavior>())
        {
            Angle = 1234f,
        };

        // Act
        var actual = particle.Angle;

        // Assert
        Assert.Equal(1234f, actual);
    }

    [Fact]
    public void TintColor_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var particle = new Particle(Array.Empty<IBehavior>())
        {
            TintColor = ParticleColor.FromArgb(11, 22, 33, 44),
        };

        // Act
        var actual = particle.TintColor;

        // Assert
        Assert.Equal(ParticleColor.FromArgb(11, 22, 33, 44), actual);
    }

    [Fact]
    public void Size_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var particle = new Particle(Array.Empty<IBehavior>())
        {
            Size = 1019f,
        };

        // Act
        var actual = particle.Size;

        // Assert
        Assert.Equal(1019f, actual);
    }

    [Fact]
    public void IsAlive_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var particle = new Particle(Array.Empty<IBehavior>())
        {
            IsAlive = true,
        };

        // Assert
        Assert.True(particle.IsAlive);
    }

    [Fact]
    public void IsDead_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var particle = new Particle(Array.Empty<IBehavior>())
        {
            IsDead = false,
        };

        // Assert
        Assert.False(particle.IsDead);
    }
    #endregion

    #region Method Tests
    [Fact]
    public void Update_WithFailedParse_ThrowsException()
    {
        // Arrange
        this.mockBehavior.SetupGet(p => p.Value).Returns("12z3");
        this.mockBehavior.SetupGet(p => p.Enabled).Returns(true);
        var particle = new Particle(new[] { this.mockBehavior.Object });

        // Act & Assert
        AssertHelpers.ThrowsWithMessage<Exception>(() =>
        {
            particle.Update(this.frameTime);
        }, $"Particle.Update Exception:\n\tParsing the behavior value '12z3' failed.\n\tValue must be a number.");
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
        this.mockBehavior.SetupGet(p => p.Value).Returns(value);
        this.mockBehavior.SetupGet(p => p.Enabled).Returns(true);
        this.mockBehavior.SetupGet(p => p.ApplyToAttribute).Returns(attribute);

        var particle = new Particle(new[] { this.mockBehavior.Object });

        // Act & Assert
        AssertHelpers.ThrowsWithMessage<Exception>(() =>
        {
            particle.Update(this.frameTime);
        }, expectedMessage);
    }

    [Fact]
    public void Update_WhenUsingRandomColorBehavior_SetsTintColor()
    {
        // Arrange
        var expected = new ParticleColor(255, 0, 0, 255);

        this.mockBehavior.SetupGet(p => p.Value).Returns("clr:255,0,0,255");
        this.mockBehavior.SetupGet(p => p.Enabled).Returns(true);
        this.mockBehavior.SetupGet(p => p.ApplyToAttribute).Returns(ParticleAttribute.Color);

        var particle = new Particle(new[] { this.mockBehavior.Object });

        // Act
        particle.Update(this.frameTime);

        // Assert
        Assert.Equal(expected, particle.TintColor);
    }

    [Fact]
    public void Update_WithDisabledBehavior_BehaviorShouldNotUpdate()
    {
        // Arrange
        this.mockBehavior.SetupGet(p => p.Value).Returns("123");
        this.mockBehavior.SetupGet(p => p.Enabled).Returns(false);
        var particle = new Particle(new[] { this.mockBehavior.Object });

        // Act
        particle.Update(this.frameTime);

        // Assert
        this.mockBehavior.Verify(m => m.Update(It.IsAny<TimeSpan>()), Times.Never);
    }

    [Fact]
    public void Update_WithEnabledBehavior_BehaviorShouldUpdate()
    {
        // Arrange
        this.mockBehavior.SetupGet(p => p.Value).Returns("123");

        var particle = new Particle(new[] { this.mockBehavior.Object });

        // Act
        particle.Update(this.frameTime);

        // Assert
        this.mockBehavior.Verify(m => m.Update(It.IsAny<TimeSpan>()), Times.Once());
    }

    [Fact]
    public void Update_WhenApplyingToXAttribute_UpdatesPositionX()
    {
        // Arrange
        this.mockBehavior.SetupGet(p => p.Value).Returns("123");
        this.mockBehavior.SetupGet(p => p.ApplyToAttribute).Returns(ParticleAttribute.X);

        var particle = new Particle(new[] { this.mockBehavior.Object });

        // Act
        particle.Update(this.frameTime);

        // Assert
        Assert.Equal("123", particle.Position.X.ToString(CultureInfo.InvariantCulture));
    }

    [Fact]
    public void Update_WhenApplyingToYAttribute_UpdatesPositionY()
    {
        // Arrange
        this.mockBehavior.SetupGet(p => p.Value).Returns("123");
        this.mockBehavior.SetupGet(p => p.ApplyToAttribute).Returns(ParticleAttribute.Y);

        var particle = new Particle(new[] { this.mockBehavior.Object });

        // Act
        particle.Update(this.frameTime);

        // Assert
        Assert.Equal("123", particle.Position.Y.ToString(CultureInfo.InvariantCulture));
    }

    [Fact]
    public void Update_WhenApplyingToAngleAttribute_UpdatesAngle()
    {
        // Arrange
        this.mockBehavior.SetupGet(p => p.Value).Returns("123");
        this.mockBehavior.SetupGet(p => p.ApplyToAttribute).Returns(ParticleAttribute.Angle);

        var particle = new Particle(new[] { this.mockBehavior.Object });

        // Act
        particle.Update(this.frameTime);

        // Assert
        Assert.Equal("123", particle.Angle.ToString(CultureInfo.InvariantCulture));
    }

    [Fact]
    public void Update_WhenApplyingToSizeAttribute_UpdatesSize()
    {
        // Arrange
        this.mockBehavior.SetupGet(p => p.Value).Returns("123");
        this.mockBehavior.SetupGet(p => p.ApplyToAttribute).Returns(ParticleAttribute.Size);

        var particle = new Particle(new[] { this.mockBehavior.Object });

        // Act
        particle.Update(this.frameTime);

        // Assert
        Assert.Equal("123", particle.Size.ToString(CultureInfo.InvariantCulture));
    }

    [Fact]
    public void Update_WhenApplyingToRedColorComponentAttribute_UpdatesRedColorComponent()
    {
        // Arrange
        this.mockBehavior.SetupGet(p => p.Value).Returns("123");
        this.mockBehavior.SetupGet(p => p.ApplyToAttribute).Returns(ParticleAttribute.RedColorComponent);

        var particle = new Particle(new[] { this.mockBehavior.Object });

        // Act
        particle.Update(this.frameTime);

        // Assert
        Assert.Equal("123", particle.TintColor.R.ToString(CultureInfo.InvariantCulture));
    }

    [Fact]
    public void Update_WhenApplyingToGreenColorComponentAttribute_UpdatesGreenColorComponent()
    {
        // Arrange
        this.mockBehavior.SetupGet(p => p.Value).Returns("123");
        this.mockBehavior.SetupGet(p => p.ApplyToAttribute).Returns(ParticleAttribute.GreenColorComponent);

        var particle = new Particle(new[] { this.mockBehavior.Object });

        // Act
        particle.Update(this.frameTime);

        // Assert
        Assert.Equal("123", particle.TintColor.G.ToString(CultureInfo.InvariantCulture));
    }

    [Fact]
    public void Update_WhenApplyingToBlueColorComponentAttribute_UpdatesBlueColorComponent()
    {
        // Arrange
        this.mockBehavior.SetupGet(p => p.Value).Returns("123");
        this.mockBehavior.SetupGet(p => p.ApplyToAttribute).Returns(ParticleAttribute.BlueColorComponent);

        var particle = new Particle(new[] { this.mockBehavior.Object });

        // Act
        particle.Update(this.frameTime);

        // Assert
        Assert.Equal("123", particle.TintColor.B.ToString(CultureInfo.InvariantCulture));
    }

    [Fact]
    public void Update_WhenApplyingToAlphaColorComponentAttribute_UpdatesAlphaColorComponent()
    {
        // Arrange
        this.mockBehavior.SetupGet(p => p.Value).Returns("123");
        this.mockBehavior.SetupGet(p => p.ApplyToAttribute).Returns(ParticleAttribute.AlphaColorComponent);

        var particle = new Particle(new[] { this.mockBehavior.Object });

        // Act
        particle.Update(this.frameTime);

        // Assert
        Assert.Equal("123", particle.TintColor.A.ToString(CultureInfo.InvariantCulture));
    }

    [Fact]
    public void Reset_WhenInvoked_ResetsAllBehaviors()
    {
        // Arrange
        var particle = new Particle(new[] { this.mockBehavior.Object });

        // Act
        particle.Reset();

        // Assert
        this.mockBehavior.Verify(m => m.Reset(), Times.Once());
    }

    [Fact]
    public void Reset_WhenInvoked_ResetsAngle()
    {
        // Arrange
        this.mockBehavior.SetupGet(p => p.Value).Returns("123");
        this.mockBehavior.SetupGet(p => p.ApplyToAttribute).Returns(ParticleAttribute.Angle);
        var particle = new Particle(new[] { this.mockBehavior.Object });

        // Act
        particle.Update(this.frameTime);
        particle.Reset();

        // Assert
        Assert.Equal(0, particle.Angle);
    }

    [Fact]
    public void Reset_WhenInvoked_ResetsTintColor()
    {
        // Arrange
        this.mockBehavior.SetupGet(p => p.Value).Returns("123");
        this.mockBehavior.SetupGet(p => p.ApplyToAttribute).Returns(ParticleAttribute.RedColorComponent);
        var particle = new Particle(new[] { this.mockBehavior.Object });

        // Act
        particle.Update(this.frameTime);
        particle.Reset();

        // Assert
        Assert.Equal(255, particle.TintColor.R);
    }

    [Fact]
    public void Reset_WhenInvoked_ResetsIsAlive()
    {
        // Arrange
        this.mockBehavior.SetupGet(p => p.Enabled).Returns(false);
        var particle = new Particle(new[] { this.mockBehavior.Object });

        // Act
        particle.Update(new TimeSpan(0, 0, 0, 10, 0));
        particle.Reset();

        // Assert
        Assert.True(particle.IsAlive);
    }

    [Fact]
    public void Equals_WithDifferentObjects_ReturnsFalse()
    {
        // Arrange
        var particle = new Particle(It.IsAny<IBehavior[]>());
        var obj = new object();

        // Act
        var actual = particle.Equals(obj);

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void Equals_WithNonEqualObjects_ReturnsFalse()
    {
        // Arrange
        var particleA = new Particle(It.IsAny<IBehavior[]>())
        {
            Size = 123f,
        };
        var particleB = new Particle(It.IsAny<IBehavior[]>());

        // Act
        var actual = particleA.Equals(particleB);

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void Equals_WithEqualObjects_ReturnsTrue()
    {
        // Arrange
        var particleA = new Particle(It.IsAny<IBehavior[]>());
        var particleB = new Particle(It.IsAny<IBehavior[]>());

        // Act
        var actual = particleA.Equals(particleB);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void GetHashCode_WhenInvoked_ReturnsCorrectResult()
    {
        // Arrange
        var behaviors = new IBehavior[]
        {
            this.mockBehavior.Object,
        };
        var particleA = new Particle(behaviors);
        var particleB = new Particle(behaviors);

        // Act
        var actual = particleA.GetHashCode() == particleB.GetHashCode();

        // Assert
        Assert.True(actual);
    }
    #endregion
}