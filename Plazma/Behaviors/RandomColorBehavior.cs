// <copyright file="RandomColorBehavior.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma.Behaviors;

using System;
using Services;

/// <summary>
/// Randomly chooses colors from a list of colors that will be applied to a particle.
/// Used to randomly set the tint color of a particle for its entire lifetime.
/// </summary>
public class RandomColorBehavior : Behavior
{
    private readonly RandomChoiceBehaviorSettings settings;
    private readonly IRandomizerService randomizer;
    private bool isColorChosen;

    /// <summary>
    /// Initializes a new instance of the <see cref="RandomColorBehavior"/> class.
    /// </summary>
    /// <param name="settings">The behavior settings.</param>
    /// <param name="randomizer">The randomizer used to randomly choose a color from a list of colors.</param>
    public RandomColorBehavior(RandomChoiceBehaviorSettings settings, IRandomizerService randomizer)
        : base(settings)
    {
        this.settings = settings;
        this.randomizer = randomizer;
    }

    /// <summary>
    /// Updates the behavior.
    /// </summary>
    /// <param name="timeElapsed">The amount of time that has elapsed since the last frame.</param>
    public override void Update(TimeSpan timeElapsed)
    {
        base.Update(timeElapsed);

        // If the amount of time has passed, disable the behavior
        Enabled = ElapsedTime < this.settings.LifeTime;

        if (this.isColorChosen)
        {
            return;
        }

        // Randomly choose a color and set the value to a floating point number that represents that color
        var randomIndex = this.randomizer.GetValue(0, this.settings.Data is null ? 0 : this.settings.Data.Count - 1);

        Value = this.settings.Data is null ? "clr:255,255,255,255" : this.settings.Data[randomIndex];

        this.isColorChosen = true;
    }

    /// <summary>
    /// Resets the behavior.
    /// </summary>
    public override void Reset()
    {
        this.isColorChosen = false;
        base.Reset();
    }
}