// <copyright file="ParticleEffect.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma;

using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using Behaviors;
using Newtonsoft.Json;

/// <summary>
/// Holds the particle setup settings data for the <see cref="ParticleEngine{TTexture}"/> to consume.
/// </summary>
public class ParticleEffect
{
    private BehaviorSettings[] behaviorSettings = Array.Empty<BehaviorSettings>();

    /// <summary>
    /// Initializes a new instance of the <see cref="ParticleEffect"/> class.
    /// </summary>
    [JsonConstructor]
    public ParticleEffect()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ParticleEffect"/> class.
    /// </summary>
    /// <param name="particleTextureName">The name of the texture used in the particle effect.</param>
    /// <param name="settings">The settings used to setup the particle effect.</param>
    public ParticleEffect(string particleTextureName, BehaviorSettings[] settings)
    {
        this.behaviorSettings = settings ?? throw new ArgumentNullException(nameof(settings), "Parameter must not be null.");
        ParticleTextureName = particleTextureName;
    }

    /// <summary>
    /// Gets the name of the particle texture used in the particle effect.
    /// </summary>
    public string ParticleTextureName { get; private set; } = string.Empty;

    /// <summary>
    /// Gets or sets the location on the screen of where to spawn the <see cref="Particle"/>s.
    /// </summary>
    public PointF SpawnLocation { get; set; }

    /// <summary>
    /// Gets or sets the total number of particles that can be alive at once.
    /// </summary>
    public int TotalParticlesAliveAtOnce { get; set; } = 1;

    /// <summary>
    /// Gets or sets the minimum spawn rate of the range that a <see cref="Particle"/> will be randomly set to.
    /// </summary>
    /// <remarks>Decrease this value to spawn particles faster over time.</remarks>
    public int SpawnRateMin { get; set; } = 250;

    /// <summary>
    /// Gets or sets the maximum spawn rate of the range that a <see cref="Particle"/> will be randomly set to.
    /// </summary>
    /// <remarks>Decrease this value to spawn particles faster over time.</remarks>
    public int SpawnRateMax { get; set; } = 1000;

    /// <summary>
    /// Gets or sets a value indicating whether particles will spawn at a limited rate.
    /// </summary>
    public bool SpawnRateEnabled { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the bursting effect is enabled or disabled.
    /// </summary>
    /// <remarks>
    ///     If enabled, the engine will spawn particles in a bursting fashion at intervals based on the timings between
    ///     the <see cref="BurstOnMilliseconds"/> and <see cref="BurstOffMilliseconds"/> timing values.
    ///     If the bursting effect is in its on cycle, the particles will use the
    ///     <see cref="ParticleEffect.BurstSpawnRateMin"/> and <see cref="ParticleEffect.BurstSpawnRateMax"/>
    ///     values and if the spawn effect is in its off cycle, it will use the <see cref="ParticleEffect.SpawnRateMin"/>
    ///     <see cref="ParticleEffect.SpawnRateMax"/> values.
    /// </remarks>
    public bool BurstEnabled { get; set; }

    /// <summary>
    /// Gets or sets the minimum particle spawn rate that can be randomly generated
    /// when <see cref="BurstEnabled"/> is enabled.
    /// </summary>
    public int BurstSpawnRateMin { get; set; }

    /// <summary>
    /// Gets or sets the maximum particle spawn rate that can be randomly generated
    /// when <see cref="BurstEnabled"/> is enabled.
    /// </summary>
    public int BurstSpawnRateMax { get; set; } = 250;

    /// <summary>
    /// Gets or sets the amount of time that the bursting effect will run in it's on cycle.
    /// </summary>
    public int BurstOnMilliseconds { get; set; } = 3000;

    /// <summary>
    /// Gets or sets the amount of time that the bursting effect will run in it's off cycle.
    /// </summary>
    public int BurstOffMilliseconds { get; set; } = 1000;

    /// <summary>
    /// Gets or sets a value indicating whether the colors will be randomly chosen from a list.
    /// </summary>
    public bool UseColorsFromList { get; set; }

    /// <summary>
    /// Gets or sets the list of behavior settings that describe how the particle effect is setup.
    /// </summary>
    public ReadOnlyCollection<BehaviorSettings> BehaviorSettings
    {
        get => new (this.behaviorSettings);
        set => this.behaviorSettings = value.ToArray();
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is not ParticleEffect effect)
        {
            return false;
        }

        return ParticleTextureName == effect.ParticleTextureName &&
               SpawnLocation == effect.SpawnLocation &&
               TotalParticlesAliveAtOnce == effect.TotalParticlesAliveAtOnce &&
               SpawnRateMin == effect.SpawnRateMin &&
               SpawnRateMax == effect.SpawnRateMax &&
               UseColorsFromList == effect.UseColorsFromList &&
               BehaviorSettings.ItemsAreEqual(effect.BehaviorSettings);
    }

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    [ExcludeFromCodeCoverage]
    public override int GetHashCode()
    {
        var hash = default(HashCode);

        hash.Add(ParticleTextureName);
        hash.Add(SpawnLocation);

        hash.Add(TotalParticlesAliveAtOnce);
        hash.Add(SpawnRateMin);
        hash.Add(SpawnRateMax);
        hash.Add(UseColorsFromList);
        hash.Add(BehaviorSettings);

        return hash.ToHashCode();
    }
}
