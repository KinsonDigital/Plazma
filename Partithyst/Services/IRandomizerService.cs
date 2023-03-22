// <copyright file="IRandomizerService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Partithyst.Services;

using System;

/// <summary>
/// Provides methods for randomizing numbers.
/// </summary>
public interface IRandomizerService : IDisposable
{
    /// <summary>
    /// Returns a true/false value that represents the flip of a coin.
    /// </summary>
    /// <returns>A random value between 0 and 1.  50% chance.</returns>
    bool FlipCoin();

    /// <summary>
    /// Gets a random number between the given <paramref name="minValue"/> and <paramref name="maxValue"/>s.
    /// A random value will be chosen between the min and max values no matter which value is less than
    /// or greater than the other.
    /// </summary>
    /// <param name="minValue">The inclusive minimum value of the range to randomly choose from.</param>
    /// <param name="maxValue">The inclusive maximum value of the range to randomly choose from.</param>
    /// <returns>A randomized value between a min and max.</returns>
    double GetValue(double minValue, double maxValue);

    /// <summary>
    /// Gets a random number between the given <paramref name="minValue"/> and <paramref name="maxValue"/>s.
    /// A random value will be chosen between the min and max values no matter which value is less than
    /// or greater than the other.
    /// </summary>
    /// <param name="minValue">The inclusive minimum value of the range to randomly choose from.</param>
    /// <param name="maxValue">The inclusive maximum value of the range to randomly choose from.</param>
    /// <returns>A randomized value between a min and max.</returns>
    float GetValue(float minValue, float maxValue);

    /// <summary>
    /// Gets a random number between the given <paramref name="minValue"/> and <paramref name="maxValue"/>s.
    /// A random value will be chosen between the min and max values no matter which value is less than
    /// or greater than the other.
    /// </summary>
    /// <param name="minValue">The inclusive minimum value of the range to randomly choose from.</param>
    /// <param name="maxValue">The inclusive maximum value of the range to randomly choose from.</param>
    /// <returns>A randomized value between a min and max.</returns>
    int GetValue(int minValue, int maxValue);
}