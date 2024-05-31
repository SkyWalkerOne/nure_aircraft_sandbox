using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class surfaceTouchMonitor : MonoBehaviour
{
    [HideInInspector]
    public bool isGrounded;
    public bool debug;
    public float altitude = 0;

    void Update () {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -Vector3.up, out hit))
            altitude = hit.distance;

        isGrounded = altitude <= 12; 

        if (debug) isGrounded = false;
    }
}
