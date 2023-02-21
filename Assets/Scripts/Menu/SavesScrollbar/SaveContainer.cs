using UnityEngine;
using UnityEngine.UI;

public class SaveContainer : MonoBehaviour
{
    private string _saveName, _pathToSave;
    private Sprite _saveCapture;
    private Image _savePreviewContainer;
    private Text _saveText;
    private RectTransform _view;

    public string SaveName => _saveName;
    public async void Initialize(string saveName)
    {
        _view = transform.GetChild(0).GetComponent<RectTransform>();
        _savePreviewContainer = _view.transform.Find("SavePreview").GetComponent<Image>();
        _saveText = _view.transform.Find("SaveName").GetComponent<Text>();
        
        _pathToSave = Application.persistentDataPath + "/Saves/" + saveName;
        
        _saveName = saveName;
        _saveText.text = _saveName;
        gameObject.name = _saveName;
        
        _saveCapture = await UniversePictures.LoadSpriteFromPath(_pathToSave + "/capture.png", 288, 162);
        _savePreviewContainer.sprite = _saveCapture;
    }

    void Update()
    {
        _view.gameObject.SetActive(_view.position.y < Screen.currentResolution.height/1.65);
    }
}
