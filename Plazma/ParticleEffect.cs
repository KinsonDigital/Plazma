// <copyright file="ParticleEffect.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using Behaviors;

/// <summary>
/// Holds the particle setup settings data for the <see cref="ParticleEngine{TTexture}"/> to consume.
/// </summary>
public readonly record struct ParticleEffect
{
    private readonly EasingRandomBehaviorSettings[] behaviorSettings = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="ParticleEffect"/> class.
    /// </summary>
    public ParticleEffect()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ParticleEffect"/> class.
    /// </summary>
    /// <param name="particleTextureName">The name of the texture used in the particle effect.</param>
    /// <param name="settings">The settings used to set up the particle effect.</param>
    public ParticleEffect(string particleTextureName, EasingRandomBehaviorSettings[] settings)
    {
        this.behaviorSettings = settings ?? throw new ArgumentNullException(nameof(settings), "Parameter must not be null.");
        ParticleTextureName = particleTextureName;
    }

    /// <summary>
    /// Gets the name of the particle texture used in the particle effect.
    /// </summary>
    public string ParticleTextureName { get; private init; } = string.Empty;

    /// <summary>
    /// Gets the location on the screen of where to spawn the <see cref="Particle"/>s.
    /// </summary>
    public Vector2 SpawnLocation { get; init; }

    /// <summary>
    /// Gets the total number of particles.
    /// </summary>
    /// <remarks>This takes into account any particle regardless if it is alive or dead.</remarks>
    public int TotalParticles { get; init; } = 1;

    /// <summary>
    /// Gets the minimum spawn rate of the range that a <see cref="Particle"/> will be randomly set to.
    /// </summary>
    /// <remarks>Decrease this value to spawn particles faster over time.</remarks>
    public float SpawnRateMin { get; init; } = 250;

    /// <summary>
    /// Gets the maximum spawn rate of the range that a <see cref="Particle"/> will be randomly set to.
    /// </summary>
    /// <remarks>Decrease this value to spawn particles faster over time.</remarks>
    public float SpawnRateMax { get; init; } = 1000;

    /// <summary>
    /// Gets a value indicating whether particles will spawn at a limited rate.
    /// </summary>
    public bool LimitSpawnRate { get; init; } = true;

    /// <summary>
    /// Gets a value indicating whether the bursting effect is enabled or disabled.
    /// </summary>
    /// <remarks>
    ///     If enabled, the engine will spawn particles in a bursting fashion at intervals based on the timings between
    ///     the <see cref="BurstOnMilliseconds"/> and <see cref="BurstOffMilliseconds"/> timing values.
    ///     If the bursting effect is in its on cycle, the particles will use the
    ///     <see cref="ParticleEffect.BurstSpawnRateMin"/> and <see cref="ParticleEffect.BurstSpawnRateMax"/>
    ///     values and if the spawn effect is in its off cycle, it will use the <see cref="ParticleEffect.SpawnRateMin"/>
    ///     <see cref="ParticleEffect.SpawnRateMax"/> values.
    /// </remarks>
    public bool BurstEnabled { get; init; }

    /// <summary>
    /// Gets the minimum particle spawn rate that can be randomly generated
    /// when <see cref="BurstEnabled"/> is enabled.
    /// </summary>
    public float BurstSpawnRateMin { get; init; }

    /// <summary>
    /// Gets the maximum particle spawn rate that can be randomly generated
    /// when <see cref="BurstEnabled"/> is enabled.
    /// </summary>
    public float BurstSpawnRateMax { get; init; } = 250;

    /// <summary>
    /// Gets the amount of time that the bursting effect will run in its on cycle.
    /// </summary>
    public float BurstOnMilliseconds { get; init; } = 3000;

    /// <summary>
    /// Gets the amount of time that the bursting effect will run in its off cycle.
    /// </summary>
    public float BurstOffMilliseconds { get; init; } = 1000;

    /// <summary>
    /// Gets a value indicating whether the colors will be randomly chosen from a list.
    /// </summary>
    public bool UseColorsFromList { get; init; }

    /// <summary>
    /// Gets the list of behavior settings that describe how the particle effect is set up.
    /// </summary>
    public ReadOnlyCollection<EasingRandomBehaviorSettings> BehaviorSettings
    {
        get => new (this.behaviorSettings);
        init => this.behaviorSettings = value.ToArray();
    }
}
