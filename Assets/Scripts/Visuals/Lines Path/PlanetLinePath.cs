using System;
using System.Collections.Generic;
using UnityEngine;

public class PlanetLinePath : MonoBehaviour
{
    public GameObject LinePrefab;
    private GameObject currentLineHolder = null;
    private LineRenderer currentLineRenderer = null;

    private int currentSegment;

    private Vector2 lastPos;

    private int PointsAdded = 0;
    
    public float SizeOfSegment = 0.2f;
    public int PointsInSingleSegment = 4;
    private GravityObjectsController _controller;
    
    public List<LineSegment> Lines = new List<LineSegment>();


    void Start() => _controller = GravityObjectsController.Instance;

    void LateUpdate()
    {
        if (!_controller.Paused && CanAddNextPoint())
        {
            AddNextPoint();
        }
    }

    void AddNextPoint()
    {
        currentLineRenderer.SetPosition(PointsAdded, transform.position);
        PointsAdded++;
    }
    
    bool CanAddNextPoint()
    {
        if (currentLineHolder == null || PointsAdded == currentLineRenderer.positionCount)
        {
            try
            {
                lastPos = currentLineRenderer.GetPosition(PointsInSingleSegment - 1);
                currentLineRenderer.enabled = _controller.LinesVisible;
                Lines[currentSegment - 1].Finished = true;
            }
            catch (Exception e)
            {
                lastPos = transform.position;
            }

            PointsAdded = 0;

            currentLineHolder = Instantiate(LinePrefab, transform.position, Quaternion.Euler(0,0,0), transform);

            currentLineRenderer = currentLineHolder.GetComponent<LineRenderer>();
            currentLineRenderer.positionCount = PointsInSingleSegment;
            Lines.Add(new LineSegment(currentLineHolder, currentLineRenderer, false));
            currentSegment = Lines.Count;
            
            currentLineRenderer.enabled = false;
            
            return false;
        }
        if(Mathf.Abs(Vector2.Distance(lastPos, transform.position)) >= SizeOfSegment)
        {
            lastPos = transform.position;
            return true;
        }

        return false;
    }

    public GameObject GetLine()
    {
        return currentLineHolder;
    }
}
