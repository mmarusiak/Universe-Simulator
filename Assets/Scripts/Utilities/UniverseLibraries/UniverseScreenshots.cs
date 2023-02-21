using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public static class UniverseScreenshots
{ 
    public static IEnumerator ScreenshotGame(string path)
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

    public static void ScreenshotView(string path)
    {
        ScreenCapture.CaptureScreenshot(path);
    }
}
