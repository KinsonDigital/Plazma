// <copyright file="AllAttributesScene.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace PlazmaTesting.Scenes;

using System.Drawing;
using System.Numerics;
using CustomControls;
using Plazma;
using Plazma.Behaviors;
using Plazma.Factories;
using Velaptor;
using Velaptor.Content;
using Velaptor.Factories;
using Velaptor.Graphics;
using Velaptor.Graphics.Renderers;
using Velaptor.Scene;

public class AllAttributesScene : SceneBase
{
    private const int Spacing = 15;
    private readonly ITextureRenderer textureRenderer;
    private readonly ITextureLoader<ITexture> textureLoader = new ParticleTextureLoader();
    private ParticleEngine<ITexture>? engine;
    private MinMax? alphaStartMinMax;
    private MinMax? redStartMinMax;
    private MinMax? greenStartMinMax;
    private MinMax? blueStartMinMax;
    private MinMax? alphaEndMinMax;
    private MinMax? redEndMinMax;
    private MinMax? greenEndMinMax;
    private MinMax? blueEndMinMax;
    private MinMax? angleStartMinMax;
    private MinMax? angleEndMinMax;

    /// <summary>
    /// Initializes a new instance of the <see cref="AllAttributesScene"/> class.
    /// </summary>
    public AllAttributesScene() => this.textureRenderer = RendererFactory.CreateTextureRenderer();

    /// <summary>
    /// Initializes a new instance of the <see cref="AllAttributesScene"/> class.
    /// </summary>
    /// <summary>
    /// Loads the content.
    /// </summary>
    public override void LoadContent()
    {
        if (IsLoaded)
        {
            return;
        }

        var allSettings = CreateSettings();

        var effect = new ParticleEffect("drop", allSettings)
        {
            SpawnRateMin = 125,
            SpawnRateMax = 125,
            TotalParticles = 50,
            SpawnLocation = new Vector2(WindowSize.Width / 2f, WindowSize.Height / 2f),
        };

        var poolFactory = new ParticlePoolFactory();
        this.engine = new ParticleEngine<ITexture>();
        this.engine.AddPool(poolFactory.Create(effect, this.textureLoader));

        this.engine.ParticlePools[0].Effect = this.engine.ParticlePools[0].Effect with
        {
            SpawnLocation = new Vector2(WindowSize.Width / 2f, WindowSize.Height / 2f)
        };
        this.engine.LoadTextures();

        CreateUi();

        base.LoadContent();
    }

    /// <summary>
    /// Unloads the content.
    /// </summary>
    public override void UnloadContent()
    {
        if (!IsLoaded)
        {
            return;
        }

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

        PositionUi();

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
                if (!particle.IsAlive)
                {
                    continue;
                }

                var renderPos = particle.Position;
                var renderPosX = renderPos.X;
                var renderPosY = renderPos.Y;

                var srcRect = new Rectangle(0, 0, (int)pool.PoolTexture.Width, (int)pool.PoolTexture.Height);
                var destRect = new Rectangle((int)renderPosX, (int)renderPosY, (int)pool.PoolTexture.Width, (int)pool.PoolTexture.Height);

                this.textureRenderer.Render(pool.PoolTexture, srcRect, destRect, 1f, -particle.Angle, particle.TintColor, RenderEffects.None);
            }
        }

        this.alphaStartMinMax.Render();
        this.alphaEndMinMax.Render();

        this.redStartMinMax.Render();
        this.redEndMinMax.Render();

        this.greenStartMinMax.Render();
        this.greenEndMinMax.Render();

        this.blueStartMinMax.Render();
        this.blueEndMinMax.Render();

        this.angleStartMinMax.Render();
        this.angleEndMinMax.Render();

        base.Render();
    }

    private void CreateUi()
    {
        CreateColorUi();

        this.angleStartMinMax = new MinMax("Angle Start");
        this.angleStartMinMax.Min = 0;
        this.angleStartMinMax.Max = 360;
        this.angleStartMinMax.Value = 0;

        this.angleEndMinMax = new MinMax("Angle End");
        this.angleEndMinMax.Min = 0;
        this.angleEndMinMax.Max = 360;

        Render();
    }

    private void CreateColorUi()
    {
        this.alphaStartMinMax = new MinMax("Alpha Start");
        this.alphaStartMinMax.Min = 0;
        this.alphaStartMinMax.Max = 255;
        this.alphaStartMinMax.MinValue = 255;
        this.alphaStartMinMax.MaxValue = 255;
        this.alphaStartMinMax.Value = 255;

        this.alphaEndMinMax = new MinMax("Alpha End");
        this.alphaEndMinMax.Min = 0;
        this.alphaEndMinMax.Max = 255;
        this.alphaEndMinMax.MinValue = 255;
        this.alphaEndMinMax.MaxValue = 255;

        this.redStartMinMax = new MinMax("Red Start");
        this.redStartMinMax.Min = 0;
        this.redStartMinMax.Max = 255;
        this.redStartMinMax.MinValue = 255;
        this.redStartMinMax.MaxValue = 255;
        this.redStartMinMax.Value = 255;

        this.redEndMinMax = new MinMax("Red End");
        this.redEndMinMax.Min = 0;
        this.redEndMinMax.Max = 255;
        this.redEndMinMax.MinValue = 0;
        this.redEndMinMax.MaxValue = 0;
        this.redEndMinMax.Value = 0;

        this.greenStartMinMax = new MinMax("Green Start");
        this.greenStartMinMax.Min = 0;
        this.greenStartMinMax.Max = 255;
        this.greenStartMinMax.MinValue = 0;
        this.greenStartMinMax.MaxValue = 0;
        this.greenStartMinMax.Value = 255;

        this.greenEndMinMax = new MinMax("Green End");
        this.greenEndMinMax.Min = 0;
        this.greenEndMinMax.Max = 255;
        this.greenEndMinMax.MinValue = 0;
        this.greenEndMinMax.MaxValue = 0;
        this.greenEndMinMax.Value = 0;

        this.blueStartMinMax = new MinMax("Blue Start");
        this.blueStartMinMax.Min = 0;
        this.blueStartMinMax.Max = 255;
        this.blueStartMinMax.MinValue = 0;
        this.blueStartMinMax.MaxValue = 0;
        this.blueStartMinMax.Value = 255;

        this.blueEndMinMax = new MinMax("Blue End");
        this.blueEndMinMax.Min = 0;
        this.blueEndMinMax.Max = 255;
        this.blueEndMinMax.MinValue = 255;
        this.blueEndMinMax.MaxValue = 255;
        this.blueEndMinMax.Value = 255;
    }

    private void PositionUi()
    {
        // Color Starts
        this.alphaStartMinMax.Position = new Point(Spacing, Spacing);
        this.redStartMinMax.Position = new Point(Spacing, (int)this.alphaStartMinMax.Bottom + Spacing);
        this.greenStartMinMax.Position = new Point(Spacing, (int)this.redStartMinMax.Bottom + Spacing);
        this.blueStartMinMax.Position = new Point(Spacing, (int)this.greenStartMinMax.Bottom + Spacing);

        // Color Ends
        this.alphaEndMinMax.Position = new Point((int)this.alphaStartMinMax.Right + Spacing, (int)this.alphaStartMinMax.Top);
        this.redEndMinMax.Position = new Point((int)this.redStartMinMax.Right + Spacing, (int)this.alphaEndMinMax.Bottom + Spacing);
        this.greenEndMinMax.Position = new Point((int)this.greenStartMinMax.Right + Spacing, (int)this.redEndMinMax.Bottom + Spacing);
        this.blueEndMinMax.Position = new Point((int)this.blueStartMinMax.Right + Spacing, (int)this.greenEndMinMax.Bottom + Spacing);

        // Angle
        this.angleStartMinMax.Position = new Point((int)WindowSize.Width - ((int)this.angleStartMinMax.Width + Spacing), Spacing);
        this.angleEndMinMax.Position = new Point((int)WindowSize.Width - ((int)this.angleEndMinMax.Width + Spacing), (int)this.angleStartMinMax.Bottom + Spacing);
    }

    private EasingRandomBehaviorSettings[] CreateSettings()
    {
        var alphaSettings = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = BehaviorAttribute.AlphaColorComponent,
            RandomStartMin = 255,
            RandomStartMax = 255,
            RandomChangeMin = 255,
            RandomChangeMax = 255,
            LifeTimeMillisecondsMin = 2000,
            LifeTimeMillisecondsMax = 2000,
            UpdateRandomStartMin = (_) => this.alphaStartMinMax.IsRandomized
                ? this.alphaStartMinMax.MinValue
                : this.alphaStartMinMax.Value,
            UpdateRandomStartMax = (_) => this.alphaStartMinMax.IsRandomized
                ? this.alphaStartMinMax.MaxValue
                : this.alphaStartMinMax.Value,
            UpdateRandomChangeMin = (_) => this.alphaEndMinMax.IsRandomized
                ? this.alphaEndMinMax.MinValue - this.alphaStartMinMax.MinValue
                : this.alphaEndMinMax.Value - this.alphaStartMinMax.Value,
            UpdateRandomChangeMax = (_) => this.alphaEndMinMax.IsRandomized
                ? this.alphaEndMinMax.MaxValue - this.alphaStartMinMax.MaxValue
                : this.alphaEndMinMax.Value - this.alphaStartMinMax.Value,
            EasingFunctionType = EasingFunction.EaseIn,
        };

        var redSettings = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = BehaviorAttribute.RedColorComponent,
            RandomStartMin = 255,
            RandomStartMax = 255,
            RandomChangeMin = -255,
            RandomChangeMax = -255,
            LifeTimeMillisecondsMin = 2000,
            LifeTimeMillisecondsMax = 2000,
            // UpdateValue = (value) => this.redEndMinMax.IsRandomized ? value : this.redEndMinMax.Value,
            UpdateRandomStartMin = (_) => this.redStartMinMax.IsRandomized
                ? this.redStartMinMax.MinValue
                : this.redStartMinMax.Value,
            UpdateRandomStartMax = (_) => this.redStartMinMax.IsRandomized
                ? this.redStartMinMax.MaxValue
                : this.redStartMinMax.Value,
            UpdateRandomChangeMin = (_) => this.redEndMinMax.IsRandomized
                ? this.redEndMinMax.MinValue - this.redStartMinMax.MinValue
                : this.redEndMinMax.Value - this.redStartMinMax.Value,
            UpdateRandomChangeMax = (_) => this.redEndMinMax.IsRandomized
                ? this.redEndMinMax.MaxValue - this.redStartMinMax.MaxValue
                : this.redEndMinMax.Value - this.redStartMinMax.Value,
            EasingFunctionType = EasingFunction.EaseIn,
        };

        var greenSettings = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = BehaviorAttribute.GreenColorComponent,
            RandomStartMin = 255,
            RandomStartMax = 255,
            RandomChangeMin = -255,
            RandomChangeMax = -255,
            LifeTimeMillisecondsMin = 2000,
            LifeTimeMillisecondsMax = 2000,
            // UpdateValue = (value) => this.greenEndMinMax.IsRandomized ? value : this.greenEndMinMax.Value,
            UpdateRandomStartMin = (_) => this.greenStartMinMax.IsRandomized
                ? this.greenStartMinMax.MinValue
                : this.greenStartMinMax.Value,
            UpdateRandomStartMax = (_) => this.greenStartMinMax.IsRandomized
                ? this.greenStartMinMax.MaxValue
                : this.greenStartMinMax.Value,
            UpdateRandomChangeMin = (_) => this.greenEndMinMax.IsRandomized
                ? this.greenEndMinMax.MinValue - this.greenStartMinMax.MinValue
                : this.greenEndMinMax.Value - this.greenStartMinMax.Value,
            UpdateRandomChangeMax = (_) => this.greenEndMinMax.IsRandomized
                ? this.greenEndMinMax.MaxValue - this.greenStartMinMax.MaxValue
                : this.greenEndMinMax.Value - this.greenStartMinMax.Value,
            EasingFunctionType = EasingFunction.EaseIn,
        };

        var blueSettings = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = BehaviorAttribute.BlueColorComponent,
            RandomStartMin = 255,
            RandomStartMax = 255,
            RandomChangeMin = 255,
            RandomChangeMax = 255,
            LifeTimeMillisecondsMin = 2000,
            LifeTimeMillisecondsMax = 2000,
            // UpdateValue = (value) => this.blueEndMinMax.IsRandomized ? value : this.blueEndMinMax.Value,
            UpdateRandomStartMin = (_) => this.blueStartMinMax.IsRandomized
                ? this.blueStartMinMax.MinValue
                : this.blueStartMinMax.Value,
            UpdateRandomStartMax = (_) => this.blueStartMinMax.IsRandomized
                ? this.blueStartMinMax.MaxValue
                : this.blueStartMinMax.Value,
            UpdateRandomChangeMin = (_) => this.blueEndMinMax.IsRandomized
                ? this.blueEndMinMax.MinValue - this.blueStartMinMax.MinValue
                : this.blueEndMinMax.Value - this.blueStartMinMax.Value,
            UpdateRandomChangeMax = (_) => this.blueEndMinMax.IsRandomized
                ? this.blueEndMinMax.MaxValue - this.blueStartMinMax.MaxValue
                : this.blueEndMinMax.Value - this.blueStartMinMax.Value,
            EasingFunctionType = EasingFunction.EaseIn,
        };

        var angleSettings = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = BehaviorAttribute.Angle,
            RandomStartMin = 0,
            RandomStartMax = 0,
            RandomChangeMin = 0,
            RandomChangeMax = 0,
            LifeTimeMillisecondsMin = 1000,
            LifeTimeMillisecondsMax = 1000,
            UpdateRandomStartMin = (_) => this.angleStartMinMax.IsRandomized
                ? this.angleStartMinMax.MinValue
                : this.angleStartMinMax.Value,
            UpdateRandomStartMax = (_) => this.angleStartMinMax.IsRandomized
                ? this.angleStartMinMax.MaxValue
                : this.angleStartMinMax.Value,
            UpdateRandomChangeMin = (_) =>
            {
                if (this.angleEndMinMax.IsRandomized)
                {
                    return this.angleEndMinMax.Value - this.angleStartMinMax.Value;
                }

                return this.angleEndMinMax.GrowValue
                    ? this.angleEndMinMax.Value - this.angleStartMinMax.Value
                    : this.angleStartMinMax.Value - this.angleEndMinMax.Value;
            },
            UpdateRandomChangeMax = (_) =>
            {
                if (this.angleEndMinMax.IsRandomized)
                {
                    return this.angleEndMinMax.Value - this.angleStartMinMax.Value;
                }

                return this.angleEndMinMax.IsRandomized
                    ? this.angleEndMinMax.Value - this.angleStartMinMax.Value
                    : this.angleStartMinMax.Value - this.angleEndMinMax.Value;
            },
            EasingFunctionType = EasingFunction.EaseIn,
        };

        var sizeSettings = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = BehaviorAttribute.Size,
            RandomStartMin = 1,
            RandomStartMax = 1,
            RandomChangeMin = 1,
            RandomChangeMax = 1,
            LifeTimeMillisecondsMin = 2000,
            LifeTimeMillisecondsMax = 2000,
            EasingFunctionType = EasingFunction.EaseIn,
        };

        var winCenterX = WindowSize.Width / 2f;
        var winCenterY = WindowSize.Height / 2f;

        var xSettings = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = BehaviorAttribute.X,
            RandomStartMin = winCenterX,
            RandomStartMax = winCenterX,
            RandomChangeMin = winCenterX,
            RandomChangeMax = winCenterX,
            LifeTimeMillisecondsMin = 2000,
            LifeTimeMillisecondsMax = 2000,
            EasingFunctionType = EasingFunction.EaseIn,
        };

        var yPosSettings = new EasingRandomBehaviorSettings
        {
            ApplyToAttribute = BehaviorAttribute.Y,
            RandomStartMin = winCenterY,
            RandomStartMax = winCenterY,
            RandomChangeMin = winCenterY,
            RandomChangeMax = winCenterY,
            LifeTimeMillisecondsMin = 2000,
            LifeTimeMillisecondsMax = 2000,
            EasingFunctionType = EasingFunction.EaseIn,
        };

        return
        [
            alphaSettings,
            redSettings,
            greenSettings,
            blueSettings,
            angleSettings,
            sizeSettings,
            xSettings,
            yPosSettings,
        ];
    }
}
