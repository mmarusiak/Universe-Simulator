using UnityEngine;

public class VectorsController : MonoBehaviour
{
    private WindowController _windowController;
    public bool VectorsShown = false;

    // on top
    private Vector3 orderInLayer = new Vector3(0, 0, -1);
    
    // 0 - x, 1 - y, 2 - r
    [SerializeField]
    private LineRenderer[] VectorsLineRenderer = new LineRenderer[3];

    private float scale = 2.25f;
    
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

        if (VectorsShown)
            ShowVectors();
        else
            HideVectors();
    }
    
    void ShowVectors()
    {
        Vector3 planetPos = GlobalVariables.Instance.CurrentGravityObject.gameObject.transform.position;
        
        VectorsLineRenderer[0].SetPositions(new []{planetPos + orderInLayer, planetPos + new Vector3(GlobalVariables.Instance.CurrentGravityObject.gameObject.GetComponent<Rigidbody2D>().velocity.x/scale, 0) + orderInLayer});
        VectorsLineRenderer[1].SetPositions(new []{planetPos + orderInLayer, planetPos + new Vector3(0, GlobalVariables.Instance.CurrentGravityObject.gameObject.GetComponent<Rigidbody2D>().velocity.y/scale) + orderInLayer});
        VectorsLineRenderer[2].SetPositions(new []{planetPos + orderInLayer, planetPos + new Vector3(GlobalVariables.Instance.CurrentGravityObject.gameObject.GetComponent<Rigidbody2D>().velocity.x/scale, 
            GlobalVariables.Instance.CurrentGravityObject.gameObject.GetComponent<Rigidbody2D>().velocity.y/scale) + orderInLayer});
    }

    void HideVectors()
    {
        VectorsLineRenderer[0].SetPositions(new []{Vector3.zero, Vector3.zero});
        VectorsLineRenderer[1].SetPositions(new []{Vector3.zero, Vector3.zero});        
        VectorsLineRenderer[2].SetPositions(new []{Vector3.zero, Vector3.zero});
    }
}
