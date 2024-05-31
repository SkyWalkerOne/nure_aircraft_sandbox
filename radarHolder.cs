using UnityEngine;

public class radarHolder : MonoBehaviour
{
    public GameObject bigPlane, icon;
    public GameObject[] lands, iconLands;
    public GameObject[] aps, iconAps;
    public float rot;

    void Update () {
        rot = bigPlane.transform.eulerAngles.y;
        icon.transform.localRotation = Quaternion.Euler(0, 0, -rot);
        icon.transform.localPosition = new Vector3 (bigPlane.transform.position.z / 120, bigPlane.transform.position.x / -120, 0);

        for (int i = 0; i < lands.Length; i++) {
            iconLands[i].SetActive(lands[i].activeSelf);
        }

        for (int i = 0; i < aps.Length; i++) {
            iconAps[i].SetActive(aps[i].activeSelf);
        }
    }
}
