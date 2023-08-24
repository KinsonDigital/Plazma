// <copyright file="ParticlePool.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

#pragma warning disable CA1303 // Do not pass literals as localized parameters
namespace Plazma;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Behaviors;
using Factories;
using Services;

/// <inheritdoc/>
public sealed class ParticlePool<TTexture> : IParticlePool<TTexture>
    where TTexture : class
{
    private readonly IRandomizerService randomService;
    private readonly ITextureLoader<TTexture> textureLoader;
    private readonly IBehaviorFactory behaviorFactory;
    private readonly IParticleFactory particleFactory;
    private readonly List<IParticle> particles = new ();
    private int spawnRate;
    private double spawnRateElapsed;
    private int burstOnTimeElapsed;
    private int burstOffTimeElapsed;
    private bool isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="ParticlePool{TTexture}"/> class.
    /// </summary>
    /// <param name="effect">The particle effect for all particles in the pool.</param>
    /// <param name="textureLoader">Loads the textures for the particle pool.</param>
    [ExcludeFromCodeCoverage(Justification = "Uses non-testable IoC container.")]
    public ParticlePool(ParticleEffect effect, ITextureLoader<TTexture> textureLoader)
    {
        ArgumentNullException.ThrowIfNull(effect);
        ArgumentNullException.ThrowIfNull(textureLoader);

        Effect = effect;
        this.textureLoader = textureLoader;
        this.randomService = IoC.Container.GetInstance<IRandomizerService>();
        this.behaviorFactory = IoC.Container.GetInstance<IBehaviorFactory>();
        this.particleFactory = IoC.Container.GetInstance<IParticleFactory>();

        CreateAllParticles();
        this.spawnRate = GetRandomSpawnRate();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ParticlePool{Texture}"/> class.
    /// </summary>
    /// <param name="textureLoader">Loads the textures for the <see cref="ParticlePool{Texture}"/>.</param>
    /// <param name="randomizer">Used for generating random values when a particle is spawned.</param>
    /// <param name="behaviorFactory">Creates behaviors.</param>
    /// <param name="particleFactory">Creates particles.</param>
    /// <param name="effect">The particle effect to be applied to all of the particles in the pool.</param>
    internal ParticlePool(
        ITextureLoader<TTexture> textureLoader,
        IRandomizerService randomizer,
        IBehaviorFactory behaviorFactory,
        IParticleFactory particleFactory,
        ParticleEffect effect)
    {
        ArgumentNullException.ThrowIfNull(textureLoader);
        ArgumentNullException.ThrowIfNull(randomizer);
        ArgumentNullException.ThrowIfNull(behaviorFactory);
        ArgumentNullException.ThrowIfNull(particleFactory);
        ArgumentNullException.ThrowIfNull(effect);

        this.textureLoader = textureLoader;
        this.randomService = randomizer;
        this.behaviorFactory = behaviorFactory;
        this.particleFactory = particleFactory;
        Effect = effect;

        CreateAllParticles();
        this.spawnRate = GetRandomSpawnRate();
    }

    /// <inheritdoc/>
    [SuppressMessage("ReSharper", "EventNeverSubscribedTo.Global", Justification = "Part of the public API.")]
    public event EventHandler<EventArgs>? LivingParticlesCountChanged;

    /// <inheritdoc/>
    public int TotalLivingParticles => this.particles.Count(p => p.IsAlive);

    /// <inheritdoc/>
    public int TotalDeadParticles => this.particles.Count(p => p.IsAlive is false);

    /// <inheritdoc/>
    public bool LimitSpawnRate
    {
        get => Effect.LimitSpawnRate;
        set => Effect.LimitSpawnRate = value;
    }

    /// <inheritdoc/>
    public bool BurstEnabled
    {
        get => Effect.BurstEnabled;
        set => Effect.BurstEnabled = value;
    }

    /// <inheritdoc/>
    public bool InBurstMode { get; set; }

    /// <inheritdoc/>
    public ImmutableArray<IParticle> Particles => this.particles.ToImmutableArray();

    /// <inheritdoc/>
    public ParticleEffect Effect { get; private set; }

    /// <inheritdoc/>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Part of the public API.")]
    public TTexture? PoolTexture { get; private set; }

    /// <inheritdoc/>
    public bool TextureLoaded => PoolTexture != null;

    /// <inheritdoc/>
    public void Update(TimeSpan timeElapsed)
    {
        this.spawnRateElapsed += timeElapsed.TotalMilliseconds;

        ManageBurstEffectTimings(timeElapsed);

        // If the amount of time to spawn a new particle has passed
        if (Effect.LimitSpawnRate is false || this.spawnRateElapsed >= this.spawnRate)
        {
            this.spawnRate = GetRandomSpawnRate();

            SpawnNewParticle();

            this.spawnRateElapsed = 0;
        }

        foreach (var t in this.particles)
        {
            if (t.IsAlive)
            {
                t.Update(timeElapsed);
            }
        }
    }

    /// <inheritdoc/>
    public void KillAllParticles() => this.particles.ForEach(p => p.IsAlive = false);

    /// <inheritdoc/>
    public void LoadTexture() => PoolTexture = this.textureLoader.LoadTexture(Effect.ParticleTextureName);

    /// <inheritdoc/>
    public void AddBehavior(EasingRandomBehaviorSettings behaviorSettings)
    {
        foreach (var particle in Particles)
        {
            particle.AddBehavior(this.behaviorFactory.CreateEasingRandomBehavior(behaviorSettings));
        }
    }

    /// <inheritdoc/>
    public void RemoveBehavior(BehaviorAttribute behaviorType)
    {
        foreach (var particle in Particles)
        {
            particle.RemoveBehavior(behaviorType);
        }
    }

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose() => Dispose(true);

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

        if (this.burstOffTimeElapsed >= Effect.BurstOffMilliseconds)
        {
            this.burstOnTimeElapsed += (int)timeElapsed.TotalMilliseconds;

            InBurstMode = false;

            if (this.burstOnTimeElapsed >= Effect.BurstOnMilliseconds)
            {
                InBurstMode = true;
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
        foreach (var t in this.particles)
        {
            if (t.IsAlive)
            {
                continue;
            }

            t.Reset();
            t.Position = Effect.SpawnLocation;

            this.LivingParticlesCountChanged?.Invoke(this, EventArgs.Empty);

            break;
        }
    }

    /// <summary>
    /// Returns a random time in milliseconds that the <see cref="Particle"/> will be spawned next.
    /// </summary>
    /// <returns>A randomized spawn rate.</returns>
    private int GetRandomSpawnRate()
    {
        var minRate = BurstEnabled && InBurstMode ? Effect.BurstSpawnRateMin : Effect.SpawnRateMin;
        var maxRate = BurstEnabled && InBurstMode ? Effect.BurstSpawnRateMax : Effect.SpawnRateMax;

        return Effect.SpawnRateMin <= Effect.SpawnRateMax
            ? this.randomService.GetValue(minRate, maxRate)
            : this.randomService.GetValue(maxRate, minRate);
    }

    /// <summary>
    /// Generates all of the particles.
    /// </summary>
    private void CreateAllParticles()
    {
        this.particles.Clear();

        for (var i = 0; i < Effect.TotalParticles; i++)
        {
            var behaviors = new List<IBehavior>();

            for (var s = 0; s < Effect.BehaviorSettings.Count; s++)
            {
                var settings = Effect.BehaviorSettings[s];
                var newBehavior = this.behaviorFactory.CreateEasingRandomBehavior(settings);
                behaviors.Add(newBehavior);
            }

            var newParticle = this.particleFactory.Create(behaviors.ToArray());
            this.particles.Add(newParticle);
        }
    }

    /// <summary>
    /// <inheritdoc cref="IDisposable.Dispose"/>
    /// </summary>
    /// <param name="disposing">True to dispose of managed resources.</param>
    private void Dispose(bool disposing)
    {
        if (this.isDisposed)
        {
            return;
        }

        if (disposing)
        {
            this.randomService.Dispose();
            this.textureLoader.Dispose();
            this.particles.Clear();
        }

        this.isDisposed = true;
    }
}
