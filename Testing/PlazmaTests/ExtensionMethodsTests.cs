// <copyright file="ExtensionMethodsTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace PlazmaTests;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using Fakes;
using Plazma;
using Plazma.Behaviors;
using Moq;
using Xunit;
using XUnitHelpers;

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
        Random random = null;

        // Act & Assert
        AssertHelpers.ThrowsWithMessage<ArgumentNullException>(() =>
        {
            random.Next(It.IsAny<float>(), It.IsAny<float>());
        }, "The parameter must not be null. (Parameter 'random')");
    }

    [Fact]
    public void Next_WhenInvokedWithMinLessThanMax_ReturnsValueWithinMinAndMax()
    {
        // Arrange
        var random = new Random();
        var expected = true;

        // Act
        var randomNum = random.Next(50f, 100f);
        var actual = randomNum >= 50f && randomNum <= 100f;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Next_WhenInvokedWithMinMoreThanMax_ReturnsMaxValue()
    {
        // Arrange
        var random = new Random();
        var expected = 98f;

        // Act
        var actual = random.Next(124f, 98f);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Add_WhenInvoking_ReturnsCorrectResult()
    {
        // Arrange
        var pointA = new PointF(10, 20);
        var pointB = new PointF(5, 3);

        // Act
        var result = pointA.Add(pointB);

        // Assert
        Assert.Equal(new PointF(15f, 23f), result);
    }

    [Fact]
    public void Mult_WhenInvoking_ReturnsCorrectResult()
    {
        // Arrange
        var point = new PointF(10, 20);

        // Act
        var result = point.Mult(2);

        // Assert
        Assert.Equal(new PointF(20f, 40f), result);
    }

    [Fact]
    [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull", Justification = "Array is meant to be null.")]
    public void Count_WhenInvokingListVersionWithNullItems_ReturnsCorrectResult()
    {
        // Arrange
        List<Particle> particles = null;

        // Act
        var actual = particles.Count(_ => true);

        // Assert
        Assert.Equal(0, actual);
    }

    [Fact]
    public void Count_WhenInvokingListVersionWithNullPredicate_ThrowsException()
    {
        // Arrange
        var particles = new List<Particle>();

        // Act & Assert
        AssertHelpers.ThrowsWithMessage<ArgumentNullException>(() =>
        {
            particles.Count(null);
        }, "The parameter must not be null. (Parameter 'predicate')");
    }

    [Fact]
    public void Count_WhenInvokingListVersion_ReturnsCorrectResult()
    {
        // Arrange
        var mockBehavior = new Mock<IBehavior>();

        var particles = new List<Particle>();

        for (var i = 0; i < 20; i++)
        {
            particles.Add(new Particle(new[] { mockBehavior.Object }) { IsAlive = i > 10 });
        }

        // Act
        var actual = particles.Count(p => p.IsAlive);

        // Assert
        Assert.Equal(9, actual);
    }

    [Fact]
    [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull", Justification = "Array is meant to be null.")]
    public void Count_WhenInvokingArrayVersionWithNullItems_ReturnsCorrectResult()
    {
        // Arrange
        Particle[] particles = null;

        // Act
        var actual = particles.Count(_ => true);

        // Assert
        Assert.Equal(0, actual);
    }

    [Fact]
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute", Justification = "Null parameter is intended.")]
    public void Count_WhenInvokingArrayVersionWithNullPredicate_ThrowsException()
    {
        // Arrange
        var particles = Array.Empty<Particle>();

        // Act & Assert
        AssertHelpers.ThrowsWithMessage<ArgumentNullException>(() =>
        {
            particles.Count(null);
        }, "The parameter must not be null. (Parameter 'predicate')");
    }

    [Fact]
    public void Count_WhenInvokingArrayVersion_ReturnsCorrectResult()
    {
        // Arrange
        var mockBehavior = new Mock<IBehavior>();

        var tempList = new List<Particle>();
        for (var i = 0; i < 20; i++)
        {
            tempList.Add(new Particle(new[] { mockBehavior.Object }) { IsAlive = i > 10 });
        }

        var particles = tempList.ToArray();

        // Act
        var actual = particles.Count(p => p.IsAlive);

        // Assert
        Assert.Equal(9, actual);
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
        Assert.Equal(expected, actual);
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
        Assert.Equal(expected, actual);
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
        Assert.True(actual);
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
        Assert.False(actual);
    }
    #endregion
}
