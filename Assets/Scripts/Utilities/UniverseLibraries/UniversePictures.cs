using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class UniversePictures
{ 
    // screenshots
    public static async Task TakeGameScreenshot(string path)
    {
        // hide all UI
        GameObject toHide = GameObject.FindWithTag("NoScreenshotable");
        toHide.SetActive(false);
        // wait for next frame, to capture hidden overlay
        await Task.Yield();
        // saving save picture
        ScreenCapture.CaptureScreenshot(path);
        await Task.Yield();
        // show back them
        toHide.SetActive(true);
    }

    public static void TakeViewScrenshot(string path)
    {
        ScreenCapture.CaptureScreenshot(path);
    }

    
    // sprite loaders
    public static async Task<Sprite> LoadSpriteFromPath(string path, int sizeX, int sizeY)
    {
        if (!UniverseTools.IsSafeImage(path)) return null;
        string correctPath = "file:///";
        if (path[0] != '/') correctPath += path;
        else correctPath += path.Remove(0, 1);
        
        Texture2D loadedTexture = await LoadTexture(correctPath);
        if (loadedTexture != null) return LoadSpriteFromTexture(loadedTexture, sizeX, sizeY);

        return null;
    }
    
    public static Sprite LoadSpriteFromTexture(Texture2D texture, int sizeX, int sizeY)
    {
        if (texture == null)
        {
            Debug.LogError("Image not available...");
            return null;
        }
            
        var loaded = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f));
      
        // resizing sprite
        ResizeTool.Resize(loaded.texture, sizeX, sizeY); 
        loaded = Sprite.Create(loaded.texture, new Rect(0, 0, sizeX, sizeY), new Vector2(0.5f, 0.5f));
        
        
        return loaded;
    }
    
    public static async Task<Texture2D> LoadTexture(string path)
    {
        using UnityWebRequest www = UnityWebRequestTexture.GetTexture(path);
        // Send the request and wait for a response
        var operation = www.SendWebRequest();
        while (!operation.isDone)
        {
            await Task.Delay(1);
        }

        // Check for errors
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            return null;
        }

        // Get the texture from the response
        Texture2D texture = DownloadHandlerTexture.GetContent(www);
        return texture;
    }
    
    
    // ---------------------------
    // SPRITE CUTTER
    public static Sprite[] SlicedSprite(Sprite baseSprite,Vector2 worldPointA, Vector2 worldPointB, Vector2 planetPos, float radius)
    {
        // Get the texture of the sprite
        Texture2D texture = baseSprite.texture;
        
        // get points that line goes thru on texture
        Vector2[] points = UniverseLine.CalculateIntersectionPointsForTexture(texture, worldPointA, worldPointB, planetPos, radius);
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

    public static List<Vector2> GetOutlineFromSprite(Sprite sprite, Transform holder, int resolution)
    {
        Texture2D spriteTexture = sprite.texture;
        int width = spriteTexture.width;
        int height = spriteTexture.height;

        // Convert sprite texture to binary image
        Color[] pixels = spriteTexture.GetPixels();
        bool[] binaryImage = new bool[pixels.Length];
        for (int i = 0; i < pixels.Length; i++)
        {
            binaryImage[i] = pixels[i].a > 0.5f;
        }

        // Apply dilation operation
        bool[] dilatedImage = new bool[pixels.Length];
        for (int x = 0; x < width; x += resolution)
        {
            for (int y = 0; y < height; y++)
            {
                int index = x + y * width;
                if (binaryImage[index])
                {
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        for (int dy = -1; dy <= 1; dy++)
                        {
                            int neighborX = x + dx;
                            int neighborY = y + dy;
                            if (neighborX >= 0 && neighborX < width &&
                                neighborY >= 0 && neighborY < height &&
                                !binaryImage[neighborX + neighborY * width])
                            {
                                dilatedImage[neighborX + neighborY * width] = true;
                            }
                        }
                    }
                }
            }
        }

        // Subtract binary image from dilated image to obtain outline
        bool[] outlineImage = new bool[pixels.Length];
        for (int i = 0; i < pixels.Length; i++)
        {
            outlineImage[i] = dilatedImage[i] && !binaryImage[i];
        }

        float yScale = holder.transform.localScale.y / spriteTexture.height; 
        float xScale = holder.transform.localScale.x / spriteTexture.width; 
        
        // Convert outline pixels to list of local space vectors
        List<Vector2> outlinePoints = new List<Vector2>();
        for (int x = 0; x < width; x += resolution)
        {
            for (int y = 0; y < height; y += resolution)
            {
                int index = x + y * width;
                if (outlineImage[index])
                {
                    Vector3 worldPos = new Vector3(x * xScale, y * yScale) - holder.localScale/2;
                    outlinePoints.Add(worldPos);
                }
            }
        }

        return outlinePoints;
    }
}
