// <copyright file="ParticleEngine.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

#pragma warning disable CA1303 // Do not pass literals as localized parameters
namespace Plazma;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Behaviors;
using Services;

/// <summary>
/// Manages multiple <see cref="Particle"/>s with various settings that dictate
/// how all of the <see cref="Particle"/>s behave and look on the screen.
/// </summary>
/// <typeparam name="TTexture">The type of texture for the particles.</typeparam>
public sealed class ParticleEngine<TTexture> : IDisposable
    where TTexture : class
{
    private readonly List<ParticlePool<TTexture>> particlePools = new ();
    private readonly ITextureLoader<TTexture> textureLoader;
    private readonly IRandomizerService randomizer;
    private bool enabled = true;
    private bool isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="ParticleEngine{TTexture}"/> class.
    /// </summary>
    /// <param name="textureLoader">Loads particle textures.</param>
    /// <param name="randomizer">Randomizes numbers.</param>
    public ParticleEngine(ITextureLoader<TTexture> textureLoader, IRandomizerService randomizer)
    {
        this.textureLoader = textureLoader;
        this.randomizer = randomizer;
    }

    /// <summary>
    /// Gets all of the particle pools.
    /// </summary>
    public ReadOnlyCollection<ParticlePool<TTexture>> ParticlePools
        => new (this.particlePools.ToArray());

    /// <summary>
    /// Gets or sets a value indicating whether the engine is enabled or disabled.
    /// </summary>
    public bool Enabled
    {
        get => this.enabled;
        set
        {
            this.enabled = value;

            // If the engine is disabled, kill all the particles
            if (!this.enabled)
            {
                KillAllParticles();
            }
        }
    }

    /// <summary>
    /// Gets a value indicating whether the textures for the <see cref="ParticlePools"/>
    /// have been loaded.
    /// </summary>
    public bool TexturesLoaded => this.particlePools.Count > 0 && this.particlePools.All(p => p.TextureLoaded);

    /// <summary>
    /// Creates a particle pool using the given particle <paramref name="effect"/>.
    /// </summary>
    /// <param name="effect">The particle effect for the pool to use.</param>
    /// <param name="behaviorFactory">The factory used for creating behaviors.</param>
    public void CreatePool(ParticleEffect effect, IBehaviorFactory behaviorFactory) =>
        this.particlePools.Add(new ParticlePool<TTexture>(behaviorFactory, this.textureLoader, effect, this.randomizer));

    /// <summary>
    /// Clears all of the current existing pools.
    /// </summary>
    /// <remarks>This will properly dispose of the texture for each pool.</remarks>
    public void ClearPools()
    {
        foreach (var pool in this.particlePools)
        {
            pool.Dispose();
        }

        this.particlePools.Clear();
    }

    /// <summary>
    /// Loads all of the textures for each <see cref="ParticlePool{TTexture}"/>
    /// in the engine.
    /// </summary>
    public void LoadTextures()
    {
        foreach (var pool in this.particlePools)
        {
            pool.LoadTexture();
        }
    }

    /// <summary>
    /// Kills all of the particles.
    /// </summary>
    public void KillAllParticles() => this.particlePools.ForEach(p => p.KillAllParticles());

    /// <summary>
    /// Updates all of the <see cref="Particle"/>s.
    /// </summary>
    /// <param name="timeElapsed">The amount of time that has passed since the last frame.</param>
    public void Update(TimeSpan timeElapsed)
    {
        if (this.particlePools.Count <= 0)
        {
            return;
        }

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
            foreach (var pool in ParticlePools)
            {
                pool.Dispose();
            }
        }

        this.isDisposed = true;
    }
}
