using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetsCreatingButtonsController : MonoBehaviour
{
    [SerializeField] private GameObject _planetPrefab;
    [SerializeField] private Transform _planetsHolder;
    [SerializeField] private Image[] _buttonsImage = new Image[2];
    private bool _isCreatingPlanet = false, _isRemovingPlanet = false;

    void Start() => UpdateButtons();
    public void AddPlanet()
    { 
        _isCreatingPlanet = !_isCreatingPlanet;
        _isRemovingPlanet = false;

        UpdateButtons();
    }

    public void DeletePlanet()
    {
        _isRemovingPlanet = !_isRemovingPlanet;
        _isCreatingPlanet = false;

        UpdateButtons();
    }

    void Update()
    {
        if (!Input.GetMouseButton(0)) return;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition); 
        RaycastHit2D[] hits = Physics2D.RaycastAll (mousePosition, new Vector2 (0, 0), 0.01f);
        
        if(_isCreatingPlanet) foreach(var hit in hits) if (hit.collider.CompareTag("Planet")) return;
        if(_isRemovingPlanet) foreach(var hit in hits)
            if (hit.collider.CompareTag("Planet"))
            {
                Destroy(hit.collider.gameObject);
                _isRemovingPlanet = false;
            }

        if (_isCreatingPlanet)
        {
            Instantiate(_planetPrefab, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.Euler(0,0,0),  _planetsHolder);
            _isCreatingPlanet = false;
        }
        
        UpdateButtons();
    }

    void UpdateButtons()
    {
        _buttonsImage[0].color = _isCreatingPlanet ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0.6f);
        _buttonsImage[1].color = _isRemovingPlanet ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0.6f);
    }
}