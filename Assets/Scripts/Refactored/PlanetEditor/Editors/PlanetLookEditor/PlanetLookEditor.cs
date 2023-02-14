using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

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

    // set image of planet to correct dropdown value
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
        
        _imageDropdownPlanet.value = 0;
    }

    void InitializeDropDown()
    {
        _imageDropdownPlanet.options.Clear();
        _imageDropdownPlanet.options.Add(new Dropdown.OptionData("None", BasicPlanetEditor.Instance.DefaultPlanetSprite));
        Debug.Log("Gere");
        
        var files = GetFilesFrom(_pathToImages, filters, false);

        foreach (var file in files)
        {
            Debug.Log(file);
            if (UniverseTools.IsSafeImage(file)) StartCoroutine(LoadSpriteFromPathToDropDown(file));
        }
        UpdateDropDownValue();
    }
    
    // https://stackoverflow.com/a/18321162/13786856
    public static String[] GetFilesFrom(String searchFolder, String[] filters, bool isRecursive)
    {
        List<String> filesFound = new List<String>();
        var searchOption = isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
        foreach (var filter in filters)
        {
            filesFound.AddRange(Directory.GetFiles(searchFolder, String.Format("*.{0}", filter), searchOption));
        }
        return filesFound.ToArray();
    }
    
    IEnumerator LoadSpriteFromPathToDropDown(string path)
    {
        string correctPath = "file:///" + path.Remove(0, 1);
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(correctPath);
        yield return request.SendWebRequest();
        
        while (!request.isDone)
            yield return null;

        if (request.result == UnityWebRequest.Result.Success)
        {
            DownloadHandlerTexture textureDownloadHandler = (DownloadHandlerTexture) request.downloadHandler;
            Texture2D texture = textureDownloadHandler.texture;

            if (texture == null)
            {
                Debug.LogError("Image not available...");
                yield break;
            }
            
            var loaded = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f));
            // resizing sprite
            if (loaded != BasicPlanetEditor.Instance.DefaultPlanetSprite)
            {
                ResizeTool.Resize(loaded.texture, 128, 128);
                loaded =
                    Sprite.Create(loaded.texture, new Rect(0, 0, 128, 128), new Vector2(0.5f, 0.5f));
            }

            _imageDropdownPlanet.options.Add(new Dropdown.OptionData(path.Replace(_pathToImages + "/", ""), loaded));
        }
    }

    public void SelectNewSprite()
    {
        EditorBase.CurrentPlanet.PlanetSprite = _imageDropdownPlanet.options[_imageDropdownPlanet.value].image;
    }
}
