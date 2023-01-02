using UnityEngine;

public class CreatorController : MonoBehaviour
{
    private EditorHandler _editorHandler;
    private GravityObjectsController _controller;
    private bool creatingPlanet = false;
    public GameObject PlanetPrefab;

    void Start()
    {
        _editorHandler = GetComponent<EditorHandler>();
        _controller = GameObject.FindWithTag("GravityController").GetComponent<GravityObjectsController>();
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
                creatingPlanet = false;
                GameObject planet = Instantiate(PlanetPrefab, mousePosition, Quaternion.Euler(0,0,0), GameObject.FindWithTag("PlanetsHolder").transform);
                planet.GetComponent<GravityObject>().UpdatePlanet();

                _editorHandler.ShowPanel(planet);
            }
        }
    }

    public void CreateCall()
    {
        if(!_controller.Paused)
            _controller.PlayPause();
        creatingPlanet = true;
        _controller.RemovingPlanet = false;
    }
    
    public void RemovePlanet()
    {
        if(!_controller.Paused)
            _controller.PlayPause();
        _controller.RemovingPlanet = !_controller.RemovingPlanet;
        creatingPlanet = false;
    }

}
