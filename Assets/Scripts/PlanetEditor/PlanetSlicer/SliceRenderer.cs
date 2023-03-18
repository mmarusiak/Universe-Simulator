using UnityEngine;

public class SliceRenderer : MonoBehaviour
{
    public static SliceRenderer Instance;
    [SerializeField] private LineRenderer lineRenderer;

    void Awake() => Instance = this;
    
    // simple renderer
    public void DrawLine(Vector2 pointA, Vector2 pointB)
    {
        lineRenderer.positionCount = 2;
        Vector3[] points = { pointA, pointB };
        lineRenderer.SetPositions(points);
        //CheckForMaxPoints();
    }

    public void Hide() => lineRenderer.positionCount = 0;
}
