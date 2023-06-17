using GameCore.SimulationCore;
using UnityEngine;

public class LogicVelocityGate : MonoBehaviour
{
    [SerializeField] private float velocity;

    void Update()
    {
        foreach (var planet in PlanetComponentsController.Instance.AllGravityComponents)
        {
            if (planet.CurrentVelocity.magnitude >= velocity)
            {
                // gate achieved
                break;
            }
        }
    }
}
