using Newtonsoft.Json;
using UnityEngine;

public class SliceData
{
    // points where slice was made
    [JsonProperty]
    private float _x1, _y1, _x2, _y2;
    // which part of slice is taken
    [JsonProperty] 
    private int _sliceIndex;

    public Vector2 StartPoint => new (_x1, _y1);
    public Vector2 EndPoint => new (_x2, _y2);

    public int SliceIndex() => _sliceIndex;
    
    public SliceData(Vector2 pointA, Vector2 pointB, int index)
    {
        _x1 = pointA.x;
        _x2 = pointB.x;
        _y1 = pointA.y;
        _y2 = pointB.y;
        _sliceIndex = index;
    }
    
    [JsonConstructor]
    public SliceData(int x1, int y1, int x2, int y2, int index)
    {
        _x1 = x1;
        _x2 = x2;
        _y1 = y1;
        _y2 = y2;
        _sliceIndex = index;
    }
}
