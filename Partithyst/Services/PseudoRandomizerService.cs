// <copyright file="PseudoRandomizerService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Partithyst.Services;

using System;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Provides methods for randomizing numbers.
/// </summary>
public class PseudoRandomizerService : IRandomizerService
{
    private readonly Random random;
    private bool isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="PseudoRandomizerService"/> class.
    /// </summary>
    public PseudoRandomizerService() => this.random = new Random();

    /// <inheritdoc/>
    [ExcludeFromCodeCoverage]
    public bool FlipCoin() => this.random.NextDouble() <= 0.5;

    /// <inheritdoc/>
    public float GetValue(float minValue, float maxValue)
    {
        var minValueAsInt = (int)((minValue + 0.001f) * 1000);
        var maxValueAsInt = (int)((maxValue + 0.001f) * 1000);

        if (minValueAsInt > maxValueAsInt)
        {
            return (float)Math.Round(this.random.Next(maxValueAsInt, minValueAsInt) / 1000f, 3);
        }
        else
        {
            return (float)Math.Round(this.random.Next(minValueAsInt, maxValueAsInt) / 1000f, 3);
        }
    }

    /// <inheritdoc/>
    public double GetValue(double minValue, double maxValue) =>
        GetValue((float)minValue, (float)maxValue);

    /// <inheritdoc/>
    public int GetValue(int minValue, int maxValue) =>
        minValue > maxValue
            ? this.random.Next(maxValue, minValue + 1)
            : this.random.Next(minValue, maxValue + 1);

    /// <inheritdoc/>
    [ExcludeFromCodeCoverage]
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="disposing">True to dispose of managed resources.</param>
    [ExcludeFromCodeCoverage]
    protected virtual void Dispose(bool disposing)
    {
        if (!this.isDisposed)
        {
            this.isDisposed = true;
        }
    }
}