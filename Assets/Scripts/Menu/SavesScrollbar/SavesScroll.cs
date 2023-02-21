using UnityEngine;

public class SavesScroll : MonoBehaviour
{
    private bool _isScrollable = false;
    [SerializeField]
    private RectTransform _savesContainer;

    [SerializeField] private float sensivity = 10;

    private void Update()
    {
        if (!_isScrollable) return;

        float scrollAxis = Input.GetAxis("Mouse ScrollWheel");
        if (scrollAxis == 0) return;
        _savesContainer.position += new Vector3(0, scrollAxis * sensivity, 0);
    }

    public void OnEnterScrollZone()
    {
        Debug.Log("enter");
        _isScrollable = true;
    }
    public void OnQuitScrollZone()
    {
        Debug.Log("quit");
        _isScrollable = false;
    }
}
