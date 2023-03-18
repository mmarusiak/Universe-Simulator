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

    void Start() => _playback.IsReset = true;
    public void PlayLevel()
    {
        _onPlayed.Invoke();
        _playback.IsPaused = false;
    }

    public void PauseLevel()
    {
        _onPaused.Invoke();
        _playback.IsPaused = true;
    }

    public void ResetLevel()
    {
        _onReset.Invoke();
        _playback.IsReset = true;
    }
}
