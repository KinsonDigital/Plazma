// <copyright file="EasingRandomBehavior.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma.Behaviors;

using System;
using Services;

/// <summary>
/// A behavior that can be applied to a particle that uses an easing function
/// to dictate the value of a particle attribute.
/// </summary>
public class EasingRandomBehavior : Behavior
{
    private readonly IRandomizerService randomizer;
    private EasingRandomBehaviorSettings settings;

    /// <summary>
    /// Initializes a new instance of the <see cref="EasingRandomBehavior"/> class.
    /// </summary>
    /// <param name="settings">The behavior settings to add to the behavior.</param>
    /// <param name="randomizer">The randomizer used for choosing values between the various setting ranges.</param>
    public EasingRandomBehavior(EasingRandomBehaviorSettings settings, IRandomizerService randomizer)
        : base(settings)
    {
        this.settings = settings;
        this.randomizer = randomizer;
        ApplyRandomization();
    }

    /// <summary>
    /// Gets or sets the starting value of the easing behavior.
    /// </summary>
    public double Start { get; set; }

    /// <summary>
    /// Gets or sets the amount of change to apply to the behavior value over time.
    /// </summary>
    public double Change { get; set; }

    /// <summary>
    /// Updates the behavior.
    /// </summary>
    /// <param name="timeElapsed">The amount of time that has elapsed for this update of the behavior.</param>
    public override void Update(TimeSpan timeElapsed)
    {
        Value = this.settings.EasingFunctionType switch
        {
            EasingFunction.EaseOutBounce => EasingFunctions.EaseOutBounce(ElapsedTime, Start, Change, LifeTime),
            EasingFunction.EaseIn => EasingFunctions.EaseInQuad(ElapsedTime, Start, Change, LifeTime),
            _ => Value
        };

        if (this.settings.UpdateValue is not null)
        {
            Value = this.settings.UpdateValue.Invoke(Value);
        }

        if (this.settings.UpdateRandomStartMin is not null)
        {
            this.settings = this.settings with { RandomStartMin = this.settings.UpdateRandomStartMin?.Invoke(Value) ?? 0f };
        }

        if (this.settings.UpdateRandomStartMax is not null)
        {
            this.settings = this.settings with { RandomStartMax = this.settings.UpdateRandomStartMax?.Invoke(Value) ?? 0f };
        }

        base.Update(timeElapsed);
    }

    /// <summary>
    /// Resets the behavior.
    /// </summary>
    public override void Reset()
    {
        ApplyRandomization();
        base.Reset();
    }

    /// <summary>
    /// Generates random values based on the <see cref="EasingRandomBehaviorSettings"/>
    /// and applies them.
    /// </summary>
    private void ApplyRandomization()
    {
        Start = this.randomizer.GetValue(this.settings.RandomStartMin, this.settings.RandomStartMax);
        Change = this.randomizer.GetValue(this.settings.RandomChangeMin, this.settings.RandomChangeMax);
        LifeTime = this.randomizer.GetValue(this.settings.LifeTimeMillisecondsMin, this.settings.LifeTimeMillisecondsMax);
    }
}
