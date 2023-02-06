using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public class PlanetSaveData
    {
        
    }

    public class GravityControllerSaveData
    {
        
    }
    
    
    public string LevelName = "New Level";
    private string filePath;

    public void SaveCurrentLevel()
    {
        filePath = Application.persistentDataPath + LevelName.Replace(" ", "_") + ".json";
        
        List<object> objectsToSave = GetSaveObjects();
        string json = "";

        foreach (var obj in objectsToSave)
        {
            json += JsonConvert.SerializeObject(obj);
        }
        
        File.WriteAllText(filePath, json);
    }
    
    List<object> GetSaveObjects()
    {
        // objects to save:
        // planets
        // gravity objects controller

        List<object> saveObjs = new List<object>();
        
        // add gravity controller
        saveObjs.Add(GravityObjectsController.Instance);
        foreach (var grav in GravityObjectsController.Instance.AllGravityObjects)
        {
            saveObjs.Add(grav.gameObject);
        }
        
        return saveObjs;
    }
}
