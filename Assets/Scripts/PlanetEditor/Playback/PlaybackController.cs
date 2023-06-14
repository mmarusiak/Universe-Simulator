using UnityEngine;
using UnityEngine.Events;

public class PlaybackController : MonoBehaviour
{
    public static PlaybackController Instance;
    void Awake() => Instance = this;

    [SerializeField]
    private Playback _playback;
    [Space]
    [SerializeField] private UnityEvent _onPaused;
    [Space]
    [SerializeField] private UnityEvent _onPlayed;
    [Space]
    [SerializeField] private UnityEvent _onReset;
    public Playback Playback => _playback;

    void Start()
    {
        _playback.SetButtonsColors();
    }

    public void PlayLevel()
    {
        _playback.IsPaused = false;
        _onPlayed.Invoke();
    }

    public void PauseLevel()
    {
        _playback.IsPaused = true;
        _onPaused.Invoke();
    }

    public void ResetLevel()
    {
        _playback.IsReset = true;
        _onReset.Invoke();
    }
}
