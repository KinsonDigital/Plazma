// <copyright file="ExtensionMethodsTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable AssignNullToNotNullAttribute
namespace PlazmaTests;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
    public void ContainsNonNumberCharacters_WhenInvoked_ReturnsCorrectResult(string valueToCheck, bool expected)
    {
        // Act
        var actual = valueToCheck.ContainsNonNumberCharacters();

        // Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public void ForMarshalAsSpan_WhenInvoked_WorksCorrectlyWithoutException()
    {
        // Arrange
        var expected = new List<int> { 2, 4, 6, 8 };
        var numbers = new List<int> { 1, 2, 3, 4 };

        // Act
        var act = () => numbers.ForMarshalAsSpan((value) => value * 2);

        // Assert
        act.Should().NotThrow();
        numbers.Should().BeEquivalentTo(expected);
    }
    #endregion
}
