using System.IO;
using UnityEngine;

public class Level
{
    // fields
    private string _name;
    private bool _isPaused;
    private bool _isReset;
    
    // properties
    public string Name
    {
        get => _name;
        set => SetName(value);
    }


    void SetName(string newName)
    {
        if(newName == _name) return;
        string currentPath = Directory.GetCurrentDirectory();
        string oldSavePath = currentPath + "/" + newName;
        Debug.Log(oldSavePath);
        
        // delete old directory for saves
        if (Directory.Exists(oldSavePath)) Directory.Delete(oldSavePath, true);
        
        _name = newName;
        // create new directory for saves
        UniverseDirectories.CreateNewDirectory(currentPath, _name);
    }

    public void LoadLevelToScene()
    {
        
    }
}
