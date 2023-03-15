using UnityEngine;

public class SlicerRenderer : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform linesHolder;
    private float _size = 10;

    public void DrawLine(Vector2 pointA, Vector2 pointB)
    {
        ClearAllLines();
        // functions params -> y = ax + b
        UniverseMath.GetLinearFunctionParams(pointA, pointB, out var a, out var b);
        float dist = Vector2.Distance(pointA, pointB);
        int segmentsCount = (int) Mathf.Floor(dist / _size);
        
        for (int i = 0; i < segmentsCount; i++)
        {
            bool isEmpty = i % 2 == 1;
            if (isEmpty) continue;
            // if not draw segment - should create a lot of line renderers??
            // https://www.reddit.com/r/Unity2D/comments/kt01nv/dotted_linerenderer_fixed/ 
        }
    }

    void ClearAllLines()
    {
        foreach (Transform child in linesHolder) Destroy(child);
    }
}
