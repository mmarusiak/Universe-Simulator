using UnityEngine;

public class PlanetComponentHandler : MonoBehaviour
{
    [SerializeField] private float mass, radius;
    [SerializeField] private string name;
    [SerializeField] private Vector2 spawnPos;
    
    private PlanetComponent _myComponent = null;

    private void Start()
    {
        _myComponent = new PlanetComponent(this, transform, transform.GetChild(0).GetComponent<SpriteRenderer>(),
            radius, mass, spawnPos, name);
    }

    void Update()
    {
        _myComponent.AddForce();
    }
}
