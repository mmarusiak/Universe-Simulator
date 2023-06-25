using System;
using GameCore.SimulationCore;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Base class for playback. It holds pause and reset state and time scale.
/// </summary>

[Serializable]
public class Playback
{
    private bool _isPaused = true;
    private bool _isReset = true;
    private float _timeScale = 1.0f;

    [SerializeField] private Image _playIMG, _pauseIMG, _resetIMG;
    
    public bool IsPaused
    {
        get => _isPaused;
        set => ChangePause(value);
    }

    public bool IsReset
    {
        get => _isReset;
        set => ChangeReset(value);
    }

    public float TimeScale
    {
        get => _timeScale;
        set => ChangeTimeScale(value);
    }

    void ChangePause(bool targetState)
    {
        _isPaused = targetState;
        Time.timeScale = _isPaused ? 0 : TimeScale;
        // if is not paused - level is played, then it means it is no longer on reset
        if (!IsPaused) IsReset = false;
        
        SetButtonsColors();
    }

    void ChangeReset(bool targetState)
    {
        _isReset = targetState;
        
        if (!IsReset) return;
        IsPaused = true;

        PlanetComponentsController.Instance.ResetLevel();

        SetButtonsColors();
    }

    void ChangeTimeScale(float targetScale)
    {
        _timeScale = targetScale;
        if(IsPaused) return;
        Time.timeScale = TimeScale;
    }

    public void SetButtonsColors()
    {
        _resetIMG.color = IsReset ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0.6f);
        _pauseIMG.color = IsPaused ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0.6f);
        _playIMG.color = !IsPaused ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0.6f);
    }
}
