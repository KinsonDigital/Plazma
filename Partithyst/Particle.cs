// <copyright file="Particle.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

#pragma warning disable CA1303 // Do not pass literals as localized parameters
namespace Partithyst;

using System;
using System.Drawing;
using System.Globalization;
using Behaviors;

/// <summary>
/// Represents a single particle with various properties that dictate how the <see cref="Particle"/>
/// behaves and looks on the screen.
/// </summary>
public class Particle
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
    public PointF Position { get; set; }

    /// <summary>
    /// Gets or sets the angle of the <see cref="Particle"/>.
    /// </summary>
    public float Angle { get; set; }

    /// <summary>
    /// Gets or sets the color that the <see cref="Texture"/> will be tinted to.
    /// </summary>
    public ParticleColor TintColor { get; set; } = ParticleColor.White;

    /// <summary>
    /// Gets or sets the size of the <see cref="Particle"/>.
    /// </summary>
    public float Size { get; set; } = 1;

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="Particle"/> is alive or dead.
    /// </summary>
    public bool IsAlive { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="Particle"/> is dead or alive.
    /// </summary>
    public bool IsDead
    {
        get => !IsAlive;
        set => IsAlive = !value;
    }

    /// <summary>
    /// Updates the particle.
    /// </summary>
    /// <param name="timeElapsed">The amount of time that has elapsed since the last frame.</param>
    public void Update(TimeSpan timeElapsed)
    {
        IsDead = true;

        static byte ClampClrValue(float value)
        {
            return (byte)(value < 0 ? 0 : value);
        }

        // Apply the behavior values to the particle attributes
        for (var i = 0; i < this.behaviors.Length; i++)
        {
            if (this.behaviors[i].Enabled)
            {
                var value = 0f;

                this.behaviors[i].Update(timeElapsed);
                IsAlive = true;

                /*NOTE:
                 * The 'parseSuccess' variable will be considered successful if the incoming behavior value is a floating
                 * point number OR if it is the special color syntax value.  This is because the color value syntax is
                 * not an actual number and cannot be parsed into a float and requires special parsing.
                 */
                var parseSuccess = this.behaviors[i].ApplyToAttribute == ParticleAttribute.Color
                                   || float.TryParse(string.IsNullOrEmpty(this.behaviors[i].Value) ? "0" : this.behaviors[i].Value, out value);

                if (!parseSuccess)
                {
                    throw new Exception($"{nameof(Particle)}.{nameof(Particle.Update)} Exception:\n\tParsing the behavior value '{this.behaviors[i].Value}' failed.\n\tValue must be a number.");
                }

                switch (this.behaviors[i].ApplyToAttribute)
                {
                    case ParticleAttribute.X:
                        Position = new PointF(value, Position.Y);
                        break;
                    case ParticleAttribute.Y:
                        Position = new PointF(Position.X, value);
                        break;
                    case ParticleAttribute.Angle:
                        Angle = value;
                        break;
                    case ParticleAttribute.Size:
                        Size = value;
                        break;
                    case ParticleAttribute.Color:
                        // Create the color
                        var (clrParseSuccess, componentValue, parseFailReason) = TryParse(this.behaviors[i].Value, out var result);

                        if (!clrParseSuccess)
                        {
                            var exceptionLocation = $"Particle.Update Exception: {parseFailReason}";

                            throw new Exception($"{exceptionLocation}");
                        }

                        TintColor = result;
                        break;
                    case ParticleAttribute.RedColorComponent:
                        TintColor.R = ClampClrValue(value);
                        break;
                    case ParticleAttribute.GreenColorComponent:
                        TintColor.G = ClampClrValue(value);
                        break;
                    case ParticleAttribute.BlueColorComponent:
                        TintColor.B = ClampClrValue(value);
                        break;
                    case ParticleAttribute.AlphaColorComponent:
                        TintColor.A = ClampClrValue(value);
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Resets the particle.
    /// </summary>
    public void Reset()
    {
        if (!(this.behaviors is null))
        {
            for (var i = 0; i < this.behaviors.Length; i++)
            {
                this.behaviors[i].Reset();
            }
        }

        Size = 1;
        Angle = 0;
        TintColor = ParticleColor.White;
        IsAlive = true;
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (!(obj is Particle particle))
        {
            return false;
        }

        return Position == particle.Position &&
               Angle == particle.Angle &&
               TintColor == particle.TintColor &&
               Size == particle.Size &&
               IsAlive == particle.IsAlive &&
               IsDead == particle.IsDead;
    }

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode() =>
        HashCode.Combine(this.behaviors, Position, Angle, TintColor, Size, IsAlive, IsDead);

    /// <summary>
    /// Parses the <paramref name="colorValue"/> string into a <see cref="ParticleColor"/> type.
    /// </summary>
    /// <param name="colorValue">The color string to parse.</param>
    /// <returns>True if the parse was successful.</returns>
    private static (bool success, string componentValue, string parseFailReason) TryParse(string colorValue, out ParticleColor color)
    {
        color = new ParticleColor(0, 0, 0, 0);

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
        color = new ParticleColor(alpha, red, green, blue);

        return (true, string.Empty, string.Empty);
    }
}
