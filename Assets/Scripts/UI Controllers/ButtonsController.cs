using UnityEngine;
using UnityEngine.UI;

public class ButtonsController : MonoBehaviour
{
    [SerializeField] private Button play, pause, reset;
    
    [SerializeField] private Color baseColor = new Color32(248, 248, 248, 255);
    [SerializeField] private float pressedOpacity = 0.5f;

    private void Start()
    {
        play.onClick.AddListener(()=> PlayPause(true, play));
        pause.onClick.AddListener(()=> PlayPause(false, pause));
    }

    void PlayPause(bool targetState, Button btn)
    {
        if (Time.timeScale == 0 == targetState)
        {
            GravityObjectsController.Instance.PlayPause();
            
            ToggleButtons();
            
            if (reset.gameObject.GetComponent<ToggleButton>().toggled)
            {
                reset.gameObject.GetComponent<ToggleButton>().Toggle();
            }
        }
    }
    
    public void ResetScene()
    {
        if (Time.timeScale != 0)
        {
            ToggleButtons();
        }
        
        GravityObjectsController.Instance.ResetScene();
        
        if (reset.gameObject.GetComponent<ToggleButton>().toggled == false)
        {
            reset.gameObject.GetComponent<ToggleButton>().Toggle();
        }
    }

    public void LineCheck() => GravityObjectsController.Instance.LineCheck();

    

    void ToggleButtons()
    {
        play.gameObject.GetComponent<ToggleButton>().Toggle();
        pause.gameObject.GetComponent<ToggleButton>().Toggle();
    }
}
