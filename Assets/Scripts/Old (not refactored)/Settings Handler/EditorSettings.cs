using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class EditorSettings : MonoBehaviour
{
    [SerializeField] private InputField pathField;
    [SerializeField] private VisualEditor _visualEditor;
    [SerializeField] private string defaultPath = "/home/mmarusiak/Desktop/UniversePictures";

    private void Start()
    {
        // Just to easier debug - in future path will be saved as settings component
#if !UNITY_EDITOR
    defaultPath = GlobalVariables.Instance.PathToImages;
#endif
        DefaultPath();
        ApplyPath();
    }

    private void DefaultPath()
    {
        pathField.text = defaultPath;
        ApplyPath();
    }
    public void ApplyPath()
    {
        string newPath = pathField.text.Replace(@"\", @"/");
        
        if (!Directory.Exists(newPath))
        {
            pathField.text = "";
            GlobalVariables.Instance.PathToImages = "";
        }
        else GlobalVariables.Instance.PathToImages = newPath;

        GravityObjectsController.Instance.ResetAllImages();

        _visualEditor.InitializeDropdown(0);
    }
}
