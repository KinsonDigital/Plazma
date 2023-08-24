// <copyright file="ParticleTextureLoader.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace PlazmaTesting;

using Plazma;
using Velaptor.Content;
using Velaptor.Factories;

/// <summary>
/// Loads particle textures.
/// </summary>
public sealed class ParticleTextureLoader : ITextureLoader<ITexture>
{
    private readonly ILoader<ITexture> loader = ContentLoaderFactory.CreateTextureLoader();
    private bool isDisposed;
    private string contentPathOrName = string.Empty;

    /// <summary>
    /// Loads a single particle texture with the given <paramref name="assetName"/>.
    /// </summary>
    /// <param name="assetName">The name of the asset to load.</param>
    /// <returns>The loaded texture.</returns>
    public ITexture LoadTexture(string assetName)
    {
        ArgumentException.ThrowIfNullOrEmpty(assetName);

        this.contentPathOrName = assetName;
        return this.loader.Load(assetName);
    }

    public void Dispose() => Dispose(true);

    private void Dispose(bool disposing)
    {
        if (this.isDisposed)
        {
            return;
        }

        if (disposing)
        {
            this.loader.Unload(this.contentPathOrName);
        }

        this.isDisposed = true;
    }
}
