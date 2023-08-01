// <copyright file="ParticleColor.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma;

using System;

/// <summary>
/// Represents a color for a particle.
/// </summary>
public class ParticleColor
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ParticleColor"/> class.
    /// </summary>
    /// <param name="a">The alpha component of the color.</param>
    /// <param name="r">The red component of the color.</param>
    /// <param name="g">The green component of the color.</param>
    /// <param name="b">The blue component of the color.</param>
    public ParticleColor(byte a, byte r, byte g, byte b)
    {
        A = a;
        R = r;
        G = g;
        B = b;
    }

    /// <summary>
    /// Gets the color AliceBlue.
    /// </summary>
    public static ParticleColor AliceBlue => new ParticleColor(255, 240, 248, 255);

    /// <summary>
    /// Gets the color AntiqueWhite.
    /// </summary>
    public static ParticleColor AntiqueWhite => new ParticleColor(255, 250, 235, 215);

    /// <summary>
    /// Gets the color Aqua.
    /// </summary>
    public static ParticleColor Aqua => new ParticleColor(255, 0, 255, 255);

    /// <summary>
    /// Gets the color Aquamarine.
    /// </summary>
    public static ParticleColor Aquamarine => new ParticleColor(255, 127, 255, 212);

    /// <summary>
    /// Gets the color Azure.
    /// </summary>
    public static ParticleColor Azure => new ParticleColor(255, 240, 255, 255);

    /// <summary>
    /// Gets the color Beige.
    /// </summary>
    public static ParticleColor Beige => new ParticleColor(255, 245, 245, 220);

    /// <summary>
    /// Gets the color Bisque.
    /// </summary>
    public static ParticleColor Bisque => new ParticleColor(255, 255, 228, 196);

    /// <summary>
    /// Gets the color Black.
    /// </summary>
    public static ParticleColor Black => new ParticleColor(255, 0, 0, 0);

    /// <summary>
    /// Gets the color BlanchedAlmond.
    /// </summary>
    public static ParticleColor BlanchedAlmond => new ParticleColor(255, 255, 235, 205);

    /// <summary>
    /// Gets the color Blue.
    /// </summary>
    public static ParticleColor Blue => new ParticleColor(255, 0, 0, 255);

    /// <summary>
    /// Gets the color BlueViolet.
    /// </summary>
    public static ParticleColor BlueViolet => new ParticleColor(255, 138, 43, 226);

    /// <summary>
    /// Gets the color Brown.
    /// </summary>
    public static ParticleColor Brown => new ParticleColor(255, 165, 42, 42);

    /// <summary>
    /// Gets the color BurlyWood.
    /// </summary>
    public static ParticleColor BurlyWood => new ParticleColor(255, 222, 184, 135);

    /// <summary>
    /// Gets the color CadetBlue.
    /// </summary>
    public static ParticleColor CadetBlue => new ParticleColor(255, 95, 158, 160);

    /// <summary>
    /// Gets the color Chartreuse.
    /// </summary>
    public static ParticleColor Chartreuse => new ParticleColor(255, 127, 255, 0);

    /// <summary>
    /// Gets the color Chocolate.
    /// </summary>
    public static ParticleColor Chocolate => new ParticleColor(255, 210, 105, 30);

    /// <summary>
    /// Gets the color Coral.
    /// </summary>
    public static ParticleColor Coral => new ParticleColor(255, 255, 127, 80);

    /// <summary>
    /// Gets the color CornflowerBlue.
    /// </summary>
    public static ParticleColor CornflowerBlue => new ParticleColor(255, 100, 149, 237);

    /// <summary>
    /// Gets the color Cornsilk.
    /// </summary>
    public static ParticleColor Cornsilk => new ParticleColor(255, 255, 248, 220);

    /// <summary>
    /// Gets the color Crimson.
    /// </summary>
    public static ParticleColor Crimson => new ParticleColor(255, 220, 20, 60);

    /// <summary>
    /// Gets the color Cyan.
    /// </summary>
    public static ParticleColor Cyan => new ParticleColor(255, 0, 255, 255);

    /// <summary>
    /// Gets the color DarkBlue.
    /// </summary>
    public static ParticleColor DarkBlue => new ParticleColor(255, 0, 0, 139);

    /// <summary>
    /// Gets the color DarkCyan.
    /// </summary>
    public static ParticleColor DarkCyan => new ParticleColor(255, 0, 139, 139);

    /// <summary>
    /// Gets the color DarkGoldenrod.
    /// </summary>
    public static ParticleColor DarkGoldenrod => new ParticleColor(255, 184, 134, 11);

    /// <summary>
    /// Gets the color DarkGray.
    /// </summary>
    public static ParticleColor DarkGray => new ParticleColor(255, 169, 169, 169);

    /// <summary>
    /// Gets the color DarkGreen.
    /// </summary>
    public static ParticleColor DarkGreen => new ParticleColor(255, 0, 100, 0);

    /// <summary>
    /// Gets the color DarkKhaki.
    /// </summary>
    public static ParticleColor DarkKhaki => new ParticleColor(255, 189, 183, 107);

    /// <summary>
    /// Gets the color DarkMagenta.
    /// </summary>
    public static ParticleColor DarkMagenta => new ParticleColor(255, 139, 0, 139);

    /// <summary>
    /// Gets the color DarkOliveGreen.
    /// </summary>
    public static ParticleColor DarkOliveGreen => new ParticleColor(255, 85, 107, 47);

    /// <summary>
    /// Gets the color DarkOrange.
    /// </summary>
    public static ParticleColor DarkOrange => new ParticleColor(255, 255, 140, 0);

    /// <summary>
    /// Gets the color DarkOrchid.
    /// </summary>
    public static ParticleColor DarkOrchid => new ParticleColor(255, 153, 50, 204);

    /// <summary>
    /// Gets the color DarkRed.
    /// </summary>
    public static ParticleColor DarkRed => new ParticleColor(255, 139, 0, 0);

    /// <summary>
    /// Gets the color DarkSalmon.
    /// </summary>
    public static ParticleColor DarkSalmon => new ParticleColor(255, 233, 150, 122);

    /// <summary>
    /// Gets the color DarkSeaGreen.
    /// </summary>
    public static ParticleColor DarkSeaGreen => new ParticleColor(255, 143, 188, 143);

    /// <summary>
    /// Gets get the color DarkSlateBlue.
    /// </summary>
    public static ParticleColor DarkSlateBlue => new ParticleColor(255, 72, 61, 139);

    /// <summary>
    /// Gets get the color DarkSlateGray.
    /// </summary>
    public static ParticleColor DarkSlateGray => new ParticleColor(255, 47, 79, 79);

    /// <summary>
    /// Gets get the color DarkTurquoise.
    /// </summary>
    public static ParticleColor DarkTurquoise => new ParticleColor(255, 0, 206, 209);

    /// <summary>
    /// Gets get the color DarkViolet.
    /// </summary>
    public static ParticleColor DarkViolet => new ParticleColor(255, 148, 0, 211);

    /// <summary>
    /// Gets get the color DeepPink.
    /// </summary>
    public static ParticleColor DeepPink => new ParticleColor(255, 255, 20, 147);

    /// <summary>
    /// Gets get the color DeepSkyBlue.
    /// </summary>
    public static ParticleColor DeepSkyBlue => new ParticleColor(255, 0, 191, 255);

    /// <summary>
    /// Gets get the color DimGray.
    /// </summary>
    public static ParticleColor DimGray => new ParticleColor(255, 105, 105, 105);

    /// <summary>
    /// Gets get the color DodgerBlue.
    /// </summary>
    public static ParticleColor DodgerBlue => new ParticleColor(255, 30, 144, 255);

    /// <summary>
    /// Gets get the color Firebrick.
    /// </summary>
    public static ParticleColor Firebrick => new ParticleColor(255, 178, 34, 34);

    /// <summary>
    /// Gets get the color FloralWhite.
    /// </summary>
    public static ParticleColor FloralWhite => new ParticleColor(255, 255, 250, 240);

    /// <summary>
    /// Gets get the color ForestGreen.
    /// </summary>
    public static ParticleColor ForestGreen => new ParticleColor(255, 34, 139, 34);

    /// <summary>
    /// Gets get the color Fuchsia.
    /// </summary>
    public static ParticleColor Fuchsia => new ParticleColor(255, 255, 0, 255);

    /// <summary>
    /// Gets get the color Gainsboro.
    /// </summary>
    public static ParticleColor Gainsboro => new ParticleColor(255, 220, 220, 220);

    /// <summary>
    /// Gets get the color GhostWhite.
    /// </summary>
    public static ParticleColor GhostWhite => new ParticleColor(255, 248, 248, 255);

    /// <summary>
    /// Gets get the color Gold.
    /// </summary>
    public static ParticleColor Gold => new ParticleColor(255, 255, 215, 0);

    /// <summary>
    /// Gets get the color Goldenrod.
    /// </summary>
    public static ParticleColor Goldenrod => new ParticleColor(255, 218, 165, 32);

    /// <summary>
    /// Gets get the color Gray.
    /// </summary>
    public static ParticleColor Gray => new ParticleColor(255, 128, 128, 128);

    /// <summary>
    /// Gets get the color Green.
    /// </summary>
    public static ParticleColor Green => new ParticleColor(255, 0, 128, 0);

    /// <summary>
    /// Gets get the color GreenYellow.
    /// </summary>
    public static ParticleColor GreenYellow => new ParticleColor(255, 173, 255, 47);

    /// <summary>
    /// Gets get the color Honeydew.
    /// </summary>
    public static ParticleColor Honeydew => new ParticleColor(255, 240, 255, 240);

    /// <summary>
    /// Gets get the color HotPink.
    /// </summary>
    public static ParticleColor HotPink => new ParticleColor(255, 255, 105, 180);

    /// <summary>
    /// Gets get the color IndianRed.
    /// </summary>
    public static ParticleColor IndianRed => new ParticleColor(255, 205, 92, 92);

    /// <summary>
    /// Gets get the color Indigo.
    /// </summary>
    public static ParticleColor Indigo => new ParticleColor(255, 75, 0, 130);

    /// <summary>
    /// Gets get the color Ivory.
    /// </summary>
    public static ParticleColor Ivory => new ParticleColor(255, 255, 255, 240);

    /// <summary>
    /// Gets get the color Khaki.
    /// </summary>
    public static ParticleColor Khaki => new ParticleColor(255, 240, 230, 140);

    /// <summary>
    /// Gets get the color Lavender.
    /// </summary>
    public static ParticleColor Lavender => new ParticleColor(255, 230, 230, 250);

    /// <summary>
    /// Gets get the color LavenderBlush.
    /// </summary>
    public static ParticleColor LavenderBlush => new ParticleColor(255, 255, 240, 245);

    /// <summary>
    /// Gets get the color LawnGreen.
    /// </summary>
    public static ParticleColor LawnGreen => new ParticleColor(255, 124, 252, 0);

    /// <summary>
    /// Gets get the color LemonChiffon.
    /// </summary>
    public static ParticleColor LemonChiffon => new ParticleColor(255, 255, 250, 205);

    /// <summary>
    /// Gets get the color LightBlue.
    /// </summary>
    public static ParticleColor LightBlue => new ParticleColor(255, 173, 216, 230);

    /// <summary>
    /// Gets get the color LightCoral.
    /// </summary>
    public static ParticleColor LightCoral => new ParticleColor(255, 240, 128, 128);

    /// <summary>
    /// Gets get the color LightCyan.
    /// </summary>
    public static ParticleColor LightCyan => new ParticleColor(255, 224, 255, 255);

    /// <summary>
    /// Gets get the color LightGoldenrodYellow.
    /// </summary>
    public static ParticleColor LightGoldenrodYellow => new ParticleColor(255, 250, 250, 210);

    /// <summary>
    /// Gets get the color LightGray.
    /// </summary>
    public static ParticleColor LightGray => new ParticleColor(255, 211, 211, 211);

    /// <summary>
    /// Gets get the color LightGreen.
    /// </summary>
    public static ParticleColor LightGreen => new ParticleColor(255, 144, 238, 144);

    /// <summary>
    /// Gets get the color LightPink.
    /// </summary>
    public static ParticleColor LightPink => new ParticleColor(255, 255, 182, 193);

    /// <summary>
    /// Gets get the color LightSalmon.
    /// </summary>
    public static ParticleColor LightSalmon => new ParticleColor(255, 255, 160, 122);

    /// <summary>
    /// Gets get the color LightSkyBlue.
    /// </summary>
    public static ParticleColor LightSkyBlue => new ParticleColor(255, 135, 206, 250);

    /// <summary>
    /// Gets get the color LightSlateGray.
    /// </summary>
    public static ParticleColor LightSlateGray => new ParticleColor(255, 119, 136, 153);

    /// <summary>
    /// Gets get the color LightSteelBlue.
    /// </summary>
    public static ParticleColor LightSteelBlue => new ParticleColor(255, 176, 196, 222);

    /// <summary>
    /// Gets get the color LightYellow.
    /// </summary>
    public static ParticleColor LightYellow => new ParticleColor(255, 255, 255, 224);

    /// <summary>
    /// Gets get the color Lime.
    /// </summary>
    public static ParticleColor Lime => new ParticleColor(255, 0, 255, 0);

    /// <summary>
    /// Gets get the color LimeGreen.
    /// </summary>
    public static ParticleColor LimeGreen => new ParticleColor(255, 50, 205, 50);

    /// <summary>
    /// Gets get the color Linen.
    /// </summary>
    public static ParticleColor Linen => new ParticleColor(255, 250, 240, 230);

    /// <summary>
    /// Gets get the color Magenta.
    /// </summary>
    public static ParticleColor Magenta => new ParticleColor(255, 255, 0, 255);

    /// <summary>
    /// Gets get the color Maroon.
    /// </summary>
    public static ParticleColor Maroon => new ParticleColor(255, 128, 0, 0);

    /// <summary>
    /// Gets get the color MediumAquamarine.
    /// </summary>
    public static ParticleColor MediumAquamarine => new ParticleColor(255, 102, 205, 170);

    /// <summary>
    /// Gets get the color MediumBlue.
    /// </summary>
    public static ParticleColor MediumBlue => new ParticleColor(255, 0, 0, 205);

    /// <summary>
    /// Gets get the color MediumOrchid.
    /// </summary>
    public static ParticleColor MediumOrchid => new ParticleColor(255, 186, 85, 211);

    /// <summary>
    /// Gets get the color MediumPurple.
    /// </summary>
    public static ParticleColor MediumPurple => new ParticleColor(255, 147, 112, 219);

    /// <summary>
    /// Gets get the color MediumSeaGreen.
    /// </summary>
    public static ParticleColor MediumSeaGreen => new ParticleColor(255, 60, 179, 113);

    /// <summary>
    /// Gets get the color MediumSlateBlue.
    /// </summary>
    public static ParticleColor MediumSlateBlue => new ParticleColor(255, 123, 104, 238);

    /// <summary>
    /// Gets get the color MediumSpringGreen.
    /// </summary>
    public static ParticleColor MediumSpringGreen => new ParticleColor(255, 0, 250, 154);

    /// <summary>
    /// Gets get the color MdeiumTurquoise.
    /// </summary>
    public static ParticleColor MediumTurquoise => new ParticleColor(255, 72, 209, 204);

    /// <summary>
    /// Gets get the color MediumVioletRed.
    /// </summary>
    public static ParticleColor MediumVioletRed => new ParticleColor(255, 199, 21, 133);

    /// <summary>
    /// Gets get the color MidnightBlue.
    /// </summary>
    public static ParticleColor MidnightBlue => new ParticleColor(255, 25, 25, 112);

    /// <summary>
    /// Gets get the color MintCream.
    /// </summary>
    public static ParticleColor MintCream => new ParticleColor(255, 245, 255, 250);

    /// <summary>
    /// Gets get the color MistyRose.
    /// </summary>
    public static ParticleColor MistyRose => new ParticleColor(255, 255, 228, 225);

    /// <summary>
    /// Gets get the color Moccasin.
    /// </summary>
    public static ParticleColor Moccasin => new ParticleColor(255, 255, 228, 181);

    /// <summary>
    /// Gets get the color NavajoWhite.
    /// </summary>
    public static ParticleColor NavajoWhite => new ParticleColor(255, 255, 222, 173);

    /// <summary>
    /// Gets get the color Navy.
    /// </summary>
    public static ParticleColor Navy => new ParticleColor(255, 0, 0, 128);

    /// <summary>
    /// Gets get the color OldLace.
    /// </summary>
    public static ParticleColor OldLace => new ParticleColor(255, 253, 245, 230);

    /// <summary>
    /// Gets get the color Olive.
    /// </summary>
    public static ParticleColor Olive => new ParticleColor(255, 128, 128, 0);

    /// <summary>
    /// Gets get the color OliveDrab.
    /// </summary>
    public static ParticleColor OliveDrab => new ParticleColor(255, 107, 142, 35);

    /// <summary>
    /// Gets get the color Orange.
    /// </summary>
    public static ParticleColor Orange => new ParticleColor(255, 255, 165, 0);

    /// <summary>
    /// Gets get the color OrangeRed.
    /// </summary>
    public static ParticleColor OrangeRed => new ParticleColor(255, 255, 69, 0);

    /// <summary>
    /// Gets get the color Orchid.
    /// </summary>
    public static ParticleColor Orchid => new ParticleColor(255, 218, 112, 214);

    /// <summary>
    /// Gets get the color PaleGoldenrod.
    /// </summary>
    public static ParticleColor PaleGoldenrod => new ParticleColor(255, 238, 232, 170);

    /// <summary>
    /// Gets get the color PaleGreen.
    /// </summary>
    public static ParticleColor PaleGreen => new ParticleColor(255, 152, 251, 152);

    /// <summary>
    /// Gets get the color PaleTurquoise.
    /// </summary>
    public static ParticleColor PaleTurquoise => new ParticleColor(255, 175, 238, 238);

    /// <summary>
    /// Gets get the color PaleVioletRed.
    /// </summary>
    public static ParticleColor PaleVioletRed => new ParticleColor(255, 219, 112, 147);

    /// <summary>
    /// Gets get the color PapayaWhip.
    /// </summary>
    public static ParticleColor PapayaWhip => new ParticleColor(255, 255, 239, 213);

    /// <summary>
    /// Gets get the color PeachPuff.
    /// </summary>
    public static ParticleColor PeachPuff => new ParticleColor(255, 255, 218, 185);

    /// <summary>
    /// Gets get the color Peru.
    /// </summary>
    public static ParticleColor Peru => new ParticleColor(255, 205, 133, 63);

    /// <summary>
    /// Gets get the color Pink.
    /// </summary>
    public static ParticleColor Pink => new ParticleColor(255, 255, 192, 203);

    /// <summary>
    /// Gets get the color Plum.
    /// </summary>
    public static ParticleColor Plum => new ParticleColor(255, 221, 160, 221);

    /// <summary>
    /// Gets get the color PowderBlue.
    /// </summary>
    public static ParticleColor PowderBlue => new ParticleColor(255, 176, 224, 230);

    /// <summary>
    /// Gets get the color Purple.
    /// </summary>
    public static ParticleColor Purple => new ParticleColor(255, 128, 0, 128);

    /// <summary>
    /// Gets get the color Red.
    /// </summary>
    public static ParticleColor Red => new ParticleColor(255, 255, 0, 0);

    /// <summary>
    /// Gets get the color RosyBrown.
    /// </summary>
    public static ParticleColor RosyBrown => new ParticleColor(255, 188, 143, 143);

    /// <summary>
    /// Gets get the color RoyalBlue.
    /// </summary>
    public static ParticleColor RoyalBlue => new ParticleColor(255, 65, 105, 225);

    /// <summary>
    /// Gets get the color SaddleBrown.
    /// </summary>
    public static ParticleColor SaddleBrown => new ParticleColor(255, 139, 69, 19);

    /// <summary>
    /// Gets get the color Salmon.
    /// </summary>
    public static ParticleColor Salmon => new ParticleColor(255, 250, 128, 114);

    /// <summary>
    /// Gets get the color SandyBrown.
    /// </summary>
    public static ParticleColor SandyBrown => new ParticleColor(255, 244, 164, 96);

    /// <summary>
    /// Gets get the color SeaGreen.
    /// </summary>
    public static ParticleColor SeaGreen => new ParticleColor(255, 46, 139, 87);

    /// <summary>
    /// Gets get the color SeaShell.
    /// </summary>
    public static ParticleColor SeaShell => new ParticleColor(255, 255, 245, 238);

    /// <summary>
    /// Gets get the color Sienna.
    /// </summary>
    public static ParticleColor Sienna => new ParticleColor(255, 160, 82, 45);

    /// <summary>
    /// Gets get the color Silver.
    /// </summary>
    public static ParticleColor Silver => new ParticleColor(255, 192, 192, 192);

    /// <summary>
    /// Gets get the color SkyBlue.
    /// </summary>
    public static ParticleColor SkyBlue => new ParticleColor(255, 135, 206, 235);

    /// <summary>
    /// Gets get the color SlateBlue.
    /// </summary>
    public static ParticleColor SlateBlue => new ParticleColor(255, 106, 90, 205);

    /// <summary>
    /// Gets get the color SlateGray.
    /// </summary>
    public static ParticleColor SlateGray => new ParticleColor(255, 112, 128, 144);

    /// <summary>
    /// Gets get the color Snow.
    /// </summary>
    public static ParticleColor Snow => new ParticleColor(255, 255, 250, 250);

    /// <summary>
    /// Gets get the color SpringGreen.
    /// </summary>
    public static ParticleColor SpringGreen => new ParticleColor(255, 0, 255, 127);

    /// <summary>
    /// Gets get the color SteelBlue.
    /// </summary>
    public static ParticleColor SteelBlue => new ParticleColor(255, 70, 130, 180);

    /// <summary>
    /// Gets get the color Tan.
    /// </summary>
    public static ParticleColor Tan => new ParticleColor(255, 210, 180, 140);

    /// <summary>
    /// Gets get the color Teal.
    /// </summary>
    public static ParticleColor Teal => new ParticleColor(255, 0, 128, 128);

    /// <summary>
    /// Gets get the color Thistle.
    /// </summary>
    public static ParticleColor Thistle => new ParticleColor(255, 216, 191, 216);

    /// <summary>
    /// Gets get the color Tomato.
    /// </summary>
    public static ParticleColor Tomato => new ParticleColor(255, 255, 99, 71);

    /// <summary>
    /// Gets get the color Turquoise.
    /// </summary>
    public static ParticleColor Turquoise => new ParticleColor(255, 64, 224, 208);

    /// <summary>
    /// Gets get the color Violet.
    /// </summary>
    public static ParticleColor Violet => new ParticleColor(255, 238, 130, 238);

    /// <summary>
    /// Gets get the color Wheat.
    /// </summary>
    public static ParticleColor Wheat => new ParticleColor(255, 245, 222, 179);

    /// <summary>
    /// Gets get the color White.
    /// </summary>
    public static ParticleColor White => new ParticleColor(255, 255, 255, 255);

    /// <summary>
    /// Gets get the color WhiteSmoke.
    /// </summary>
    public static ParticleColor WhiteSmoke => new ParticleColor(255, 245, 245, 245);

    /// <summary>
    /// Gets get the color Yellow.
    /// </summary>
    public static ParticleColor Yellow => new ParticleColor(255, 255, 255, 0);

    /// <summary>
    /// Gets get the color YelloGreen.
    /// </summary>
    public static ParticleColor YellowGreen => new ParticleColor(255, 154, 205, 50);

    /// <summary>
    /// Gets or sets the red component of the color.
    /// </summary>
    public byte R { get; set; }

    /// <summary>
    /// Gets or sets the green component of the color.
    /// </summary>
    public byte G { get; set; }

    /// <summary>
    /// Gets or sets the blue component of the color.
    /// </summary>
    public byte B { get; set; }

    /// <summary>
    /// Gets or sets the alpha component of the color.
    /// </summary>
    public byte A { get; set; }

    /// <summary>
    /// Returns a value indicating if the given colors are equal.
    /// </summary>
    /// <param name="clrA">The left operand.</param>
    /// <param name="clrB">The right operand.</param>
    /// <returns>True if both operands are equal.</returns>
    public static bool operator ==(ParticleColor clrA, ParticleColor clrB)
    {
        if (clrA is null && clrB is null)
        {
            return true;
        }

        if ((clrA is null && !(clrB is null)) || (!(clrA is null) && clrB is null))
        {
            return false;
        }

        if (!(clrA is null) && !(clrB is null))
        {
            return clrA.Equals(clrB);
        }

        return false;
    }

    /// <summary>
    /// Returns a value indicating if the given colors are not equal.
    /// </summary>
    /// <param name="clrA">The left operand.</param>
    /// <param name="clrB">The right operand.</param>
    /// <returns>True if both operands are not equal.</returns>
    public static bool operator !=(ParticleColor clrA, ParticleColor clrB) => !(clrA == clrB);

    /// <summary>
    /// Creates a new <see cref="ParticleColor"/> using the given color component values.
    /// </summary>
    /// <param name="a">The alpha component of the color.</param>
    /// <param name="r">The red component of the color.</param>
    /// <param name="g">The green component of the color.</param>
    /// <param name="b">The blue component of the color.</param>
    /// <returns>A <see cref="ParticleColor"/> instance.</returns>
    public static ParticleColor FromArgb(byte a, byte r, byte g, byte b) => new ParticleColor(a, r, g, b);

    /// <summary>
    /// Gets the brightness value of the <see cref="ParticleColor"/>.
    /// </summary>
    /// <returns>The brightness of the color.</returns>
    public float GetBrightness()
    {
        var min = Math.Min(Math.Min(R, G), B);
        var max = Math.Max(Math.Max(R, G), B);

        return (max + min) / (byte.MaxValue * 2f) * 100f;
    }

    /// <summary>
    /// Gets the hue value of the <see cref="ParticleColor"/>.
    /// </summary>
    /// <returns>The hue value of the color.</returns>
    public float GetHue()
    {
        if (R == G && G == B)
        {
            return 0f;
        }

        var min = Math.Min(Math.Min(R, G), B);
        var max = Math.Max(Math.Max(R, G), B);

        var delta = (float)(max - min);
        float hue;

        if (R == max)
        {
            hue = (G - B) / delta;
        }
        else if (G == max)
        {
            hue = ((B - R) / delta) + 2f;
        }
        else
        {
            hue = ((R - G) / delta) + 4f;
        }

        hue *= 60f;

        if (hue < 0f)
        {
            hue += 360f;
        }

        return hue;
    }

    /// <summary>
    /// Gets the saturation value of the <see cref="ParticleColor"/>.
    /// </summary>
    /// <returns>The saturation of the color.</returns>
    public float GetSaturation()
    {
        var r = R / 255f;
        var b = B / 255f;

        return (b - r) / (b + r) * 100f;
    }

    /// <summary>
    /// Returns the string representation of the <see cref="ParticleColor"/>.
    /// </summary>
    /// <returns>A string representation of the <see cref="ParticleColor"/>.</returns>
    public override string ToString() => $"A = {A}, R = {R}, G = {G}, B = {B}";

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (!(obj is ParticleColor color))
        {
            return false;
        }

        var alphaSame = A == color.A;
        var redSame = R == color.R;
        var greenSame = G == color.G;
        var blueSame = B == color.B;

        return alphaSame && redSame && greenSame && blueSame;
    }

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode() => HashCode.Combine(R, G, B, A);
}
