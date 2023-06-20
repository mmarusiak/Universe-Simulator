using System.IO;
using System.Threading.Tasks;
using GameCore.LevelController;
using GameCore.SimulationCore;
using Newtonsoft.Json;
using PauseOverlay;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities.SaveSystem.Data;
using Utilities.UniverseLibraries;

namespace Utilities.SaveSystem
{
    public class SavingHandler : MonoBehaviour
    {
        public static SavingHandler Instance;
        private static SavingHandler _next;
        private string _pathToSaves;
    
        void Awake()
        {
            // menu instance
            if (Instance == null) Instance = this;
            // on load instance
            else _next = this;
        
            _pathToSaves = Application.persistentDataPath + "/Saves";
            Debug.Log(_pathToSaves);
        }
    
        private string _saveFileName = "data_save";
        private string _captureFileName = "capture";
        public string SaveFileName => _saveFileName;
        public string CaptureFileName => _captureFileName;

        public async void SaveLevel()
        {
            string saveName = LevelInfoHolder.Instance.LevelName;
            string pathToTargetSave = _pathToSaves + "/" + saveName;
            UniverseDirectories.CreateNewDirectory(_pathToSaves, saveName);
        
            // saving save data
            LevelSaveData newData = new LevelSaveData(PlanetComponentsController.Instance, PlaybackController.Instance);
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            string jsonData = JsonConvert.SerializeObject(newData, settings);
            File.WriteAllText(pathToTargetSave + "/" + _saveFileName + ".json", jsonData);
            await UniversePictures.TakeGameScreenshot(_pathToSaves + "/" + saveName + "/" + _captureFileName + ".png");
        }

        public async Task LoadLevel(bool fromMenu, string saveName = "new_save")
        {
            while (PlanetComponentsController.Instance == null) await Task.Yield();
            PlanetComponentsController.Instance.ClearLevel();
            LevelSaveData newData = GetLoadedData<LevelSaveData>(saveName);
            // load planets
            newData.LoadList();
            // load playback
            PlaybackController.Instance.Playback.IsPaused = newData.IsPaused;
            PlaybackController.Instance.Playback.IsReset = newData.IsReset;
            PlaybackController.Instance.Playback.TimeScale = newData.TimeScale;
            PlaybackController.Instance.Playback.SetButtonsColors();
            // load name
            LevelInfoHolder.Instance.LevelName = newData.LevelName;
            // calling overlay that there is no more temp pause
            PauseOverlayHandler.Instance.OnLoad();

            if (fromMenu)
            {
                Instance = _next;
                Destroy(gameObject);
            }
        }

        public async Task CreateNewLevel(InputField inputName)
        {
            string name = inputName.text;
            if (string.Empty == name) return;

            DontDestroyOnLoad(this);
            UniverseScenes.LoadScene("Game");

            while (LevelInfoHolder.Instance == null) await Task.Yield();

            LevelInfoHolder.Instance.LevelName = name;
            Instance = _next;
            Instance.SaveLevel();
            Instance.InitializeNewLevel();
            Destroy(gameObject);
        }
    
        private T GetLoadedData<T>(string saveName = "new_save")
        {
            if (string.Empty == saveName) return default;
            string targetPath = _pathToSaves + "/" + saveName + "/" + _saveFileName + ".json";
            if (!File.Exists(targetPath))
            {
                Debug.LogError("Save file not found in " + targetPath);
                return default;
            }
        
            string json = File.ReadAllText(targetPath);
            return JsonConvert.DeserializeObject<T>(json);
        }

        private void InitializeNewLevel() => PlaybackController.Instance.ResetLevel();
        
    }
}
