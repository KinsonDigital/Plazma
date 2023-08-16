// <copyright file="IParticlePool.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma;

using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Behaviors;

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
    event EventHandler<EventArgs>? LivingParticlesCountChanged;

    /// <summary>
    /// Gets current total number of living <see cref="Particle"/>s.
    /// </summary>
    int TotalLivingParticles { get; }

    /// <summary>
    /// Gets the current total number of dead <see cref="Particle"/>s.
    /// </summary>
    int TotalDeadParticles { get; }

    /// <summary>
    /// Gets or sets a value indicating whether particles will spawn at a limited rate.
    /// </summary>
    bool LimitSpawnRate { get; set; }

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
    bool BurstEnabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the bursting effect is currently bursting.
    /// </summary>
    /// <remarks>
    ///     Indicates if the bursting effect is currently in its on in the on/off cycle.
    ///     True is bursting and false means it is not.
    /// </remarks>
    bool InBurstMode { get; set; }

    /// <summary>
    /// Gets the list of particle in the pool.
    /// </summary>
    ImmutableArray<IParticle> Particles { get; }

    /// <summary>
    /// Gets the particle effect of the pool.
    /// </summary>
    ParticleEffect Effect { get; }

    /// <summary>
    /// Gets the texture of the particles in the pool.
    /// </summary>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Part of the public API.")]
    TTexture? PoolTexture { get; }

    /// <summary>
    /// Gets a value indicating whether the pool texture has been loaded.
    /// </summary>
    bool TextureLoaded { get; }

    /// <summary>
    /// Updates the particle pool.
    /// </summary>
    /// <param name="timeElapsed">The amount of time that has passed since the last frame.</param>
    void Update(TimeSpan timeElapsed);

    /// <summary>
    /// Kills all of the particles.
    /// </summary>
    void KillAllParticles();

    /// <summary>
    /// Loads the texture for the pool to use for rendering the particles.
    /// </summary>
    void LoadTexture();

    /// <summary>
    /// Add a behavior to all of the particles using the given behavior settings.
    /// </summary>
    /// <param name="behaviorSettings">The behavior settings.</param>
    void AddBehavior(EasingRandomBehaviorSettings behaviorSettings);

    /// <summary>
    /// Removes a behavior from all of the particles that matches the given <paramref name="behaviorType"/>.
    /// </summary>
    /// <param name="behaviorType">The type of behavior to remove.</param>
    void RemoveBehavior(BehaviorAttribute behaviorType);
}
