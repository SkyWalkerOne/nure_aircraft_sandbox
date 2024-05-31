using UnityEngine;

public class TimeReciever : MonoBehaviour
{
    [SerializeField] private bool simple;
    [SerializeField] private Light sun;
    [SerializeField] private TimeAndWeatherAsset[] timeAssets;

    void Start()
    {
        int index = PlayerPrefs.GetInt("time");
        RenderSettings.skybox = timeAssets[index].skybox;
        if (!simple) {
            sun.color = new Color32(timeAssets[index].r, timeAssets[index].g, timeAssets[index].b, 1);
            sun.intensity = timeAssets[index].sunIntensity;
            sun.transform.eulerAngles = new Vector3(30, timeAssets[index].sunVerticalRotation, 0);
            
            RenderSettings.ambientLight = new Color32(timeAssets[index].r, timeAssets[index].g, timeAssets[index].b, 1);

            RenderSettings.fog = PlayerPrefs.GetInt("fog") == 1;
        }
        
        sun.shadows = (PlayerPrefs.GetInt("shadows") == 1) ? LightShadows.Soft : LightShadows.None;
    }
}
