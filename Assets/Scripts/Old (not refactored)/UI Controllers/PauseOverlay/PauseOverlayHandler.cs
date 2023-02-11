using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private GameObject[] overlays = new GameObject[3];
    [SerializeField] private Image[] overlaysButtonsImages = new Image[3];

    // Start is called before the first frame update
    void Start() => ShowOverlay(overlayState.LevelSettings);

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(ActivationKey))
        {
            GlobalVariables.Instance.OverlayShown = !GlobalVariables.Instance.OverlayShown;
            TogglePause();
            ShowOverlay(overlayState.LevelSettings);
        }
    }

    
    public void OverlayButtonClicked(int targetState) => ShowOverlay((overlayState) targetState);
    void ShowOverlay(overlayState state)
    {
        Debug.Log(state);
        pauseOverlayHolder.SetActive(GlobalVariables.Instance.OverlayShown);
        for (int overlayIndex = 0; overlayIndex < overlays.Length; overlayIndex++)
        {
            // set section active
            overlays[overlayIndex].SetActive(overlayIndex == (int)state && GlobalVariables.Instance.OverlayShown);
            
            // change buttons color
            Color newColor = overlaysButtonsImages[overlayIndex].color;
            newColor.a = overlayIndex == (int)state ? 1 : 0.38f;
            overlaysButtonsImages[overlayIndex].color = newColor;
            overlaysButtonsImages[overlayIndex].GetComponent<Shadow>().enabled = (int) state == overlayIndex;
        }
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
