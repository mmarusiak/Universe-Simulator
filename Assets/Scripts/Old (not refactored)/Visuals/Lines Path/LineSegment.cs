using UnityEngine;

[System.Serializable]
public class LineSegment
{
    public GameObject SegmentHolder;
    public LineRenderer Renderer;
    public bool Finished;

    public LineSegment(GameObject holder, LineRenderer renderer, bool finished)
    {
        SegmentHolder = holder;
        Renderer = renderer;
        Finished = finished;
    }
}
