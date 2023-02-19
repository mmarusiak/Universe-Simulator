using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class LevelSaveData
{
    [JsonProperty] private List<PlanetComponentSaveData> _savedComponents = new();
    [JsonProperty] private bool _isPaused, _isReset;
    [JsonProperty] private float _timeScale;

    [JsonIgnore]
    public List<PlanetComponentSaveData> SavedComponents => _savedComponents;

    [JsonIgnore]
    public bool IsPaused => _isPaused;
    [JsonIgnore]
    public bool IsReset => _isReset;
    [JsonIgnore]
    public float TimeScale => _timeScale;

    [JsonConstructor]
    public LevelSaveData(List<PlanetComponentSaveData> dataList, bool isP, bool isR, float timeS)
    {
        _savedComponents = dataList;
        _isPaused = isP;
        _isReset = isR;
        _timeScale = timeS;
    }
    
    public LevelSaveData(PlanetComponentsController components, PlaybackController playback)
    {
        _savedComponents = new();
        foreach (var comp in components.AllGravityComponents)
        {
            _savedComponents.Add(comp);
            Debug.Log(_savedComponents.Count);
        }
        Debug.Log(_savedComponents.Count);
        _isPaused = playback.Playback.IsPaused;
        _isReset = playback.Playback.IsReset;
        _timeScale = playback.Playback.TimeScale;
    }

    public void LoadList()
    {
        List<PlanetComponent> newList = new ();
        foreach (var data in _savedComponents)
        {
            PlanetComponent newComp = data;
        }
    }
}
