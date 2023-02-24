using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PlanetCut : MonoBehaviour
{
    private SpriteMask spriteMask;
    private Collider2D collider2D;

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
            // now we should create two new game objects -> calculate mass of them -> add a bit of force and offset to de-attach them
            var sprite = planet.GetComponent<SpriteMask>().sprite;
            // intersection points
            Vector2[] points = CalculateIntersectionPoints(sprite.texture, pointA, pointB, planet.transform.position,
                planet.transform.lossyScale.x/2);
            // slice sprite
            planet.GetComponent<SpriteMask>().sprite = SlicedSprite(sprite, points[0], points[1])[0];
            // slice collider
            SliceCollider(planet, planet.GetComponent<SpriteMask>().sprite);
        }
    }

    void SliceCollider(GameObject planet, Sprite targetShape)
    {
        // Get the sprite's vertices and sort them in clockwise order
        Vector2[] spriteVertices = targetShape.vertices;
        spriteVertices = SortVerticesClockwise(spriteVertices);

        Destroy(planet.GetComponent<Collider2D>());
        // Set up the collider using the sorted vertices
        PolygonCollider2D polygonCollider = planet.AddComponent<PolygonCollider2D>();
        polygonCollider.points = spriteVertices;
    }
    
    Sprite[] SlicedSprite(Sprite baseSprite, Vector2 a, Vector2 b)
    {
        // Get the texture of the sprite
        Texture2D texture = baseSprite.texture;

        // Slice the textures along the line using the intersection points as the fixed points
        Texture2D[] slicedTextures = SliceTexture(texture, a, b);
        
        // Create a new sprites using the sliced textures
        Sprite[] slicedSprites = new Sprite[slicedTextures.Length];
        for (int i = 0; i < slicedTextures.Length; i ++ )
        {
            Sprite slicedSprite = Sprite.Create(slicedTextures[i],
                new Rect(0, 0, slicedTextures[i].width, slicedTextures[i].height),
                new Vector2(0.5f, 0.5f), baseSprite.pixelsPerUnit);
            slicedSprites[i] = slicedSprite;
        }

        // Set the sliced sprite on the game object
        return slicedSprites;
    }

    // Helper method to slice a texture along a line
    Texture2D[] SliceTexture(Texture2D texture, Vector2 startCoord, Vector2 endCoord)
    {
        int width = texture.width;
        int height = texture.height;

        // Create a new texture to hold the sliced result
        Texture2D[] slicedTexture = new[]
        {
            new Texture2D(width, height, TextureFormat.RGBA32, false),
            new Texture2D(width, height, TextureFormat.RGBA32, false),
        };

        // Copy the pixels from the original texture to the sliced texture
        slicedTexture[0].SetPixels32(texture.GetPixels32());
        slicedTexture[1].SetPixels32(texture.GetPixels32());

        // Calculate the slope and y-intercept of the slice line
        float slope = (endCoord.y - startCoord.y) / (endCoord.x - startCoord.x);
        float yIntercept = startCoord.y - slope
            * startCoord.x;

        // Iterate over all pixels in the texture
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Calculate the y-coordinate of the slice line at this x-coordinate
                float sliceY = slope * x + yIntercept;

                // Set the pixel to transparent
                Color32 pixel = slicedTexture[0].GetPixel(x, y);
                pixel.a = 0;
                
                // Check if the current pixel is above or below the slice line
                if (y > sliceY) slicedTexture[1].SetPixel(x, y, pixel);
                
                else slicedTexture[0].SetPixel(x, y, pixel);
            }
        }

        // Apply the changes to the sliced texture
        slicedTexture[0].Apply();
        slicedTexture[1].Apply();

        return slicedTexture;
    }

    // Helper method to get intersection points
    Vector2[] CalculateIntersectionPoints(Texture2D texture, Vector2 worldPointA, Vector2 worldPointB,
        Vector2 planetPos, float radius)
    {
        // here we are converting world points to texture points
        float slope = (worldPointB.y - worldPointA.y) / (worldPointB.x - worldPointA.x);
        float bias = worldPointA.y - slope * worldPointA.x;
        // 0 = ax^2 + bx + c -> quadratic formula
        float a = 1 + Mathf.Pow(slope, 2);
        float b = -2 * planetPos.x + 2 * slope * (worldPointA.y - slope * worldPointA.x) - 2 * planetPos.y * slope;
        float c = Mathf.Pow(planetPos.x, 2) + Mathf.Pow((worldPointA.y - slope * worldPointA.x), 2) +
            Mathf.Pow(planetPos.y, 2) - 2 *
            planetPos.y * (worldPointA.y - slope * worldPointA.x) - Mathf.Pow(radius, 2);

        float delta = Mathf.Pow(b, 2) - 4 * a * c;
        // line goes thru the circle
        if (delta > 0)
        {
            // world coordinates
            float x1 = (-b - Mathf.Sqrt(delta)) / (2 * a);
            float y1 = slope * x1 + bias;

            float x2 = (-b + Mathf.Sqrt(delta)) / (2 * a);
            float y2 = slope * x2 + bias;

            // texture coordinates
            float diffx = planetPos.x - radius, diffy = planetPos.y - radius;
            float scaler = texture.width / (radius * 2);
            
            float tx1 = (x1 - diffx) * scaler;
            float ty1 = (y1 - diffy) * scaler;
            float tx2 = (x2 - diffx) * scaler;
            float ty2 = (y2 - diffy) * scaler;
            
            return new[]
            {
                new Vector2(tx1, ty1),
                new Vector2(tx2, ty2)
            };
        }
        
        return new []{Vector2.zero, Vector2.zero};
    }
    
    private Vector2[] SortVerticesClockwise(Vector2[] vertices)
    {
        // Calculate the centroid of the polygon
        Vector2 centroid = Vector2.zero;
        float signedArea = 0;
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector2 vertex1 = vertices[i];
            Vector2 vertex2 = vertices[(i + 1) % vertices.Length];
            float crossProduct = (vertex1.x * vertex2.y) - (vertex1.y * vertex2.x);
            signedArea += crossProduct;
            centroid += (vertex1 + vertex2) * crossProduct;
        }
        signedArea *= 0.5f;
        centroid /= (6 * signedArea);

        // Sort the vertices in clockwise order around the centroid
        Vector2[] sortedVertices = vertices.OrderBy(v => Mathf.Atan2(v.y - centroid.y, v.x - centroid.x)).ToArray();
        return sortedVertices;
    }
}
