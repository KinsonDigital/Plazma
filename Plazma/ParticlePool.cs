// <copyright file="ParticlePool.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

#pragma warning disable CA1303 // Do not pass literals as localized parameters
namespace Plazma;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel;
using System.Drawing;
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
    private readonly List<Particle> particles = [];
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
    /// <param name="effect">The particle effect to be applied to all the particles in the pool.</param>
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
    public ImmutableArray<Particle> Particles => [..this.particles];

    /// <inheritdoc/>
    public ParticleEffect Effect { get; }

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

        for (var i = 0; i < this.particles.Count; i++)
        {
            if (this.particles[i].IsAlive)
            {
                this.particles[i] = UpdateParticle(this.particles[i], timeElapsed);
            }
        }
    }

    /// <inheritdoc/>
    public void KillAllParticles()
    {
        for (var i = 0; i < this.particles.Count; i++)
        {
            this.particles[i] = this.particles[i] with { IsAlive = false };
        }
    }

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
    /// Updates the given <paramref name="particle"/> and returns it updated.
    /// </summary>
    /// <param name="particle">The particle to update.</param>
    /// <param name="timeElapsed">The amount of time that has passed since the last frame.</param>
    /// <returns>The updated particle.</returns>
    /// <exception cref="InvalidEnumArgumentException">
    ///     Thrown if the particle behavior attribute is an invalid enumeration value.
    /// </exception>
    private Particle UpdateParticle(Particle particle, TimeSpan timeElapsed)
    {
        particle = particle with { IsAlive = false };

        // Apply the behavior values to the particle attributes
        foreach (var behavior in particle.Behaviors)
        {
            if (behavior.Enabled)
            {
                behavior.Update(timeElapsed);
                particle = particle with { IsAlive = true };

                var value = (float)behavior.Value;

                switch (behavior.BehaviorType)
                {
                    case BehaviorAttribute.X:
                        particle = particle with { Position = particle.Position with { X = value } };
                        break;
                    case BehaviorAttribute.Y:
                        particle = particle with { Position = particle.Position with { Y = value } };
                        break;
                    case BehaviorAttribute.Angle:
                        particle = particle with { Angle = value };
                        break;
                    case BehaviorAttribute.Size:
                        particle = particle with { Size = value };
                        break;
                    case BehaviorAttribute.AlphaColorComponent:
                        particle = particle with
                        {
                            TintColor = Color.FromArgb(
                                ClampClrValue(value),
                                particle.TintColor.R,
                                particle.TintColor.G,
                                particle.TintColor.B)
                        };
                        break;
                    case BehaviorAttribute.RedColorComponent:
                        particle = particle with
                        {
                            TintColor = Color.FromArgb(
                                particle.TintColor.A,
                                ClampClrValue(value),
                                particle.TintColor.G,
                                particle.TintColor.B)
                        };
                        break;
                    case BehaviorAttribute.GreenColorComponent:
                        particle = particle with
                        {
                            TintColor = Color.FromArgb(
                                particle.TintColor.A,
                                particle.TintColor.R,
                                ClampClrValue(value),
                                particle.TintColor.B)
                        };
                        break;
                    case BehaviorAttribute.BlueColorComponent:
                        particle = particle with
                        {
                            TintColor = Color.FromArgb(
                                particle.TintColor.A,
                                particle.TintColor.R,
                                particle.TintColor.G,
                                ClampClrValue(value))
                        };
                        break;
                    default:
                        throw new InvalidEnumArgumentException(nameof(BehaviorAttribute), (int)behavior.BehaviorType, typeof(BehaviorAttribute));
                }
            }
        }

        return particle;

        static byte ClampClrValue(float value)
        {
            return (byte)(value < 0 ? 0 : value);
        }
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
    /// Resets all the particles.
    /// </summary>
    private void SpawnNewParticle()
    {
        for (var i = 0; i < this.particles.Count; i++)
        {
            if (this.particles[i].IsAlive)
            {
                continue;
            }

            foreach (var behavior in this.particles[i].Behaviors)
            {
                behavior.Reset();
            }

            this.particles[i] = this.particles[i] with
            {
                Size = 1,
                Angle = 0,
                TintColor = Color.White,
                IsAlive = true,
            };

            this.particles[i] = this.particles[i] with { Position = Effect.SpawnLocation };

            this.LivingParticlesCountChanged?.Invoke(this, EventArgs.Empty);

            break;
        }
    }

    /// <summary>
    /// Returns a random time in milliseconds that a <see cref="Particle"/> will be spawned next.
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
    /// Generates all the particles.
    /// </summary>
    private void CreateAllParticles()
    {
        this.particles.Clear();

        for (var i = 0; i < Effect.TotalParticles; i++)
        {
            var behaviors = new List<IBehavior>();

            foreach (var settings in Effect.BehaviorSettings)
            {
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
            this.textureLoader.Dispose();
            this.particles.Clear();
        }

        this.isDisposed = true;
    }
}
