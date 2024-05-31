using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class liveryAdapter : MonoBehaviour
{
    public GameObject[] liveries;
    public bool isMultiplayer;

    private int time = 0;
    private liveryController controller;

    void Start () {
        controller = Object.FindObjectOfType<liveryController>();
        applyLivery(controller.getIndex());
    }

    void FixedUpdate() {
        if (isMultiplayer) {
            time++;
            if(time == controller.mpUpdateTime) {
                applyLivery(controller.getIndex());
                time = 0;
            }
        }
    }

    void applyLivery (int index) {
        foreach (GameObject obj in liveries)
            obj.SetActive(false);

        liveries[index].SetActive(true);
    }
}
