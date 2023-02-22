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
}
