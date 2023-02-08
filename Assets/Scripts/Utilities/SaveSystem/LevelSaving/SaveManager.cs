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

    public void LoadData() => GravityObjectsController.Instance.SetList(GetLoadedData<List<PlanetSaveData>>(LevelToLoad));
    
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

        foreach(var data in GravityObjectsController.Instance.AllGravityObjects)
            planetsDataList.Add(data);

        return planetsDataList;
    }
    
    
    public class VectorSaveData
    {
        [JsonProperty]
        private float X { get; set; }
        [JsonProperty]
        private float Y { get; set; }

        [JsonConstructor]
        public VectorSaveData(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static implicit operator Vector2 (VectorSaveData data) => new(data.X, data.Y);
        public static implicit operator VectorSaveData(Vector2 vec) => new(vec.x, vec.y);
    }

    public class PlanetSaveData
    {
        [JsonProperty]
        private readonly VectorSaveData _initialPos;
        [JsonProperty]
        private readonly VectorSaveData _initialVel;
        [JsonProperty]
        private readonly string _name;
        [JsonProperty]
        private readonly float _radius;
        [JsonProperty]
        private readonly float _mass;

        [JsonConstructor]
        private PlanetSaveData(VectorSaveData initialPos, VectorSaveData initialVel, string name, float radius, float mass)
        {
            _initialPos = initialPos;
            _initialVel = initialVel;
            _name = name;
            _radius = radius;
            _mass = mass;
        }

        public static implicit operator PlanetSaveData(GravityObject grav) => new(grav.InitialPos, grav.InitialVelocity, 
            grav.PlanetName, grav.Radius, grav.Mass);

        
        public void InitializeData(GravityObject receiver)
        {
            receiver.InitialPos = _initialPos;
            receiver.InitialVelocity = _initialVel;
            receiver.Mass = _mass;
            receiver.Radius = _radius;
        }
    }
}
