using System.Collections.Generic;
using GameCore.LevelController;
using GameCore.SimulationCore;
using Newtonsoft.Json;

namespace Utilities.SaveSystem.Data
{
    public class LevelSaveData
    {
        [JsonProperty] private List<PlanetComponentSaveData> _savedComponents = new();
        [JsonProperty] public bool IsPaused { get; }
        [JsonProperty] public bool IsReset { get; }
        [JsonProperty] public float TimeScale { get; }
        [JsonProperty] public string LevelName { get; }

        [JsonIgnore] public List<PlanetComponentSaveData> SavedComponents => _savedComponents;
    
        [JsonConstructor]
        public LevelSaveData(List<PlanetComponentSaveData> dataList, bool isPaused, bool isReset, float timeScale, string levelName)
        {
            _savedComponents = dataList;
            IsPaused = isPaused;
            IsReset = isReset;
            TimeScale = timeScale;
            LevelName = levelName;
        }
    
        public LevelSaveData(PlanetComponentsController components, PlaybackController playback)
        {
            IsPaused = playback.Playback.IsPaused;
            IsReset = playback.Playback.IsReset;
            TimeScale = playback.Playback.TimeScale;
            LevelName = LevelInfoHolder.Instance.LevelName;
        
            _savedComponents = new();
            foreach (var comp in components.AllGravityComponents)
            {
                if(!comp.Handler.isCreatedByPlayerInLogicLevel) _savedComponents.Add(comp);
            }
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
}
