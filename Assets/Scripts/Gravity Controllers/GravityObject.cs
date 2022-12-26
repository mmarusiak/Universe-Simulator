using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GravityObject : MonoBehaviour
{
    public float Mass = 10f;
    public float Radius = 5f;
    public string PlanetName;
    private GravityObjectsController Controller;
    private Rigidbody2D _rigidbody2D;
    private List<GravityObject> listHolder;

    public Vector2 InitialVelocity;
    public Vector2 StartPos;

    private Vector2 moveVector;

    //private float GStaticValue = 6.674f * (float)Math.Pow(10, -11);
    private float GConstantValue = 0.06674f;

    private Vector2 currentGravityForceVector;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale += new Vector3(Radius * 2, Radius * 2, Radius * 2);
        StartPos = transform.position;

        Controller = GameObject.FindWithTag("GravityController").GetComponent<GravityObjectsController>();
        Controller.AddGravityObject(this);
        
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.mass = Mass;
        _rigidbody2D.velocity = InitialVelocity;

        this.name = PlanetName;
        CreatePlanetNameHolder();
    }

    void CreatePlanetNameHolder()
    {
        GameObject textObject = new GameObject(PlanetName);
        textObject.AddComponent<PlanetNameHolder>().PlanetController = this;
        Text text = textObject.AddComponent<Text>();
        text.text = PlanetName;
        text.color = Color.white;
        text.font = Font.CreateDynamicFontFromOSFont("Arial", 20);
        text.fontSize = 20;
        text.alignment = TextAnchor.MiddleCenter;
        text.fontStyle = FontStyle.BoldAndItalic;

        textObject.transform.SetParent(GameObject.Find("PlanetsNames").transform);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0)
        {
            foreach (var obj in listHolder)
                ApplyAndCalculateForce(Vector2.Distance(transform.position, obj.transform.position), obj.Mass,
                    AddVectors2D(transform.position, -obj.transform.position));
            _rigidbody2D.AddForce(currentGravityForceVector, ForceMode2D.Impulse);
            currentGravityForceVector = Vector2.zero;
        }
    }
    
    private void ApplyAndCalculateForce(float distance, float mass, Vector2 vectorDist)
    {
        float forceValue = GConstantValue * mass * Mass / (distance * distance);
        float proportionScale = forceValue / distance;

        float xForceValue = -proportionScale * vectorDist.x;
        float yForceValue = -proportionScale * vectorDist.y;

        currentGravityForceVector = AddVectors2D(currentGravityForceVector, new Vector2(xForceValue, yForceValue));
    }
    
    private void OnMouseDown()
    {
        if (Time.timeScale == 0)
            moveVector = new Vector2(transform.position.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                transform.position.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
    }

    private void OnMouseDrag()
    {
        if (Time.timeScale == 0)
        {
            transform.position =
                new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x + moveVector.x,
                    Camera.main.ScreenToWorldPoint(Input.mousePosition).y + moveVector.y);
            if(Controller.Reseted)
                StartPos = transform.position;
        }
    }

    public void UpdatePrivateList() => listHolder = Controller.GetObjects(this);

    Vector2 AddVectors2D(Vector2 a, Vector2 b)
    {
        return new Vector2(a.x + b.x, a.y + b.y);
    }
}
