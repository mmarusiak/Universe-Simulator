using System.Collections.Generic;
using GameCore.LevelController;
using GameCore.SimulationCore;
using LogicLevels;
using Newtonsoft.Json;

namespace Utilities.SaveSystem.Data
{
    public class LevelSaveData
    {
        [JsonProperty] private List<PlanetComponentSaveData> _savedComponents;
        [JsonProperty] public List<LogicAreaData> LogicAreaDataList = new();
        [JsonProperty] public List<LogicVelocityData> LogicVelocityList = new();
        [JsonProperty] public bool IsSandbox { get; }
        [JsonProperty] public bool IsPaused { get; }
        [JsonProperty] public bool IsReset { get; }
        [JsonProperty] public float TimeScale { get; }
        [JsonProperty] public string LevelName { get; }
        [JsonProperty] public int PlanetActions { get; }

        [JsonIgnore] public List<PlanetComponentSaveData> SavedComponents => _savedComponents;
    
        [JsonConstructor]
        public LevelSaveData(List<PlanetComponentSaveData> dataList, List<LogicAreaData> areas, List<LogicVelocityData> velocities, bool isSandbox, bool isPaused, bool isReset, float timeScale, string levelName, int planetActions)
        {
            _savedComponents = dataList;
            IsSandbox = isSandbox;
            IsPaused = isPaused;
            IsReset = isReset;
            TimeScale = timeScale;
            LevelName = levelName;
            PlanetActions = planetActions;
            LogicAreaDataList = areas;
            LogicVelocityList = velocities;
        }
    
        public LevelSaveData(PlanetComponentsController components, PlaybackController playback)
        {
            IsSandbox = LogicLevelController.Instance == null;
            IsPaused = playback.Playback.IsPaused;
            IsReset = playback.Playback.IsReset;
            TimeScale = playback.Playback.TimeScale;
            LevelName = LevelInfoHolder.Instance.LevelName;

            if (!IsSandbox)
            {
                LogicAreaDataList = LogicLevelController.Instance.AreaDataList;
                LogicVelocityList = LogicLevelController.Instance.VelocityDataList;
                PlanetActions = LogicLevelController.Instance.PlanetActions;
            }
        
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
