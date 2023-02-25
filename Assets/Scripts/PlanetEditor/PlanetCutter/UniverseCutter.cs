using UnityEngine;

public static class UniverseCutter
{ 
    public static Sprite[] SlicedSprite(Sprite baseSprite,Vector2 worldPointA, Vector2 worldPointB, Vector2 planetPos, float radius)
    {
        // Get the texture of the sprite
        Texture2D texture = baseSprite.texture;
        
        // get points that line goes thru on texture
        Vector2[] points = CalculateIntersectionPoints(texture, worldPointA, worldPointB, planetPos, radius);
        if (points == null) return null;
        
        // Slice the textures along the line using the intersection points as the fixed points
        Texture2D[] slicedTextures = SliceTexture(texture, points[0], points[1]);
        
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
    static Texture2D[] SliceTexture(Texture2D texture, Vector2 startCoord, Vector2 endCoord)
    {
        int width = texture.width;
        int height = texture.height;

        // Create a new texture to hold the sliced result
        Texture2D[] slicedTexture = 
        {
            new (width, height, TextureFormat.RGBA32, false),
            new (width, height, TextureFormat.RGBA32, false),
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
    static Vector2[] CalculateIntersectionPoints(Texture2D texture, Vector2 worldPointA, Vector2 worldPointB,
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

        return null;
    }
}
