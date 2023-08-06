using LogicLevels;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities.UniverseLibraries
{
    public class UniverseCamera : MonoBehaviour
    {
        public static UniverseCamera Instance;
        public static readonly Vector3 CameraInitialPosition = new (0, 0, - 10);
        public static readonly float CameraInitialZoom = 85;
        private void Awake() => Instance = this;
    
        [SerializeField]
        private Camera myCamera;
        [SerializeField] private Text outPos;
        public Camera Camera => myCamera;

        private bool _isCameraMoving;

        private void Start()
        {
            if (myCamera == null)
            {
                Debug.LogError("Camera has not been set for UniverseCamera script!");
                enabled = false;
                return;
            }
            UpdatePos();
        }


        public void SetCameraPosition(Vector3 newPos)
        {
            myCamera.transform.position = newPos;
            UpdatePos();
        }

        public void Reset()
        {
            SetCameraPosition(CameraInitialPosition);
            myCamera.orthographicSize = CameraInitialZoom;
        }

        public Vector3 WorldToScreen(Vector3 world) => myCamera.WorldToScreenPoint(world);

        public Vector3 ScreenToWorld(Vector3 screen) => myCamera.ScreenToWorldPoint(screen);

        public Vector3 GetMousePosInWorld() => ScreenToWorld(Input.mousePosition);

        public void ChangeMoveState(bool target) => _isCameraMoving = target;

        public bool GetMoveState() => _isCameraMoving;


        private void UpdatePos()
        {
            if (GatesController.Instance != null) GatesController.Instance.UpdateGatesPositions();
            if (outPos == null) return;
            
            var position = myCamera.transform.position;
            string x = "<color=red>" + UniverseTools.RoundOutput(position.x) + "</color>";
            string y = "<color=green>" + UniverseTools.RoundOutput(position.y) + "</color>";
        
            string output = $"Camera position ({x}, {y})";
            outPos.text = output;
        }

        public bool IsMouseOverGameObject(Transform target)
        {
            Vector2 mousePos = GetMousePosInWorld();

            var position = target.position;
            var lossyScale = target.lossyScale;

            float leftEdge = position.x;
            float rightEdge = position.x + (lossyScale.x);
            float lowerEdge = position.y - (lossyScale.y);
            float upperEdge = position.y;

            return (mousePos.x >= leftEdge && mousePos.x <= rightEdge && mousePos.y >= lowerEdge &&
                    mousePos.y <= upperEdge);
        }
    }
}
