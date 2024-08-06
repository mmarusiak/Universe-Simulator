using UnityEngine;
using Utilities.UniverseLibraries;

// TO REFACTOR !!!
public class GlobalVariables : MonoBehaviour
{
    public static GlobalVariables Instance { get; private set; }

    public bool OverlayShown = false;
    public Font GlobalFont;
    public string PathToImages;
    public float CameraDefSize;
    

    public static float GravitationalConstant = 0.06674f;
    
    // global prefabs
    public GameObject VelocityPrefab, DistancePrefab;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            CameraDefSize = Camera.main.orthographicSize;
        }
    }
    
    // Update is called once per frame
    async void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            
            await UniversePictures.TakeGameScreenshot("/home/mmarusiak/Documents/UniverseScreens/NEWCAPTURE");
    }
}
