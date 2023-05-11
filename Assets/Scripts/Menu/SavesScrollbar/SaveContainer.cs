using System;
using System.Globalization;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Utilities.UniverseLibraries;

namespace Menu.SavesScrollbar
{
    public class SaveContainer : MonoBehaviour
    {
        private string _saveName, _pathToSave;
        private Sprite _saveCapture;
        private Image _savePreviewContainer;
        private Text _saveText, _dateText;
        private RectTransform _view;
        private DateTime _lastModified;

        public DateTime LastModified => _lastModified;

        public string SaveName => _saveName;
        public async Task Initialize(string saveName)
        {
            _view = transform.GetChild(0).GetComponent<RectTransform>();
            _savePreviewContainer = _view.GetChild(0).transform.Find("SavePreview").GetComponent<Image>();
            _saveText = _view.GetChild(0).transform.Find("SaveName").GetComponent<Text>();
            _dateText = _view.GetChild(0).transform.Find("DateText").GetComponent<Text>();
        
            _pathToSave = Application.persistentDataPath + "/Saves/" + saveName;
        
            _saveName = saveName;
            _saveText.text = _saveName;
            gameObject.name = _saveName;
        
            _lastModified = UniverseDirectories.LastTimeModified(_pathToSave + "/capture.png");
            _dateText.text = _lastModified.ToString(CultureInfo.InvariantCulture);
        
            _savePreviewContainer.transform.localScale.Set(.15f, .15f, .15f);
            _saveCapture = await UniversePictures.LoadSpriteFromPath(_pathToSave + "/capture.png", 1920, 1080);
            _savePreviewContainer.sprite = _saveCapture;
        
        }

        void Update()
        {
            _view.gameObject.SetActive(_view.position.y < Screen.currentResolution.height/1.65);
        }
    }
}
