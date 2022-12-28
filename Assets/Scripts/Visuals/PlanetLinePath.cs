using System.Collections;
using UnityEngine;

public class PlanetLinePath : MonoBehaviour
{
    public GameObject LinePrefab;
    private GameObject currentLineHolder;
    public LineRenderer CurrentLineRenderer;
    public float WaitCount=  0.1f;
    public bool DrawState = false, ReadyForDraw = true;
    private GravityObjectsController _controller;


    void Start()
    {
        _controller = GameObject.FindWithTag("GravityController").GetComponent<GravityObjectsController>();
    }

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
            CurrentLineRenderer = currentLineHolder.GetComponent<LineRenderer>();
        }
        CurrentLineRenderer.SetPosition(DrawState ? 1 : 0, transform.position);
        
        DrawState = !DrawState;
        currentLineHolder.SetActive(!DrawState);

        if (!_controller.LinesVisible)
        {
            CurrentLineRenderer.enabled = false;
        }
        yield return new WaitForSeconds(WaitCount);
        ReadyForDraw = true;
    }
}
