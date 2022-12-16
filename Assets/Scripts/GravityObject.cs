using UnityEngine;

public class GravityObject : MonoBehaviour
{
    public float Mass = 10f;
    public float Radius = 5f;
    public float GravityForce;
    private GravityObjectsController Controller;
    
    
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale += new Vector3(Radius / 2, Radius / 2, Radius / 2);
        Controller = GameObject.Find("GravityController").GetComponent<GravityObjectsController>();
        Controller.AllGravityObjects.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
