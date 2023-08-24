// <copyright file="VerticalMovementScene.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace PlazmaTesting.Scenes;

using System.Drawing;
using System.Numerics;
using Plazma;
using Plazma.Behaviors;
using Plazma.Factories;
using PlazmaTesting;
using Velaptor;
using Velaptor.Content;
using Velaptor.Factories;
using Velaptor.Graphics;
using Velaptor.Graphics.Renderers;
using Velaptor.Input;
using Velaptor.Scene;

/// <summary>
/// Demonstrates vertical movement of particles.
/// </summary>
public class VerticalMovementScene : SceneBase
{
    private readonly ITextureLoader<ITexture> textureLoader = new ParticleTextureLoader();
    private readonly ITextureRenderer textureRenderer;
    private readonly IAppInput<MouseState> mouse;
    private ParticleEngine<ITexture>? engine;
    private Point mousePos;

    /// <summary>
    /// Initializes a new instance of the <see cref="VerticalMovementScene"/> class.
    /// </summary>
    public VerticalMovementScene()
    {
        this.mouse = InputFactory.CreateMouse();

        var rendererFactory = new RendererFactory();
        this.textureRenderer = rendererFactory.CreateTextureRenderer();
    }

    /// <summary>
    /// Loads the content.
    /// </summary>
    public override void LoadContent()
    {
        this.engine = new ParticleEngine<ITexture>();

        var allSettings = new[]
        {
            CreateSettings(),
        };

        var effect = new ParticleEffect("drop", allSettings)
        {
            SpawnLocation = new Vector2(400, 400),
            SpawnRateMin = 62,
            SpawnRateMax = 250,
            TotalParticles = 100,
        };

        var poolFactory = new ParticlePoolFactory();
        this.engine.AddPool(poolFactory.Create(effect, this.textureLoader));
        this.engine.LoadTextures();

        base.LoadContent();
    }

    /// <summary>
    /// Unloads the content.
    /// </summary>
    public override void UnloadContent()
    {
        this.engine?.Dispose();
        this.textureLoader.Dispose();

        base.UnloadContent();
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

        base.Update(frameTime);
    }

    /// <summary>
    /// Renders the scene.
    /// </summary>
    public override void Render()
    {
        foreach (var pool in this.engine.ParticlePools)
        {
            foreach (var particle in pool.Particles)
            {
                if (particle.IsAlive is false)
                {
                    continue;
                }

                var renderPos = particle.Position;
                var renderPosX = renderPos.X;
                var renderPosY = renderPos.Y;

                var srcRect = new Rectangle(0, 0, (int)pool.PoolTexture.Width, (int)pool.PoolTexture.Height);
                var destRect = new Rectangle((int)renderPosX, (int)renderPosY, (int)pool.PoolTexture.Width, (int)pool.PoolTexture.Height);

                this.textureRenderer.Render(pool.PoolTexture, srcRect, destRect, 1f, 0f, Color.White, RenderEffects.None);
            }
        }

        base.Render();
    }

    /// <summary>
    /// Creates the settings.
    /// </summary>
    /// <returns>The new settings.</returns>
    private EasingRandomBehaviorSettings CreateSettings()
    {
        const int changeMin = 1000;
        const int changeMax = 2000;

        return new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = BehaviorAttribute.Y,
            LifeTimeMillisecondsMin = 2500,
            LifeTimeMillisecondsMax = 5000,
            RandomStartMin = WindowCenter.Y,
            RandomStartMax = WindowCenter.Y,
            RandomChangeMin = changeMin,
            RandomChangeMax = changeMax,
            UpdateRandomStartMin = (_) => this.mousePos.Y,
            UpdateRandomStartMax = (_) => this.mousePos.Y,
        };
    }
}
