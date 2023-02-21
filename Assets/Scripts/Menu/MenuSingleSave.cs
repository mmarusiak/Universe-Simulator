using UnityEngine;
using UnityEngine.UI;

public class MenuSingleSave
{
    private Sprite _previewCapture;
    private Image _captureContainer;
    private string _saveName;

    public Sprite PreviewCapture
    {
        get => _previewCapture;
        set
        {
            _previewCapture = value;
            _captureContainer.sprite = _previewCapture;
        }
    }

    public Image CaptureContainer
    {
        get => _captureContainer;
        set => _captureContainer = value;
    }

    public string SaveName
    {
        get => _saveName;
        set => _saveName = value;
    }

    public MenuSingleSave(Image container, Sprite capture, string saveName)
    {
        _captureContainer = container;
        _previewCapture = capture;
        _saveName = saveName;
    }
}
