// <copyright file="TestItem.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
#pragma warning disable CA1062 // Validate arguments of public methods
namespace KDParticleEngineTests.Fakes;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Used for testing <see cref="IEnumerable{T}"/> item comparison.
/// </summary>
[ExcludeFromCodeCoverage]
public class TestItem : IEquatable<TestItem>
{
    /// <summary>
    /// Gets or sets sample property value.
    /// </summary>
    public int Number { get; set; }

    public bool Equals([AllowNull] TestItem other) => Number == other.Number;

    public override bool Equals(object obj)
    {
        if (!(obj is TestItem item))
        {
            return false;
        }

        return Equals(item);
    }
}