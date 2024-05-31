using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSetter : MonoBehaviour
{
    public Material[] skyboxes;
    public string[] timeNames;
    public Text timeText, fogText, shadowsText;

    int index;
    bool isFoggy, shadows;

    void Start()
    {
        index = PlayerPrefs.GetInt("time");
        setTime();

        isFoggy = PlayerPrefs.GetInt("fog") == 1;
        setFoggy();

        shadows = PlayerPrefs.GetInt("shadows") == 1;
        setShadows();
    }

    void setFoggy () {
        fogText.text = (isFoggy) ? "fog: on" : "fog: off";
        RenderSettings.fog = isFoggy;
    }

    void setShadows () {
        shadowsText.text = (shadows) ? "shadows: on" : "shadows: off";
    }

    void setTime () {
        RenderSettings.skybox = skyboxes[index];
        timeText.text = timeNames[index];
    }

    public void moveNext () {
        index++;
        if (index == skyboxes.Length) index = 0;
        setTime();
        PlayerPrefs.SetInt("time", index);
    }

    public void movePrevious () {
        index--;
        if (index == -1) index = skyboxes.Length - 1;
        setTime();
        PlayerPrefs.SetInt("time", index);
    }

    public void switchFog () {
        isFoggy = !isFoggy;
        PlayerPrefs.SetInt("fog", (isFoggy) ? 1 : 0);
        setFoggy();
    }

    public void switchShadows () {
        shadows = !shadows;
        PlayerPrefs.SetInt("shadows", (shadows) ? 1 : 0);
        setShadows();
    }
}
