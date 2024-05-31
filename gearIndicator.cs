using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gearIndicator : MonoBehaviour
{
    public gearController gears;
    public Text[] indicators;

    void Update()
    {
        foreach (Text t in indicators)
            t.text = (gears.isDown()) ? "down" : "up";
    }
}
