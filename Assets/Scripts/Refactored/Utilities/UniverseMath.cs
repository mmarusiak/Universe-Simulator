using System.Globalization;

public static class UniverseMath
{
    public static float StringToFloat(string str) 
    {
        return float.Parse(str, CultureInfo.InvariantCulture.NumberFormat);
    }
}
