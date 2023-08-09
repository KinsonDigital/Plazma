// <copyright file="ColorTransitionBehaviorSettings.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma.Behaviors;

using System.Drawing;

/// <summary>
/// Transitions from one color to another over a specific amount of time
/// using an easing function.
/// </summary>
public class ColorTransitionBehaviorSettings : BehaviorSettings, IEasingCapable
{
    /*NOTE:
     * The "change amount props" are build on the premise that if the start color component
     * value is less than the stop color component value, then returning a negative positive
     * value will make sure that the change amount being fed into the ease in function
     * will increase the start value until it reaches the stop value.
     */

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorTransitionBehaviorSettings"/> class.
    /// </summary>
    public ColorTransitionBehaviorSettings() => ApplyToAttribute = ParticleAttribute.Color;

    /// <inheritdoc/>
    public EasingFunction EasingFunctionType { get; set; } = EasingFunction.EaseIn;

    /// <summary>
    /// Gets or sets the color that the transition will start from.
    /// </summary>
    public Color StartColor { get; set; } = Color.White;

    /// <summary>
    /// Gets or sets the desired color to transition to.
    /// </summary>
    public Color StopColor { get; set; } = Color.White;

    /// <summary>
    /// Gets or sets the amount of time the behavior will run.
    /// </summary>
    public double LifeTime { get; set; }

    /// <summary>
    /// Gets the amount of change to apply over time to the alpha color component.
    /// </summary>
    internal int AlphaChangeAmount => StopColor.A - StartColor.A;

    /// <summary>
    /// Gets the amount of change to apply over time to the red color component.
    /// </summary>
    internal int RedChangeAmount => StopColor.R - StartColor.R;

    /// <summary>
    /// Gets the amount of change to apply over time to the green color component.
    /// </summary>
    internal int GreenChangeAmount => StopColor.G - StartColor.G;

    /// <summary>
    /// Gets the amount of change to apply over time to the blue color component.
    /// </summary>
    internal int BlueChangeAmount => StopColor.B - StartColor.B;
}
