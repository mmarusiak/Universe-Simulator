using System;
using System.Globalization;
using System.IO;
using UnityEngine;

public class UniverseTools : MonoBehaviour
{
    public static string RoundOutput(float input, int lenRound = 3)
    {
        int rounder = (int) Mathf.Pow(10, ((int) input).ToString(CultureInfo.InvariantCulture).Length);
        string output =
            (Mathf.Round(input * rounder) / rounder).ToString(CultureInfo.InvariantCulture);

        if (output.Contains("."))
        {
            while (output.Split(".")[1].Length < lenRound)
            {
                output += "0";
            }
        }
        else
            output += ".000";

        return output;
    }

    public static bool IsSafeImage(string filePath)
    {
        try
        {
            using FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            byte[] header = new byte[8];
            stream.Read(header, 0, 8);
            string headerString = BitConverter.ToString(header).Replace("-", string.Empty);

            // check header of a known image format (JPEG, PNG, or GIF) by reading first 8 bytes
            if (headerString.StartsWith("FFD8FF") ||
                headerString.StartsWith("89504E47") ||
                headerString.StartsWith("47494638"))
            {
                return true;
            }

            return false;
        }
        catch (FileNotFoundException)
        {
            return false;
        }
    }
    

    public static class OrbitCalculator
    {
        public class Planet
        {  
            public float Mass { get; set; }
            public float[] Velocity { get; set; }
            public float[] Position { get; set; }

            public Planet(float mass, float[] velocity, float[] position)
            {
                Mass = mass;
                Velocity = velocity;
                Position = position;
            }

            public static implicit operator Planet(GravityObject value)
            {
                return new Planet(value.Mass, new []{value.InitialVelocity.x, value.InitialVelocity.y}, new [] {value.InitialPos.x, value.InitialPos.y});
            }
        }

        public static float[] MinimumVelocity(Planet star, Planet target, Planet[] others)
        {
            //float G = 6.67430E-11f;
            float G = GlobalVariables.GravitationalConstant; // Gravitational constant

            float[] targetToStar = new float[2];
            for (int i = 0; i < 2; i++)
            {
                targetToStar[i] = star.Position[i] - target.Position[i];
            }

            float targetToStarDistance = (float) Math.Sqrt(targetToStar[0] * targetToStar[0] +
                                                           targetToStar[1] * targetToStar[1]);

            float starGravitationalPotential = -G * star.Mass / targetToStarDistance;

            float[] targetRelativeVelocity = new float[2];
            for (int i = 0; i < 2; i++)
            {
                targetRelativeVelocity[i] = star.Velocity[i] - target.Velocity[i];
            }

            float targetRelativeSpeed = (float) Math.Sqrt(targetRelativeVelocity[0] * targetRelativeVelocity[0] +
                                                          targetRelativeVelocity[1] * targetRelativeVelocity[1]);

            float otherGravitationalPotential = 0;
            
            foreach (var t in others)
            {
                float[] targetToOther = new float[2];
                for (int j = 0; j < 2; j++)
                {
                    targetToOther[j] = t.Position[j] - target.Position[j];
                }

                float targetToOtherDistance = (float) Math.Sqrt(targetToOther[0] * targetToOther[0] +
                                                                targetToOther[1] * targetToOther[1]);

                otherGravitationalPotential += -G * t.Mass / targetToOtherDistance;

                float[] targetRelativeOtherVelocity = new float[2];
                for (int j = 0; j < 2; j++)
                {
                    targetRelativeOtherVelocity[j] = t.Velocity[j] - target.Velocity[j];
                }

                float targetRelativeOtherSpeed = (float) Math.Sqrt(
                    targetRelativeOtherVelocity[0] * targetRelativeOtherVelocity[0] +
                    targetRelativeOtherVelocity[1] * targetRelativeOtherVelocity[1]);

                otherGravitationalPotential += targetRelativeOtherSpeed * targetRelativeOtherSpeed / 2;
            }

            float minimumSpeed = (float) Math.Sqrt(2 * (starGravitationalPotential + otherGravitationalPotential) +
                                                   targetRelativeSpeed * targetRelativeSpeed);

            float[] minimumVelocity = new float[2];
            for (int i = 0; i < 2; i++)
            {
                minimumVelocity[i] = (targetRelativeVelocity[i] / targetRelativeSpeed) * minimumSpeed;
            }

            return minimumVelocity;
        }
    }
}


