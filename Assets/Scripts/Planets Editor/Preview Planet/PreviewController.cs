using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PreviewController : MonoBehaviour
{
    public GameObject ImagesParent;
    public Dropdown ImagesDropdown;
    public Sprite DefaultSprite;
    public string Path;

    private EditorHandler editorHandler;
    private Texture2D selectedImage;

    void Start()
    {
        editorHandler = transform.parent.transform.parent.GetComponent<EditorHandler>();
        ImagesDropdown.options.Add(new Dropdown.OptionData("None", DefaultSprite));
        ImagesDropdown.value = 0;
        if(Path != "") LoadImagesFromPath();
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
    
    public void LoadImagesFromPath()
    {
//        Path = ImagesParent.transform.GetChild(1).GetComponent<InputField>().text;
        
        // clear dropdown
        ImagesDropdown.options.Clear();
        
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
            StartCoroutine(LoadSpriteFromPath(file));
        }
    }

    public void GetImage()
    {
        if (!ImagesParent.GetComponent<WindowController>().Shown)
            ImagesParent.GetComponent<WindowController>().Show(true);
    }

    public void LoadSpriteFromDropdown()
    {
        LoadSpriteToPreview(ImagesDropdown.options[ImagesDropdown.value].image);
    }
    
    public void LoadSpriteToPreview(Sprite planetSprite)
    {
        float radius = editorHandler.Planet.GetComponent<GravityObject>().Radius;
        ResizeTool.Resize(planetSprite.texture, 256, 256);
        editorHandler.PlanetImage = Sprite.Create(planetSprite.texture, new Rect(0, 0, 256, 256), new Vector2(0.5f, 0.5f));
        
        Image img = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        img.sprite = editorHandler.PlanetImage;
        
        SpriteRenderer planetSpriteRend = editorHandler.Planet.transform.GetChild(0).GetComponent<SpriteRenderer>();
        planetSpriteRend.sprite = editorHandler.PlanetImage;
        
        Debug.Log(planetSpriteRend.sprite.rect);
    }

    public void LoadColorToPreview(Color planetColor)
    {
        editorHandler.PlanetColor = planetColor;
        transform.GetChild(0).GetChild(0).GetComponent<Image>().color = planetColor;
    }

    public void ApplyColor() => editorHandler.Planet.transform.GetChild(0).GetComponent<SpriteRenderer>().color = transform.GetChild(0).GetChild(0).GetComponent<Image>().color;

    IEnumerator LoadSpriteFromPath(string path)
    {
        string correctPath = "file:///" + path.Remove(0, 1);
        Debug.Log(correctPath);
        WWW www = new WWW(correctPath);
        while (!www.isDone)
            yield return null;
        Debug.Log(www.texture);
        var loaded = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));
        ImagesDropdown.options.Add(new Dropdown.OptionData(path.Replace(Path + "/", ""), loaded));
    }
}
