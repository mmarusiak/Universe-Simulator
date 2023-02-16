using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public static GlobalVariables Instance { get; private set; }

    public bool OverlayShown = false;

    public Font GlobalFont;

    public string PathToImages;

    public static float GravitationalConstant = 0.06674f;
    
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
