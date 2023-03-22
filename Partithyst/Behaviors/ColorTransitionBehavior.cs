// <copyright file="ColorTransitionBehavior.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Partithyst.Behaviors;

using System;

/// <summary>
/// Adds behavior that transitions from one color to another over a period of time.
/// </summary>
public class ColorTransitionBehavior : Behavior
{
    private readonly ColorTransitionBehaviorSettings settings;

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorTransitionBehavior"/> class.
    /// </summary>
    /// <param name="settings">The color transition related behavior settings.</param>
    public ColorTransitionBehavior(ColorTransitionBehaviorSettings settings)
        : base(settings)
    {
        this.settings = settings;
        this.settings.ApplyToAttribute = ParticleAttribute.Color;

        LifeTime = this.settings.LifeTime;
    }

    /// <inheritdoc/>
    public override void Update(TimeSpan timeElapsed)
    {
        base.Update(timeElapsed);

        byte alpha = 0;
        byte red = 0;
        byte green = 0;
        byte blue = 0;

        // Transition the red color component
        switch (this.settings.EasingFunctionType)
        {
            case EasingFunction.EaseIn:
                (alpha, red, green, blue) = EaseInQuad();
                break;
            case EasingFunction.EaseOutBounce:
                (alpha, red, green, blue) = EaseOutBounce();
                break;
        }

        Value = $"clr:{alpha},{red},{green},{blue}";
    }

    /// <summary>
    /// Returns the alpha, red, green and blue color components after the ease in quad easing function has been applied.
    /// </summary>
    /// <returns>The component values after the easing function changes.</returns>
    private (byte alphaResult, byte redResult, byte greenResult, byte blueResult) EaseInQuad()
    {
        var alpha = EasingFunctions.EaseInQuad(
            ElapsedTime,
            this.settings.StartColor.A,
            this.settings.AlphaChangeAmount,
            this.settings.LifeTime);

        alpha = alpha > 255 ? 255 : alpha;
        alpha = alpha < 0 ? 0 : alpha;

        var red = EasingFunctions.EaseInQuad(
            ElapsedTime,
            this.settings.StartColor.R,
            this.settings.RedChangeAmount,
            this.settings.LifeTime);

        red = red > 255 ? 255 : red;
        red = red < 0 ? 0 : red;

        var green = EasingFunctions.EaseInQuad(
            ElapsedTime,
            this.settings.StartColor.G,
            this.settings.GreenChangeAmount,
            this.settings.LifeTime);

        green = green > 255 ? 255 : green;
        green = green < 0 ? 0 : green;

        var blue = EasingFunctions.EaseInQuad(
            ElapsedTime,
            this.settings.StartColor.B,
            this.settings.BlueChangeAmount,
            this.settings.LifeTime);

        blue = blue > 255 ? 255 : blue;
        blue = blue < 0 ? 0 : blue;

        return ((byte)alpha, (byte)red, (byte)green, (byte)blue);
    }

    /// <summary>
    /// Returns the alpha, red, green and blue color components after the ease out bounce easing function has been applied.
    /// </summary>
    /// <returns>The component values after the easing function changes.</returns>
    private (byte alphaResult, byte redResult, byte greenResult, byte blueResult) EaseOutBounce()
    {
        var alpha = EasingFunctions.EaseOutBounce(
            ElapsedTime,
            this.settings.StartColor.A,
            this.settings.AlphaChangeAmount,
            this.settings.LifeTime);

        alpha = alpha > 255 ? 255 : alpha;
        alpha = alpha < 0 ? 0 : alpha;

        var red = EasingFunctions.EaseOutBounce(
            ElapsedTime,
            this.settings.StartColor.R,
            this.settings.RedChangeAmount,
            this.settings.LifeTime);

        red = red > 255 ? 255 : red;
        red = red < 0 ? 0 : red;

        var green = EasingFunctions.EaseOutBounce(
            ElapsedTime,
            this.settings.StartColor.G,
            this.settings.GreenChangeAmount,
            this.settings.LifeTime);

        green = green > 255 ? 255 : green;
        green = green < 0 ? 0 : green;

        var blue = EasingFunctions.EaseOutBounce(
            ElapsedTime,
            this.settings.StartColor.B,
            this.settings.BlueChangeAmount,
            this.settings.LifeTime);

        blue = blue > 255 ? 255 : blue;
        blue = blue < 0 ? 0 : blue;

        return ((byte)alpha, (byte)red, (byte)green, (byte)blue);
    }
}