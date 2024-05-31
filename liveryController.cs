using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class liveryController : MonoBehaviour
{
    public bool isMultiplayer;
    public int mpUpdateTime;
    int index = 0;

    public int getIndex() {
        return (isMultiplayer) ? index : PlayerPrefs.GetInt("airline");
    }

    public void applyNewIndex(int newIndex) => index = newIndex;
}
