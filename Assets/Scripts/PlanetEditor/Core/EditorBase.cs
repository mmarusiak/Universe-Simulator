using System;
using GameCore.SimulationCore;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Editor base for every editor. It contains bunch of events that are the same for every editor.
/// </summary>
[Serializable]
public class EditorBase
{
    private PlanetComponent _currentPlanet;
    
    private bool _shown;
    public bool Shown
    {
        get => _shown;
        set => Show(value);
    }
    
    // graphics
    [SerializeField] private GameObject _editorContainer;
    [SerializeField] private Text _barText;
    [SerializeField] private string _windowTitle;
    [SerializeField] private Vector2 _offsetToShow;
    [Space]
    [SerializeField] private UnityEvent _onEditedPlanetChanged;
    [Space]
    [SerializeField] private UnityEvent _onWindowShow;
    [Space]
    [SerializeField] private UnityEvent _onWindowHide;
    
    private Vector2 _hiddenPos = new(-9000, -9000);

    public PlanetComponent CurrentPlanet
    {
        get => _currentPlanet;
        set
        {
            _currentPlanet = value;
            if (_currentPlanet == null)
            {
                Shown = false;
                return;
            }
            UpdateDisplayedPlanetName();
            _onEditedPlanetChanged.Invoke();
        }
    }

    private void Show(bool target)
    {
        _shown = target;
        ChangeDisplay();
    }

    private void ChangeDisplay()
    {
        if (_shown)
        {
            Vector2 windowDim = new(_editorContainer.GetComponent<RectTransform>().rect.width,
                _editorContainer.GetComponent<RectTransform>().rect.height);
            float leftXEdge = Input.mousePosition.x + _offsetToShow.x - windowDim.x/2;
            float rightXEdge = Input.mousePosition.x + _offsetToShow.x + windowDim.x/2;
            float upperYEdge = Input.mousePosition.y + _offsetToShow.y + windowDim.y/2;
            float loweYEdge = Input.mousePosition.y + _offsetToShow.y - windowDim.y/2;

            bool isXOnScreen = leftXEdge > 0 && rightXEdge < Screen.currentResolution.width;
            bool isYOnScreen = loweYEdge > 0 && upperYEdge < Screen.currentResolution.height;

            float offsetX = isXOnScreen ? _offsetToShow.x : -_offsetToShow.x;
            float offsetY = isYOnScreen ? _offsetToShow.y : -_offsetToShow.y;
            
            _editorContainer.GetComponent<RectTransform>().position = Input.mousePosition + new Vector3(offsetX, offsetY);
            _onWindowShow.Invoke();
            return;
        }
        
        _editorContainer.GetComponent<RectTransform>().position = _hiddenPos;
        _onWindowHide.Invoke();
    }

    public void UpdateDisplayedPlanetName()
    {
        if (_currentPlanet is null) return;
        _barText.text = $"{_currentPlanet.Name}'s {_windowTitle}";
    }
}
