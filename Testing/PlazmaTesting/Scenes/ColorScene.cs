// <copyright file="ColorScene.cs" company="KinsonDigital">
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
using Velaptor.Scene;

/// <summary>
/// Demonstrates the use of particle colors.
/// </summary>
public class ColorScene : SceneBase
{
    private const float TextureHalfWidth = 22.5f;
    private const float TextureHalfHeight = 31.5f;
    private readonly ITextureLoader<ITexture> textureLoader = new ParticleTextureLoader();
    private readonly TrueRandomizerService randomService = new ();
    private readonly ParticleEngine<ITexture> engine;
    private readonly ITextureRenderer textureRenderer;

    /// <summary>
    /// Creates a new instance of <see cref="ColorScene"/>.
    /// </summary>
    public ColorScene()
    {
        var rendererFactory = new RendererFactory();
        this.textureRenderer = rendererFactory.CreateTextureRenderer();
        this.engine = new ParticleEngine<ITexture>(this.textureLoader, this.randomService);
    }

    /// <summary>
    /// Loads the content.
    /// </summary>
    public override void LoadContent()
    {
        var behaviorFactory = new BehaviorFactory();
        var allSettings = CreateSettings();

        var effect = new ParticleEffect("drop", allSettings)
        {
            SpawnRateMin = 62,
            SpawnRateMax = 62,
            TotalParticlesAliveAtOnce = 100,
        };

        this.engine.CreatePool(effect, behaviorFactory);

        this.engine.ParticlePools[0].Effect.SpawnLocation = new Vector2(WindowSize.Width / 2f, WindowSize.Height / 2f);
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

                this.textureRenderer.Render(pool.PoolTexture, srcRect, destRect, 1f, 0, particle.TintColor, RenderEffects.None);
            }
        }

        base.Render();
    }

    /// <summary>
    /// Creates the settings.
    /// </summary>
    /// <returns>The new settings.</returns>
    private IBehaviorSettings[] CreateSettings()
    {
        var windowCenter = new Vector2(WindowSize.Width / 2f, WindowSize.Height / 2f);

        var winHalfWidth = windowCenter.X;
        var winHalfHeight = windowCenter.Y;

        var xPosSettings = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = ParticleAttribute.X,
            LifeTimeMinMilliseconds = 2000,
            LifeTimeMaxMilliseconds = 2000,
            RandomChangeMin = -winHalfWidth - TextureHalfWidth,
            RandomChangeMax = winHalfWidth + TextureHalfWidth,
            RandomStartMin = windowCenter.X,
            RandomStartMax = windowCenter.X,
        };

        var yPosSettings = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = ParticleAttribute.Y,
            LifeTimeMinMilliseconds = 2000,
            LifeTimeMaxMilliseconds = 2000,
            RandomChangeMin = -winHalfHeight - TextureHalfHeight,
            RandomChangeMax = winHalfHeight + TextureHalfHeight,
            RandomStartMin = windowCenter.Y,
            RandomStartMax = windowCenter.Y,
        };

        var white = Color.White;
        var purple = Color.MediumPurple;

        var redSettings = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = ParticleAttribute.RedColorComponent,
            LifeTimeMinMilliseconds = 1500,
            LifeTimeMaxMilliseconds = 1500,
            RandomChangeMin = purple.R - white.R,
            RandomChangeMax = purple.R - white.R,
            RandomStartMin = white.R,
            RandomStartMax = white.R,
        };

        var greenSettings = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = ParticleAttribute.GreenColorComponent,
            LifeTimeMinMilliseconds = 1500,
            LifeTimeMaxMilliseconds = 1500,
            RandomChangeMin = purple.G - white.G,
            RandomChangeMax = purple.G - white.G,
            RandomStartMin = white.G,
            RandomStartMax = white.G,
        };

        var blueSettings = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = ParticleAttribute.BlueColorComponent,
            LifeTimeMinMilliseconds = 1500,
            LifeTimeMaxMilliseconds = 1500,
            RandomChangeMin = purple.B - white.B,
            RandomChangeMax = purple.B - white.B,
            RandomStartMin = white.B,
            RandomStartMax = white.B,
        };

        return new IBehaviorSettings[]
        {
            xPosSettings,
            yPosSettings,
            redSettings,
            greenSettings,
            blueSettings,
        };
    }
}
