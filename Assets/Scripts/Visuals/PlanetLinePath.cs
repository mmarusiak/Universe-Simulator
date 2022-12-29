using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetLinePath : MonoBehaviour
{
    public GameObject LinePrefab;
    private GameObject currentLineHolder;
    private LineRenderer currentLineRenderer;
    public float WaitCount=  0.05f;
    public bool DrawState = false, ReadyForDraw = true;
    private GravityObjectsController _controller;

    public List<GameObject> Lines = new List<GameObject>();


    void Start() => _controller = GameObject.FindWithTag("GravityController").GetComponent<GravityObjectsController>();

    void LateUpdate()
    {
        if (!_controller.Paused && ReadyForDraw)
        {
            ReadyForDraw = false;
            StartCoroutine(DrawLine());
        }
    }

    IEnumerator DrawLine()
    {
        if (!DrawState)
        {
            currentLineHolder = Instantiate(LinePrefab, transform);
            currentLineHolder.transform.position = transform.position;
            currentLineHolder.SetActive(false);
            currentLineRenderer = currentLineHolder.GetComponent<LineRenderer>();
            
            Lines.Add(currentLineHolder);
        }
        currentLineRenderer.SetPosition(DrawState ? 1 : 0, transform.position);
        
        DrawState = !DrawState;
        currentLineHolder.SetActive(!DrawState);

        if (!_controller.LinesVisible)
        {
            currentLineRenderer.enabled = false;
        }
        yield return new WaitForSeconds(WaitCount);
        ReadyForDraw = true;
    }
}
