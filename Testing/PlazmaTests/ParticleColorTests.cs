// <copyright file="ParticleColorTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace PlazmaTests;

using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using FluentAssertions.Execution;
using Plazma;
using Xunit;

/// <summary>
/// Tests the <see cref="ParticleColor"/> class.
/// </summary>
public class ParticleColorTests
{
    #region Prop Tests
    [Fact]
    public void ColorProps_WhenGettingValues_ReturnsCorrectValue()
    {
        // Arrange
        var propsToTest = GetColorPropNames();

        foreach (var prop in propsToTest)
        {
            var expected = Color.FromName(prop);
            var actual = GetColorPropValue(prop);

            actual.A.Should().Be(expected.A, $"Clr Name: {prop} | Expected: {expected.A} | Actual: {actual.A}");
            actual.R.Should().Be(expected.R, $"Clr Name: {prop} | Expected: {expected.R} | Actual: {actual.R}");
            actual.G.Should().Be(expected.G, $"Clr Name: {prop} | Expected: {expected.G} | Actual: {actual.G}");
            actual.B.Should().Be(expected.B, $"Clr Name: {prop} | Expected: {expected.B} | Actual: {actual.B}");
        }
    }
    #endregion

    #region Method Tests
    [Fact]
    public void GetBrightness_WhenInvoked_ReturnsCorrectResult()
    {
        // Arrange
        var color = new ParticleColor(255, 24, 98, 118);

        // Act
        var actual = color.GetBrightness();

        // Assert
        actual.Should().Be(27.843138f);
    }

    [Theory]
    [InlineData(118, 98, 118, 300f)]
    [InlineData(10, 30, 20, 150f)]
    [InlineData(10, 10, 10, 0f)]
    [InlineData(24, 98, 118, 192.76596f)]
    public void GetHue_WhenInvoked_ReturnsCorrectValue(byte red, byte green, byte blue, float result)
    {
        // Arrange
        var color = new ParticleColor(255, red, green, blue);

        // Act
        var actual = color.GetHue();

        // Assert
        actual.Should().Be(result);
    }

    [Fact]
    public void GetSaturation_WhenInvoked_ReturnsCorrectResult()
    {
        // Arrange
        var color = new ParticleColor(255, 24, 98, 118);

        // Act
        var actual = color.GetSaturation();

        // Assert
        Math.Round(actual, 5).Should().Be(66.19718);
    }

    [Theory]
    [InlineData(255, 255, 255, 255, true)]
    [InlineData(11, 22, 33, 44, false)]
    public void EqualOperator_WithEqualColors_ReturnsTrue(byte alpha, byte red, byte green, byte blue, bool expectedResult)
    {
        // Arrange
        var colorA = new ParticleColor(255, 255, 255, 255);
        var colorB = new ParticleColor(alpha, red, green, blue);

        // Act
        var actual = colorA == colorB;

        // Assert
        actual.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(255, 255, 255, 255, false)]
    [InlineData(11, 22, 33, 44, true)]
    public void NotEqualOperator_WithNotEqualColors_ReturnsTrue(byte alpha, byte red, byte green, byte blue, bool expectedResult)
    {
        // Arrange
        var colorA = new ParticleColor(255, 255, 255, 255);
        var colorB = new ParticleColor(alpha, red, green, blue);

        // Act
        var actual = colorA != colorB;

        // Assert
        actual.Should().Be(expectedResult);
    }

    [Fact]
    public void Equals_WithTwoDifferentObjectTypes_ReturnsFalse()
    {
        // Arrange
        var color = new ParticleColor(11, 22, 33, 44);
        var otherObj = new object();

        // Act
        var actual = color.Equals(otherObj);

        // Assert
        actual.Should().BeFalse();
    }

    [Fact]
    public void Equals_WithTwoNonMatchingObject_ReturnsFalse()
    {
        // Arrange
        var clrA = new ParticleColor(11, 22, 33, 44);
        var clrB = new ParticleColor(11, 22, 33, 99);

        // Act
        var actual = clrA.Equals(clrB);

        // Assert
        actual.Should().BeFalse();
    }

    [Fact]
    public void Equals_WithTwoMatchingObject_ReturnsTrue()
    {
        // Arrange
        var clrA = new ParticleColor(11, 22, 33, 44);
        var clrB = new ParticleColor(11, 22, 33, 44);

        // Act
        var actual = clrA.Equals(clrB);

        // Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public void GetHashCode_WhenInvoked_ReturnsCorrectResult()
    {
        // Arrange
        var colorA = new ParticleColor(11, 22, 33, 44);
        var colorB = new ParticleColor(11, 22, 33, 44);

        // Act
        var actual = colorA.GetHashCode() == colorB.GetHashCode();

        // Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public void ToString_WhenInvoked_ReturnsCorrectResult()
    {
        // Arrange
        var color = new ParticleColor(10, 20, 30, 40);

        // Act
        var actual = color.ToString();

        // Assert
        actual.Should().Be("A = 10, R = 20, G = 30, B = 40");
    }
    #endregion

    /// <summary>
    /// Returns all of the names of the <see cref="ParticleColor"/> color property names.
    /// </summary>
    /// <returns>A list of properties of the type <see cref="ParticleColor"/>.</returns>
    private static string[] GetColorPropNames() => typeof(ParticleColor).GetProperties(BindingFlags.Public | BindingFlags.Static)
        .Where(p => p.PropertyType == typeof(ParticleColor)).Select(p => p.Name).ToArray();

    /// <summary>
    /// Gets the <see cref="ParticleColor"/> property value that matches the given <paramref name="name"/>.
    /// </summary>
    /// <param name="name">The name of the property.</param>
    /// <returns>
    ///     The value of a property that matches the given <paramref name="name"/>
    ///     for the type <see cref="ParticleColor"/>.
    /// </returns>
    private static ParticleColor GetColorPropValue(string name)
    {
        var props = typeof(ParticleColor)
            .GetProperties(BindingFlags.Public | BindingFlags.Static);

        var foundProp = Array.Find(props, p => p.PropertyType == typeof(ParticleColor) && p.Name == name);

        if (foundProp is null)
        {
            throw new AssertionFailedException($"Could not find the property '{name}'.");
        }

        if (foundProp.GetValue(null, null) is not ParticleColor value)
        {
            throw new AssertionFailedException("The value of the property is null.");
        }

        return value;
    }
}
