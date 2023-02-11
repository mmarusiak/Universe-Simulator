using UnityEngine;

public class GravityComponentHandler : MonoBehaviour
{
    [SerializeField] private float mass, radius;
    [SerializeField] private string name;
    [SerializeField] private Vector2 spawnPos;
    
    private GravityComponent _myComponent = null;

    private void Start()
    {
        _myComponent = new GravityComponent(transform, transform.GetChild(0).GetComponent<SpriteRenderer>(),
            radius, mass, spawnPos, name);
    }

    void Update()
    {
        _myComponent.AddForce();
    }
}
