using UnityEngine;

public class VectorsController : MonoBehaviour
{
    private WindowController _windowController;
    public bool VectorsShown = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _windowController = gameObject.GetComponent<WindowController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (VectorsShown != _windowController.Shown)
            VectorsShown = !VectorsShown;
        
        if(VectorsShown)
            ShowVectors();
    }

    void ShowVectors()
    {
        // Render lines
    }
}
