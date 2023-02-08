using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlanetLinePath))]
public class GravityObject : MonoBehaviour
{
    private bool DemoPlanet = false;
    public float Mass = 10f;
    public float Radius = 5f;
    public string PlanetName;
    private GravityObjectsController _controller;
    private Rigidbody2D _rigidbody2D;
    private List<GravityObject> _listHolder;

    // if player starts drag during pause, there will be auto pause
    private bool _tempPause = false;
    private float _dragTime = 0;

    public Vector2 InitialVelocity;
    public Vector2 InitialPos;
    public Vector2 CurrentGravityForceVector;

    private Vector2 _moveVector;

    public GameObject NameHolder;

    // Start is called before the first frame update
    void Start()
    {
        if(DemoPlanet)
            UpdatePlanet();
    }

    public void InitializePlanet()
    {
        transform.localScale = new Vector3(Radius * 2, Radius * 2, Radius * 2);
        _controller = GravityObjectsController.Instance;
        if(!_controller.AllGravityObjects.Contains(this))
            _controller.AddGravityObject(this);
        
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.mass = Mass;
        _rigidbody2D.velocity = InitialVelocity;
        transform.position = InitialPos;

        name = PlanetName;
        
        CreateNameHolder();
    }
    
    public void UpdatePlanet()
    {
        transform.localScale = new Vector3(Radius * 2, Radius * 2, Radius * 2);
        InitialPos = transform.position;

        _controller = GravityObjectsController.Instance;
        if(!_controller.AllGravityObjects.Contains(this))
            _controller.AddGravityObject(this);
        
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.mass = Mass;
        _rigidbody2D.velocity = InitialVelocity;
        InitialPos = transform.position;

        name = PlanetName;
        
        CreateNameHolder();
    }

    void CreateNameHolder()
    {
        if (NameHolder == null)
            CreatePlanetNameHolder();
        else
            NameHolder.name = PlanetName;
    }

    void CreatePlanetNameHolder()
    {
        NameHolder = new GameObject(PlanetName);
        NameHolder.AddComponent<PlanetNameHolder>().PlanetController = this;
        Text text = NameHolder.AddComponent<Text>();
        text.text = PlanetName;
        text.color = Color.white;
        text.font = GlobalVariables.Instance.GlobalFont;
        text.fontSize = 20;
        text.alignment = TextAnchor.MiddleCenter;
        text.fontStyle = FontStyle.BoldAndItalic;

        NameHolder.transform.SetParent(GameObject.Find("PlanetsNames").transform);
        NameHolder.transform.localScale = Vector3.one;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0)
        {
            CurrentGravityForceVector = Vector2.zero;
            foreach (var obj in _listHolder)
                ApplyAndCalculateForce(Vector2.Distance(transform.position, obj.transform.position), obj.Mass,
                    transform.position - obj.transform.position);
            _rigidbody2D.AddForce(CurrentGravityForceVector, ForceMode2D.Impulse);
        }
    }
    
    private void ApplyAndCalculateForce(float distance, float mass, Vector2 vectorDist)
    {
        float forceValue = GlobalVariables.GravitationalConstant * mass * Mass / (distance * distance);
        float proportionScale = forceValue / distance;

        float xForceValue = -proportionScale * vectorDist.x;
        float yForceValue = -proportionScale * vectorDist.y;

        CurrentGravityForceVector += new Vector2(xForceValue, yForceValue);
    }

    private void OnMouseDown()
    {
        if (!GlobalVariables.Instance.OverlayShown)
        {
            if (Time.timeScale == 0 && !_controller.RemovingPlanet)
                _moveVector = new Vector2(transform.position.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                    transform.position.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y);

            else if (Time.timeScale == 0 && _controller.RemovingPlanet)
                _controller.RemovePlanet(gameObject);
        }
    }

    private void OnMouseUp()
    {
        if (!GlobalVariables.Instance.OverlayShown)
        {
            if (_dragTime < 0.25f)
                GameObject.FindWithTag("EditorController").GetComponent<EditorHandler>().ShowPanel(gameObject);

            if (_controller.Reseted)
                InitialPos = transform.position;

            if (_tempPause && Time.timeScale == 0)
            {
                _controller.PlayPause();
                _tempPause = false;
            }

            GlobalVariables.Instance.CurrentGravityObject = this;
        }

        _dragTime = 0;
    }

    private void OnMouseDrag()
    {
        if (Time.timeScale == 0 && !GlobalVariables.Instance.OverlayShown)
        {
            _dragTime += Time.unscaledDeltaTime;
            
            if (_dragTime >= .25f || !_tempPause)
            {
                transform.position =
                    new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x + _moveVector.x,
                        Camera.main.ScreenToWorldPoint(Input.mousePosition).y + _moveVector.y);

                GameObject currentLineHolder = GetComponent<PlanetLinePath>().GetLine();

                var path = GetComponent<PlanetLinePath>();
                if (path.Lines.Count > 0 && !path.Lines[^1].Finished)
                {
                    Destroy(path.Lines[^1].SegmentHolder);
                    path.Lines.Remove(path.Lines[^1]);
                }

                if (currentLineHolder != null && !currentLineHolder.activeSelf)
                {
                    Destroy(currentLineHolder);
                }
            }
        }
        else if(!GlobalVariables.Instance.OverlayShown)
        {
            _tempPause = true;
            _controller.PlayPause();
        }
    }

    public void UpdatePrivateList() => _listHolder = _controller.GetObjects(this);
}
