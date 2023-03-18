using UnityEngine;

public class SlicerRenderer : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    //[SerializeField] private int maxPoints = 2;

    // only for debug
    void Start() => lineRenderer = GetComponent<LineRenderer>();
    
    // simple renderer
    public void DrawLine(Vector2 pointA, Vector2 pointB)
    {
        Vector3[] points = { pointA, pointB };
        lineRenderer.SetPositions(points);
        //CheckForMaxPoints();
    }
}
