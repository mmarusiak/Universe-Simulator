using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class InfoHandler : MonoBehaviour
{
    public GameObject AttachedPlanet;
    private GravityObject attachedController;
    public Image MainPlanetImg;
    public Image BackgroundPlanetImg;

    // x - 0, y - 1, value - 2
    public Text[] VelocityOutputs = new Text[3];
    public Text[] ForceOutputs = new Text[3];
    public Text[] PositionOutputs = new Text[2];

    private bool setOnReset = false;

    void Update()
    {
        if (Time.timeScale > 0 && AttachedPlanet != null && !GravityObjectsController.Instance.Reseted)
        {
            UpdateData();
            setOnReset = false;
        }
        else if (!setOnReset)
        {
            UpdateData();
            setOnReset = true;
        }
    }

    void UpdateData()
    {
        var velocityOutArr = OutputsData(AttachedPlanet.GetComponent<Rigidbody2D>().velocity.x*10,
            AttachedPlanet.GetComponent<Rigidbody2D>().velocity.y*10);
        var forceOutArr = OutputsData(attachedController.CurrentGravityForceVector.x*1000,
            attachedController.CurrentGravityForceVector.y*1000);

        VelocityOutputs[0].text = "<color=red>" + velocityOutArr[0].Split('.')[0] + "</color><color=grey>." + velocityOutArr[0].Split('.')[1] + "</color>";
        VelocityOutputs[1].text = "<color=green>" + velocityOutArr[1].Split('.')[0] + "</color><color=grey>." + velocityOutArr[1].Split('.')[1] + "</color>";
        VelocityOutputs[2].text = velocityOutArr[2];

        ForceOutputs[0].text = "<color=red>" + forceOutArr[0].Split('.')[0] + "</color><color=grey>." + forceOutArr[0].Split('.')[1] + "</color>";
        ForceOutputs[1].text = "<color=green>" + forceOutArr[1].Split('.')[0] + "</color><color=grey>." + forceOutArr[1].Split('.')[1] + "</color>";
        ForceOutputs[2].text = forceOutArr[2];

        // need to format it
        PositionOutputs[0].text = AttachedPlanet.transform.position.x.ToString(CultureInfo.InvariantCulture);
        PositionOutputs[1].text = AttachedPlanet.transform.position.y.ToString(CultureInfo.InvariantCulture);
    }

    string[] OutputsData(float x, float y)
    {
        string xStr = SingleOutput(x);
        string yStr = SingleOutput(y);
        
        var magnitude = Mathf.Sqrt(x * x + y * y);
        string magnitudeStr = SingleOutput(magnitude);

        return new[] {xStr, yStr, magnitudeStr};
    }

    string SingleOutput(float input)
    {
        int rounder = (int)Mathf.Pow(10, ((int)input).ToString(CultureInfo.InvariantCulture).Length);
        string output = 
            (Mathf.Round(input*rounder)/rounder).ToString(CultureInfo.InvariantCulture);

        if (output.Contains("."))
        {
            while (output.Split(".")[1].Length < 3)
            {
                output += "0";
            }
        }
        else
            output += ".000";

        return output;
    }
    
    public void LoadInfoData(GameObject dataContainer)
    {
        var gravityForce = dataContainer.GetComponent<GravityObject>().CurrentGravityForceVector;
        transform.Find("MenuPanel").Find("PlanetNameTXT").GetComponent<Text>().text = dataContainer.name + "'s info";

        SpriteRenderer planetSprite = dataContainer.transform.GetChild(0).GetComponent<SpriteRenderer>();
        MainPlanetImg.color = planetSprite.color;
        MainPlanetImg.sprite = planetSprite.sprite;
        BackgroundPlanetImg.color = new Color(planetSprite.color.r, planetSprite.color.g, planetSprite.color.b, 0.4f);
        BackgroundPlanetImg.sprite = planetSprite.sprite;

        AttachedPlanet = dataContainer;
        attachedController = AttachedPlanet.GetComponent<GravityObject>();
        
        UpdateData();
    }

    public void Close()
    {
        Destroy(gameObject);
    }
}
