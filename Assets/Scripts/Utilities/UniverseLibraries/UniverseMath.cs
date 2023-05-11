using System.Globalization;
using UnityEngine;

namespace Utilities.UniverseLibraries
{
    public static class UniverseMath
    {
        public static float StringToFloat(string str) 
        {
            return float.Parse(str, CultureInfo.InvariantCulture.NumberFormat);
        }

        // y = ax + b, where a is slope and b is bias
        public static void GetLinearFunctionParams(Vector2 pointA, Vector2 pointB, out float slope, out float bias)
        {
            slope = (pointA.y - pointB.y) / (pointA.x - pointB.x);
            bias = pointA.y - pointA.x * slope;
        }
    }
}
