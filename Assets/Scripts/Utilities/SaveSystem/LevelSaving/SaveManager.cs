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
        string json = JsonConvert.SerializeObject(SavedData.GetData(dataList));

        Debug.Log(filePath);
        File.WriteAllText(filePath, json);
    }

    public void LoadData()
    {
        SavedData data = GetLoadedData<SavedData>(LevelToLoad);
        Debug.Log(data.Data.Count);
    }
    
    public T GetLoadedData<T>(string levelName){
        
        filePath = Application.persistentDataPath + levelName.Replace(" ", "_") + ".json";
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(json);
        }
        else
        {
            Debug.LogError("Save file not found in " + filePath);
            return default(T);
        }
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
        private float X { get; set; }
        private float Y { get; set; }

        private VectorSaveData(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static implicit operator Vector2 (VectorSaveData data) => new(data.X, data.Y);
        public static implicit operator VectorSaveData(Vector2 vec) => new(vec.x, vec.y);
    }

    public class ColorSaveData
    {
        
    }
    
    public class PlanetSaveData
    {
        private VectorSaveData InitialPos { get; set; }
        private VectorSaveData InitialVel { get; set; }
        private string Name;
        private float Radius;
        private float Mass;

        [JsonConstructor]
        private PlanetSaveData(VectorSaveData initialPos, VectorSaveData initialVel, string name, float radius, float mass)
        {
            InitialPos = initialPos;
            InitialVel = initialVel;
            Name = name;
            Radius = radius;
            Mass = mass;
        }

        public static implicit operator PlanetSaveData(GravityObject grav) => new(grav.StartPos, grav.InitialVelocity, 
            grav.PlanetName, grav.Radius, grav.Mass);

        
        public void InitializeData(GravityObject receiver)
        {
            receiver.StartPos = InitialPos;
            receiver.InitialVelocity = InitialVel;
            receiver.Mass = Mass;
            receiver.Radius = Radius;
        }
    }

    public class SavedData
    {
        public List<PlanetSaveData> Data;

        public SavedData (List<PlanetSaveData> data) => Data = data;

        public static SavedData GetData(List<PlanetSaveData> data) { return new SavedData(data); }

        public static implicit operator SavedData(List<PlanetSaveData> data) => new (data);
        
        public static implicit operator List<PlanetSaveData> (SavedData data) => data.Data;
    }

}
