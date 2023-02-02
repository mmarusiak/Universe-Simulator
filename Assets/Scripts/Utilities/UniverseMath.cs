using System.Globalization;
using UnityEngine;

public class UniverseMath : MonoBehaviour
{
    public static string RoundOutput(float input, int lenRound = 3)
    {
        int rounder = (int)Mathf.Pow(10, ((int)input).ToString(CultureInfo.InvariantCulture).Length);
        string output = 
            (Mathf.Round(input*rounder)/rounder).ToString(CultureInfo.InvariantCulture);

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
}
