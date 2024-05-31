using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class doorsController : MonoBehaviour
{
    public GameObject frontLeft, frontRight, backLeft, backRight;
    public float xSpeed, zSpeed, duration, timeSpeed;

    door[] allDoors = new door[4];

    public bool isMultiPlayer;
    public PhotonView sync; //nullable
    public int mpUpdateTime;

    int time = 0;
    private adminMonitor monitor;

    void Start () {
        allDoors[0] = new door (frontLeft, xSpeed, zSpeed, false);
        allDoors[1] = new door (frontRight, -xSpeed, zSpeed, false);
        allDoors[2] = new door (backLeft, xSpeed, zSpeed, false);
        allDoors[3] = new door (backRight, -xSpeed, zSpeed, false);

        if (sync != null)
            monitor = sync.gameObject.GetComponent<adminMonitor>();
    }

    public void moveFrontLeft () {
        if (!isMultiPlayer) allDoors[0].setMoving(true);
        else sendDoorsData(new bool[]{!allDoors[0].checkIsOpen(), allDoors[1].checkIsOpen(), allDoors[2].checkIsOpen(), allDoors[3].checkIsOpen()});
    }

    public void moveFrontRight () {
        if (!isMultiPlayer) allDoors[1].setMoving(true);
        else sendDoorsData(new bool[]{allDoors[0].checkIsOpen(), !allDoors[1].checkIsOpen(), allDoors[2].checkIsOpen(), allDoors[3].checkIsOpen()});
    }

    public void moveBackLeft () {
        if (!isMultiPlayer) allDoors[2].setMoving(true);
        else sendDoorsData(new bool[]{allDoors[0].checkIsOpen(), allDoors[1].checkIsOpen(), !allDoors[2].checkIsOpen(), allDoors[3].checkIsOpen()});
    }

    public void moveBackRight () {
        if (!isMultiPlayer) allDoors[3].setMoving(true);
        else sendDoorsData(new bool[]{allDoors[0].checkIsOpen(), allDoors[1].checkIsOpen(), allDoors[2].checkIsOpen(), !allDoors[3].checkIsOpen()});
    }

    void FixedUpdate() {
        foreach (door d in allDoors) {
            if (d.isMoving()) {

                d.appendTime(timeSpeed);
                if (d.getTime() >= duration) {
                    d.setMoving(false);
                    d.clearTime();
                    d.changeOpenStatus();
                    return;
                }
            
                if (d.checkIsOpen())
                    d.getObject().transform.Translate(d.getXSpeed() * -1, 0 , d.getZSpeed() * -1);
                else 
                    d.getObject().transform.Translate(d.getXSpeed(), 0 , d.getZSpeed());
            }
        }

        if (isMultiPlayer) {
            time++;
            if(time == mpUpdateTime) {
                if (monitor.isAdmin) sendDoorsData(new bool[]{allDoors[0].checkIsOpen(), allDoors[1].checkIsOpen(), allDoors[2].checkIsOpen(), allDoors[3].checkIsOpen()});
                time = 0;
            }
        }
    }

    private void sendDoorsData(bool[] newOptions) {
        if (sync != null)
            sync.RPC("applyDoors", RpcTarget.All, newOptions);
    }

    public void recieveDoorData(bool[] doorOptions) {
        for (int i = 0; i < doorOptions.Length; i++)
            if (allDoors[i].checkIsOpen() != doorOptions[i]) allDoors[i].setMoving(true);
    }
}

public class door {
    private GameObject obj;
    private bool isOpen;
    private bool moving;
    private float time;
    private float xSpeed, zSpeed;

    public door (GameObject obj, float xSpeed, float zSpeed, bool isOpen) {
        this.obj = obj;
        this.isOpen = isOpen;
        this.moving = false;
        this.time = 0;
        this.xSpeed = xSpeed;
        this.zSpeed = zSpeed;
    }

    //getters

    public GameObject getObject () {
        return this.obj;
    }

    public bool checkIsOpen () {
        return this.isOpen;
    }

    public bool isMoving () {
        return this.moving;
    }

    public float getTime () {
        return this.time;
    }

    public float getXSpeed () {
        return this.xSpeed;
    }

    public float getZSpeed () {
        return this.zSpeed;
    }

    //functions

    public void appendTime (float time) {
        this.time += time;
    }

    public void changeOpenStatus () {
        this.isOpen = !this.isOpen;
    }

    public void setMoving (bool moving) {
        this.moving = moving;
    }

    public void clearTime () {
        this.time = 0;
    }
}
