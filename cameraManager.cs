using UnityEngine;

public class cameraManager : MonoBehaviour
{
    public GameObject firstPersonal, sideCam, cockpit, leftWing, rightWing, cockpitZoomed, terminal, bottom;
    public GameObject[] personalUI, pilotUI;
    public bool scrollBarDrag;

    private GameObject[] allCameras;

    void Start() {
        allCameras = new GameObject[]{firstPersonal, sideCam, cockpit, leftWing, rightWing, cockpitZoomed, terminal, bottom};
    }

    public void SetFPC () {
        disableAll();
        firstPersonal.SetActive(true);
        enableFPC_UI(true);
        enablePilot_UI(false);
    }

    public void SetSideCam () {
        disableAll();
        sideCam.SetActive(true);
        enableFPC_UI(false);
        enablePilot_UI(true);
    }

    public void SetCockpitCam () {
        disableAll();
        cockpit.SetActive(true);
        enableFPC_UI(false);
        enablePilot_UI(true);
    }

    public void SetCockpitZoomedCam () {
        disableAll();
        cockpitZoomed.SetActive(true);
        enableFPC_UI(false);
        enablePilot_UI(true);
    }

    public void SetLeftWCam () {
        disableAll();
        leftWing.SetActive(true);
        enableFPC_UI(false);
        enablePilot_UI(true);
    }

    public void SetRightWCam () {
        disableAll();
        rightWing.SetActive(true);
        enableFPC_UI(false);
        enablePilot_UI(true);
    }

    public void SetTerminalCam () {
        disableAll();
        terminal.SetActive(true);
        enableFPC_UI(false);
        enablePilot_UI(true);
    }

    public void SetBottomCam () {
        disableAll();
        bottom.SetActive(true);
        enableFPC_UI(false);
        enablePilot_UI(true);
    }

    public void startDragScrollBar () {
        scrollBarDrag = true;
    }

    public void stopDragScrollBar () {
        scrollBarDrag = false;
    }

    private void disableAll () {
        foreach(GameObject cam in allCameras) {
            cam.SetActive(false);
        }
    }

    private void enableFPC_UI (bool isEnabled) {
        foreach (GameObject element in personalUI)
            element.SetActive(isEnabled);
    }

    private void enablePilot_UI (bool isEnabled) {
        foreach (GameObject element in pilotUI)
            element.SetActive(isEnabled);
    }
}
