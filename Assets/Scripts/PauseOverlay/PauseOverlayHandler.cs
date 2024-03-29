using UnityEngine;
using UnityEngine.UI;

namespace PauseOverlay
{
    public class PauseOverlayHandler : MonoBehaviour
    {
        public static PauseOverlayHandler Instance;
        void Awake() => Instance = this;
    
        enum OverlayState
        {
            GameSettings,
            LevelSettings,
            EditorSettings
        }
    
    
        [SerializeField] private KeyCode activationKey = KeyCode.Escape;
        [SerializeField] private GameObject pauseOverlayHolder;
        [SerializeField] private GameObject[] overlays = new GameObject[3];
        [SerializeField] private Image[] overlaysButtonsImages = new Image[3];

        // Start is called before the first frame update
        private void Start() => ShowOverlay(OverlayState.LevelSettings);

        // Update is called once per frame
        private void Update()
        {
           CheckForActivationKey();
        }

        private void CheckForActivationKey()
        {
            if (Input.GetKeyDown(activationKey))
            {
                GlobalVariables.Instance.OverlayShown = !GlobalVariables.Instance.OverlayShown;
                TogglePause();
                ShowOverlay(OverlayState.LevelSettings);
            }
        }

    
        public void OverlayButtonClicked(int targetState) => ShowOverlay((OverlayState) targetState);

        private void ShowOverlay(OverlayState state)
        {
            pauseOverlayHolder.SetActive(GlobalVariables.Instance.OverlayShown);
            if (!GlobalVariables.Instance.OverlayShown) return;
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

        private bool _needToUnpause;

        private void TogglePause()
        {
            if (_needToUnpause) PlaybackController.Instance.PlayLevel();

            if (GlobalVariables.Instance.OverlayShown && !PlaybackController.Instance.Playback.IsPaused)
            {
                _needToUnpause = true;
                PlaybackController.Instance.PauseLevel();
                return;
            }
            _needToUnpause = false;
        }
    
        public void OnLoad() => _needToUnpause = false;
    }
}
