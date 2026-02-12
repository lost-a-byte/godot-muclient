using System;
namespace MuClient.Extensions;

public static class NumericExtensions
{
    /// <summary>
    /// Converts a value from degrees to radians.
    /// </summary>
    /// <param name="val">The value to convert to radians.</param>
    /// <returns>The value in radians.</returns>
    public static double ToRadians(this double val)
    {
        return Math.PI / 180.0 * val;
    }
    /// <summary>
    /// Converts a value from degrees to radians.
    /// </summary>
    /// <param name="val">The value to convert to radians.</param>
    /// <returns>The value in radians.</returns>
    public static double ToRadians(this float val)
    {
        return Math.PI / 180.0 * val;
    }
    /// <summary>
    /// Converts a value from degrees to radians.
    /// </summary>
    /// <param name="val">The value to convert to radians.</param>
    /// <returns>The value in radians.</returns>
    public static double ToRadians(this int val)
    {
        return Math.PI / 180.0 * val;
    }
}