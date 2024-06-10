// <copyright file="ParticleTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace PlazmaTests;

using System;
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
    #region Ctor Tests
    [Fact]
    [Trait(Category, Constructors)]
    public void Ctor_WithNullBehaviorsParam_ThrowsException()
    {
        // Arrange & Act
        var act = () =>
        {
            _ = new Particle(null);
        };

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'behaviors')");
    }
    #endregion

    #region Prop Tests
    [Fact]
    [Trait(Category, Props)]
    public void Position_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var sut = new Particle([])
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
        var sut = new Particle([])
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
        var sut = new Particle([])
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
        var sut = new Particle([])
        {
            IsAlive = true,
        };

        // Assert
        sut.IsAlive.Should().BeTrue();
    }
    #endregion

    #region Method Tests
    [Fact]
    public void AddBehavior_WhenBehaviorDoesNotAlreadyExist_AddsBehavior()
    {
        // Arrange
        var mockBehavior = Substitute.For<IBehavior>();

        var sut = default(Particle);

        // Act
        sut.AddBehavior(mockBehavior);

        // Assert
        sut.Behaviors.Should().ContainSingle("no items existed when adding the new behavior.");
    }

    [Fact]
    public void AddBehavior_WhenBehaviorAlreadyExists_DoesNotAddBehavior()
    {
        // Arrange
        var mockBehavior = Substitute.For<IBehavior>();

        var sut = new Particle(new[] { mockBehavior });

        // Act
        sut.AddBehavior(mockBehavior);

        // Assert
        sut.Behaviors.Should().ContainSingle("an item with the same behavior type already exists in the particle.");
    }

    [Fact]
    public void RemoveBehavior_WhenBehaviorDoesNotAlreadyExist_DoesNotAttemptRemoval()
    {
        // Arrange
        var mockBehavior = Substitute.For<IBehavior>();
        mockBehavior.BehaviorType.Returns(BehaviorAttribute.Angle);

        var sut = default(Particle);

        // Act
        sut.RemoveBehavior(BehaviorAttribute.Angle);

        // Assert
        sut.Behaviors.Should().BeEmpty("the behavior did not exist in the particle.");
    }

    [Fact]
    public void RemoveBehavior_WhenBehaviorDoesAlreadyExist_RemovesBehavior()
    {
        // Arrange
        var mockBehaviorA = Substitute.For<IBehavior>();
        mockBehaviorA.BehaviorType.Returns(BehaviorAttribute.Angle);

        var mockBehaviorB = Substitute.For<IBehavior>();
        mockBehaviorB.BehaviorType.Returns(BehaviorAttribute.BlueColorComponent);

        var sut = new Particle(new[] { mockBehaviorA, mockBehaviorB });

        // Act
        sut.RemoveBehavior(BehaviorAttribute.Angle);

        // Assert
        sut.Behaviors.Should().ContainSingle("the behavior did not exist in the particle.");
    }
    #endregion
}
