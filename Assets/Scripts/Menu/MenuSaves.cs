using System.Collections.Generic;
using UnityEngine;

public class MenuSaves : MonoBehaviour
{
    private string _pathToSaves;
    [SerializeField] private GameObject saveContainerPrefab; 

    private List<MenuSingleSave> _saves = new();

    void Start()
    {
        _pathToSaves = Application.persistentDataPath + "/Saves";
        UniverseDirectories.CreateDirectoryIfNotExists(_pathToSaves);
        GetAllSaves();
    }

    void GetAllSaves()
    {
        var saves = UniverseDirectories.GetFoldersInDirectory(_pathToSaves);
        for (int i = 0; i < saves.Length; i ++)
        {
            SaveContainer container = Instantiate(saveContainerPrefab, new Vector3(400, 550 - 350 * i), Quaternion.identity,  GameObject.Find("SavesContainer").transform).GetComponent<SaveContainer>();
            container.Initialize(saves[i]);
        }
    }
}
