using System.Collections.Generic;
using UnityEngine;

public class TimersController : MonoBehaviour
{
    public static TimersController Instance;
    void Awake() => Instance = this;

    private List<UniverseTimer> _timers = new ();

    public void BeginTimer(UniverseTimer timer)
    {
        _timers.Add(timer);
    }

    public void EndTimer(UniverseTimer timer)
    {
        _timers.Remove(timer);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var rTimer in _timers)
        {
            rTimer.Time += Time.deltaTime;
        }
    }
}
