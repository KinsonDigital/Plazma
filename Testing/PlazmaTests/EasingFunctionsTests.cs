// <copyright file="EasingFunctionsTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace PlazmaTests;

using FluentAssertions;
using Plazma;
using Xunit;

/// <summary>
/// Tests the <see cref="EasingFunctions"/> class.
/// </summary>
public class EasingFunctionsTests
{
    #region Method Tests
    [Theory]
    [InlineData(0.3636f, 2.0f, 3.0f, 3.0f, 2.33326667f)]
    [InlineData(0.4f, 2.0f, 3.0f, 1.0f, 4.7299999999999995f)]
    [InlineData(0.8f, 2.0f, 3.0f, 1.0f, 4.82f)]
    [InlineData(1.0f, 2.0f, 3.0f, 1.0f, 5.090511363636364f)]
    public void EaseOutBounce_WhenInvoked_ReturnsCorrectValue(float t, float b, float c, float d, float expected)
    {
        // Act
        var actual = EasingFunctions.EaseOutBounce(t, b, c, d);

        // Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public void EaseInQuad_WhenInvoked_ReturnsCorrectResult()
    {
        // Arrange
        var expected = 30.444444444444443f;

        // Act
        var actual = EasingFunctions.EaseInQuad(16f, 2.0f, 4.0f, 6.0f);

        // Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public void EaseInQuad_WhenUsingNegativeChange_ResultDecreases()
    {
        // Arrange
        var firstValue = EasingFunctions.EaseInQuad(16f, 2.0f, -1.0f, 6.0f);

        // Act
        var secondValue = EasingFunctions.EaseInQuad(32f, 2.0f, -1.0f, 6.0f);

        // Assert
        secondValue.Should().BeLessThan(firstValue);
    }
    #endregion
}
