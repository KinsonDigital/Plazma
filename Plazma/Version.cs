// <copyright file="Version.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma;

using System.Reflection;

/// <summary>
/// Holds the version information of the <see cref="ParticleEngine{TTexture}"/> library.
/// </summary>
public static class Version
{
    /// <summary>
    /// Gets the version of the library.
    /// </summary>
    /// <returns>The version string.</returns>
    public static string GetVersion()
    {
        var versionInfo = Assembly.GetExecutingAssembly()?.GetName().Version;

        return versionInfo is null ? "error: version unknown" : $"Particle Engine: v{versionInfo.Major}.{versionInfo.Minor}.{versionInfo.Build}";
    }
}