// <copyright file="EasingFunctions.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Plazma;

/// <summary>
/// Provides various easing functions.
/// </summary>
public static class EasingFunctions
{
    /// <summary>
    /// Ease out bounce easing function.
    /// </summary>
    /// <param name="t">The current time/step in milliseconds.</param>
    /// <param name="b">The starting value.</param>
    /// <param name="c">The amount of change. (end - start).</param>
    /// <param name="d">The total amount of time(milliseconds)/steps.</param>
    /// <returns>The result of the easing function.</returns>
    public static double EaseOutBounce(double t, double b, double c, double d)
    {
        t /= 1000.0; // Convert to seconds
        d /= 1000.0; // Convert to seconds

        if ((t /= d) < 0.36363636363636363636363636363636)
        {
            return (c * (7.5625 * t * t)) + b;
        }
        else if (t < 0.72727272727272727272727272727273)
        {
            return (c * ((7.5625 * (t -= 0.54545454545454545454545454545455) * t) + 0.75)) + b;
        }
        else if (t < 0.90909090909090909090909090909091)
        {
            return (c * ((7.5625 * (t -= 0.81818181818181818181818181818182) * t) + 0.9375)) + b;
        }
        else
        {
            return (c * ((7.5625 * (t -= 0.9) * t) + 0.95454545454545454545454545454545)) + b;
        }
    }

    /// <summary>
    /// Ease in quad easing function.
    /// </summary>
    /// <param name="t">The current time/step.</param>
    /// <param name="b">The starting value.</param>
    /// <param name="c">The amount of change. (end - start).</param>
    /// <param name="d">The total amount of time(milliseconds)/steps.</param>
    /// <returns>The result of the easing function.</returns>
    public static double EaseInQuad(double t, double b, double c, double d)
    {
        t /= 1000.0; // Convert to seconds
        d /= 1000.0; // Convert to seconds

        t /= d;

        return (c * t * t) + b;
    }
}
