using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Utilities.UniverseLibraries;

/// <summary>
/// Planet look editor is editor where player can set planet's look by changing its colors and image.
/// </summary>
public class PlanetLookEditor : PlanetEditor
{
    public static PlanetLookEditor Instance;
    private static string[] filters = new[] {"jpg","png","JPEG"};

    void Awake() => Instance = this;
    
    [SerializeField] private Image _planetPreviewImage;
    [SerializeField] private ColorWheelControl _colorWheelControl;
    [SerializeField] private Dropdown _imageDropdownPlanet;
    
    private string _pathToImages;
    public Image PlanetPreviewImage => _planetPreviewImage;

    public string PathToImages
    {
        get => _pathToImages;
        set => SetNewPath(value);
    }
    
    void Start()
    {
        Show(false);
        // only for debug
        PathToImages = "/home/mmarusiak/Desktop/UniversePictures";
    }

    public void OnPlanetChanged()
    {
        _colorWheelControl.PickColor(EditorBase.CurrentPlanet.PlanetColor);
        UpdateDropDownValue();
    }

    public void UpdateColorToPreview() => _planetPreviewImage.color = _colorWheelControl.Selection;

    public void ApplyColorToPlanet() => EditorBase.CurrentPlanet.PlanetColor = _colorWheelControl.Selection;

    public void SetNewPath(string newPath)
    {
        newPath = newPath.Replace(@"\", @"/");
        if (!Directory.Exists(newPath)) return; // display message about incorrect path

        _pathToImages = newPath;
        
        InitializeDropDown();
    }

    /// Set image of planet to correct dropdown value.
    void UpdateDropDownValue()
    {
        if (EditorBase.CurrentPlanet == null) return;
        
        foreach (var option in _imageDropdownPlanet.options)
        {
            if (option.image == EditorBase.CurrentPlanet.PlanetSprite)
            {
                _imageDropdownPlanet.value = _imageDropdownPlanet.options.IndexOf(option);
                return;
            }
        }
        // if not detected then it's probably default sprite (so value of dropdown should be set to NONE (value = 0))
        _imageDropdownPlanet.value = 0;
    }

    void InitializeDropDown()
    {
        _imageDropdownPlanet.options.Clear();
        _imageDropdownPlanet.options.Add(new Dropdown.OptionData("None", BasicPlanetEditor.Instance.DefaultPlanetSprite));

        var files = UniverseDirectories.GetFilesFromDirectory(_pathToImages, filters, false);
        foreach (var file in files) if (UniverseTools.IsSafeImage(file)) LoadSpriteFromPathToDropDown(file);
        
        UpdateDropDownValue();
    }

    async void LoadSpriteFromPathToDropDown(string path)
    {
        var loaded = await UniversePictures.LoadSpriteFromPath(path, 128, 128);
        _imageDropdownPlanet.options.Add(new Dropdown.OptionData(path.Replace(_pathToImages + "/", ""), loaded));
    }

    public void SelectNewSprite()
    {
        EditorBase.CurrentPlanet.PlanetSprite = _imageDropdownPlanet.options[_imageDropdownPlanet.value].image;
    }
}
