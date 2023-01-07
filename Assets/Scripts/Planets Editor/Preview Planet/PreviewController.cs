using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PreviewController : MonoBehaviour
{
    public GameObject ImagesParent;
    public Dropdown ImagesDropdown;
    public Sprite DefaultSprite;
    public string Path;
    public string CurrentPath;

    private EditorHandler editorHandler;
    private Texture2D selectedImage;

    private void Start()
    {
        SetDropdown(0);
    }

    public void SetDropdown(int value)
    {
        CurrentPath = Path;
        editorHandler = transform.parent.transform.parent.GetComponent<EditorHandler>();
        // clear dropdown
        ImagesDropdown.options.Clear();
        ImagesDropdown.options.Add(new Dropdown.OptionData("None", DefaultSprite));
        if(Path != "") LoadImagesFromPath(value);
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
    
    public void LoadImagesFromPath(int value)
    {
        // Path = ImagesParent.transform.GetChild(1).GetComponent<InputField>().text;

        // assign new items to dropdown
        Path = Path.Replace(@"\", @"/");
        if (Path[^1].Equals("/"))
        {
            Path.Remove(Path.Length - 1);
        }
        
        String searchFolder = Path;
        var filters = new String[] { "jpg", "JPEG", "png"};
        var files = GetFilesFrom(searchFolder, filters, false);
        
        foreach (var file in files)
        {
            StartCoroutine(LoadSpriteFromPath(file, value));
        }
    }

    public void GetImage()
    {
        if (!ImagesParent.GetComponent<WindowController>().Shown)
            ImagesParent.GetComponent<WindowController>().Show(true);
    }

    public void LoadSpriteFromDropdown() => LoadSpriteToPreview(ImagesDropdown.options[ImagesDropdown.value].image);
    
    
    public void LoadSpriteToPreview(Sprite planetSprite)
    {
        editorHandler.PlanetImage = planetSprite;

        Image img = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        img.sprite = editorHandler.PlanetImage;

        SpriteRenderer planetSpriteRend = editorHandler.Planet.transform.GetChild(0).GetComponent<SpriteRenderer>();
        planetSpriteRend.sprite = editorHandler.PlanetImage;
        
        // setting scale of image
        planetSpriteRend.transform.localScale = planetSprite == DefaultSprite ? Vector3.one : new Vector3(.75f, .75f, 1);

    }

    public void LoadColorToPreview(Color planetColor)
    {
        editorHandler.PlanetColor = planetColor;
        transform.GetChild(0).GetChild(0).GetComponent<Image>().color = planetColor;
    }

    public void ApplyColor() => editorHandler.Planet.transform.GetChild(0).GetComponent<SpriteRenderer>().color = transform.GetChild(0).GetChild(0).GetComponent<Image>().color;

    IEnumerator LoadSpriteFromPath(string path, int value)
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
            if (loaded != DefaultSprite)
            {
                ResizeTool.Resize(loaded.texture, 128, 128);
                loaded =
                    Sprite.Create(loaded.texture, new Rect(0, 0, 128, 128), new Vector2(0.5f, 0.5f));
            }

            ImagesDropdown.options.Add(new Dropdown.OptionData(path.Replace(Path + "/", ""), loaded));
            if (ImagesDropdown.options.Count >= value && ImagesDropdown.value != value)
                ImagesDropdown.SetValueWithoutNotify(value);
        }
    }
}
