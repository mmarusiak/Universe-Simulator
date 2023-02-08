using System.Collections.Generic;
using UnityEngine;

public class GravityObjectsController : MonoBehaviour
{
    public static GravityObjectsController Instance { get; private set; }
    public Transform PlanetsHolder;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    
    [SerializeField]
    public List<GravityObject> AllGravityObjects = new List<GravityObject>();

    public bool Reseted = true, Paused = true, LinesVisible = true, RemovingPlanet = false;

    void Start()
    {
        PlayPause();
    }
    
    
    public void AddGravityObject(GravityObject obj)
    {
        AllGravityObjects.Add(obj);
        UpdateLisInObjects();
    }

    void UpdateLisInObjects()
    {
        foreach (var gravobj in AllGravityObjects)
            gravobj.UpdatePrivateList();
    }
    
    public List<GravityObject> GetObjects(GravityObject gravityObject)
    {
        List<GravityObject> returnList = new List<GravityObject>();

        foreach (var obj in AllGravityObjects)
            if(obj != gravityObject)
                returnList.Add(obj);
        
        return returnList;
    }

    public void PlayPause()
    {
        //DebugMath();
        
        if (Time.timeScale == 0)
            UnPause();
        else
            Pause();
    }

    void DebugMath()
    {
        GravityObject star = AllGravityObjects[1];
        GravityObject targetPlanet = AllGravityObjects[0];
        targetPlanet.InitialVelocity = Vector2.zero;
        List<UniverseTools.OrbitCalculator.Planet> objects = new List<UniverseTools.OrbitCalculator.Planet>();

        foreach (var obj in AllGravityObjects)
        {
            if (star != obj && targetPlanet != obj)
                objects.Add(obj);
        }

        var output = UniverseTools.OrbitCalculator.MinimumVelocity(star, targetPlanet, objects.ToArray());
        Debug.Log($"Minimal velocity to reach orbit by {targetPlanet.PlanetName} on {star.PlanetName} is {output[0]}, {output[1]}");
    }

    void Pause()
    {
        Time.timeScale = 0;
        Paused = true;
    }

    void UnPause()
    {
        Time.timeScale = 1;
        Paused = false;
        Reseted = false;
        RemovingPlanet = false;
    }
    
    public void ResetScene()
    {
        Pause();

        foreach (var grav in AllGravityObjects)
        {
            grav.gameObject.transform.position = grav.InitialPos;
            grav.GetComponent<Rigidbody2D>().velocity = grav.InitialVelocity;
            
            PlanetLinePath path = grav.GetComponent<PlanetLinePath>();

            foreach (var line in path.Lines)
            {
                Destroy(line.SegmentHolder);
            }
            path.Lines.Clear();
        }
        
        Camera.main.gameObject.transform.position = new Vector3(0, 0, -10);
        Camera.main.orthographicSize = 85;

        Reseted = true;
    }

    public void LineCheck()
    {
        LinesVisible = !LinesVisible;

        foreach (var grav in AllGravityObjects)
        {
            PlanetLinePath path = grav.GetComponent<PlanetLinePath>();

            foreach (var line in path.Lines)
            {
                if (line.Finished)
                    line.Renderer.enabled = LinesVisible;
            }
        }
    }

    public void RemovePlanet(GameObject planet)
    {
        GameObject.Find("RemovePlanet").GetComponent<ToggleButton>().Toggle();
        RemovingPlanet = false;
        AllGravityObjects.Remove(planet.GetComponent<GravityObject>());
        Destroy(planet.GetComponent<GravityObject>().NameHolder.GetComponent<PlanetNameHolder>().DistText);
        Destroy(planet.GetComponent<GravityObject>().NameHolder.GetComponent<PlanetNameHolder>().VelocityText);
        Destroy(planet.GetComponent<GravityObject>().NameHolder);
        Destroy(planet);
        UpdateLisInObjects();
    }

    public void ClearScene()
    {
        int planetsCount = PlanetsHolder.childCount;

        for (int i = 0; i < planetsCount; i++)
        {
            RemovePlanet(PlanetsHolder.GetChild(i).gameObject);
        }
    }

    // when path changed we reset image of each planet to avoid weird bugs
    public void ResetAllImages()
    {
        foreach (var planet in AllGravityObjects)
            planet.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = VisualEditor.DefaultSprite;
    }

    public void SetList(List<SaveManager.PlanetSaveData> dataList)
    {
        ClearScene();

        foreach (var data in dataList)
        {
            var newPlanet = Instantiate(CreatorController.Instance.PlanetPrefab, PlanetsHolder);
            var grav = newPlanet.GetComponent<GravityObject>();
            data.InitializeData(grav);
            grav.InitializePlanet();
        }
    }
    
}
