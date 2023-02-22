using UnityEngine;

public class SavesScroll : MonoBehaviour
{
    private bool _isScrollable = false;
    [SerializeField] private RectTransform savesContainer;
    [SerializeField] private float sensivity = 10;

    private void Update()
    {
        if (!_isScrollable) return;

        float scrollAxis = Input.GetAxis("Mouse ScrollWheel");
        if (scrollAxis == 0) return;
        savesContainer.position += new Vector3(0, -scrollAxis * sensivity, 0);
    }

    public void OnEnterScrollZone() => _isScrollable = true;

    public void OnQuitScrollZone() => _isScrollable = false;
    
}
