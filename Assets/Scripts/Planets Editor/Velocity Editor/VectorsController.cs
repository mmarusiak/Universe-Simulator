using UnityEngine;

public class VectorsController : MonoBehaviour
{
    private WindowController _windowController;
    public bool VectorsShown = false;

    // 0 - x
    // 1 - y
    // 2 - r
    [SerializeField]
    private LineRenderer[] VectorsLineRenderer = new LineRenderer[3];
    
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
        Vector3 planetPos = GlobalVariables.Instance.CurrentGravityObject.gameObject.transform.position;
        
        VectorsLineRenderer[0].SetPositions(new []{planetPos, planetPos + new Vector3(GlobalVariables.Instance.CurrentGravityObject.gameObject.GetComponent<Rigidbody2D>().velocity.x, 0)});
        VectorsLineRenderer[1].SetPositions(new []{planetPos, planetPos + new Vector3(0, GlobalVariables.Instance.CurrentGravityObject.gameObject.GetComponent<Rigidbody2D>().velocity.y)});
        VectorsLineRenderer[2].SetPositions(new []{planetPos, planetPos + new Vector3(GlobalVariables.Instance.CurrentGravityObject.gameObject.GetComponent<Rigidbody2D>().velocity.x, 
            GlobalVariables.Instance.CurrentGravityObject.gameObject.GetComponent<Rigidbody2D>().velocity.y)});
    }
}
