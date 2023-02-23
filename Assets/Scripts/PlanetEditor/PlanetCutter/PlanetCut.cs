using System;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCut : MonoBehaviour
{
    public SpriteMask spriteMask;
    public Collider2D collider2D;
    public SpriteRenderer spriteRenderer;

    public static PlanetCut Instance;

    void Awake() => Instance = this;
    
    List<GameObject> PlanetsOnLine(Vector2 start, Vector2 end)
    {
        RaycastHit2D[] hits = Physics2D.LinecastAll(start, end);
        List <GameObject> planetsOnLine = new();
        foreach (var hit in hits)
        {
            if(hit.transform.gameObject.CompareTag("Planet")) planetsOnLine.Add(hit.transform.gameObject);
        }

        return planetsOnLine;
    }

    public void Slice(Vector2 pointA, Vector2 pointB)
    {
        List <GameObject> planetsToCut = PlanetsOnLine(pointA, pointB);
        foreach (var planet in planetsToCut)
        {
            SpriteMask mask = planet.GetComponent<SpriteMask>();
            SpriteRenderer renderer = planet.transform.GetChild(0).GetComponent<SpriteRenderer>();
            
            // Calculate the direction of the slice
            Vector2 direction = pointB - pointA;
            direction.Normalize();

            // Calculate the normal to the slice
            Vector2 normal = new Vector2(-direction.y, direction.x);

            // Create a plane that passes through point A and is perpendicular to the slice direction
            Plane slicePlane = new Plane(normal, pointA);

            // Get the bounds of the sprite
            Bounds bounds = renderer.bounds;

            // Calculate the distance from the slice plane to the center of the sprite
            float distance = slicePlane.GetDistanceToPoint(bounds.center);

            // Check if the sprite is on the side of the slice plane that we want to keep
            bool isInside = distance < 0;

            // Slice the sprite
            Texture2D texture = renderer.sprite.texture;
            Color[] pixels = texture.GetPixels();

            for (int y = 0; y < texture.height; y++)
            {
                for (int x = 0; x < texture.width; x++)
                {
                    Vector2 pixelPosition = new Vector2(x, y);
                    pixelPosition -= (Vector2) bounds.min;
                    pixelPosition /= bounds.size;

                    Vector2 pixelUV = new Vector2(pixelPosition.x * texture.width, pixelPosition.y * texture.height);

                    Vector2 pixelWorldPosition = renderer.transform.TransformPoint(pixelPosition);
                    float pixelDistance = slicePlane.GetDistanceToPoint(pixelWorldPosition);

                    if (pixelDistance < 0 && isInside || pixelDistance > 0 && !isInside)
                    {
                        pixels[y * texture.width + x] = Color.clear;
                    }
                }
            }

            // Update the texture with the sliced pixels
            texture.SetPixels(pixels);
            texture.Apply();

            // Update the sprite renderer
            renderer.sprite = Sprite.Create(texture, renderer.sprite.rect, new Vector2(0.5f, 0.5f));

            // Slice the sprite mask
            mask.sprite = renderer.sprite;
            mask.transform.position = renderer.transform.position;
            mask.transform.rotation = renderer.transform.rotation;
        }
    }
}
