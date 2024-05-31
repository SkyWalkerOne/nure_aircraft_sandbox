using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class screenshotMaker : MonoBehaviour
{
    public string canvasName;

    public void OnClickScreenCaptureButton()
    {
        StartCoroutine(CaptureScreen());
    }

    public IEnumerator CaptureScreen()
    {
        // Wait till the last possible moment before screen rendering to hide the UI
        yield return null;
        GameObject.Find(canvasName).GetComponent<Canvas>().enabled = false;

        // Wait for screen rendering to complete
        yield return new WaitForEndOfFrame();

        // Take screenshot
        ScreenCapture.CaptureScreenshot("AircraftSandbox_screenshot.png");

        // Show UI after we're done
        GameObject.Find(canvasName).GetComponent<Canvas>().enabled = true;
    }
}
