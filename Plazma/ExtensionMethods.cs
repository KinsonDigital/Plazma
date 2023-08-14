// <copyright file="ExtensionMethods.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

#pragma warning disable CA1303 // Do not pass literals as localized parameters
namespace Plazma;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

/// <summary>
/// Provides extensions to various things to help make better code.
/// </summary>
public static class ExtensionMethods
{
    private static readonly char[] ValidNumChars = { '-', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

    /// <summary>
    /// Returns a random value between the given <paramref name="minValue"/> and <paramref name="maxValue"/>.
    /// </summary>
    /// <param name="random">The random instance to use.</param>
    /// <param name="minValue">The minimum value that the result will be.</param>
    /// <param name="maxValue">The maximum value that the result will be.</param>
    /// <returns>A randomized number between the min and max values.</returns>
    public static float Next(this Random random, float minValue, float maxValue)
    {
        if (random is null)
        {
            throw new ArgumentNullException(nameof(random), "The parameter must not be null.");
        }

        var minValueAsInt = (int)(minValue * 1000);
        var maxValueAsInt = (int)(maxValue * 1000);

        if (minValueAsInt > maxValueAsInt)
        {
            return maxValue;
        }

        var randomResult = random.Next(minValueAsInt, maxValueAsInt);

        return randomResult / 1000f;
    }

    /// <summary>
    /// Counts the given <paramref name="items"/> based on given <paramref name="predicate"/> returning true.
    /// </summary>
    /// <typeparam name="T">The type of object in list to count.</typeparam>
    /// <param name="items">The list of items to count based on the predicate.</param>
    /// <param name="predicate">The predicate that when returns true, counts the item.</param>
    /// <returns>The number of items that match the predicate..</returns>
    [SuppressMessage("csharpsquid", "S3267", Justification = "Not needed.")]
    public static int Count<T>(this List<T>? items, Predicate<T> predicate)
    {
        if (items is null)
        {
            return 0;
        }

        if (predicate is null)
        {
            throw new ArgumentNullException(nameof(predicate), "The parameter must not be null.");
        }

        var result = 0;

        foreach (var t in items)
        {
            if (predicate(t))
            {
                result++;
            }
        }

        return result;
    }

    /// <summary>
    /// Counts the given <paramref name="items"/> based on given <paramref name="predicate"/> returning true.
    /// </summary>
    /// <typeparam name="T">The type of object in list to count.</typeparam>
    /// <param name="items">The list of items to count based on the predicate.</param>
    /// <param name="predicate">The predicate that when returns true, counts the item.</param>
    /// <returns>The number of items that match the predicate..</returns>
    [SuppressMessage("csharpsquid", "S3267", Justification = "Not needed.")]
    public static int Count<T>(this T[]? items, Predicate<T> predicate)
    {
        if (items is null)
        {
            return 0;
        }

        if (predicate is null)
        {
            throw new ArgumentNullException(nameof(predicate), "The parameter must not be null.");
        }

        var result = 0;

        foreach (var t in items)
        {
            if (predicate(t))
            {
                result++;
            }
        }

        return result;
    }

    /// <summary>
    /// Returns a value indicating if the given string <paramref name="value"/> is a valid number.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <returns>True if the string contains non number characters.</returns>
    public static bool ContainsNonNumberCharacters(this string value) =>
        !string.IsNullOrEmpty(value) && Array.Exists(value.ToCharArray(), c => !ValidNumChars.Contains(c));

    /// <summary>
    /// Returns a value indicating if each item in a given list position is
    /// equal to the items in the same position of th given <paramref name="compareItems"/> list.
    /// </summary>
    /// <typeparam name="T">The type of item in the lists.</typeparam>
    /// <param name="items">The current instance of <see cref="IEnumerable{T}"/> items.</param>
    /// <param name="compareItems">The items to compare to the this list of items.</param>
    /// <returns>True if each arrays are equal.</returns>
    public static bool ItemsAreEqual<T>(this IEnumerable<T>? items, IEnumerable<T>? compareItems)
        where T : class
    {
        var enumeratedItems = items is null ? Array.Empty<T>() : items.ToArray();
        var enumeratedCompareItems = compareItems is null ? Array.Empty<T>() : compareItems.ToArray();

        if (enumeratedItems.Length != enumeratedCompareItems.Length)
        {
            return false;
        }

        for (var i = 0; i < enumeratedItems.Length; i++)
        {
            if (!enumeratedItems[i].Equals(enumeratedCompareItems[i]))
            {
                return false;
            }
        }

        return true;
    }
}
