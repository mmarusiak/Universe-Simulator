using System.Collections.Generic;
using UnityEngine;

public class GravityObjectsController : MonoBehaviour
{
    [SerializeField]
    public List<GravityObject> AllGravityObjects = new List<GravityObject>();

    public bool Reseted = false, Paused = true, LinesVisible = true;

    void Start()
    {
        PlayPause();
    }
    
    public void AddGravityObject(GravityObject obj)
    {
        AllGravityObjects.Add(obj);
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
        if (Time.timeScale == 0)
            UnPause();
        else
            Pause();
    }

    void Pause()
    {
        Time.timeScale = 0;
        Paused = true;
    }

    void UnPause()
    {
        Time.timeScale = 1;
        Reseted = false;
        Paused = false;
    }
    
    public void ResetScene()
    {
        Time.timeScale = 0;

        foreach (var grav in AllGravityObjects)
        {
            grav.gameObject.transform.position = grav.StartPos;
            grav.GetComponent<Rigidbody2D>().velocity = grav.InitialVelocity;
        }
        
        Camera.main.gameObject.transform.position = new Vector3(0, 0, -10);
        Camera.main.orthographicSize = 60;
        
        Reseted = true;
    }

    public void LineCheck()
    {
        LinesVisible = !LinesVisible;
        var lines = GameObject.FindGameObjectsWithTag("PlanetLine");

        foreach (var line in lines)
        {
            line.GetComponent<LineRenderer>().enabled = LinesVisible;
        }
    }
}
