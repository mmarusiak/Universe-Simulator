using System.Collections.Generic;
using UnityEngine;


public class PlanetCut : MonoBehaviour
{
    public static PlanetCut Instance;
    void Awake() => Instance = this;

    List<GameObject> PlanetsOnLine(Vector2 start, Vector2 end)
    {
        List<GameObject> result = new ();
        foreach (var component in PlanetComponentsController.Instance.AllGravityComponents)
        {
            var spriteBounds = component.Handler.GetComponent<SpriteMask>().bounds;
            Vector2 spriteMin = new Vector2(spriteBounds.min.x, spriteBounds.min.y);
            Vector2 spriteMax = new Vector2(spriteBounds.max.x, spriteBounds.max.y);
            
            if(UniverseLine.Intersect(start, end, spriteMin, spriteMax)) result.Add(component.Handler.gameObject);
        }

        return result;
    }

    public void Slice(Vector2 pointA, Vector2 pointB)
    {
        List<GameObject> planetsToCut = PlanetsOnLine(pointA, pointB);
        foreach (var planet in planetsToCut)
        {
            Debug.Log(planet.name);
            // now we should create two new game objects -> calculate mass of them -> add a bit of force and offset to de-attach them
            var sprite = planet.GetComponent<SpriteMask>().sprite;
            Vector2 planetPos = planet.transform.position;
            float radius = planet.transform.lossyScale.x / 2;
            
            // slice sprite
            planet.GetComponent<SpriteMask>().sprite = UniversePictures.SlicedSprite(sprite, pointA, pointB,planetPos, radius)[0];
            // slice collider
            Destroy(planet.GetComponent<Collider2D>());
            PolygonCollider2D polygonCollider = planet.AddComponent<PolygonCollider2D>();
            SliceCollider(planet.GetComponent<SpriteMask>(), polygonCollider);
            
            // center transform to center of new sprite
            var center = GetCenterFromCollider(polygonCollider);
            Debug.Log(center);
            MovePivot(center, planet.transform.parent);
        }
    }

    void SliceCollider(SpriteMask mask, PolygonCollider2D polygonCollider)
    {
        var sprite = mask.sprite;
        Vector2[] vertices = sprite.vertices;
        vertices = SortVerticesClockwise(vertices);
        
        polygonCollider.SetPath(0, vertices);
        polygonCollider.pathCount = 1;
    }
    
    
    // Helper method to sort the vertices in clockwise order
    private Vector2[] SortVerticesClockwise(Vector2[] vertices)
    {
        // Find the center point of the vertices
        Vector2 center = Vector2.zero;
        foreach (var vert in vertices)
        {
            center += vert;
        }
        center /= vertices.Length;

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
    
    void MovePivot(Vector3 position, Transform target)
    {
        Vector3 offset = target.position - position;
        Debug.Log(offset);
        foreach (Transform child in target)
            child.transform.position += offset;
        target.position = position;
    }

    Vector2 GetCenterFromCollider(PolygonCollider2D collider)
    {
        Vector2[] vertices = collider.GetPath(0);
        float area = 0f;
        Vector2 centroid = Vector2.zero;
        for (int i = 0; i < vertices.Length; i++) {
            Vector2 vertex1 = vertices[i];
            Vector2 vertex2 = vertices[(i + 1) % vertices.Length];
            float crossProduct = vertex1.x * vertex2.y - vertex2.x * vertex1.y;
            area += crossProduct;
            centroid += (vertex1 + vertex2) * crossProduct;
        }
        centroid /= 3f * area;
        
        return collider.transform.TransformPoint(centroid);
    }
}
