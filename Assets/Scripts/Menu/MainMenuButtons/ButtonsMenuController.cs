using UnityEngine;

public class ButtonsMenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainSectionsContainer, loaderContainer;
    
    public void NewLevel()
    {
        // we need to create new level eventually also we should set name of the new level ???
    }

    public void LoaderSwitch(bool state)
    {
        // go to loader from main menu
        mainSectionsContainer.SetActive(!state);
        loaderContainer.SetActive(state);
    }
}
