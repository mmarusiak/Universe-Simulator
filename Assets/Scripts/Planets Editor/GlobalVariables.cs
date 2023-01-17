using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public static GlobalVariables Instance { get; private set; }
    
    public GravityObject CurrentGravityObject;
    
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
        if(GameObject.FindWithTag("EditorController").GetComponent<EditorHandler>().Planet != null)
            CurrentGravityObject = GameObject.FindWithTag("EditorController").GetComponent<EditorHandler>().Planet
                .GetComponent<GravityObject>();
    }
}
