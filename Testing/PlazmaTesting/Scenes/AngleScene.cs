// <copyright file="AngleScene.cs" company="KinsonDigital">
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
using Velaptor.Input;
using Velaptor.Scene;

/// <summary>
/// Demonstrates the use of particle angles.
/// </summary>
public class AngleScene : SceneBase
{
    private const float TextureHalfWidth = 22.5f;
    private const float TextureHalfHeight = 31.5f;
    private readonly ITextureLoader<ITexture> textureLoader = new ParticleTextureLoader();
    private readonly ITextureRenderer textureRenderer;
    private readonly IAppInput<MouseState> mouse;
    private ParticleEngine<ITexture>? engine;
    private MouseState prevMouseState;

    /// <summary>
    /// Initializes a new instance of the <see cref="AngleScene"/> class.
    /// </summary>
    public AngleScene()
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

        var allSettings = CreateSettings();

        var effect = new ParticleEffect("drop", allSettings)
        {
            SpawnRateMin = 125,
            SpawnRateMax = 125,
            TotalParticles = 50,
        };

        var poolFactory = new ParticlePoolFactory();
        this.engine.AddPool(poolFactory.Create(effect, this.textureLoader));

        this.engine.ParticlePools[0].Effect.SpawnLocation = new Vector2(WindowSize.Width / 2f, WindowSize.Height / 2f);
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
        ProcessMouseInput();
        this.engine.Update(frameTime.ElapsedTime);

        base.Update(frameTime);
    }

    /// <summary>
    /// Renders the scene.
    /// </summary>
    public override void Render()
    {
        var clr = Color.LightGreen.DecreaseBrightness(0.2f);

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

                this.textureRenderer.Render(pool.PoolTexture, srcRect, destRect, 1f, -particle.Angle, clr, RenderEffects.None);
            }
        }

        base.Render();
    }

    /// <summary>
    /// Processes mouse input.
    /// </summary>
    private void ProcessMouseInput()
    {
        var mouseState = this.mouse.GetState();

        var mouseNotOverButtons = !MainWindow.ButtonsArea.Contains(mouseState.GetPosition());

        if (this.prevMouseState.IsLeftButtonDown() && mouseState.IsLeftButtonUp() && mouseNotOverButtons)
        {
            this.engine.Enabled = !this.engine.Enabled;
        }

        this.prevMouseState = mouseState;
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
            RandomChangeMin = -winHalfWidth - TextureHalfWidth,
            RandomChangeMax = winHalfWidth + TextureHalfWidth,
        };

        var yPosSettings = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = BehaviorAttribute.Y,
            LifeTimeMillisecondsMin = 2000,
            LifeTimeMillisecondsMax = 2000,
            RandomStartMin = windowCenter.Y,
            RandomStartMax = windowCenter.Y,
            RandomChangeMin = -winHalfHeight - TextureHalfHeight,
            RandomChangeMax = winHalfHeight + TextureHalfHeight,
        };

        var angleSettings = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = BehaviorAttribute.Angle,
            LifeTimeMillisecondsMin = 2000,
            LifeTimeMillisecondsMax = 2000,
            RandomStartMin = 0,
            RandomStartMax = 0,
            RandomChangeMin = 360,
            RandomChangeMax = 360,
        };

        return new[]
        {
            xPosSettings,
            yPosSettings,
            angleSettings,
        };
    }
}
