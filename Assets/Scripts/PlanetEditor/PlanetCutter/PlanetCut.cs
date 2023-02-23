using System;
using System.Collections.Generic;
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
            Debug.Log(planet.transform.lossyScale);
            var sprite = planet.GetComponent<SpriteMask>().sprite;
            planet.GetComponent<SpriteMask>().sprite = SlicedSprite(sprite, pointA, pointB, planet.transform.position,
                planet.transform.lossyScale.x / 2);
        }
    }

    Sprite SlicedSprite(Sprite baseSprite, Vector2 worldPointA, Vector2 worldPointB, Vector2 planetPos, float radius)
    {
        // Get the texture of the sprite
        Texture2D texture = baseSprite.texture;

        // Calculate the position of the slice line in texture coordinates
        Vector2 startTextureCoord = baseSprite.textureRect.position;
        Vector2 endTextureCoord = startTextureCoord + baseSprite.textureRect.size;

        // Get the start and end points of the slice line in world space
        Vector3 startWorldPoint = transform.position + transform.up * baseSprite.textureRect.yMin;
        Vector3 endWorldPoint = transform.position + transform.up * baseSprite.textureRect.yMax;

        // Convert the world space start and end points to texture coordinates
        startTextureCoord.x += startWorldPoint.x / texture.width;
        startTextureCoord.y += startWorldPoint.y / texture.height;
        endTextureCoord.x += endWorldPoint.x / texture.width;
        endTextureCoord.y += endWorldPoint.y / texture.height;

        // Calculate the intersection points of the slice line with the edges of the texture
        Vector2[] intersectionPoints =
            CalculateIntersectionPoints(texture, worldPointA, worldPointB, planetPos, radius);

        // Slice the texture along the line using the intersection points as the fixed points
        Texture2D slicedTexture = SliceTexture(texture, intersectionPoints[0], intersectionPoints[1]);

        // Create a new sprite using the sliced texture
        Sprite slicedSprite = Sprite.Create(slicedTexture, new Rect(0, 0, slicedTexture.width, slicedTexture.height),
            new Vector2(0.5f, 0.5f), baseSprite.pixelsPerUnit);

        // Set the sliced sprite on the game object
        return slicedSprite;
    }

    // Helper method to slice a texture along a line
    Texture2D SliceTexture(Texture2D texture, Vector2 startCoord, Vector2 endCoord)
    {
        int width = texture.width;
        int height = texture.height;

        // Create a new texture to hold the sliced result
        Texture2D slicedTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);

        // Copy the pixels from the original texture to the sliced texture
        slicedTexture.SetPixels32(texture.GetPixels32());

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

                // Check if the current pixel is above or below the slice line
                if (y > sliceY)
                {
                    // Set the pixel to transparent
                    Color32 pixel = slicedTexture.GetPixel(x, y);
                    pixel.a = 0;
                    slicedTexture.SetPixel(x, y, pixel);
                }
            }
        }

// Apply the changes to the sliced texture
        slicedTexture.Apply();

        return slicedTexture;
    }

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
            
            Debug.Log($"{x1}, {y1} and {x2}, {y2}");

            // texture coordinates
            float diffx = planetPos.x - radius, diffy = planetPos.y - radius;
            float scaler = texture.width / (radius * 2);
            
            float tx1 = (x1 - diffx) * scaler;
            float ty1 = (y1 - diffy) * scaler;
            float tx2 = (x2 - diffx) * scaler;
            float ty2 = (y2 - diffy) * scaler;
            Debug.Log($"{tx1}, {ty1} and {tx2}, {ty2}");
            
            return new[]
            {
                new Vector2(tx1, ty1),
                new Vector2(tx2, ty2)
            };
        }
        
        return new []{Vector2.zero, Vector2.zero};
    }

}
