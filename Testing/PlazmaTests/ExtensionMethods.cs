// <copyright file="ExtensionMethods.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace PlazmaTests;

using System;

/// <summary>
/// Provides helper methods for tests.
/// </summary>
public static class ExtensionMethods
{
    /// <summary>
    /// Returns a <see cref="TimeSpan"/> from the given milliseconds.
    /// </summary>
    /// <param name="value">The milliseconds to use.</param>
    /// <returns>A <see cref="TimeSpan"/> with only milliseconds.</returns>
    public static TimeSpan ToMillisecondsTimeSpan(this int value) => new (0, 0, 0, 0, value);
}
