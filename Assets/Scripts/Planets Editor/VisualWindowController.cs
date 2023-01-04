using UnityEngine;

public class VisualWindowController : MonoBehaviour
{
    void Start()
    {
        GetComponent<WindowController>().Close();
    }
}
