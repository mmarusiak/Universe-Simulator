using UnityEngine;

public class MenuSaves : MonoBehaviour
{
    private string _pathToSaves;
    
    
    void Start()
    {
        _pathToSaves = Application.persistentDataPath + "/Saves";
        UniverseDirectories.CreateDirectoryIfNotExists(_pathToSaves);
    }
    
    public void GetAllSaves()
    {
        
    }
}
