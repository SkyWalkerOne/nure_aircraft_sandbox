using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class chunksGenerator : MonoBehaviour
{
    public float xMaxBorder, xMinBorder, zMaxBorder, zMinBorder;
    public Transform plane;

    public GameObject[] chunks;
    public GameObject[] airports; //nullable
    private int chunkIndex = 0;

    [Space(10)]
    [Header("multiplayer ops")]
    public bool isMultiplayer;
    public PhotonView sync;
    public adminMonitor monitor;

    int time = 0;
    public int maxUpdateTime;

    void Update () {
        if (plane.position.x > xMaxBorder) {
            plane.position = new Vector3(xMinBorder, plane.position.y, plane.position.z);
            replaceChunk();
            Debug.Log("x max");
        }

        if (plane.position.x < xMinBorder) {
            plane.position = new Vector3(xMaxBorder, plane.position.y, plane.position.z);
            replaceChunk();
            Debug.Log("x min");
        }

        if (plane.position.z > zMaxBorder) {
            plane.position = new Vector3(plane.position.x, plane.position.y, zMinBorder);
            replaceChunk();
            Debug.Log("z max");
        }

        if (plane.position.z < zMinBorder) {
            plane.position = new Vector3(plane.position.x, plane.position.y, zMaxBorder);
            replaceChunk();
            Debug.Log("z min");
        }
    }

    void FixedUpdate() {
        if (isMultiplayer && monitor.isAdmin) {
            time += 1;
            if(time == maxUpdateTime) {
                sync.RPC("applyLocation", RpcTarget.All, chunkIndex);
                time = 0;
            }
        }
    }

    void replaceChunk () {
        if (chunkIndex < chunks.Length - 1) chunkIndex++;

        if (isMultiplayer && monitor.isAdmin) sync.RPC("applyLocation", RpcTarget.All, chunkIndex);
        else if (!isMultiplayer) setLocation(chunkIndex);
    }

    public void setLocation(int chunkIndex) {
        foreach (GameObject o in chunks)
            o.SetActive(false);
        foreach (GameObject o in airports)
            if (o != null) o.SetActive(false);

        chunks[chunkIndex].SetActive(true);
        if (airports[chunkIndex] != null) airports[chunkIndex].SetActive(true);
    }
}
