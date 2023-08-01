// <copyright file="AssertExt.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDParticleEngineTests.XUnitHelpers;

using Xunit;

/// <summary>
/// Provides extensions to the <see cref="Assert"/> class.
/// </summary>
public static class AssertExt
{
    /// <summary>
    /// Asserts that the given <paramref name="value"/> falls within the <paramref name="min"/> and <paramref name="max"/>.
    /// </summary>
    /// <param name="value">The value that must be between the <paramref name="min"/> and <paramref name="max"/> to pass.</param>
    /// <param name="min">The minimum value of the range.</param>
    /// <param name="max">The maximum value of the range.</param>
    public static void WithinRange(int value, int min, int max)
    {
        if (value >= min && value <= max)
        {
            return;
        }

        Assert.True(false, $"Incorrect Value: {value}\nMin Value: {min}\nMax Value: {max}");
    }

    /// <summary>
    /// Asserts that the given <paramref name="value"/> falls within the <paramref name="min"/> and <paramref name="max"/>.
    /// </summary>
    /// <param name="value">The value that must be between the <paramref name="min"/> and <paramref name="max"/> to pass.</param>
    /// <param name="min">The minimum value of the range.</param>
    /// <param name="max">The maximum value of the range.</param>
    public static void WithinRange(float value, float min, float max)
    {
        if (value >= min && value <= max)
        {
            return;
        }

        Assert.True(false, $"Incorrect Value: {value}\nMin Value: {min}\nMax Value: {max}");
    }

    /// <summary>
    /// Asserts that the given <paramref name="value"/> falls within the <paramref name="min"/> and <paramref name="max"/>.
    /// </summary>
    /// <param name="value">The value that must be between the <paramref name="min"/> and <paramref name="max"/> to pass.</param>
    /// <param name="min">The minimum value of the range.</param>
    /// <param name="max">The maximum value of the range.</param>
    public static void WithinRange(double value, double min, double max)
    {
        if (value >= min && value <= max)
        {
            return;
        }

        Assert.True(false, $"Incorrect Value: {value}\nMin Value: {min}\nMax Value: {max}");
    }
}