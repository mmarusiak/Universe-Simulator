using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class LevelSaveData
{
    [JsonProperty] private List<PlanetComponentSaveData> _savedComponents = new();
    [JsonProperty] public bool IsPaused { get; }
    [JsonProperty] public bool IsReset { get; }
    [JsonProperty] public float TimeScale { get; }
    [JsonProperty] public string LevelName { get; }

    [JsonIgnore] public List<PlanetComponentSaveData> SavedComponents => _savedComponents;
    
    [JsonConstructor]
    public LevelSaveData(List<PlanetComponentSaveData> dataList, bool isP, bool isR, float timeS, string levelName)
    {
        _savedComponents = dataList;
        IsPaused = isP;
        IsReset = isR;
        TimeScale = timeS;
        LevelName = levelName;
    }
    
    public LevelSaveData(PlanetComponentsController components, PlaybackController playback)
    {
        _savedComponents = new();
        foreach (var comp in components.AllGravityComponents)
        {
            _savedComponents.Add(comp);
        }
        IsPaused = playback.Playback.IsPaused;
        IsReset = playback.Playback.IsReset;
        TimeScale = playback.Playback.TimeScale;
        LevelName = LevelInfoHolder.Instance.LevelName;
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
