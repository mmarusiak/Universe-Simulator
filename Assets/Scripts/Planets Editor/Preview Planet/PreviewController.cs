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
    public string Path;

    private EditorHandler componentsHandler;
    private Texture2D selectedImage;
    private Sprite loaded;
    
    void Start()
    {
        componentsHandler = transform.parent.transform.parent.GetComponent<EditorHandler>();
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
    
    public void GetPath()
    {
        Path = ImagesParent.transform.GetChild(1).GetComponent<InputField>().text;
        
        // clear dropdown
        ImagesDropdown.options.Clear();
        
        // assign new items to dropdown
        Path = Path.Replace(@"\", @"/");
        if (Path[^1].Equals("/"))
        {
            Path.Remove(Path.Length - 1);
        }
        
        String searchFolder = Path;
        var filters = new String[] { "jpg", "jpeg", "png"};
        var files = GetFilesFrom(searchFolder, filters, false);

        foreach (var file in files)
        {
            Debug.Log(file);
            Debug.Log("Path " + Path);
            StartCoroutine(LoadImage(file));
        }
    }

    public void GetImage()
    {
        if (!ImagesParent.activeSelf)
            ImagesParent.SetActive(true);
        else if (ImagesDropdown.value > -1)
        {
            componentsHandler.PlanetImage = ImagesDropdown.options[ImagesDropdown.value].image;
            transform.GetChild(0).GetComponent<Image>().sprite = componentsHandler.PlanetImage;
            ImagesParent.SetActive(false);
        }


    }
    
    IEnumerator LoadImage(string path)
    {
        string correctPath = "file:///" + path.Remove(0, 1);
        Debug.Log(correctPath);
        WWW www = new WWW(correctPath);
        while (!www.isDone)
            yield return null;
        Debug.Log(www.texture);
        loaded = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));
        ImagesDropdown.options.Add(new Dropdown.OptionData(path.Replace(Path + "/", ""), loaded));
    }
}
