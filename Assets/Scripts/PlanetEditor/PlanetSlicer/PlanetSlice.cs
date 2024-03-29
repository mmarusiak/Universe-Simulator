using System.Collections.Generic;
using System.Threading.Tasks;
using GameCore.SimulationCore.Handlers;
using UnityEngine;
using Utilities.SaveSystem.Data;
using Utilities.UniverseLibraries;


/// <summary>
/// Base class for slicing planets.
/// </summary>

public class PlanetSlice : MonoBehaviour
{
    public static PlanetSlice Instance;
    private void Awake() => Instance = this;

    /// <summary>
    /// Gets all planets that are on line between start and end vector.
    /// </summary>
    /// <param name="start">Start point for line.</param>
    /// <param name="end">End point for line.</param>
    /// <returns></returns>
    private static List<GameObject> PlanetsOnLine(Vector2 start, Vector2 end)
    {
        List<GameObject> result = new ();
       
        var hit = Physics2D.LinecastAll(start, end);
        foreach (var coll in hit)
            if (coll.collider != null)
                result.Add(coll.collider.gameObject);
        
        return result;
    }

    /// <summary>
    /// Slices all planets between point a and b.
    /// </summary>
    /// <param name="start">Start point for line.</param>
    /// <param name="end">End point for line.</param>
    public void Slice(Vector2 start, Vector2 end)
    {
        List<GameObject> planetsToCut = PlanetsOnLine(start, end);
        foreach (var planet in planetsToCut)
        {
            // NEED TO REFACTOR THIS PIECE 
            if (planet is null) continue;

            var originalT = planet.transform.parent;
            var originalHandler = planet.GetComponent<PlanetComponentHandler>();
            var originalSprite = planet.GetComponent<SpriteMask>().sprite;
            // slice sprites
            var slicedSprites = UniversePictures.SlicedSprite(originalSprite, start, end,planet.transform.position, planet.transform.lossyScale.x/2);

            if (slicedSprites is null) continue;
            
            // original area is used to calculate masses
            float originalArea = CalculatePolygonArea(planet.GetComponent<PolygonCollider2D>().points); // action
            
            var clonedHandler = CreateSlice(originalHandler, originalT);
            var cloneT = clonedHandler.transform.parent;

            var originalPos = (Vector2) originalT.GetChild(0).position;
            var clonedPos = (Vector2) cloneT.GetChild(0).position;
            
            // apply sliced sprites to planets and slice collider
            ApplySlice(originalHandler, slicedSprites[0], originalArea, true);
            originalHandler.MyComponent.Slices.Add(new SliceData(start - originalPos, end - originalPos, 0, PlaybackController.Instance.Playback.IsReset));
            
            ApplySlice(clonedHandler, slicedSprites[1], originalArea, true);
            clonedHandler.MyComponent.Slices.Add(new SliceData(start - clonedPos, end - clonedPos, 1, PlaybackController.Instance.Playback.IsReset));
            
            // apply the same sprite to the slice
            clonedHandler.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = originalHandler.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;

            // now we need to move them out a bit
            int xFlag = originalT.position.x > cloneT.position.x ? 1 : -1, yFlag = originalT.position.y > cloneT.position.y ? 1 : -1;
            Vector2 posToMove = new(xFlag, yFlag);
            originalHandler.MyComponent.CurrentPosition += posToMove/10;
            clonedHandler.MyComponent.CurrentPosition -= posToMove/10;
        }
    }
    
    /// <summary>
    /// Loads slices from handler (loaded from json file) where they are stored just as 2 points - start and end of line.
    /// </summary>
    /// <param name="handler">Planet component handler for which slices are loaded.</param>
    public async Task LoadSlices(PlanetComponentHandler handler)
    {
        // keep values that will change on slice separately
        var velocity = handler.MyComponent.CurrentVelocity;
        var initialPos = handler.MyComponent.CurrentPosition;

        if (handler.MyComponent.Slices.Count == 0) return;
        
        GameObject planet = handler.gameObject;
        Transform planetT = handler.transform;
        
        foreach (var slice in handler.MyComponent.Slices)
        {
            var originalSprite = planet.GetComponent<SpriteMask>().sprite;
            var slicedSprites = UniversePictures.SlicedSprite(originalSprite, slice.StartPoint, slice.EndPoint, planetT.localPosition,
                planet.transform.lossyScale.x / 2);
            
            float originalArea = CalculatePolygonArea(planet.GetComponent<PolygonCollider2D>().points);
            ApplySlice(handler, slicedSprites[slice.SliceIndex()], originalArea, false);
        }

        await Task.Yield();
        handler.MyComponent.CurrentPosition = initialPos;
        handler.MyComponent.CurrentVelocity = velocity;
    }
    
    /// <summary>
    /// Creates slice - creates new Game Object.
    /// </summary>
    /// <param name="originalHandler">Planet Component Handler that is being sliced.</param>
    /// <param name="originalT">Transform that is being sliced.</param>
    /// <returns>Planet Component Handler that is on new Game Object.</returns>
    private static PlanetComponentHandler CreateSlice(PlanetComponentHandler originalHandler, Transform originalT)
    {
        // making new slice "clone"
        bool isOriginalClone = originalHandler.IsCloned;
        originalHandler.IsCloned = true;
        // cloning planet - clone t contains real position of planet - position on which forces are calculated, also contains rigidbody
        var cloneT = Instantiate(originalT.gameObject, originalT.parent);
        // clone base contains mask and collider
        var cloneBase = cloneT.transform.GetChild(0).gameObject;
            
        // load cloned component for handler
        var clonedHandler = cloneBase.GetComponent<PlanetComponentHandler>();
        clonedHandler.NullTexts();
        clonedHandler.LoadAsSlice(originalHandler.MyComponent);
        originalHandler.IsCloned = isOriginalClone;
        clonedHandler.MyComponent.PlanetSprite = originalHandler.MyComponent.PlanetSprite;
        clonedHandler.MyComponent.Slices = new List<SliceData>(originalHandler.MyComponent.Slices);
        return clonedHandler;
    }

    /// <summary>
    /// Applies slice - slices collider, sets new sprite, centers all children, sets mass.
    /// </summary>
    /// <param name="handler">Planet Component Handler for which we apply slice.</param>
    /// <param name="sprite">New sliced sprite.</param>
    /// <param name="originalArea">Original planet's area (used to calculate new mass).</param>
    /// <param name="massApplier">Just for debug, indicates if we'll change the mass or not.</param>
    private void ApplySlice(PlanetComponentHandler handler, Sprite sprite, float originalArea, bool massApplier)
    {
        var target = handler.gameObject;
        // slice sprite
        target.GetComponent<SpriteMask>().sprite = sprite;
        // slice collider
        PolygonCollider2D polygonCollider = target.GetComponent<PolygonCollider2D>();
        SliceCollider(target.GetComponent<SpriteMask>(), polygonCollider);
            
        // center transform to center of new sprite
        var center = GetCenterFromCollider(polygonCollider);
        MovePivot(center, target.transform.parent);

        if(!massApplier) return;
        // change mass of the slices
        float divider = CalculatePolygonArea(polygonCollider.points)/originalArea;
        handler.MyComponent.Mass *= divider;
        handler.MyComponent.GetPosFromTransform();
    }

    /// <summary>
    /// Slices collider - makes collider identical to sprite of sprite mask.
    /// </summary>
    /// <param name="mask">Sprite mask that is base for our slice.</param>
    /// <param name="polygonCollider">Collider to be sliced.</param>
    public void SliceCollider(SpriteMask mask, PolygonCollider2D polygonCollider)
    {
        var sprite = mask.sprite;
        Vector2[] vertices = UniversePictures.GetOutlineFromSprite(sprite, mask.transform, 2).ToArray();
        vertices = SortVerticesClockwise(vertices);
        
        polygonCollider.SetPath(0, vertices);
        polygonCollider.pathCount = 1;
    }

    /// Helper method to sort the vertices in clockwise order.
    private Vector2[] SortVerticesClockwise(Vector2[] vertices)
    {
        // Find the center point of the vertices
        Vector2 center = GetCenterFromVertices(vertices);

        // Sort the vertices by angle relative to the center point
        List<Vector2> sortedVertices = new List<Vector2>(vertices);
        sortedVertices.Sort((a, b) =>
        {
            float angleA = Mathf.Atan2(a.y - center.y, a.x - center.x);
            float angleB = Mathf.Atan2(b.y - center.y, b.x - center.x);
            return angleA.CompareTo(angleB);
        });

        return sortedVertices.ToArray();
    }
    
    /// <summary>
    /// Moves pivot to some position and centers all children.
    /// </summary>
    /// <param name="position">Target position.</param>
    /// <param name="target">Transform on which we want to apply our action.</param>
    private static void MovePivot(Vector3 position, Transform target)
    {
        Vector3 offset = target.position - position;
        foreach (Transform child in target)
            child.transform.position += offset;
        target.position = position;
    }

    private Vector2 GetCenterFromCollider(PolygonCollider2D collider)
    {
        Vector2[] vertices = collider.GetPath(0);
        return collider.transform.TransformPoint(GetCenterFromVertices(vertices));
    }

    private static Vector2 GetCenterFromVertices(Vector2[] vertices)
    {
        Vector2 center = Vector2.zero;
        foreach (var vert in vertices)
        {
            center += vert;
        }
        center /= vertices.Length;
        return center;
    }

    private static float CalculatePolygonArea(Vector2[] points)
    {
        float area = 0;

        for (int i = 0; i < points.Length; i++)
        {
            Vector2 p1 = points[i];
            Vector2 p2 = i == points.Length - 1 ? points[0] : points[i + 1];

            area += (p1.x * p2.y - p2.x * p1.y);
        }

        area /= 2;
        area = Mathf.Abs(area);

        return area;
    }
}
