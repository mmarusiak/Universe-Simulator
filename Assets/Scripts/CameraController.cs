using UnityEngine;
using Utilities.UniverseLibraries;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float sensitivity = 0.1f;
    private Transform _cam;
    private Vector2 _pointerStartPos, _scaler;
    
    // Start is called before the first frame update
    void Start() => _cam = UniverseCamera.Instance.Camera.transform;
    
    // Update is called once per frame
    void Update()
    {
        // triggers while user pressed
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            _pointerStartPos = Input.mousePosition*sensitivity;
            _scaler = new Vector2( _cam.position.x - _pointerStartPos.x, _cam.position.y - _pointerStartPos.y);
        }
        
        // triggers while user holds
        if (Input.GetKey(KeyCode.Mouse1))
        {
            UniverseCamera.Instance.ChangeMoveState(true);
            MoveCamera();    
        }
        // with this booleans ^v texts don't follow smooth planet when camera is being moved by player
        // texts are UI, not Scene Objects!
        else UniverseCamera.Instance.ChangeMoveState(false);

        // get scroll axis
        float scrollAxis = Input.GetAxis("Mouse ScrollWheel");
    
        // scroll if user scrolls
        if (scrollAxis != 0)
        {
            // we should also display zoom ?
            UniverseCamera.Instance.Camera.orthographicSize -= scrollAxis * 18;
        }
    }

    void MoveCamera()
    { 
        Vector3 newPos = new Vector3(Input.mousePosition.x*sensitivity + _scaler.x, Input.mousePosition.y*sensitivity + _scaler.y, -10);
        UniverseCamera.Instance.SetCameraPosition(newPos);
    }
}
