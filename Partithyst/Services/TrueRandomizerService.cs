// <copyright file="TrueRandomizerService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Partithyst.Services;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

/// <summary>
/// Provides methods for randomizing numbers.
/// </summary>
public class TrueRandomizerService : IRandomizerService
{
    private readonly RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
    private readonly byte[] uint32Buffer = new byte[4];
    private bool isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="TrueRandomizerService"/> class.
    /// </summary>
    public TrueRandomizerService()
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
        else
        {
            return (float)Math.Round(GetValue(minValueAsInt, maxValueAsInt) / 1000f, 3);
        }
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
            var valueTemp = minValue;
            minValue = maxValue;
            maxValue = valueTemp;
        }

        if (minValue == maxValue)
        {
            return minValue;
        }

        maxValue += 1;

        var diff = (long)(maxValue - minValue);

        while (true)
        {
            this.provider.GetBytes(this.uint32Buffer);

            var rand = Math.Abs((int)BitConverter.ToUInt32(this.uint32Buffer, 0));
            var max = 1 + (long)int.MaxValue;
            var remainder = max % diff;

            if (rand < max - remainder)
            {
                return (int)(minValue + (rand % diff));
            }
        }
    }

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
            if (disposing)
            {
                this.provider.Dispose();
            }

            this.isDisposed = true;
        }
    }
}