using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public string LevelName = "New Level", LevelToLoad = "Test";
    private string filePath;

    public void SaveCurrentLevel(string levelName)
    {
        if (levelName == "") levelName = LevelName;
        
        filePath = Application.persistentDataPath + levelName.Replace(" ", "_") + ".json";

        List<PlanetSaveData> dataList = GetSaveData();
        string json = JsonConvert.SerializeObject(dataList);

        Debug.Log(filePath);
        File.WriteAllText(filePath, json);
    }

    //public void LoadData() => GravityObjectsController.Instance.SetList(GetLoadedData<List<PlanetSaveData>>(LevelToLoad));
    
    private T GetLoadedData<T>(string levelName){
        
        filePath = Application.persistentDataPath + levelName.Replace(" ", "_") + ".json";
        if (!File.Exists(filePath))
        {
            Debug.LogError("Save file not found in " + filePath);
            return default;
        }
        
        string json = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<T>(json);

    }

    List<PlanetSaveData> GetSaveData()
    {
        List<PlanetSaveData> planetsDataList = new List<PlanetSaveData>();

        /*foreach(var data in GravityObjectsController.Instance.AllGravityObjects)
            planetsDataList.Add(data);*/

        return planetsDataList;
    }
}
