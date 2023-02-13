using UnityEngine;

public class PlaybackController : MonoBehaviour
{
    public static PlaybackController Instance;
    void Awake() => Instance = this;

    [SerializeField]
    private Playback _playback;
    public Playback Playback => _playback;

    void Start() => _playback.IsReset = true;
    public void PlayLevel()
    {
        _playback.IsPaused = false;
    }

    public void PauseLevel()
    {
        _playback.IsPaused = true;
    }

    public void ResetLevel()
    {
        _playback.IsReset = true;
    }
}
