using GameCore.SimulationCore;
using GameCore.SimulationCore.Handlers;
using UnityEngine;
using UnityEngine.UI;
using UniverseSound;
using Utilities.UniverseLibraries;

public class LeftSideButtonsController : MonoBehaviour
{
    [SerializeField] private Image[] _buttonsImage = new Image[3];
    private bool _isCreatingPlanet = false, _isRemovingPlanet = false, _isCutable = false, _cutState;
    private Vector2[] _cutsVectors = new Vector2[2];

    void Start() => UpdateButtons();
    
    // setting on add/create new planet mode
    public void AddPlanet()
    { 
        _isCreatingPlanet = !_isCreatingPlanet;
        _isRemovingPlanet = false;
        _isCutable = false;
        _cutsVectors = new Vector2[2];
        _cutState = false;
        UniverseSoundTree.Instance.PlaySoundByName("TEST", "Add", this);

        UpdateButtons();
    }

    // setting on delete/remove mode
    public void DeletePlanet()
    {
        _isRemovingPlanet = !_isRemovingPlanet;
        _isCreatingPlanet = false;
        _isCutable = false;
        _cutsVectors = new Vector2[2];
        _cutState = false;
        UniverseSoundTree.Instance.PlaySoundByName("TEST", "Delete", this);
        
        UpdateButtons();
    }

    // setting on cutter/slicer
    public void Cutter()
    {
        _isCutable = !_isCutable;
        _isRemovingPlanet = false;
        _isCreatingPlanet = false;
        _cutsVectors = new Vector2[2];
        _cutState = false;
        UniverseSoundTree.Instance.PlaySoundByName("TEST", "Cutter", this);
        
        UpdateButtons();
    }

    void Update()
    {
        if (GlobalVariables.Instance.OverlayShown) return;
        Vector2 mousePosition = UniverseCamera.Instance.ScreenToWorld(Input.mousePosition);
        if (_cutState) DrawSliceLine(mousePosition);
        else HideSliceLine();
       
        if (!Input.GetMouseButtonDown(0)) return;
        
        RaycastHit2D[] hits = Physics2D.RaycastAll (mousePosition, new Vector2 (0, 0), 0.01f);
        
        // if player wants to place new planet on existing planet
        if(_isCreatingPlanet) foreach(var hit in hits) if (hit.collider.CompareTag("Planet")) return;
        
        // actual adding new planet happens here
        if (_isCreatingPlanet)
        {
            PlanetComponentsController.Instance.CreatePlanet();
            _isCreatingPlanet = false;
        }
        
        // actual removing planet happens here
        if(_isRemovingPlanet) foreach(var hit in hits)
            if (hit.collider.CompareTag("Planet"))
            {
                PlanetComponentsController.Instance.DestroyPlanet(hit.collider.gameObject.GetComponent<PlanetComponentHandler>());
                _isRemovingPlanet = false;
            }
        
        // actual cut/slice happens here
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
                _isCutable = false;
                _cutState = false;
                
                PlanetSlice.Instance.Slice(_cutsVectors[0], _cutsVectors[1]);
            }
        }

        UpdateButtons();
    }

    private Vector2 tempOffset = new (1, 1);
    void DrawSliceLine(Vector2 mousePos) => SliceRenderer.Instance.DrawLine(_cutsVectors[0], mousePos - tempOffset);
    void HideSliceLine() => SliceRenderer.Instance.Hide();

    void UpdateButtons()
    {
        _buttonsImage[0].color = _isCreatingPlanet ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0.6f);
        _buttonsImage[1].color = _isRemovingPlanet ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0.6f);
        _buttonsImage[2].color = _isCutable ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0.6f);
    }
}
