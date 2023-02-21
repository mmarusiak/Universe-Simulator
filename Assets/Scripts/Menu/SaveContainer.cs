using UnityEngine;
using UnityEngine.UI;

public class SaveContainer : MonoBehaviour
{
    private string _saveName, _pathToSave;
    private Sprite _saveCapture;
    private Image _savePreviewContainer;
    private Text _saveText;

    public async void Initialize(string saveName)
    {
        _savePreviewContainer = transform.Find("SavePreview").GetComponent<Image>();
        _saveText = transform.Find("SaveName").GetComponent<Text>();
        
        _pathToSave = Application.persistentDataPath + "/Saves/" + saveName;
        _saveName = saveName;
        _saveText.text = _saveName;
        _saveCapture = await UniversePictures.LoadSpriteFromPath(_pathToSave + "/capture.png", 288, 162);
        _savePreviewContainer.sprite = _saveCapture;
    }
}
