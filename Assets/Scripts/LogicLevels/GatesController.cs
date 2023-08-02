using System.Collections.Generic;
using LogicLevels.Gates.AreaGate;
using UnityEngine;
using Utilities.UniverseLibraries;

// Handles all action that player can make on already existing gate
namespace LogicLevels
{
    public class GatesController : MonoBehaviour
    {
        public static GatesController Instance;
        private static LogicLevelController _levelController = LogicLevelController.Instance;

        private readonly List<LogicAreaGate> _areaGates = new();
        
        // lets add for now editor for area gates
        void Awake() => Instance = this;

        public void AddAreaGate(LogicAreaGate gate)
        {
            _areaGates.Add(gate);
        }

        private bool _isGateMoving = false, _isGateScaling = false;
        
        void Update()
        {
            if (_areaGates.Count == 0 || !Input.GetMouseButton(0) || !PlaybackController.Instance.Playback.IsPaused || !LogicLevelController.Instance.IsLevelInEditMode)
            {
                _isGateMoving = false;
                _isGateScaling = false;
                return;
            }

            if (_isGateScaling)
            {
                ScaleAreaGate(_lastGate);
                return;
            }
            
            CheckForGate();
        }

        public void UpdateGatesPositions()
        {
            foreach (var gate in _areaGates) gate.DrawSelf();
        }
        
        void CheckForGate()
        {
            foreach (var areaGate in _areaGates)
            {
                if (UniverseCamera.Instance.IsMouseOverGameObject(areaGate.Panel.transform))
                {
                    Debug.Log("We are handling input of: " + areaGate.Position);
                    // scale control
                    if (Input.GetKey(KeyCode.S))
                    {
                        _isGateMoving = false;
                        ScaleAreaGate(areaGate);
                        return;
                    }

                    _isGateScaling = false;
                    MoveAreaGate(areaGate);
                }
            }
        }

        // in moving it's offset, in scaling it's last position of the mouse
        private Vector2 _helperVector;
        private Vector2 _initialScale;
        private LogicAreaGate _lastGate;
        void ScaleAreaGate(LogicAreaGate gate)
        {
            _lastGate = gate;
            if (!_isGateScaling)
            {
                _helperVector = UniverseCamera.Instance.GetMousePosInWorld();
                _initialScale = gate.Size;
                _isGateScaling = true;
                return;
            }

            Vector2 currentPos = UniverseCamera.Instance.GetMousePosInWorld();
            Vector2 scale = currentPos - _helperVector;
            _helperVector = currentPos;
            gate.Size += scale;
        }
        
        
        void MoveAreaGate(LogicAreaGate gate)
        {
            if (!_isGateMoving)
            {
                _helperVector = (Vector2)UniverseCamera.Instance.GetMousePosInWorld() - gate.Position;
                _isGateMoving = true;
            }

            Vector2 currentPos = UniverseCamera.Instance.GetMousePosInWorld();
            gate.Position = (currentPos - _helperVector);
        }
    }
}
