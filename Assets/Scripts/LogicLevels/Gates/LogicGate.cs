using UnityEngine;
using UnityEngine.Events;

namespace LogicLevels
{
    public class LogicGate
    {
        private bool _triggered = false;
        public UnityEvent OnGateReset = new();

        public string ID { get; set; }

        public bool Triggered => _triggered;

        /// <summary>
        /// Triggers gate, if condition was met.
        /// </summary>
        public void Trigger()
        {
            Debug.Log("triggered....");
            _triggered = true;
            LogicLevelController.Instance.CheckGates();
        }

        public void Reset()
        {
            OnGateReset.Invoke();
            _triggered = false;
        }
    }
}
