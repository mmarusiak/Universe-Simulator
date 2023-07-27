using System.Collections.Generic;
using UnityEngine;

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
            foreach (var areaGate in _areaGates)
            {
                // check mouse input and handle it
            }
        }
    }
}
