using GameCore.SimulationCore;
using UnityEngine;

namespace LogicLevels
{
    public class LogicVelocityGate : MonoBehaviour
    {
        [SerializeField] private float velocity;
        private LogicGate _myGate;

        public float Velocity
        {
            get => velocity;
            set => velocity = value;
        }

        void Start()
        {
            _myGate = LogicLevelController.Instance.AddNewGate(this);
            LogicLevelController.Instance.VelocityDataList.Add(this);
        }

        void Update()
        {
            if (PlaybackController.Instance.Playback.IsPaused) return;
        
            foreach (var planet in PlanetComponentsController.Instance.AllGravityComponents)
            {
                if (planet.CurrentVelocity.magnitude >= velocity)
                {
                    _myGate.Trigger();
                    break;
                }
            }
        }
    }
}
