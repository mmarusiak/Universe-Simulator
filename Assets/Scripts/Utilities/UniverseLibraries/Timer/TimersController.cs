using System.Collections.Generic;
using UnityEngine;

namespace Utilities.UniverseLibraries.Timer
{
    public class TimersController : MonoBehaviour
    {
        public static TimersController Instance;
        void Awake() => Instance = this;

        private List<UniverseTimer> _timers = new ();

        public void StartTimer(UniverseTimer timer)
        {
            timer.Time = 0;
            if (_timers.Contains(timer)) StopTimer(timer);
            _timers.Add(timer);
        }

        public void StopTimer(UniverseTimer timer) => _timers.Remove(timer);

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
