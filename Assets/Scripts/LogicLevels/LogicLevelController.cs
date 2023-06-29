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
        private int _planetActions = 10; // creates, change velocity etc.
        
        [SerializeField] private Text modeTxt;
        [SerializeField] private GameObject logicEditorSection;
        
        public bool IsLevelInEditMode => _isLevelInEditMode;
        public int PlanetActions => _planetActions;

        void Awake() => Instance = this;
        
        
        /// <summary>
        /// Creates and add new gate to controller's gates list.
        /// </summary>
        /// <param name="sender">
        /// Object that called function. Used to generate "unique" (unique as possible) ID.
        /// </param>
        /// <returns>
        /// Created gate.
        /// </returns>
        public LogicGate AddNewGate(object sender)
        {
            LogicGate newGate = new LogicGate
            {
                ID = sender.ToString() + sender.GetHashCode() + Random.Range(0, 100)
            };
            _gates.Add(newGate);
            return newGate;
        }

        /// <summary>
        /// Removes gate from controller's gates list.
        /// </summary>
        /// <param name="id">
        /// Gate's unique ID.
        /// </param>
        /// <returns></returns>
        public void RemoveGate(string id)
        {
            foreach (var gate in _gates)
            {
                if (gate.ID != id) continue;

                _gates.Remove(gate);
                return;
            }
        }

        /// <summary>
        /// Check if all gates were triggered. If were that means that player's completed level.
        /// </summary>
        /// <returns></returns>
        public void CheckGates()
        {
            foreach (var gate in _gates) if (!gate.Triggered) return;
            
            TimersController.Instance.StopTimer(_levelTimer);
            
            Debug.Log($"Level completed at time: {_levelTimer.Time}");
        }

        

        /// <summary>
        /// Logic controller void that is called when player hits "play" button. It's assigned in Unity Editor.
        /// </summary>
        /// <returns></returns>
        public void OnPlayLogicController()
        {
            TimersController.Instance.StartTimer(_levelTimer);
        }

        /// <summary>
        /// Logic controller void that is called when player hits "pause" button. It's assigned in Unity Editor.
        /// </summary>
        /// <returns></returns>
        public void OnPauseLogicController()
        {
            TimersController.Instance.StopTimer(_levelTimer);
        }

        /// <summary>
        /// Logic controller void that is called when player hits "reset" button. It's assigned in Unity Editor.
        /// </summary>
        /// <returns></returns>
        public void OnResetLogicController()
        {
            // reset timer
            TimersController.Instance.ResetTimer(_levelTimer);
            // reset gates
            foreach (var gate in _gates) gate.Reset();
        }

        /// <summary>
        /// Void to change between "test" and "edit" level modes. It's assigned to button via Unity Editor.
        /// </summary>
        /// <returns></returns>
        public void ChangeMode()
        {
            _isLevelInEditMode = !_isLevelInEditMode;
            logicEditorSection.SetActive(_isLevelInEditMode);
            if (_isLevelInEditMode)
            {
                modeTxt.text = "Test Level";
                // reset logic level?
                PlaybackController.Instance.ResetLevel();
                return;
            }

            modeTxt.text = "Edit Level";
        }

        /// <summary>
        /// Called when new planet is created in logic level in test/play mode.
        /// </summary>
        /// <returns></returns>
        public void PlanetCreated()
        {
            _planetActions--;
            Debug.Log($"Planet created! : {this.name}");
        }

        /// <summary>
        /// Called when player's planet is removed in logic level in test/play mode.
        /// </summary>
        /// <returns></returns>
        public void PlanetRemoved()
        {
            _planetActions++;
            Debug.Log($"Planet removed! : {this.name}");
        }
    }
}