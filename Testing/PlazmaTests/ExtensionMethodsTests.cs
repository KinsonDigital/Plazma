// <copyright file="ExtensionMethodsTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable AssignNullToNotNullAttribute
namespace PlazmaTests;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Numerics;
using Fakes;
using FluentAssertions;
using NSubstitute;
using Plazma;
using Plazma.Behaviors;
using Xunit;

/// <summary>
/// Tests the <see cref="ExtensionMethods"/> class.
/// </summary>
public class ExtensionMethodsTests
{
    #region Method Tests
    [Fact]
    public void Next_WhenInvokedWithNullRandomParam_ThrowsException()
    {
        // Arrange
        Random? random = null;

        // Act
        var act = () => random.Next(0f, 0f);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'random')");
    }

    [Fact]
    public void Next_WhenInvokedWithMinLessThanMax_ReturnsValueWithinMinAndMax()
    {
        // Arrange
        var random = new Random();
        const bool expected = true;

        // Act
        var randomNum = random.Next(50f, 100f);
        var actual = randomNum is >= 50f and <= 100f;

        // Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public void Next_WhenInvokedWithMinMoreThanMax_ReturnsMaxValue()
    {
        // Arrange
        var random = new Random();
        const float expected = 98f;

        // Act
        var actual = random.Next(124f, 98f);

        // Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public void Add_WhenInvoking_ReturnsCorrectResult()
    {
        // Arrange
        var pointA = new Vector2(10, 20);
        var pointB = new Vector2(5, 3);

        // Act
        var result = pointA.Add(pointB);

        // Assert
        result.Should().Be(new Vector2(15f, 23f));
    }

    [Fact]
    public void Mult_WhenInvoking_ReturnsCorrectResult()
    {
        // Arrange
        var point = new Vector2(10, 20);

        // Act
        var result = point.Mult(2);

        // Assert
        result.Should().Be(new Vector2(20f, 40f));
    }

    [Fact]
    [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull", Justification = "Array is meant to be null.")]
    public void Count_WhenInvokingListVersionWithNullItems_ReturnsCorrectResult()
    {
        // Arrange
        List<Particle>? particles = null;

        // Act
        var actual = particles.Count(_ => true);

        // Assert
        actual.Should().Be(0);
    }

    [Fact]
    public void Count_WhenInvokingListVersionWithNullPredicate_ThrowsException()
    {
        // Arrange
        var particles = new List<Particle>();

        // Act
        var act = () => particles.Count(null);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'predicate')");
    }

    [Fact]
    public void Count_WhenInvokingListVersion_ReturnsCorrectResult()
    {
        // Arrange
        var mockBehavior = Substitute.For<IBehavior>();

        var particles = new List<Particle>();

        for (var i = 0; i < 20; i++)
        {
            particles.Add(new Particle(new[] { mockBehavior }) { IsAlive = i > 10 });
        }

        // Act
        var actual = particles.Count(p => p.IsAlive);

        // Assert
        actual.Should().Be(9);
    }

    [Fact]
    [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull", Justification = "Array is meant to be null.")]
    public void Count_WhenInvokingArrayVersionWithNullItems_ReturnsCorrectResult()
    {
        // Arrange
        Particle[]? particles = null;

        // Act
        var actual = particles.Count(_ => true);

        // Assert
        actual.Should().Be(0);
    }

    [Fact]
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute", Justification = "Null parameter is intended.")]
    public void Count_WhenInvokingArrayVersionWithNullPredicate_ThrowsException()
    {
        // Arrange
        var particles = Array.Empty<Particle>();

        // Act
        var act = () => particles.Count(null);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'predicate')");
    }

    [Fact]
    public void Count_WhenInvokingArrayVersion_ReturnsCorrectResult()
    {
        // Arrange
        var mockBehavior = Substitute.For<IBehavior>();

        var tempList = new List<Particle>();
        for (var i = 0; i < 20; i++)
        {
            tempList.Add(new Particle(new[] { mockBehavior }) { IsAlive = i > 10 });
        }

        var particles = tempList.ToArray();

        // Act
        var actual = particles.Count(p => p.IsAlive);

        // Assert
        actual.Should().Be(9);
    }

    [Theory]
    [InlineData("123", false)]
    [InlineData("-123", false)]
    [InlineData("12T3", true)]
    [InlineData(null, false)]
    public void ContainsNonNumberCharacters_WhenInvoked_ReturnsCorrectResult(string valueToCheck, bool expected)
    {
        // Act
        var actual = valueToCheck.ContainsNonNumberCharacters();

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData(null, null, true)]
    [InlineData(null, new[] { "item" }, false)]
    [InlineData(new[] { "item" }, null, false)]
    [InlineData(new[] { "item" }, new[] { "item", "item" }, false)]
    [InlineData(new[] { "item" }, new[] { "item" }, true)]
    [InlineData(new[] { "item" }, new[] { "other-item" }, false)]
    public void ItemsAreEqual_WhenInvoked_ReturnsCorrectResult(string[] listA, string[] listB, bool expected)
    {
        // Act
        var actual = listA.ItemsAreEqual(listB);

        // Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public void ItemsAreEqual_WhenInvokedEqualObjects_ReturnsTrue()
    {
        // Arrange
        var itemsA = new[]
        {
            new TestItem { Number = 10 },
        };

        var itemsB = new[]
        {
            new TestItem { Number = 10 },
        };

        // Act
        var actual = itemsA.ItemsAreEqual(itemsB);

        // Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public void ItemsAreEqual_WhenInvokedNonEqualObjects_ReturnsFalse()
    {
        // Arrange
        var itemsA = new[]
        {
            new TestItem { Number = 10 },
        };

        var itemsB = new[]
        {
            new TestItem { Number = 20 },
        };

        // Act
        var actual = itemsA.ItemsAreEqual(itemsB);

        // Assert
        actual.Should().BeFalse();
    }
    #endregion
}
