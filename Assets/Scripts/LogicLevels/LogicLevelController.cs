using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LogicLevels
{
    public class LogicLevelController : MonoBehaviour
    {
        public static LogicLevelController Instance;
        private List<LogicGate> _gates = new ();

        void Awake() => Instance = this;
        
        public LogicGate AddNewGate(object sender)
        {
            LogicGate newGate = new LogicGate
            {
                ID = sender.ToString() + sender.GetHashCode() + Random.Range(0, 100)
            };
            _gates.Add(newGate);
            return newGate;
        }

        public void RemoveGate(string id)
        {
            foreach (var gate in _gates)
            {
                if (gate.ID != id) continue;

                _gates.Remove(gate);
                return;
            }
        }

        public void CheckGates()
        {
            foreach (var gate in _gates) if (!gate.Triggered) return;
            
            Debug.Log("Level completed!");
        }

        public void ResetGates()
        {
            foreach (var gate in _gates) gate.Reset();
        }
    }
}
