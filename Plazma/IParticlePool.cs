﻿// <copyright file="IParticlePool.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma;

using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Contains a pool of reusable particles with each particle having various behaviors.
/// </summary>
/// <typeparam name="TTexture">The texture for the particles in the pool.</typeparam>
public interface IParticlePool<out TTexture> : IDisposable
    where TTexture : class
{
    /// <summary>
    /// Occurs every time the total amount of living particles has changed.
    /// </summary>
    public event EventHandler<EventArgs>? LivingParticlesCountChanged;

    /// <summary>
    /// Gets current total number of living <see cref="Particle"/>s.
    /// </summary>
    public int TotalLivingParticles { get; }

    /// <summary>
    /// Gets the current total number of dead <see cref="Particle"/>s.
    /// </summary>
    public int TotalDeadParticles { get; }

    /// <summary>
    /// Gets or sets a value indicating whether particles will spawn at a limited rate.
    /// </summary>
    public bool LimitSpawnRate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the bursting effect is enabled or disabled.
    /// </summary>
    /// <remarks>
    ///     If enabled, the engine will spawn particles in a bursting fashion at intervals based on the timings between
    ///     the <see cref="ParticleEffect.BurstOnMilliseconds"/> and <see cref="ParticleEffect.BurstOffMilliseconds"/> timing values.
    ///     If the bursting effect is in its on cycle, the particles will use the
    ///     <see cref="ParticleEffect.BurstSpawnRateMin"/> and <see cref="ParticleEffect.BurstSpawnRateMax"/>
    ///     values and if the spawn effect is in its off cycle, it will use the <see cref="ParticleEffect.SpawnRateMin"/>
    ///     <see cref="ParticleEffect.SpawnRateMax"/> values.
    /// </remarks>
    public bool BurstEnabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the bursting effect is currently bursting.
    /// </summary>
    /// <remarks>
    ///     Indicates if the bursting effect is currently in its on in the on/off cycle.
    ///     True is bursting and false means it is not.
    /// </remarks>
    public bool InBurstMode { get; set; }

    /// <summary>
    /// Gets the list of particle in the pool.
    /// </summary>
    // TODO: Look into truely readonly array.  Refer to nick chapsas videos
    public ReadOnlyCollection<IParticle> Particles { get; }

    /// <summary>
    /// Gets the particle effect of the pool.
    /// </summary>
    public ParticleEffect Effect { get; }

    /// <summary>
    /// Gets the texture of the particles in the pool.
    /// </summary>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Part of the public API.")]
    public TTexture? PoolTexture { get; }

    /// <summary>
    /// Gets a value indicating whether the pool texture has been loaded.
    /// </summary>
    public bool TextureLoaded { get; }

    /// <summary>
    /// Updates the particle pool.
    /// </summary>
    /// <param name="timeElapsed">The amount of time that has passed since the last frame.</param>
    public void Update(TimeSpan timeElapsed);

    /// <summary>
    /// Kills all of the particles.
    /// </summary>
    public void KillAllParticles();

    /// <summary>
    /// Loads the texture for the pool to use for rendering the particles.
    /// </summary>
    public void LoadTexture();
}
