// <copyright file="TrueRandomizerServiceTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDParticleEngineTests.Services;

using Plazma.Services;
using KDParticleEngineTests.XUnitHelpers;
using Xunit;

/// <summary>
/// Tests the <see cref="TrueRandomizerService"/> class.
/// </summary>
public class TrueRandomizerServiceTests
{
    #region Method Tests
    [Theory]
    [InlineData(1, 2)]
    [InlineData(1, 4)]
    [InlineData(1, 8)]
    [InlineData(1, 16)]
    public void GetValue_WhenInvokingWithIntValuesAndMinIsLessThanMax_ReturnsWithinRange(int minValue, int maxValue)
    {
        // Arrange
        var randomizer = new TrueRandomizerService();

        for (var i = 0; i < 1000; i++)
        {
            // Act
            var result = randomizer.GetValue(minValue, maxValue);

            // Assert
            AssertExt.WithinRange(result, minValue, maxValue);
        }
    }

    [Theory]
    [InlineData(1, 12)]
    [InlineData(1, 42)]
    [InlineData(1, 80)]
    [InlineData(1, 160)]
    public void GetValue_WhenInvokingWithIntValuesAndMinIsGreaterThanMax_ReturnsWithinRange(int minValue, int maxValue)
    {
        // Arrange
        var randomizer = new TrueRandomizerService();

        for (var i = 0; i < 1000; i++)
        {
            // Act
            var result = randomizer.GetValue(maxValue, minValue);

            // Assert
            AssertExt.WithinRange(result, minValue, maxValue);
        }
    }

    [Fact]
    public void GetValue_WhenInvokingWithIntValuesAndMinIsEqualToMax_ReturnsValueThatMatchesMinOrMax()
    {
        // Arrange
        var randomizer = new TrueRandomizerService();

        for (var i = 0; i < 1000; i++)
        {
            // Act
            var result = randomizer.GetValue(10, 10);

            // Assert
            Assert.Equal(10, result);
        }
    }

    [Theory]
    [InlineData(1.001f, 2.001f)]
    [InlineData(1.001f, 4.001f)]
    [InlineData(1.001f, 8.001f)]
    [InlineData(1.001f, 16.001f)]
    public void GetValue_WhenInvokingWithFloatValuesAndMinIsLessThanMax_ReturnsWithinRange(float minValue, float maxValue)
    {
        // Arrange
        var randomizer = new TrueRandomizerService();

        for (var i = 0; i < 100000; i++)
        {
            // Act
            var result = randomizer.GetValue(minValue, maxValue);

            // Assert
            // Assert with accuracy of +/- 0.001
            Assert.InRange(result, minValue - 0.001f, maxValue + 0.001f);
        }
    }

    [Theory]
    [InlineData(1.000f, 2.123f)]
    [InlineData(1.987f, 4.456f)]
    [InlineData(1.654f, 8.789f)]
    [InlineData(1.321f, 16.000f)]
    public void GetValue_WhenInvokingWithFloatValuesAndMaxIsGreaterThanMin_ReturnsWithinRange(float minValue, float maxValue)
    {
        // Arrange
        var randomizer = new TrueRandomizerService();

        for (var i = 0; i < 100000; i++)
        {
            // Act
            var result = randomizer.GetValue(maxValue, minValue);

            // Assert
            // Assert with accuracy of +/- 0.001
            Assert.InRange(result, minValue - 0.001f, maxValue + 0.001f);
        }
    }

    [Theory]
    [InlineData(1.001, 2.001)]
    [InlineData(1.001, 4.001)]
    [InlineData(1.001, 8.001)]
    [InlineData(1.001, 16.001)]
    public void GetValue_WhenInvokingWithDoubleValuesAndMinIsLessThanMax_ReturnsWithinRange(double minValue, double maxValue)
    {
        // Arrange
        var randomizer = new TrueRandomizerService();

        for (var i = 0; i < 100000; i++)
        {
            // Act
            var result = randomizer.GetValue(minValue, maxValue);

            // Assert
            // Assert with accuracy of +/- 0.001
            Assert.InRange(result, minValue - 0.001, maxValue + 0.001);
        }
    }

    [Theory]
    [InlineData(1.000, 2.123)]
    [InlineData(1.987, 4.456)]
    [InlineData(1.654, 8.789)]
    [InlineData(1.321, 16.000)]
    public void GetValue_WhenInvokingWithDoubleValuesAndMaxIsGreaterThanMin_ReturnsWithinRange(double minValue, double maxValue)
    {
        // Arrange
        var randomizer = new TrueRandomizerService();

        for (var i = 0; i < 100000; i++)
        {
            // Act
            var result = randomizer.GetValue(maxValue, minValue);

            // Assert
            // Assert with accuracy of +/- 0.001
            Assert.InRange(result, minValue - 0.001, maxValue + 0.001);
        }
    }
    #endregion
}