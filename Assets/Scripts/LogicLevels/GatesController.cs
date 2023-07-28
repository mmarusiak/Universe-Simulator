using System.Collections.Generic;
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

        void Update()
        {
            if (_areaGates.Count == 0 || !Input.GetMouseButton(0)) return;
            foreach (var areaGate in _areaGates)
            {
                if (UniverseCamera.Instance.IsMouseOverGameObject(areaGate.Panel.transform))
                {
                    Debug.Log("We are handling input of: " + areaGate.Position);
                    // handle controls!
                    // move / scale ?
                }
            }
        }
    }
}
