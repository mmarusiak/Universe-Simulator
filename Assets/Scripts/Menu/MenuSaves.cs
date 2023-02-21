using System.Collections.Generic;
using UnityEngine;

public class MenuSaves : MonoBehaviour
{
    private string _pathToSaves;

    private List<MenuSingleSave> _saves = new();

    void Start()
    {
        _pathToSaves = Application.persistentDataPath + "/Saves";
        UniverseDirectories.CreateDirectoryIfNotExists(_pathToSaves);
        GetAllSaves();
    }

    void GetAllSaves()
    {
            
    }
}
