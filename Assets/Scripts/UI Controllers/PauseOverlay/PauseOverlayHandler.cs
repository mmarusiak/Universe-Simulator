using UnityEngine;

public class PauseOverlayHandler : MonoBehaviour
{
    public KeyCode ActivationKey = KeyCode.Escape;
    [SerializeField]
    private GameObject pauseOverlayHolder;
    
    
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
            if(!GravityObjectsController.Instance.Paused == GlobalVariables.Instance.OverlayShown) GravityObjectsController.Instance.PlayPause();
            
            Debug.Log("Overlay pause shown");
        }
    }
}
