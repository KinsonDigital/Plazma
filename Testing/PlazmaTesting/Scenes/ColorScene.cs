// <copyright file="ColorScene.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace PlazmaTesting.Scenes;

using System.Drawing;
using System.Globalization;
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
using Velaptor.UI;

/// <summary>
/// Demonstrates the use of particle colors.
/// </summary>
public class ColorScene : SceneBase
{
    private readonly ITextureLoader<ITexture> textureLoader = new ParticleTextureLoader();
    private readonly ITextureRenderer textureRenderer;
    private readonly IAppInput<MouseState> mouse;
    private ParticleEngine<ITexture>? engine;
    private Label? lblInstructions;
    private Label? lblSpread;
    private float spread;

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorScene"/> class.
    /// </summary>
    public ColorScene()
    {
        this.mouse = HardwareFactory.GetMouse();
        var rendererFactory = new RendererFactory();
        this.textureRenderer = rendererFactory.CreateTextureRenderer();
    }

    /// <summary>
    /// Loads the content.
    /// </summary>
    public override void LoadContent()
    {
        this.spread = WindowSize.Height / 2f;
        this.engine = new ParticleEngine<ITexture>();
        var allSettings = CreateSettings();

        var effect = new ParticleEffect("drop", allSettings)
        {
            SpawnRateMin = 62,
            SpawnRateMax = 62,
            TotalParticles = 300,
        };

        var poolFactory = new ParticlePoolFactory();
        this.engine.AddPool(poolFactory.Create(effect, this.textureLoader));

        this.engine.ParticlePools[0].Effect.SpawnLocation = new Vector2(WindowSize.Width / 2f, WindowSize.Height / 2f);
        this.engine.LoadTextures();

        this.lblInstructions = new Label
        {
            Color = Color.White,
            Text = "Scroll up to cluster and down to spread.",
            Position = new Point(WindowCenter.X, 50),
        };

        this.lblSpread = new Label
        {
            Color = Color.White,
            Text = $"Spread: {this.spread.ToString(CultureInfo.InvariantCulture)}",
            Position = new Point(WindowCenter.X, 100),
        };

        AddControl(this.lblInstructions);
        AddControl(this.lblSpread);
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
        var mouseState = this.mouse.GetState();

        this.spread = mouseState.GetScrollDirection() switch
        {
            MouseScrollDirection.ScrollUp => this.spread - 50 <= 0 ? 0 : this.spread - 50,
            MouseScrollDirection.ScrollDown => this.spread + 50 >= WindowSize.Height / 2f ? WindowSize.Height / 2f : this.spread + 50,
            _ => this.spread,
        };

        this.lblSpread.Text = $"Spread: {this.spread.ToString(CultureInfo.InvariantCulture)}";

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
                this.textureRenderer.Render(pool.PoolTexture, srcRect, destRect, particle.Size, 0, tintClr, RenderEffects.None);
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

        const int change = 400;

        var xPosSettings = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = BehaviorAttribute.X,
            LifeTimeMillisecondsMin = 2000,
            LifeTimeMillisecondsMax = 2000,
            RandomStartMin = windowCenter.X - this.spread,
            RandomStartMax = windowCenter.X + this.spread,
            UpdateRandomStartMin = (_) => windowCenter.X - this.spread,
            UpdateRandomStartMax = (_) => windowCenter.X + this.spread,
            RandomChangeMin = -change,
            RandomChangeMax = change,
        };

        var yPosSettings = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = BehaviorAttribute.Y,
            LifeTimeMillisecondsMin = 2000,
            LifeTimeMillisecondsMax = 2000,
            RandomStartMin = windowCenter.Y - this.spread,
            RandomStartMax = windowCenter.Y + this.spread,
            UpdateRandomStartMin = (_) => windowCenter.Y - this.spread,
            UpdateRandomStartMax = (_) => windowCenter.Y + this.spread,
            RandomChangeMin = -change,
            RandomChangeMax = change,
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
            RandomChangeMin = -255f,
            RandomChangeMax = -255f,
        };

        const float clrChangeTime = 1000f;
        var redSettings = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = BehaviorAttribute.RedColorComponent,
            LifeTimeMillisecondsMin = clrChangeTime,
            LifeTimeMillisecondsMax = clrChangeTime,
            RandomStartMin = white.R,
            RandomStartMax = white.R,
            RandomChangeMin = purple.R - white.R,
            RandomChangeMax = purple.R - white.R,
        };

        var greenSettings = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = BehaviorAttribute.GreenColorComponent,
            LifeTimeMillisecondsMin = clrChangeTime,
            LifeTimeMillisecondsMax = clrChangeTime,
            RandomStartMin = white.G,
            RandomStartMax = white.G,
            RandomChangeMin = purple.G - white.G,
            RandomChangeMax = purple.G - white.G,
        };

        var blueSettings = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = BehaviorAttribute.BlueColorComponent,
            LifeTimeMillisecondsMin = clrChangeTime,
            LifeTimeMillisecondsMax = clrChangeTime,
            RandomStartMin = white.B,
            RandomStartMax = white.B,
            RandomChangeMin = purple.B - white.B,
            RandomChangeMax = purple.B - white.B,
        };

        var sizeSettings = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = BehaviorAttribute.Size,
            LifeTimeMillisecondsMin = 5000,
            LifeTimeMillisecondsMax = 5000,
            RandomStartMin = 0.3f,
            RandomStartMax = 0.6f,
            RandomChangeMin = -1f,
            RandomChangeMax = -1f,
            UpdateValue = (value) => value <= 0.0 ? 0.0 : value,
        };

        return new[]
        {
            xPosSettings,
            yPosSettings,
            alphaSettings,
            redSettings,
            greenSettings,
            blueSettings,
            sizeSettings,
        };
    }
}
