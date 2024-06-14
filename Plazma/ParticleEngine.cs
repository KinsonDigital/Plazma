// <copyright file="ParticleEngine.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable ForCanBeConvertedToForeach
#pragma warning disable CA1303 // Do not pass literals as localized parameters
namespace Plazma;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Manages multiple <see cref="Particle"/>s with various settings that dictate
/// how all the <see cref="Particle"/>s behave and look on the screen.
/// </summary>
/// <typeparam name="TTexture">The type of texture for the particles.</typeparam>
public sealed class ParticleEngine<TTexture> : IDisposable
    where TTexture : class
{
    // TODO: Convert to iterable and IEnumerable for particle pools
    private readonly List<IParticlePool<TTexture>> particlePools = [];
    private bool isDisposed;

    /// <summary>
    /// Gets all the particle pools.
    /// </summary>
    public ReadOnlyCollection<IParticlePool<TTexture>> ParticlePools
        => new (this.particlePools.ToArray());

    /// <summary>
    /// Gets or sets a value indicating whether the engine is enabled or disabled.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Gets a value indicating whether the textures for the <see cref="ParticlePools"/>
    /// have been loaded.
    /// </summary>
    public bool TexturesLoaded => this.particlePools.Count > 0 && this.particlePools.TrueForAll(p => p.TextureLoaded);

    /// <summary>
    /// Adds the given particle <paramref name="pool"/> to the engine.
    /// </summary>
    /// <param name="pool">The particle pool to add.</param>
    public void AddPool(IParticlePool<TTexture> pool) => this.particlePools.Add(pool);

    /// <summary>
    /// Clears all the current existing pools.
    /// </summary>
    /// <remarks>This will properly dispose of the texture for each pool.</remarks>
    public void ClearPools()
    {
        for (var i = 0; i < this.particlePools.Count; i++)
        {
            this.particlePools[i].Dispose();
        }

        this.particlePools.Clear();
    }

    /// <summary>
    /// Loads all the textures for each <see cref="ParticlePool{TTexture}"/>
    /// in the engine.
    /// </summary>
    public void LoadTextures()
    {
        for (var i = 0; i < this.particlePools.Count; i++)
        {
            this.particlePools[i].LoadTexture();
        }
    }

    /// <summary>
    /// Kills all the particles.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Part of public API.")]
    public void KillAllParticles() => this.particlePools.ForEach(p => p.KillAllParticles());

    /// <summary>
    /// Updates all the <see cref="Particle"/>s.
    /// </summary>
    /// <param name="timeElapsed">The amount of time that has passed since the last frame.</param>
    public void Update(TimeSpan timeElapsed)
    {
        if (!TexturesLoaded)
        {
            throw new Exception("The textures must be loaded first.");
        }

        if (!Enabled)
        {
            return;
        }

        this.particlePools.ForEach(p => p.Update(timeElapsed));
    }

    /// <inheritdoc/>
    public void Dispose() => Dispose(true);

    /// <inheritdoc cref="IDisposable.Dispose"/>
    /// <param name="disposing">True to dispose of managed resources.</param>
    private void Dispose(bool disposing)
    {
        if (this.isDisposed)
        {
            return;
        }

        if (disposing)
        {
            for (var i = 0; i < this.particlePools.Count; i++)
            {
                this.particlePools[i].Dispose();
            }

            this.particlePools.Clear();
        }

        this.isDisposed = true;
    }
}
