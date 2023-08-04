using UnityEngine;
using UnityEngine.EventSystems;
using Utilities.UniverseLibraries;

namespace LogicLevels
{
    public class LogicEditor : MonoBehaviour
    {
        enum EditorState
        {
            Idle,
            CreatingArea,
            CreatingVelocity
        }

        private bool _areaFlag;
        private Vector2 _newGatePos, _newGateSize;
        private EditorState _state;

        public void BtnAddPlanetAction() => LogicLevelController.Instance.AddPlanetAction();
        public void BtnRemovePlanetAction() => LogicLevelController.Instance.RemovePlanetAction();

        public void BtnCreateAreaGate() => _state = EditorState.CreatingArea;
    
        public void BtnCreateVelocityGate() => _state = EditorState.CreatingVelocity;
    
        void Update()
        {
            if (_state != EditorState.CreatingArea) return;

            CreateArea();
        }

        private void CreateArea()
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                _newGatePos = UniverseCamera.Instance.GetMousePosInWorld();
                _areaFlag = true;
                return;
            }
        
            if (Input.GetMouseButtonUp(0) && _areaFlag)
            {
                _areaFlag = false;
                _newGateSize =  UniverseCamera.Instance.GetMousePosInWorld();
                _newGateSize -= _newGatePos;
            
                LogicLevelController.Instance.CreateAreaGate(_newGatePos, _newGateSize * new Vector2(1, -1));
            
                _state = EditorState.Idle;
            }
        }
    }
}
