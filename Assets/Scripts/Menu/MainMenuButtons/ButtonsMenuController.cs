using UnityEngine;
using UnityEngine.UI;

public class ButtonsMenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainSectionsContainer, loaderContainer, newLevelContainer;
    
    public async void NewLevel(InputField field)
    {
        await SavingHandler.Instance.CreateNewLevel(field);
    }

    public void NewLevelSwitch(bool state)
    {
        // go to new level creator from main menu
        mainSectionsContainer.SetActive(!state);
        newLevelContainer.SetActive(state);   
    }

    public void LoaderSwitch(bool state)
    {
        // go to loader from main menu
        mainSectionsContainer.SetActive(!state);
        loaderContainer.SetActive(state);
    }
}
