using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class EditorSettings : MonoBehaviour
{
    [SerializeField] private InputField pathField;
    [SerializeField] private VisualEditor _visualEditor;
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
