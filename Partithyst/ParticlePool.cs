// <copyright file="ParticlePool.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

#pragma warning disable CA1303 // Do not pass literals as localized parameters
namespace Partithyst;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Behaviors;
using Services;

/// <summary>
/// Contains a number of reusable particles with a given particle effect applied to them.
/// </summary>
/// <typeparam name="TTexture">The texture for the particles in the pool.</typeparam>
public class ParticlePool<TTexture> : IDisposable
    where TTexture : class, IDisposable
{
    private readonly IRandomizerService randomService;
    private readonly ITextureLoader<TTexture> textureLoader;
    private List<Particle> particles = new List<Particle>();
    private bool isDisposed;
    private int spawnRate;
    private double spawnRateElapsed;
    private int burstOnTimeElapsed;
    private int burstOffTimeElapsed;

    /// <summary>
    /// Initializes a new instance of the <see cref="ParticlePool{Texture}"/> class.
    /// </summary>
    /// <param name="behaviorFactory">The factory used for creating new behaviors for each particle.</param>
    /// <param name="textureLoader">Loads the textures for the <see cref="ParticlePool{Texture}"/>.</param>
    /// <param name="effect">The particle effect to be applied to all of the particles in the pool.</param>
    /// <param name="randomizer">Used for generating random values when a particle is spawned.</param>
    public ParticlePool(IBehaviorFactory behaviorFactory, ITextureLoader<TTexture> textureLoader, ParticleEffect effect, IRandomizerService randomizer)
    {
        if (behaviorFactory is null)
        {
            throw new ArgumentNullException(nameof(behaviorFactory), "The parameter must not be null.");
        }

        if (effect is null)
        {
            throw new ArgumentNullException(nameof(effect), "The parameter must not be null.");
        }

        this.textureLoader = textureLoader;
        Effect = effect;
        this.randomService = randomizer;

        CreateAllParticles(behaviorFactory);
        this.spawnRate = GetRandomSpawnRate();
    }

    /// <summary>
    /// Occurs every time the total amount of living particles has changed.
    /// </summary>
#pragma warning disable CS0067 // The event is never used
    public event EventHandler<EventArgs>? LivingParticlesCountChanged;
#pragma warning restore CS0067

    /// <summary>
    /// Gets current total number of living <see cref="Particle"/>s.
    /// </summary>
    public int TotalLivingParticles => this.particles.Count(p => p.IsAlive);

    /// <summary>
    /// Gets the current total number of dead <see cref="Particle"/>s.
    /// </summary>
    public int TotalDeadParticles => this.particles.Count(p => p.IsDead);

    /// <summary>
    /// Gets or sets a value indicating whether particles will spawn at a limited rate.
    /// </summary>
    public bool SpawnRateEnabled
    {
        get => Effect.SpawnRateEnabled;
        set => Effect.SpawnRateEnabled = value;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the bursting effect is enabled or disabled.
    /// </summary>
    /// <remarks>
    ///     If enabled, the engine will spawn particles in a bursting fashion at intervals based on the timings between
    ///     the <see cref="ParticleEffect.BurstOnTime"/> and <see cref="ParticleEffect.BurstOffTime"/> timing values.
    ///     If the bursting effect is in its on cycle, the particles will use the
    ///     <see cref="ParticleEffect.BurstSpawnRateMin"/> and <see cref="ParticleEffect.BurstSpawnRateMax"/>
    ///     values and if the spawn effect is in its off cycle, it will use the <see cref="ParticleEffect.SpawnRateMin"/>
    ///     <see cref="ParticleEffect.SpawnRateMax"/> values.
    /// </remarks>
    public bool BurstEnabled
    {
        get => Effect.BurstEnabled;
        set => Effect.BurstEnabled = value;
    }

    /// <summary>
    /// Gets a value indicating whether the bursting effect is currently bursting.
    /// </summary>
    /// <remarks>
    ///     Indicates if the bursting effect is currently in its on in the on/off cycle.
    ///     True is bursting and false means it is not.
    /// </remarks>
    public bool IsCurrentlyBursting { get; private set; }

    /// <summary>
    /// Gets the list of particle in the pool.
    /// </summary>
    public ReadOnlyCollection<Particle> Particles => new ReadOnlyCollection<Particle>(this.particles.ToArray());

    /// <summary>
    /// Gets the particle effect of the pool.
    /// </summary>
    public ParticleEffect Effect { get; private set; }

    /// <summary>
    /// Gets the texture of the particles in the pool.
    /// </summary>
    public TTexture? PoolTexture { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the pool texture has been loaded.
    /// </summary>
    public bool TextureLoaded => PoolTexture != null;

    /// <summary>
    /// Updates the particle pool.
    /// </summary>
    /// <param name="timeElapsed">The amount of time that has passed since the last frame.</param>
    public void Update(TimeSpan timeElapsed)
    {
        this.spawnRateElapsed += timeElapsed.TotalMilliseconds;

        ManageBurstEffectTimings(timeElapsed);

        // If the amount of time to spawn a new particle has passed
        if (this.spawnRateElapsed >= this.spawnRate || !Effect.SpawnRateEnabled)
        {
            this.spawnRate = GetRandomSpawnRate();

            SpawnNewParticle();

            this.spawnRateElapsed = 0;
        }

        for (var i = 0; i < this.particles.Count; i++)
        {
            if (this.particles[i].IsDead)
            {
                continue;
            }

            this.particles[i].Update(timeElapsed);
        }
    }

    /// <summary>
    /// Kills all of the particles.
    /// </summary>
    public void KillAllParticles() => this.particles.ForEach(p => p.IsDead = true);

    /// <summary>
    /// Loads the texture for the pool to use for rendering the particles.
    /// </summary>
    public void LoadTexture() => PoolTexture = this.textureLoader.LoadTexture(Effect.ParticleTextureName);

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (!(obj is ParticlePool<TTexture> pool))
        {
            return false;
        }

        return TotalLivingParticles == pool.TotalLivingParticles &&
               TotalDeadParticles == pool.TotalDeadParticles &&
               this.particles.Count == pool.Particles.Count &&
               Effect == pool.Effect;
    }

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    [ExcludeFromCodeCoverage]
    public override int GetHashCode() =>
        HashCode.Combine(TotalLivingParticles.GetHashCode(), TotalDeadParticles.GetHashCode(), Effect.GetHashCode(), PoolTexture?.GetHashCode());

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="disposing">True to dispose of managed resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (this.isDisposed)
        {
            return;
        }

        // Dispose of managed resources
        if (disposing)
        {
            if (!(PoolTexture is null))
            {
                PoolTexture.Dispose();
            }

            this.isDisposed = false;
            this.spawnRate = 0;
            this.spawnRateElapsed = 0;
            this.particles = new List<Particle>();
        }

        this.isDisposed = true;
    }

    /// <summary>
    /// Manages the timings for the burst effect on and off cycle.
    /// </summary>
    /// <param name="timeElapsed">The amount of time that has passed since the last frame.</param>
    private void ManageBurstEffectTimings(TimeSpan timeElapsed)
    {
        if (!Effect.BurstEnabled)
        {
            return;
        }

        this.burstOffTimeElapsed += (int)timeElapsed.TotalMilliseconds;

        if (this.burstOffTimeElapsed >= Effect.BurstOffTime)
        {
            this.burstOnTimeElapsed += (int)timeElapsed.TotalMilliseconds;

            IsCurrentlyBursting = false;

            if (this.burstOnTimeElapsed >= Effect.BurstOnTime)
            {
                IsCurrentlyBursting = true;
                this.burstOffTimeElapsed = 0;
                this.burstOnTimeElapsed = 0;
            }
        }
    }

    /// <summary>
    /// Resets all of the particles.
    /// </summary>
    private void SpawnNewParticle()
    {
        for (var i = 0; i < this.particles.Count; i++)
        {
            if (this.particles[i].IsDead)
            {
                this.particles[i].Position = Effect.SpawnLocation;
                this.particles[i].Reset();
                break;
            }
        }
    }

    /// <summary>
    /// Returns a random time in milliseconds that the <see cref="Particle"/> will be spawned next.
    /// </summary>
    /// <returns>A randomized spawn rate.</returns>
    private int GetRandomSpawnRate()
    {
        var minRate = BurstEnabled && IsCurrentlyBursting ? Effect.BurstSpawnRateMin : Effect.SpawnRateMin;
        var maxRate = BurstEnabled && IsCurrentlyBursting ? Effect.BurstSpawnRateMax : Effect.SpawnRateMax;

        if (Effect.SpawnRateMin <= Effect.SpawnRateMax)
        {
            return this.randomService.GetValue(minRate, maxRate);
        }

        return this.randomService.GetValue(maxRate, minRate);
    }

    /// <summary>
    /// Generates all of the particles.
    /// </summary>
    private void CreateAllParticles(IBehaviorFactory behaviorFactory)
    {
        this.particles.Clear();

        for (var i = 0; i < Effect.TotalParticlesAliveAtOnce; i++)
        {
            this.particles.Add(new Particle(behaviorFactory.CreateBehaviors(Effect.BehaviorSettings.ToArray(), this.randomService)));
        }
    }
}