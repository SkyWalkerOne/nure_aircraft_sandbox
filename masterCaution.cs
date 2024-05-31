using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class masterCaution : MonoBehaviour
{
    public AudioSource pullUp, stall, overspeed;
    public autoPilotManager apm;
    public gearController gears;

    void Update () {
        pullUp.enabled = apm.getAltitude() <= 100 && !gears.isDown();
        stall.enabled = apm.getSpeed() < 80 && !apm.onGround();
        overspeed.enabled = apm.getSpeed() > 320;
    }
}
