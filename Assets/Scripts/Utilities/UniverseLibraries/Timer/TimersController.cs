using System.Collections.Generic;
using UnityEngine;

namespace Utilities.UniverseLibraries.Timer
{
    /// <summary>
    /// Here timers are counting time.
    /// </summary>
    public class TimersController : MonoBehaviour
    {
        public static TimersController Instance;
        private void Awake() => Instance = this;

        private readonly List<UniverseTimer> _timers = new ();

        /// Starts new timer, counting from 0.
        public void StartNewTimer(UniverseTimer timer)
        {
            timer.Time = 0;
            StartTimer(timer);
        }

        /// Starting any timer, counting from old Time value.
        public void StartTimer(UniverseTimer timer)
        {
            if (_timers.Contains(timer)) return;
            _timers.Add(timer);
        }
        
        /// Pausing timer.
        public void StopTimer(UniverseTimer timer) => _timers.Remove(timer);

        /// Resets timer - pausing it and resetting time of it.
        public void ResetTimer(UniverseTimer timer)
        {
            StopTimer(timer);
            timer.Time = 0;
        }

        // Update is called once per frame
        private void Update()
        {
            foreach (var rTimer in _timers)
            {
                rTimer.Time += Time.unscaledDeltaTime;
            }
        }
    }
}
