using UnityEngine;

public class MultipleToggle : MonoBehaviour
{
    [SerializeField] private ToggleButton[] togglesToDisable;
    [SerializeField] private ToggleButton[] togglesToActivate;
    
    public void Toggle()
    {
        foreach (var tog in togglesToDisable)
        {
            if(tog.toggled)
                tog.Toggle();
        }
        
        foreach (var tog in togglesToActivate)
        { 
            tog.Toggle();
        }
    }
}
