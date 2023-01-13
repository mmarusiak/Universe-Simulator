using UnityEngine;
using UnityEngine.UI;

public class InfoHandler : MonoBehaviour
{
    public GameObject AttachedPlanet;
    public Image MainPlanetImg;
    public Image BackgroundPlanetImg;

    void Update()
    {
        // Update data ???
    }
    
    public void LoadInfoData(GameObject dataContainer)
    {
        var gravityForce = dataContainer.GetComponent<GravityObject>().CurrentGravityForceVector;
        transform.Find("MenuPanel").Find("PlanetNameTXT").GetComponent<Text>().text = dataContainer.name + "'s info";

        SpriteRenderer planetSprite = dataContainer.transform.GetChild(0).GetComponent<SpriteRenderer>();
        MainPlanetImg.color = planetSprite.color;
        MainPlanetImg.sprite = planetSprite.sprite;
        BackgroundPlanetImg.color = new Color(planetSprite.color.r, planetSprite.color.g, planetSprite.color.b, 0.35f);
        BackgroundPlanetImg.sprite = planetSprite.sprite;
        
        Debug.Log(gravityForce);
    }

    public void Close()
    {
        Destroy(gameObject);
    }
}
