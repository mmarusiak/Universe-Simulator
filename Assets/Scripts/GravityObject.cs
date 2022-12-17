using System;
using System.Collections.Generic;
using UnityEngine;

public class GravityObject : MonoBehaviour
{
    public float Mass = 10f;
    public float Radius = 5f;
    public float GravityForce;
    private GravityObjectsController Controller;
    private Rigidbody2D _rigidbody2D;
    private List<GravityObject> listHolder;

    public Vector2 InitialVelocity;

    //private float GStaticValue = 6.674f * (float)Math.Pow(10, -11);
    private float GStaticValue = 0.06674f;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale += new Vector3(Radius / 2, Radius / 2, Radius / 2);
        Controller = GameObject.FindWithTag("GravityController").GetComponent<GravityObjectsController>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        Controller.AddGravityObject(this);
        _rigidbody2D.mass = Mass;

        _rigidbody2D.velocity = InitialVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var obj in listHolder)
            ApplyAndCalculateForce(Vector2.Distance(transform.position, obj.transform.position), obj.Mass,
                new Vector2(transform.position.x - obj.transform.position.x,
                    transform.position.y - obj.transform.position.y));
    }

    private void ApplyAndCalculateForce(float distance, float mass, Vector2 vectorDist)
    {
        float forceValue = GStaticValue * mass * Mass / (distance * distance);
        float flag = forceValue / distance;

        float xForceValue = -flag * vectorDist.x;
        float yForceValue = -flag * vectorDist.y;

        _rigidbody2D.AddForce(new Vector2(xForceValue, yForceValue),ForceMode2D.Impulse);
    }
    
    public void UpdatePrivateList()
    {
        listHolder = Controller.GetObjects(this);
    }
}
