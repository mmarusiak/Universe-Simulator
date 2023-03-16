using System.Collections.Generic;
using UnityEngine;

public class SlicerRenderer : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform linesHolder;
    private List<Vector2[]> _points = new();
    private float _size = 10;

    public void DrawLine(Vector2 pointA, Vector2 pointB)
    {
        ClearAllLines();
        // functions params -> y = ax + b
        UniverseMath.GetLinearFunctionParams(pointA, pointB, out var a, out var b);
        float dist = Vector2.Distance(pointA, pointB);
        int segmentsCount = (int) Mathf.Floor(dist / _size);
        float lastX = pointA.x, lastY = pointA.y;
        
        for (int i = 0; i < segmentsCount; i++)
        {
            bool isEmpty = i % 2 == 1;
            if (isEmpty) continue;
            // if not draw segment - should create a lot of line renderers??
            // https://www.reddit.com/r/Unity2D/comments/kt01nv/dotted_linerenderer_fixed/ 
        }
    }

    void FindNextCoord(out float x, out float y, float slope, float bias, float lastX, float lastY, float size)
    {
        // y = ax + b
        // (x1 - x2)^2 + (y1 - ax2 + b)^2 = _size^2
        // x1x1 + x2x2 - 2x1x2 + y1y1 + (ax2 + b)^2 - 2y1 * (ax2 + b) = _size^2
        // x1x1 + x2x2 - 2x1x2 + y1y1 + ax2ax2 + bb + 2ax2b - 2y1ax1 - 2y1b = _size^2
        // x1x1 + y1y1 + bb - 2y1ax1 - 2y1b = _size^2
        // x2x2 + ax2ax2 - 2x1x2 + 2ax2b + x1x1 + y1y1 + bb - 2y1ax1 - 2y1b - size^2 = 0
        // x2x2(1 + aa) + x2(-2x1 + 2ab) + x1x1 + y1y1 + bb - 2y1ax1 - 2y1b - size^2 = 0
        float a = 1 + Mathf.Pow(slope, 2);
        float b = 2 * slope * bias - 2 * lastX;
        float c = Mathf.Pow(lastX, 2) + Mathf.Pow(bias, 2) - 2 * slope * lastY * lastX - 2 * lastY * bias -
                  Mathf.Pow(size, 2);
        float delta = Mathf.Pow(b, 2) - 4 * a * c;
        if (delta < 0)
        {
            Debug.Log(delta);
            x = 0;
            y = 0;
            return;
        }

        x = (-b + Mathf.Sqrt(delta)) / (2 * a);
        y = slope * x + bias;
    }

    void ClearAllLines()
    {
        _points = new List<Vector2[]>();
        foreach (Transform child in linesHolder) Destroy(child);
    }
}
