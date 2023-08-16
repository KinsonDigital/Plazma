// <copyright file="ColorScene.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace PlazmaTesting.Scenes;

using System.Drawing;
using System.Numerics;
using Plazma;
using Plazma.Behaviors;
using Plazma.Factories;
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
    private readonly ParticleEngine<ITexture> engine;
    private readonly ITextureRenderer textureRenderer;

    /// <summary>
    /// Creates a new instance of <see cref="ColorScene"/>.
    /// </summary>
    public ColorScene()
    {
        var rendererFactory = new RendererFactory();
        this.textureRenderer = rendererFactory.CreateTextureRenderer();
        this.engine = new ParticleEngine<ITexture>();
    }

    /// <summary>
    /// Loads the content.
    /// </summary>
    public override void LoadContent()
    {
        var allSettings = CreateSettings();

        var effect = new ParticleEffect("drop", allSettings)
        {
            SpawnRateMin = 62,
            SpawnRateMax = 62,
            TotalParticles = 100,
        };

        var poolFactory = new ParticlePoolFactory();
        this.engine.AddPool(poolFactory.Create(effect, this.textureLoader));

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

                var tintClr = particle.TintColor;
                this.textureRenderer.Render(pool.PoolTexture, srcRect, destRect, 1f, 0, tintClr, RenderEffects.None);
            }
        }

        base.Render();
    }

    /// <summary>
    /// Creates the settings.
    /// </summary>
    /// <returns>The new settings.</returns>
    private EasingRandomBehaviorSettings[] CreateSettings()
    {
        var windowCenter = new Vector2(WindowSize.Width / 2f, WindowSize.Height / 2f);

        var winHalfWidth = windowCenter.X;
        var winHalfHeight = windowCenter.Y;

        var xPosSettings = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = BehaviorAttribute.X,
            LifeTimeMillisecondsMin = 2000,
            LifeTimeMillisecondsMax = 2000,
            RandomStartMin = windowCenter.X,
            RandomStartMax = windowCenter.X,
            RandomStopMin = -winHalfWidth - TextureHalfWidth,
            RandomStopMax = winHalfWidth + TextureHalfWidth,
        };

        var yPosSettings = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = BehaviorAttribute.Y,
            LifeTimeMillisecondsMin = 2000,
            LifeTimeMillisecondsMax = 2000,
            RandomStartMin = windowCenter.Y,
            RandomStartMax = windowCenter.Y,
            RandomStopMin = -winHalfHeight - TextureHalfHeight,
            RandomStopMax = winHalfHeight + TextureHalfHeight,
        };

        var white = Color.White;
        var purple = Color.MediumPurple;

        var alphaSettings = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = BehaviorAttribute.AlphaColorComponent,
            LifeTimeMillisecondsMin = 2000,
            LifeTimeMillisecondsMax = 2000,
            RandomStartMin = 255,
            RandomStartMax = 255,
            RandomStopMin = -255f,
            RandomStopMax = -255f,
        };

        var redSettings = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = BehaviorAttribute.RedColorComponent,
            LifeTimeMillisecondsMin = 1500,
            LifeTimeMillisecondsMax = 1500,
            RandomStartMin = white.R,
            RandomStartMax = white.R,
            RandomStopMin = purple.R - white.R,
            RandomStopMax = purple.R - white.R,
        };

        var greenSettings = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = BehaviorAttribute.GreenColorComponent,
            LifeTimeMillisecondsMin = 1500,
            LifeTimeMillisecondsMax = 1500,
            RandomStartMin = white.G,
            RandomStartMax = white.G,
            RandomStopMin = purple.G - white.G,
            RandomStopMax = purple.G - white.G,
        };

        var blueSettings = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = BehaviorAttribute.BlueColorComponent,
            LifeTimeMillisecondsMin = 1500,
            LifeTimeMillisecondsMax = 1500,
            RandomStartMin = white.B,
            RandomStartMax = white.B,
            RandomStopMin = purple.B - white.B,
            RandomStopMax = purple.B - white.B,
        };

        return new []
        {
            xPosSettings,
            yPosSettings,
            alphaSettings,
            redSettings,
            greenSettings,
            blueSettings,
        };
    }
}
