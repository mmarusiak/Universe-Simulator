using UnityEngine;
using UnityEngine.UI;

public class MenuSingleSave
{
    private Sprite _previewCapture;
    private Image _captureContainer;
    private string _path;

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

    public string Path
    {
        get => _path;
        set => _path = value;
    }

    public MenuSingleSave(Image container, Sprite capture, string path)
    {
        
    }
}
