using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public static GlobalVariables Instance { get; private set; }
    
    public GravityObject CurrentGravityObject;

    public bool OverlayShown = false;

    public Font GlobalFont;
    
    // global prefabs
    public GameObject VelocityPrefab, DistancePrefab;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
    }
}
