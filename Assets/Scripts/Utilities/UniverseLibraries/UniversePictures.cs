using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public static class UniversePictures
{ 
    public static IEnumerator TakeGameScreenshot(string path)
    {
        // hide all UI
        GameObject toHide = GameObject.FindWithTag("NoScreenshotable");
        toHide.SetActive(false);
        // wait for next frame, to capture hidden overlay
        yield return new WaitForNextFrameUnit();
        // saving save picture
        ScreenCapture.CaptureScreenshot(path);
        yield return new WaitForNextFrameUnit();
        // show back them
        toHide.SetActive(true);
    }

    public static void TakeViewScrenshot(string path)
    {
        ScreenCapture.CaptureScreenshot(path);
    }

    public static Sprite LoadSpriteFromRequest(UnityWebRequest request, int sizeX, int sizeY)
    {
        DownloadHandlerTexture textureDownloadHandler = (DownloadHandlerTexture) request.downloadHandler;
        Texture2D texture = textureDownloadHandler.texture;

        if (texture == null)
        {
            Debug.LogError("Image not available...");
            return null;
        }
            
        var loaded = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f));
        // resizing sprite
        if (loaded != BasicPlanetEditor.Instance.DefaultPlanetSprite)
        {
            ResizeTool.Resize(loaded.texture, sizeX, sizeY);
            loaded =
                Sprite.Create(loaded.texture, new Rect(0, 0, sizeY, sizeX), new Vector2(0.5f, 0.5f));
        }
        
        return loaded;
    }
}
