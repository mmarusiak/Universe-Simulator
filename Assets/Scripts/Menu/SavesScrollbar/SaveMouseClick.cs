using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SaveMouseClick : MonoBehaviour, IPointerClickHandler
{
    private string _saveName;

    void Start() => _saveName = transform.parent.parent.GetComponent<SaveContainer>().SaveName;

    public async void OnPointerClick(PointerEventData eventData)
    {
        DontDestroyOnLoad(SavingHandler.Instance);
        SceneManager.LoadScene("Game");
        await SavingHandler.Instance.LoadLevel(true, _saveName);
    }
}
