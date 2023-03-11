using UnityEngine;
using UnityEngine.UI;

public class LeftSideButtonsController : MonoBehaviour
{
    [SerializeField] private Image[] _buttonsImage = new Image[3];
    private bool _isCreatingPlanet = false, _isRemovingPlanet = false, _isCutable = false, _cutState;
    private Vector2[] _cutsVectors = new Vector2[2];

    void Start() => UpdateButtons();
    public void AddPlanet()
    { 
        _isCreatingPlanet = !_isCreatingPlanet;
        _isRemovingPlanet = false;
        _isCutable = false;
        _cutsVectors = new Vector2[2];
        _cutState = false;

        UpdateButtons();
    }

    public void DeletePlanet()
    {
        _isRemovingPlanet = !_isRemovingPlanet;
        _isCreatingPlanet = false;
        _isCutable = false;
        _cutsVectors = new Vector2[2];
        _cutState = false;

        UpdateButtons();
    }

    public void Cutter()
    {
        _isCutable = !_isCutable;
        _isRemovingPlanet = false;
        _isCreatingPlanet = false;
        _cutsVectors = new Vector2[2];
        _cutState = false;
        
        UpdateButtons();
    }

    void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition); 
        RaycastHit2D[] hits = Physics2D.RaycastAll (mousePosition, new Vector2 (0, 0), 0.01f);
        
        // if player wants to place new planet on existing planet
        if(_isCreatingPlanet) foreach(var hit in hits) if (hit.collider.CompareTag("Planet")) return;
        
        if (_isCreatingPlanet)
        {
            PlanetComponentsController.Instance.CreatePlanet();
            _isCreatingPlanet = false;
        }
        
        if(_isRemovingPlanet) foreach(var hit in hits)
            if (hit.collider.CompareTag("Planet"))
            {
                PlanetComponentsController.Instance.DestroyPlanet(hit.collider.gameObject.GetComponent<PlanetComponentHandler>());
                _isRemovingPlanet = false;
            }

        if (_isCutable)
        {
            if (!_cutState)
            {
                _cutsVectors[0] = mousePosition;
                _cutState = true;
            }
            else
            {
                _cutsVectors[1] = mousePosition;
                PlanetSlice.Instance.Slice(_cutsVectors[0], _cutsVectors[1]);
                _isCutable = false;
            }
        }

        UpdateButtons();
    }

    void UpdateButtons()
    {
        _buttonsImage[0].color = _isCreatingPlanet ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0.6f);
        _buttonsImage[1].color = _isRemovingPlanet ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0.6f);
        _buttonsImage[2].color = _isCutable ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0.6f);
    }
}
