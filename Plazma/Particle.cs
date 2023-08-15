// <copyright file="Particle.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma;

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.Numerics;
using Behaviors;

/// <summary>
/// <inheritdoc/>
/// </summary>
public class Particle : IParticle
{
    private readonly IBehavior[] behaviors;

    /// <summary>
    /// Initializes a new instance of the <see cref="Particle"/> class.
    /// </summary>
    /// <param name="behaviors">The list of behaviors to add to the <see cref="Particle"/>.</param>
    public Particle(IBehavior[] behaviors) => this.behaviors = behaviors;

    /// <summary>
    /// Gets or sets the position of the <see cref="Particle"/>.
    /// </summary>
    public Vector2 Position { get; set; }

    /// <summary>
    /// Gets or sets the angle of the <see cref="Particle"/>.
    /// </summary>
    public float Angle { get; set; }

    /// <summary>
    /// Gets or sets the color that the texture will be tinted to.
    /// </summary>
    public Color TintColor { get; set; } = Color.White;

    /// <summary>
    /// Gets or sets the size of the <see cref="Particle"/>.
    /// </summary>
    public float Size { get; set; } = 1;

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="Particle"/> is alive or dead.
    /// </summary>
    public bool IsAlive { get; set; }

    /// <summary>
    /// Updates the particle.
    /// </summary>
    /// <param name="timeElapsed">The amount of time that has elapsed since the last frame.</param>
    public void Update(TimeSpan timeElapsed)
    {
        IsAlive = false;

        static byte ClampClrValue(float value)
        {
            return (byte)(value < 0 ? 0 : value);
        }

        // Apply the behavior values to the particle attributes
        foreach (var behavior in this.behaviors)
        {
            if (behavior.Enabled)
            {
                behavior.Update(timeElapsed);
                IsAlive = true;

                /*NOTE:
                 * The 'parseSuccess' variable will be considered successful if the incoming behavior value is a floating
                 * point number OR if it is the special color syntax value.  This is because the color value syntax is
                 * not an actual number and cannot be parsed into a float and requires special parsing.
                 */
                // TODO: Parsing needs to be removed once the behavior value is of type double
                var parseSuccess = float.TryParse(string.IsNullOrEmpty(behavior.Value) ? "0" : behavior.Value, out var value);

                if (!parseSuccess)
                {
                    // TODO: Create custom exception
                    throw new Exception($"{nameof(Particle)}.{nameof(Update)} Exception:\n\tParsing the behavior value '{behavior.Value}' failed.\n\tValue must be a number.");
                }

                switch (behavior.ApplyToAttribute)
                {
                    case ParticleAttribute.X:
                        Position = new Vector2(value, Position.Y);
                        break;
                    case ParticleAttribute.Y:
                        Position = new Vector2(Position.X, value);
                        break;
                    case ParticleAttribute.Angle:
                        Angle = value;
                        break;
                    case ParticleAttribute.Size:
                        Size = value;
                        break;
                    case ParticleAttribute.AlphaColorComponent:
                        TintColor = Color.FromArgb(ClampClrValue(value), TintColor.R, TintColor.G, TintColor.B);
                        break;
                    case ParticleAttribute.RedColorComponent:
                        TintColor = Color.FromArgb(TintColor.A, ClampClrValue(value), TintColor.G, TintColor.B);
                        break;
                    case ParticleAttribute.GreenColorComponent:
                        TintColor = Color.FromArgb(TintColor.A, TintColor.R, ClampClrValue(value), TintColor.B);
                        break;
                    case ParticleAttribute.BlueColorComponent:
                        TintColor = Color.FromArgb(TintColor.A, TintColor.R, TintColor.G, ClampClrValue(value));
                        break;
                    default:
                        throw new InvalidEnumArgumentException(nameof(ParticleAttribute), (int)behavior.ApplyToAttribute, typeof(ParticleAttribute));
                }
            }
        }
    }

    /// <summary>
    /// Resets the particle.
    /// </summary>
    public void Reset()
    {
        foreach (var behavior in this.behaviors)
        {
            behavior.Reset();
        }

        Size = 1;
        Angle = 0;
        TintColor = Color.White;
        IsAlive = true;
    }

    /// <summary>
    /// Parses the <paramref name="colorValue"/> string into a <see cref="Color"/> type.
    /// </summary>
    /// <param name="colorValue">The color string to parse.</param>
    /// <param name="color">The parsed color.</param>
    /// <returns>True if the parse was successful.</returns>
    [SuppressMessage("ReSharper", "UnusedTupleComponentInReturnValue", Justification = "Part of the public API.")]
    private static (bool success, string componentValue, string parseFailReason) TryParse(string colorValue, out Color color)
    {
        color = Color.FromArgb(0, 0, 0, 0);

        /*Parse the string data into color components to create a color from
         * Example Data: clr:10,20,30,40
        */
        const string syntaxAsFollowsMsg = "\n\tSyntax is as follows: clr:<alpha>,<red>,<green>,<blue>";

        // Check to make sure that the 'clr' prefix exists
        if (!colorValue.Contains("clr", StringComparison.InvariantCulture))
        {
            return (false, "all", $"Error #900. Invalid Syntax. Missing 'clr' prefix.{syntaxAsFollowsMsg}");
        }

        // Check to make sure the ':' character exists
        if (!colorValue.Contains(':', StringComparison.OrdinalIgnoreCase))
        {
            return (false, "all", $"Error #1000. Invalid Syntax. Missing ':'.{syntaxAsFollowsMsg}");
        }

        // Split into sections to separate 'clr' section and the '10,20,30,40' pieces of the string
        // Section 1 => clr
        // Section 2 => 10,20,30,40
        var valueSections = colorValue.Split(':');

        // Split the color components to separate each number
        // Section 1 => 10 (Alpha)
        // Section 2 => 20 (Red)
        // Section 3 => 30 (Green)
        // Section 4 => 40 (Blue)
        var clrComponents = valueSections[1].Split(',', StringSplitOptions.RemoveEmptyEntries);

        // Check if any of the color components are missing
        if (clrComponents.Length != 4)
        {
            return (false, "all", "Error #1100. Invalid Syntax. Missing color component.\n\tSyntax is as follows: clr:<alpha>,<red>,<green>,<blue>");
        }

        // Check for non number digits contained in the components
        if (clrComponents[0].ContainsNonNumberCharacters())
        {
            return (false, clrComponents[0].ToString(CultureInfo.InvariantCulture), $"Error #1200. Invalid Syntax. Alpha color component must only contain numbers.");
        }

        if (clrComponents[1].ContainsNonNumberCharacters())
        {
            return (false, clrComponents[1].ToString(CultureInfo.InvariantCulture), $"Error #1300. Invalid Syntax. Alpha color component must only contain numbers.");
        }

        if (clrComponents[2].ContainsNonNumberCharacters())
        {
            return (false, clrComponents[2].ToString(CultureInfo.InvariantCulture), $"Error #1400. Invalid Syntax. Alpha color component must only contain numbers.");
        }

        if (clrComponents[3].ContainsNonNumberCharacters())
        {
            return (false, clrComponents[3].ToString(CultureInfo.InvariantCulture), $"Error #1500. Invalid Syntax. Alpha color component must only contain numbers.");
        }

        // Parse values and check for values out of the range of 0-255
        var alphaParseSuccess = byte.TryParse(clrComponents[0], out var alpha);
        var redParseSuccess = byte.TryParse(clrComponents[1], out var red);
        var greenParseSuccess = byte.TryParse(clrComponents[2], out var green);
        var blueParseSuccess = byte.TryParse(clrComponents[3], out var blue);

        if (!alphaParseSuccess)
        {
            return (false, clrComponents[0].ToString(CultureInfo.InvariantCulture), "Error #1500. Invalid Syntax. Alpha color component out of range.");
        }

        if (!redParseSuccess)
        {
            return (false, clrComponents[1].ToString(CultureInfo.InvariantCulture), "Error #1600. Invalid Syntax. Red color component out of range.");
        }

        if (!greenParseSuccess)
        {
            return (false, clrComponents[2].ToString(CultureInfo.InvariantCulture), "Error #1700. Invalid Syntax. Green color component out of range.");
        }

        if (!blueParseSuccess)
        {
            return (false, clrComponents[3].ToString(CultureInfo.InvariantCulture), "Error #1800. Invalid Syntax. Blue color component out of range.");
        }

        // Create the color
        color = Color.FromArgb(alpha, red, green, blue);

        return (true, string.Empty, string.Empty);
    }
}
