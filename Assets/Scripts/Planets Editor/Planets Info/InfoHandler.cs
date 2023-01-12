using UnityEngine;

public class InfoHandler : MonoBehaviour
{
    public GameObject AttachedPlanet;

    void Update()
    {
        // Update data ???
    }
    
    public void LoadInfoData(GameObject dataContainer)
    {
        var gravityForce = dataContainer.GetComponent<GravityObject>().CurrentGravityForceVector;
        Debug.Log(gravityForce);
    }

    public void Close()
    {
        Destroy(gameObject);
    }
}
