using System.Collections.Generic;
using UnityEngine;

namespace Utilities.UniverseLibraries.Timer
{
    public class TimersController : MonoBehaviour
    {
        public static TimersController Instance;
        void Awake() => Instance = this;

        private readonly List<UniverseTimer> _timers = new ();

        // starts new timer, counting from 0
        public void StartNewTimer(UniverseTimer timer)
        {
            timer.Time = 0;
            StartTimer(timer);
        }

        // starting any timer, counting from old Time value
        public void StartTimer(UniverseTimer timer)
        {
            if (_timers.Contains(timer)) return;
            _timers.Add(timer);
        }
        
        public void StopTimer(UniverseTimer timer) => _timers.Remove(timer);

        public void ResetTimer(UniverseTimer timer)
        {
            StopTimer(timer);
            timer.Time = 0;
        }

        // Update is called once per frame
        void Update()
        {
            foreach (var rTimer in _timers)
            {
                rTimer.Time += Time.unscaledDeltaTime;
            }
        }
    }
}
