using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class UniverseCamera : MonoBehaviour
{
    public static UniverseCamera Instance;
    void Awake() => Instance = this;
    
    [SerializeField]
    private Camera myCamera;
    [SerializeField] private Text outPos;
    public Camera Camera => myCamera;

    void Start()
    {
        if (myCamera == null)
        {
            Debug.LogError("Camera has not been set for UniverseCamera script!");
            enabled = false;
            return;
        }
        UpdatePos();
    }
     
    

    public void SetCameraPosition(Vector3 newPos)
    {
        myCamera.transform.position = newPos;
        UpdatePos();
    }

    public Vector3 WorldToScreen(Vector3 world)
    {
        return myCamera.WorldToScreenPoint(world);
    }

    public Vector3 ScreenToWorld(Vector3 screen)
    {
        return myCamera.ScreenToWorldPoint(screen);
    }
    
    void UpdatePos()
    {
        if (outPos is null) return;
        
        string x = "<color=red>" + UniverseTools.RoundOutput(myCamera.transform.position.x) + "</color>";
        string y = "<color=green>" + UniverseTools.RoundOutput(myCamera.transform.position.y) + "</color>";
        
        string output = $"Camera position ({x}, {y})";
        outPos.text = output;
    }
}