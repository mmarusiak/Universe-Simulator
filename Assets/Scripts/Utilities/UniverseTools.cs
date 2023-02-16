using System;
using System.Globalization;
using System.IO;
using UnityEngine;

public class UniverseTools : MonoBehaviour
{
    public static string RoundOutput(float input, int lenRound = 3)
    {
        int negOrPosFlag = input > 0 ? 1 : -1;
        int rounder = Math.Abs((int) Mathf.Pow(10, ((int) input).ToString(CultureInfo.InvariantCulture).Length));
        string output =
            (negOrPosFlag * Mathf.Round(input * rounder) / rounder).ToString(CultureInfo.InvariantCulture);

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
}


