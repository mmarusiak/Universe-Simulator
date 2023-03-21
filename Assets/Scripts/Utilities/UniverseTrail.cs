using System;
using UnityEngine;

[RequireComponent(typeof (LineRenderer))]
public class UniverseTrail : MonoBehaviour
{
    private LineRenderer _renderer;
    private Vector3 _lastPos;

    [SerializeField] private float vertSize = 0.5f;
    [SerializeField] private int amountOfPoints = 1000;

    void Start()
    {
        _renderer = GetComponent<LineRenderer>();
        _lastPos = transform.position;
    }

    void Update()
    {
        if (Time.timeScale <= 0) return;
        
        CreateNewPoint();
    }

    void CreateNewPoint()
    {
        if (Vector3.Distance(_lastPos, transform.position) < vertSize) return;
        
        _lastPos = transform.position;
        _renderer.positionCount++;
        _renderer.SetPosition(_renderer.positionCount - 1, _lastPos);
        DeleteOldPoints();
    }

    void DeleteOldPoints()
    {
        if (_renderer.positionCount <= amountOfPoints) return;

        Vector3[] currentPoints = new Vector3[_renderer.positionCount];
        Vector3[] newPoints = new Vector3[amountOfPoints];

        _renderer.GetPositions(currentPoints);
        Array.Copy(currentPoints, _renderer.positionCount - amountOfPoints, newPoints, 0, amountOfPoints);

        _renderer.positionCount = amountOfPoints;
        _renderer.SetPositions(newPoints);
    }

    public void Clear() => _renderer.positionCount = 0;
    
    public void SetColor(Color color) => _renderer.endColor = color;
}
