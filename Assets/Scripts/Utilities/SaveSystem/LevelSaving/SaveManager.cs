using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public class VectorSaveData
    {
        public float X { get; set; }
        public float Y { get; set; }

        public VectorSaveData(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public static implicit operator Vector2 (VectorSaveData data) => new(data.X, data.Y);
        public static implicit operator VectorSaveData(Vector2 vec) => new VectorSaveData(vec.x, vec.y);
    }

    public class ColorSaveData
    {
        
    }
    
    public class PlanetSaveData
    {
        public VectorSaveData InitialPos { get; set; }
        public VectorSaveData InitialVel { get; set; }
        public string Name;
        public float Radius;
        public float Mass;

        public PlanetSaveData(VectorSaveData initialPos, VectorSaveData initialVel, string name, float radius, float mass)
        {
            this.InitialPos = initialPos;
            this.InitialVel = initialVel;
            this.Name = name;
            this.Radius = radius;
            this.Mass = mass;
        }

        public static implicit operator PlanetSaveData(GravityObject grav) => new PlanetSaveData(grav.StartPos,
            grav.InitialVelocity, grav.PlanetName, grav.Radius, grav.Mass);

        
        public void InitializeData(GravityObject receiver)
        {
            receiver.StartPos = InitialPos;
            receiver.InitialVelocity = InitialVel;
            receiver.Mass = Mass;
            receiver.Radius = Radius;
        }
    }
    
    
    public string LevelName = "New Level";
    private string filePath;

    public void SaveCurrentLevel()
    {
        filePath = Application.persistentDataPath + LevelName.Replace(" ", "_") + ".json";

        List<PlanetSaveData> dataList = GetSaveData();
        string json = "";

        foreach (var data in dataList)
        {
            json += JsonConvert.SerializeObject(data);
        }
        Debug.Log(filePath);
        File.WriteAllText(filePath, json);
    }
    
    List<PlanetSaveData> GetSaveData()
    {
        List<PlanetSaveData> planetsDataList = new List<PlanetSaveData>();

        foreach(var data in GravityObjectsController.Instance.AllGravityObjects)
            planetsDataList.Add(data);

        return planetsDataList;
    }
}
