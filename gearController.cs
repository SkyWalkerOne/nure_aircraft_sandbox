using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class gearController : MonoBehaviour
{
    public GameObject frontGear, leftGear, rightGear;
    public GameObject frontFLdoor, frontFRdoor, frontBLdoor, frontBRdoor;
    public GameObject leftHelper, rightHelper, leftHelper2, rightHelper2;
    public GameObject leftBackDoor, rightBackDoor;

    public float speed;

    bool moving, closing, opening;
    bool gearDown = true;

    public bool isMultiPlayer;
    public PhotonView sync; //nullable
    public int mpUpdateTime;

    int time = 0;
    private adminMonitor monitor;

    void Start() {
        if (sync != null)
            monitor = sync.gameObject.GetComponent<adminMonitor>();
    }

    public void switchGear () {
        if (!closing && !moving && !isMultiPlayer) opening = true;
        else if (!closing && !moving) sendGearData(!gearDown);
    }

    public bool isDown() {
        return gearDown;
    }

    private void sendGearData(bool isGearDown) {
        if (sync != null)
            sync.RPC("applyGear", RpcTarget.All, isGearDown);
    }

    public void recieveGearData(bool isGearDown) {
        if (gearDown != isGearDown && !closing && !moving) opening = true;
    }

    void FixedUpdate () {
        if (moving) {

            if (gearDown) {
                frontGear.transform.Rotate(-speed, 0, 0);

                leftGear.transform.Rotate(0, 0, speed);
                rightGear.transform.Rotate(0, 0, speed);

                frontBLdoor.transform.Rotate(0, 0, speed);
                frontBRdoor.transform.Rotate(0, 0, -speed);

                leftHelper.transform.Rotate(0, 0, -speed * 1.5f);
                rightHelper.transform.Rotate(0, 0, -speed * 1.5f);

                leftHelper2.transform.Rotate(0, 0, -speed);
                rightHelper2.transform.Rotate(0, 0, -speed);
            } else {
                frontGear.transform.Rotate(speed, 0, 0);

                leftGear.transform.Rotate(0, 0, -speed);
                rightGear.transform.Rotate(0, 0, -speed);

                frontBLdoor.transform.Rotate(0, 0, -speed);
                frontBRdoor.transform.Rotate(0, 0, speed);

                leftHelper.transform.Rotate(0, 0, speed * 1.5f);
                rightHelper.transform.Rotate(0, 0, speed * 1.5f);

                leftHelper2.transform.Rotate(0, 0, speed);
                rightHelper2.transform.Rotate(0, 0, speed);
            }

            if (frontGear.transform.localRotation.x >= 0) {
                //return all objects to zero
                frontGear.transform.localRotation = Quaternion.Euler(0, 0, 0);
                
                leftGear.transform.localRotation = Quaternion.Euler(0, 0, -90);
                rightGear.transform.localRotation = Quaternion.Euler(0, 0, -90);

                frontBLdoor.transform.localRotation = Quaternion.Euler(0, 0, -90);
                frontBRdoor.transform.localRotation = Quaternion.Euler(0, 0, 90);

                leftHelper.transform.localRotation = Quaternion.Euler(0, 0, 90);
                rightHelper.transform.localRotation = Quaternion.Euler(0, 0, 90);

                moving = false;
                gearDown = !gearDown;
                closing = true;
            }

            if (frontGear.transform.localRotation == Quaternion.Euler(-90f, 0, 0)) {
                moving = false;
                gearDown = !gearDown;
                closing = true;
            }
        } else if (closing) {
            frontFRdoor.transform.Rotate(0, 0, -speed);
            frontFLdoor.transform.Rotate(0, 0, speed);
            rightBackDoor.transform.Rotate(0, 0, -speed);
            leftBackDoor.transform.Rotate(0, 0, -speed);

            if (frontFRdoor.transform.localRotation == Quaternion.Euler(0, 0, 0)) {
                frontFRdoor.transform.localRotation = Quaternion.Euler(0, 0, 0);
                frontFLdoor.transform.localRotation = Quaternion.Euler(0, 0, 0);
                rightBackDoor.transform.localRotation = Quaternion.Euler(0, 0, 0);
                leftBackDoor.transform.localRotation = Quaternion.Euler(0, 0, 0);

                closing = false;
            }
        } else if (opening) {
            frontFRdoor.transform.Rotate(0, 0, speed);
            frontFLdoor.transform.Rotate(0, 0, -speed);
            rightBackDoor.transform.Rotate(0, 0, speed);
            leftBackDoor.transform.Rotate(0, 0, speed);

            if (rightBackDoor.transform.localEulerAngles.z >= 90) {
                opening = false;
                moving = true;
            }
        }

        if (isMultiPlayer) {
            time++;
            if(time == mpUpdateTime) {
                if (monitor.isAdmin) sendGearData(gearDown);
                time = 0;
            }
        }
    }
}
