using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ComponentEditor : MonoBehaviour
{
    private EditorHandler componentsHandler;

    // planet name = 0
    // planet mass = 1
    // planet radius = 2
    public InputField[] inputs = new InputField[3];
    
    void Start()
    {
        componentsHandler = transform.parent.transform.parent.GetComponent<EditorHandler>();

        GetInputs();
    }

    void GetInputs()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            inputs[i] = transform.GetChild(i).transform.GetChild(0).GetComponent<InputField>();
        }
    }
    
    public void UpdateText(string name, float mass, float radius)
    {
        if(inputs.Contains(null))
            GetInputs();
        inputs[0].text = name;
        inputs[1].text = mass.ToString(CultureInfo.InvariantCulture);
        inputs[2].text = radius.ToString(CultureInfo.InvariantCulture);
    }
    
    public void OnNameChanged()
    {
        componentsHandler.Planet.GetComponent<GravityObject>().PlanetName = inputs[0].text;
        GameObject.Find("PlanetNameTXT").GetComponent<Text>().text = componentsHandler.Planet.GetComponent<GravityObject>().PlanetName;
        componentsHandler.Planet.GetComponent<GravityObject>().NameHolder.GetComponent<Text>().text = inputs[0].text;
        GameObject.Find("VisualWindowPlanetTXT").GetComponent<Text>().text = inputs[0].text + "'s look editor";
        componentsHandler.Planet.GetComponent<GravityObject>().UpdatePlanet();
    }

    public void OnMassChanged()
    {
        componentsHandler.Planet.GetComponent<GravityObject>().Mass = float.Parse(inputs[1].text, CultureInfo.InvariantCulture.NumberFormat);
        componentsHandler.Planet.GetComponent<GravityObject>().UpdatePlanet();
    }

    public void OnRadiusChanged()
    {
        componentsHandler.Planet.GetComponent<GravityObject>().Radius = float.Parse(inputs[2].text, CultureInfo.InvariantCulture.NumberFormat);
        componentsHandler.Planet.GetComponent<GravityObject>().UpdatePlanet();
    }
}
