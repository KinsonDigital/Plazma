// <copyright file="HorizontalMovementScene.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace PlazmaTesting.Scenes;

using System.Drawing;
using System.Numerics;
using Plazma;
using Plazma.Behaviors;
using Plazma.Services;
using Velaptor;
using Velaptor.Content;
using Velaptor.Factories;
using Velaptor.Graphics;
using Velaptor.Graphics.Renderers;
using Velaptor.Input;
using Velaptor.Scene;

/// <summary>
/// Demonstrates horizontal movement of particles.
/// </summary>
public class HorizontalMovementScene : SceneBase
{
    private readonly ITextureLoader<ITexture> textureLoader = new ParticleTextureLoader();
    private readonly TrueRandomizerService randomService = new ();
    private readonly ParticleEngine<ITexture> engine;
    private readonly ITextureRenderer textureRenderer;
    private readonly IAppInput<MouseState> mouse;
    private Point mousePos;
    private MouseState previousMouseState;

    /// <summary>
    /// Creates a new instance of <see cref="HorizontalMovementScene"/>.
    /// </summary>
    public HorizontalMovementScene()
    {
        this.mouse = InputFactory.CreateMouse();

        var rendererFactory = new RendererFactory();
        this.textureRenderer = rendererFactory.CreateTextureRenderer();

        this.engine = new ParticleEngine<ITexture>(this.textureLoader, this.randomService);

        var allSettings = new BehaviorSettings[]
        {
            CreateSettings(),
        };

        var behaviorFactory = new BehaviorFactory();

        var effect = new ParticleEffect("drop", allSettings)
        {
            SpawnLocation = new Vector2(400, 400),
            SpawnRateMin = 62,
            SpawnRateMax = 250,
            TotalParticlesAliveAtOnce = 100,
        };

        this.engine.CreatePool(effect, behaviorFactory);
    }

    /// <summary>
    /// Loads the content.
    /// </summary>
    public override void LoadContent()
    {
        this.engine.LoadTextures();

        base.LoadContent();
    }

    /// <summary>
    /// Updates the scene.
    /// </summary>
    /// <param name="frameTime">The time passed for the current frame.</param>
    public override void Update(FrameTime frameTime)
    {
        this.engine.Update(frameTime.ElapsedTime);

        var mouseState = this.mouse.GetState();
        this.mousePos = mouseState.GetPosition();

        this.engine.ParticlePools[0].Effect.SpawnLocation = new Vector2(this.mousePos.X, this.mousePos.Y);

        this.previousMouseState = mouseState;
        base.Update(frameTime);
    }

    /// <summary>
    /// Renders the scene.
    /// </summary>
    public override void Render()
    {
        foreach (ParticlePool<ITexture> pool in this.engine.ParticlePools)
        {
            foreach (Particle particle in pool.Particles)
            {
                if (particle.IsDead)
                {
                    continue;
                }

                var renderPos = particle.Position;
                var renderPosX = renderPos.X;
                var renderPosY = renderPos.Y;

                var srcRect = new Rectangle(0, 0, (int)pool.PoolTexture.Width, (int)pool.PoolTexture.Height);
                var destRect = new Rectangle((int)renderPosX, (int)renderPosY, (int)pool.PoolTexture.Width, (int)pool.PoolTexture.Height);

                this.textureRenderer.Render(pool.PoolTexture, srcRect, destRect, 1f, 270f, Color.White, RenderEffects.None);
            }
        }

        base.Render();
    }

    /// <summary>
    /// Creates the settings.
    /// </summary>
    /// <returns>The new settings.</returns>
    private BehaviorSettings CreateSettings()
    {
        const int changeMin = 1000;
        const int changeMax = 2000;
        const int startMin = 200;
        const int startMax = 400;

        return new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = ParticleAttribute.X,
            LifeTimeMinMilliseconds = 3500,
            LifeTimeMaxMilliseconds = 7000,
            RandomChangeMin = changeMin,
            RandomChangeMax = changeMax,
            RandomStartMin = startMin,
            RandomStartMax = startMax,
            UpdateRandomStartMin = () => this.mousePos.X,
            UpdateRandomStartMax = () => this.mousePos.X,
        };
    }
}
