// <copyright file="FakeTextureLoader.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace PerfShared;

using Plazma;

/// <summary>
/// Loads fake particle textures for the purpose of performance testing.
/// </summary>
public sealed class FakeTextureLoader : ITextureLoader<IFakeTexture>
{
    /// <summary>
    /// Loads a fake texture.
    /// </summary>
    /// <param name="textureName">The fake texture name.</param>
    /// <returns>The loaded texture.</returns>
    public IFakeTexture LoadTexture(string textureName) => new FakeTexture();

    /// <inheritdoc/>
    public void Dispose()
    {
    }
}
