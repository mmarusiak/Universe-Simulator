using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utilities.SaveSystem;
using Utilities.UniverseLibraries;

namespace GameCore.LevelController
{
    public class LevelInfoHolder : MonoBehaviour
    {
        public static LevelInfoHolder Instance;
        void Awake() => Instance = this;

        private string _levelName = "new_level";
        [SerializeField] private InputField nameField;

        public string LevelName
        {
            get => _levelName;
            set => SetNewName(value);
        }

        public void OnLevelNameChanged(string value)
        {
            // validating input
            if (value == _levelName || string.Empty == value) return;
            // quitting input field - hiding it if mouse is not over it
            EventSystem.current.SetSelectedGameObject(null);
            // getting save paths
            string oldPath = Application.persistentDataPath + "/" + _levelName;
            string newPath = Application.persistentDataPath + "/" + value;
            SetNewName(value);
            // renaming save path
            MoveSaves(oldPath, newPath);
            // save current level to new path
            SavingHandler.Instance.SaveLevel();
        }
    
        void SetNewName(string value)
        {
            // set new level name to new value
            _levelName = value;
            nameField.text = _levelName;
        }

        void MoveSaves(string oldPath, string newPath)
        {
            if (!Directory.Exists(oldPath)) return;
            UniverseDirectories.RenameDirectory(oldPath, newPath);
        }
    }
}
