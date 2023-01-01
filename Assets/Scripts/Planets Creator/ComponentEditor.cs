using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class ComponentEditor : MonoBehaviour
{
    private CreatorHandler componentsHandler;

    // planet name = 0
    // planet mass = 1
    // planet radius = 2
    private InputField[] inputs = new InputField[3];
    
    void Start()
    {
        componentsHandler = transform.parent.transform.parent.GetComponent<CreatorHandler>();

        for (int i = 0; i < transform.childCount; i++)
        {
            inputs[i] = transform.GetChild(i).transform.GetChild(0).GetComponent<InputField>();
        }
    }
    
    public void OnNameChanged()
    {
        componentsHandler.PlanetName = inputs[0].text;
        GameObject.Find("PlanetNameTXT").GetComponent<Text>().text = componentsHandler.PlanetName;
    }

    public void OnMassChanged()
    {
        componentsHandler.Mass = float.Parse(inputs[1].text, CultureInfo.InvariantCulture.NumberFormat);
    }

    public void OnRadiusChanged()
    {
        componentsHandler.Radius = float.Parse(inputs[2].text, CultureInfo.InvariantCulture.NumberFormat);
    }
}
