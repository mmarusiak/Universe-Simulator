using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class InfoHandler : MonoBehaviour
{
    public GameObject AttachedPlanet;
    private GravityObject atatchedController;
    public Image MainPlanetImg;
    public Image BackgroundPlanetImg;

    // x - 0, y - 1, value - 2
    public Text[] VelocityOutputs = new Text[3];
    public Text[] ForceOutputs = new Text[3];
    public Text[] PositionOutputs = new Text[2];

    void Update()
    {
        if(Time.timeScale > 0 && AttachedPlanet != null)
            UpdateData();
    }

    void UpdateData()
    {
        var velocityOutArr = OutputsData(AttachedPlanet.GetComponent<Rigidbody2D>().velocity.x,
            AttachedPlanet.GetComponent<Rigidbody2D>().velocity.y);
        var forceOutArr = OutputsData(atatchedController.CurrentGravityForceVector.x,
            atatchedController.CurrentGravityForceVector.y);

        VelocityOutputs[0].text = velocityOutArr[0];
        VelocityOutputs[1].text = velocityOutArr[1];
        VelocityOutputs[2].text = velocityOutArr[2];

        ForceOutputs[0].text = forceOutArr[0];
        ForceOutputs[1].text = forceOutArr[1];
        ForceOutputs[2].text = forceOutArr[2];

        PositionOutputs[0].text = AttachedPlanet.transform.position.x.ToString(CultureInfo.InvariantCulture);
        PositionOutputs[1].text = AttachedPlanet.transform.position.y.ToString(CultureInfo.InvariantCulture);
    }

    static string[] OutputsData(float x, float y)
    {
        string xStr = (Mathf.Round(x*10000)/10000).ToString(CultureInfo.InvariantCulture);
        while (xStr.Length < 6)
        {
            xStr += "0";
        }
        
        string yStr = (Mathf.Round(y*10000)/10000).ToString(CultureInfo.InvariantCulture);
        while (yStr.Length < 6)
        {
            yStr += "0";
        }
        
        var magnitude = Mathf.Sqrt(x * x + y * y);
        string magnitudeStr = (Mathf.Round(magnitude * 1000) / 1000).ToString(CultureInfo.InvariantCulture);
        while (magnitudeStr.Length < 6)
        {
            magnitudeStr += "0";
        }
        
        return new[] {xStr, yStr, magnitudeStr};
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
        atatchedController = AttachedPlanet.GetComponent<GravityObject>();
        
        UpdateData();
    }

    public void Close()
    {
        Destroy(gameObject);
    }
}
