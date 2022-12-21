using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float Sensivity = 0.1f;
    private Camera mainCamera;
    private GameObject cameraGO;
    private Vector2 pointerStartPos, scaler;
    
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        cameraGO = mainCamera.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // triggers while user pressed
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            pointerStartPos = Input.mousePosition*Sensivity;
            scaler = new Vector2(cameraGO.transform.position.x - pointerStartPos.x,
                cameraGO.transform.position.y - pointerStartPos.y);
        }
        
        // triggers while user holds
        if (Input.GetKey(KeyCode.Mouse1))
        {
            MoveCamera();    
        }
    }

    void MoveCamera()
    {
        cameraGO.transform.position = new Vector3(Input.mousePosition.x*Sensivity + scaler.x, Input.mousePosition.y*Sensivity + scaler.y, -10);
    }
}
