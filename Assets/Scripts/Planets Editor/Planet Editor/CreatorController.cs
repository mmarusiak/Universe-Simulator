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
        // Checking if no planet is overlapping
        if (Input.GetMouseButtonDown(0))
        {
            bool flag = true;
            flag = !EventSystem.current.IsPointerOverGameObject();

            if (flag && creatingPlanet)
            {
                creatingPlanet = false;
                GameObject planet = Instantiate(PlanetPrefab, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.Euler(0, 0, 0),
                    GameObject.FindWithTag("PlanetsHolder").transform);
                
                GlobalVariables.Instance.CurrentGravityObject = planet.GetComponent<GravityObject>();
                GlobalVariables.Instance.CurrentGravityObject.UpdatePlanet();

                _editorHandler.ShowPanel(planet);
                GameObject.Find("AddPlanet").GetComponent<ToggleButton>().Toggle();
            }else if (flag)
            {
                GlobalVariables.Instance.CurrentGravityObject = null;
            }
        }
    }
}
