using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class onGroundDetector : MonoBehaviour
{
    public bool fullyGrounded;
    public Button[] ButtonsInteractableOnGround;
    public Button[] ButtonsDisabledOnGround;

    public surfaceTouchMonitor monitor;

    void Update () {
        fullyGrounded = monitor.isGrounded;

        foreach (Button b in ButtonsInteractableOnGround) {
            b.interactable = fullyGrounded;
        }
        foreach (Button b in ButtonsDisabledOnGround) {
            b.interactable = !fullyGrounded;
        }
    }
}
