using System;
using UnityEngine;
using UnityEngine.UI;

public class CreatorHandler : MonoBehaviour
{
    public GameObject PlanetPrefab;
    // planets' creator page args/components
    public string PlanetName;
    public Vector2 InitialVelocity;
    public float Mass, Radius;
    public Vector2 StartPos;
    private GravityObjectsController _controller;
    public GameObject Panel;
    private bool creatingPlanet = false;
    public Sprite PlanetImage;

    void Start()
    {
        _controller = GameObject.FindWithTag("GravityController").GetComponent<GravityObjectsController>();
        Panel.SetActive(false);
    }

    void LateUpdate()
    {
        if (creatingPlanet && Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll (mousePosition, new Vector2 (0, 0), 0.01f);

            bool flag = true;
            foreach(var hit in hits)
                if (hit.collider.TryGetComponent(out GravityObject component))
                    flag = false;

            if (flag)
            {
                StartPos = mousePosition;
                ShowPanel();
                creatingPlanet = false;
            }
        }
    }

    public void CreateCall()
    {
        if(!_controller.Paused)
            _controller.PlayPause();
        creatingPlanet = true;
    }

    public void ShowPanel()
    {
        Panel.GetComponent<RectTransform>().position =
            new Vector3( Input.mousePosition.x - Panel.GetComponent<RectTransform>().sizeDelta.x,
                Input.mousePosition.y + Panel.GetComponent<RectTransform>().sizeDelta.y/2);
        Panel.SetActive(true);
    }
    
    public void CreatePlanet()
    {
        if (PlanetName != String.Empty && Mass > 0 && Radius > 0)
        {
            GameObject newPlanet = Instantiate(PlanetPrefab);
            var newPlanetController = newPlanet.AddComponent<GravityObject>();
            newPlanetController.Mass = Mass;
            newPlanetController.PlanetName = PlanetName;
            newPlanetController.Radius = Radius;
            newPlanetController.StartPos = StartPos;
            newPlanetController.InitialVelocity = InitialVelocity;

            newPlanetController.Init();
        }
    }

    public void RemovePlanet()
    {
        if(!_controller.Paused)
            _controller.PlayPause();
        _controller.RemovingPlanet = !_controller.RemovingPlanet;
    }

    public void Close()
    {
        creatingPlanet = false;
        Panel.SetActive(false);
    }
}
