﻿// <copyright file="AssertHelpers.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDParticleEngineTests.XUnitHelpers;

using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

/// <summary>
/// Provides helper methods for the <see cref="XUnit"/>'s <see cref="Assert"/> class.
/// </summary>
[ExcludeFromCodeCoverage]
public static class AssertHelpers
{
    /// <summary>
    /// Verifies that the exact exception is thrown (and not a derived exception type) and that
    /// the exception message matches the given <paramref name="expectedMessage"/>.
    /// </summary>
    /// <typeparam name="T">The type of exception that the test is verifying.</typeparam>
    /// <param name="testCode">The code that will be throwing the expected exception.</param>
    /// <param name="expectedMessage">The expected message of the exception.</param>
    public static void ThrowsWithMessage<T>(Action testCode, string expectedMessage)
        where T : Exception
        => Assert.Equal(expectedMessage, Assert.Throws<T>(testCode).Message);
}