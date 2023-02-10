using Newtonsoft.Json;
using UnityEngine;

public class ColorSaveData
{
    [JsonProperty] private float _r, _g, _b, _a;

    [JsonConstructor]
    public ColorSaveData(float r, float g, float b, float a)
    {
        _r = r;
        _g = g;
        _b = b;
        _a = a;
    }

    public static implicit operator ColorSaveData(Color color) => new (color.r, color.g, color.b, color.a);
    public static implicit operator Color(ColorSaveData data) => new (data._r, data._g, data._b, data._a);
}
