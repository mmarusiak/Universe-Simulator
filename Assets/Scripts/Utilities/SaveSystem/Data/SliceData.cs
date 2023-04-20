using Newtonsoft.Json;
using UnityEngine;

public class SliceData
{
    // points where slice was made
    [JsonProperty] private VectorSaveData _start, _end;
    // which part of slice is taken
    [JsonProperty] private int _sliceIndex;
    [JsonProperty] public bool CreatedOnReset { get; }

    [JsonIgnore] public Vector2 StartPoint => _start;
    [JsonIgnore] public Vector2 EndPoint => _end;

    public int SliceIndex() => _sliceIndex;
    
    public SliceData(Vector2 pointA, Vector2 pointB, int index, bool createdOnReset)
    {
        _start = pointA;
        _end = pointB;
        _sliceIndex = index;
        CreatedOnReset = createdOnReset;
    }
    
    [JsonConstructor]
    public SliceData(VectorSaveData start, VectorSaveData end, int index, bool createdOnReset)
    {
        _start = start;
        _end = end;
        _sliceIndex = index;
        CreatedOnReset = createdOnReset;
    }
}
