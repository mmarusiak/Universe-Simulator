using UnityEngine;

public class PauseOverlayHandler : MonoBehaviour
{
    enum overlayState
    {
        GameSettings,
        LevelSettings,
        EditorSettings
    }
    
    
    [SerializeField]
    private KeyCode ActivationKey = KeyCode.Escape;
    [SerializeField]
    private GameObject pauseOverlayHolder;
    
    private overlayState _state;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(ActivationKey))
        {
            GlobalVariables.Instance.OverlayShown = !GlobalVariables.Instance.OverlayShown;
            TogglePause();
            ShowOverlay(_state = overlayState.LevelSettings);
        }
    }

    void ShowOverlay(overlayState state)
    {
        Debug.Log(state);
        pauseOverlayHolder.SetActive(GlobalVariables.Instance.OverlayShown);
        Debug.Log("Overlay pause shown");
    }

    
    private bool needToUnpause;
    void TogglePause()
    {
        if (needToUnpause) GravityObjectsController.Instance.PlayPause();

        if (GlobalVariables.Instance.OverlayShown && !GravityObjectsController.Instance.Paused)
        {
            needToUnpause = true;
            GravityObjectsController.Instance.PlayPause();
        }
        else needToUnpause = false;
    }
}
