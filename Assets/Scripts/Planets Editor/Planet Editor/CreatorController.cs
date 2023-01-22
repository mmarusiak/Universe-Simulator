using UnityEngine;
using UnityEngine.EventSystems;

public class CreatorController : MonoBehaviour
{
    private EditorHandler _editorHandler;
    private GravityObjectsController _controller;
    private bool creatingPlanet = false;
    public GameObject PlanetPrefab;
    
    void Start()
    {
        _editorHandler = GetComponent<EditorHandler>();
        _controller = GravityObjectsController.Instance;
    }

    
    public void CreateCall()
    {
        creatingPlanet = !creatingPlanet;
        _controller.RemovingPlanet = false;
        if(!_controller.Paused)
            _controller.PlayPause();
    }
    
    public void RemovePlanet()
    {
        _controller.RemovingPlanet = !_controller.RemovingPlanet;
        creatingPlanet = false;
        if(!_controller.Paused)
            _controller.PlayPause();
    }

    void LateUpdate()
    {
        if (creatingPlanet && Input.GetMouseButton(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, new Vector2(0, 0), 0.01f);

            bool flag = true;
            flag = !EventSystem.current.IsPointerOverGameObject();

            if (flag)
            {
                creatingPlanet = false;
                GameObject planet = Instantiate(PlanetPrefab, mousePosition, Quaternion.Euler(0, 0, 0),
                    GameObject.FindWithTag("PlanetsHolder").transform);
                planet.GetComponent<GravityObject>().UpdatePlanet();

                _editorHandler.ShowPanel(planet);
                GameObject.Find("AddPlanet").GetComponent<ToggleButton>().Toggle();
            }
        }
    }
}
