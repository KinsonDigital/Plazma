// <copyright file="RandomizerService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma.Services;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

/// <summary>
/// Provides methods for randomizing numbers.
/// </summary>
public sealed class RandomizerService : IRandomizerService
{
    private bool isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="RandomizerService"/> class.
    /// </summary>
    public RandomizerService()
    {
    }

    /// <inheritdoc/>
    [ExcludeFromCodeCoverage]
    public bool FlipCoin() => GetValue(0f, 1f) <= 0.5f;

    /// <inheritdoc/>
    public float GetValue(float minValue, float maxValue)
    {
        var minValueAsInt = (int)(minValue * 1000);
        var maxValueAsInt = (int)(maxValue * 1000);

        if (minValueAsInt > maxValueAsInt)
        {
            return (float)Math.Round(GetValue(maxValueAsInt, minValueAsInt) / 1000f, 3);
        }

        return (float)Math.Round(GetValue(minValueAsInt, maxValueAsInt) / 1000f, 3);
    }

    /// <inheritdoc/>
    public double GetValue(double minValue, double maxValue) =>
        GetValue((float)minValue, (float)maxValue);

    /// <inheritdoc/>
    public int GetValue(int minValue, int maxValue)
    {
        // If the min value is greater than the max,
        // swap the values.
        if (minValue > maxValue)
        {
            (minValue, maxValue) = (maxValue, minValue);
        }

        return minValue == maxValue ? minValue : RandomNumberGenerator.GetInt32(minValue, maxValue + 1);
    }

    /// <inheritdoc/>
    [ExcludeFromCodeCoverage]
    public void Dispose() => Dispose(disposing: true);

    /// <inheritdoc cref="IDisposable.Dispose"/>
    /// <param name="disposing">True to dispose of managed resources.</param>
    [ExcludeFromCodeCoverage]
    private void Dispose(bool disposing)
    {
        if (this.isDisposed)
        {
            return;
        }

        if (disposing)
        {
            this.provider.Dispose();
        }

        this.isDisposed = true;
    }
}
