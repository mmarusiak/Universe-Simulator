using System.Collections.Generic;
using UnityEngine;

public class GravityObjectsController : MonoBehaviour
{
    [SerializeField]
    public List<GravityObject> AllGravityObjects = new List<GravityObject>();

    public bool isPaused = true;

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
        isPaused = !isPaused;

        foreach (var grav in AllGravityObjects)
        {
            Rigidbody2D rbody = grav.gameObject.GetComponent<Rigidbody2D>();

            if (isPaused)
                grav.Velocity = rbody.velocity;
       
            rbody.bodyType =
                isPaused ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic;
            rbody.velocity = isPaused ? Vector2.zero : grav.Velocity;
            grav.enabled = !isPaused;
        }
    }
    
    public void ResetScene()
    {
        if(!isPaused)
            PlayPause();

        foreach (var grav in AllGravityObjects)
        {
            grav.gameObject.transform.position = grav.StartPos;
            grav.Velocity = grav.InitialVelocity;
        }
    }
}
