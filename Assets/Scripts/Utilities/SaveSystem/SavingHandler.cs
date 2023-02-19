using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class SavingHandler : MonoBehaviour
{
    public static SavingHandler Instance;
    private string _pathToSaves;

    void Awake()
    {
        Instance = this;
        _pathToSaves = Application.persistentDataPath;
    }

    [SerializeField] private string _levelName = "new_save";
    private static string _saveFileName = "data_save";

    public void SaveLevel()
    {
        string saveName = UniverseTools.RemoveAccents(_levelName).Replace(" ", "_");
        string pathToTargetSave = _pathToSaves + "/" + saveName;
        UniverseDirectories.CreateNewDirectory(_pathToSaves, saveName);
        
        LevelSaveData newData = new LevelSaveData(PlanetComponentsController.Instance, PlaybackController.Instance);
        Debug.Log(newData.SavedComponents.Count);
        var settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        string jsonData = JsonConvert.SerializeObject(newData, settings);
        Debug.Log(jsonData.Split("_planetName").Length);
        File.WriteAllText(pathToTargetSave + "/" + _saveFileName + ".json", jsonData);
    }

    public void LoadLevel(string saveName = "new_save")
    {
        PlanetComponentsController.Instance.ClearLevel();
        LevelSaveData newData = GetLoadedData<LevelSaveData>(saveName);
        // load planets
        newData.LoadList();
        // load playback
        PlaybackController.Instance.Playback.IsPaused = newData.IsPaused;
        PlaybackController.Instance.Playback.IsReset = newData.IsReset;
        PlaybackController.Instance.Playback.TimeScale = newData.TimeScale;
        // calling overlay that there is no more temp pause
        PauseOverlayHandler.Instance.OnLoad();
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
}
