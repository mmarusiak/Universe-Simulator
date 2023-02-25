using System.Collections.Generic;
using UnityEngine;


public class PlanetCut : MonoBehaviour
{
    public static PlanetCut Instance;
    void Awake() => Instance = this;

    List<GameObject> PlanetsOnLine(Vector2 start, Vector2 end)
    {
        RaycastHit2D[] hits = Physics2D.LinecastAll(start, end);
        List<GameObject> planetsOnLine = new();
        foreach (var hit in hits)
        {
            if (hit.transform.gameObject.CompareTag("Planet")) planetsOnLine.Add(hit.transform.gameObject);
        }

        return planetsOnLine;
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
            planet.GetComponent<SpriteMask>().sprite = UniverseCutter.SlicedSprite(sprite, pointA, pointB,planetPos, radius)[0];
            // slice collider
            Destroy(planet.GetComponent<Collider2D>());
            PolygonCollider2D polygonCollider = planet.AddComponent<PolygonCollider2D>();
            SliceCollider(planet.GetComponent<SpriteMask>(), polygonCollider);
            
            // center transform to center of new sprite
            MovePivot(planet.GetComponent<SpriteMask>().sprite.bounds.center, planet.transform.parent);
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
    
    public void MovePivot(Vector3 position, Transform target)
    {
        Vector3 offset = target.position - position;
        foreach (Transform child in target)
            child.transform.position += offset;
        target.position = position;
    }
}
