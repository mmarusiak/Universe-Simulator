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
        VelocityOutputs[0].text = AttachedPlanet.GetComponent<Rigidbody2D>().velocity.x.ToString(CultureInfo.InvariantCulture);
        VelocityOutputs[1].text = AttachedPlanet.GetComponent<Rigidbody2D>().velocity.y.ToString(CultureInfo.InvariantCulture);
        VelocityOutputs[2].text = AttachedPlanet.GetComponent<Rigidbody2D>().velocity.magnitude.ToString(CultureInfo.InvariantCulture);

        ForceOutputs[0].text = atatchedController.CurrentGravityForceVector.x.ToString(CultureInfo.InvariantCulture);
        ForceOutputs[1].text = atatchedController.CurrentGravityForceVector.x.ToString(CultureInfo.InvariantCulture);
        ForceOutputs[2].text = Mathf.Sqrt(
            atatchedController.CurrentGravityForceVector.x * atatchedController.CurrentGravityForceVector.x +
            atatchedController.CurrentGravityForceVector.y * atatchedController.CurrentGravityForceVector.y).ToString(CultureInfo.InvariantCulture);

        PositionOutputs[0].text = AttachedPlanet.transform.position.x.ToString(CultureInfo.InvariantCulture);
        PositionOutputs[1].text = AttachedPlanet.transform.position.y.ToString(CultureInfo.InvariantCulture);
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
    }

    public void Close()
    {
        Destroy(gameObject);
    }
}
