using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities.UniverseLibraries.Timer;
using Random = UnityEngine.Random;

namespace LogicLevels
{
    public class LogicLevelController : MonoBehaviour
    {
        public static LogicLevelController Instance;
        private readonly List<LogicGate> _gates = new ();
        private readonly UniverseTimer _levelTimer = new();
        private bool _isLevelInEditMode;
        [SerializeField] private Text modeTxt;

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
            
            TimersController.Instance.StopTimer(_levelTimer);
            
            Debug.Log($"Level completed at time: {_levelTimer.Time}");
        }

        public void OnPlayLogicController()
        {
            TimersController.Instance.StartTimer(_levelTimer);
        }

        public void OnPauseLogicController()
        {
            TimersController.Instance.StopTimer(_levelTimer);
        }

        public void OnResetLogicController()
        {
            // reset timer
            TimersController.Instance.ResetTimer(_levelTimer);
            // reset gates
            foreach (var gate in _gates) gate.Reset();
        }

        public void ChangeMode()
        {
            _isLevelInEditMode = !_isLevelInEditMode;
            if (_isLevelInEditMode)
            {
                modeTxt.text = "Test Level";
                // reset logic level?
                PlaybackController.Instance.ResetLevel();
                return;
            }

            modeTxt.text = "Edit Level";
        }
    }
}
