using UnityEngine;

public class SlicerRenderer : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    private float _size = 10;

    public void DrawLine(Vector2 pointA, Vector2 pointB)
    {
        // functions params -> y = ax + b
        UniverseMath.GetLinearFunctionParams(pointA, pointB, out var a, out var b);
        float dist = Vector2.Distance(pointA, pointB);
        int segmentsCount = (int) Mathf.Floor(dist / _size);

        for (int i = 0; i < segmentsCount; i++)
        {
            bool isEmpty = i % 2 == 1;
            if (isEmpty) continue;
            // if not draw segment - should create a lot of line renderers??
        }
    }
}
