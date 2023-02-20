using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelInfoHolder : MonoBehaviour
{
    public static LevelInfoHolder Instance;
    void Awake() => Instance = this;

    private string _levelName = "new_level";
    [SerializeField] private InputField nameField;

    public string LevelName
    {
        get => _levelName;
        set => Submit(value);
    }
    public void Submit(string value)
    {
        if (value == _levelName || string.Empty == value) return;
        // quitting input field - hiding it if mouse is not over it
        EventSystem.current.SetSelectedGameObject(null);
        // getting save paths
        string oldPath = Application.persistentDataPath + "/" + _levelName;
        string newPath = Application.persistentDataPath + "/" + value;
        // set new level name to new value
        _levelName = value;
        nameField.text = _levelName;
        // renaming save path
        MoveSaves(oldPath, newPath);
    }

    void MoveSaves(string oldPath, string newPath)
    {
        if(Directory.Exists(oldPath)) UniverseDirectories.RenameDirectory(oldPath, newPath);
        else SavingHandler.Instance.SaveLevel();
    }
}
