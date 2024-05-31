using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class lightController : MonoBehaviour
{
    [SerializeField] private Text[] landingIndicators, navIndicators, strobeIndicators;
    [SerializeField] private Text cabinLightsIndicator;
    [SerializeField] private GameObject[] landingLights, navLights, strobeLights;
    [SerializeField] private GameObject cabinLights;

    private bool _landOn, _navOn, _strobeOn, _cabinOn = true;
    [SerializeField] private float onTime, offTime;

    [SerializeField] private bool isMultiPlayer;
    [SerializeField] private PhotonView sync; //nullable
    
    private adminMonitor _monitor;

    void Start() {
        if (sync != null)
            _monitor = sync.gameObject.GetComponent<adminMonitor>();

        StartCoroutine(SwitchLights());
    }

    [ContextMenu("Switch landing lights")]
    public void switchLanding() {
        if (!isMultiPlayer) {
            _landOn = !_landOn;
            foreach (GameObject l in landingLights) {
                l.SetActive(_landOn);
            }
            foreach (Text t in landingIndicators) {
                t.text = (_landOn) ? "on" : "off";
            }
        } else {
            sendLightData(new bool[]{!_landOn, _navOn, _strobeOn, _cabinOn});
        }
    }

    [ContextMenu("Switch NAV")]
    public void switchNAV() {
        if (!isMultiPlayer) {
            _navOn = !_navOn;
            foreach (GameObject l in navLights) {
                l.SetActive(_navOn);
            }
            foreach (Text t in navIndicators) {
                t.text = (_navOn) ? "on" : "off";
            }
        } else {
            sendLightData(new bool[]{_landOn, !_navOn, _strobeOn, _cabinOn});
        }
    }

    [ContextMenu("Switch cabin lights")]
    public void switchCabin() {
        if (!isMultiPlayer) {
            _cabinOn = !_cabinOn;
            cabinLights.SetActive(_cabinOn);
            if (cabinLightsIndicator != null) cabinLightsIndicator.text = (_cabinOn) ? "Cabin: on" : "Cabin: off";
        } else {
            sendLightData(new bool[]{_landOn, _navOn, _strobeOn, !_cabinOn});
        }
    }

    [ContextMenu("Switch strobe/beacon")]
    public void switchStrobe () {
        if (!isMultiPlayer) {
            _strobeOn = !_strobeOn;
            foreach (Text t in strobeIndicators) {
                t.text = (_strobeOn) ? "on" : "off";
            }
        } else {
            sendLightData(new bool[]{_landOn, _navOn, !_strobeOn, _cabinOn});
        }
    }

    private IEnumerator SwitchLights()
    {
        for (;;)
        {
            if (isMultiPlayer && _monitor.isAdmin) sendLightData(new bool[]{_landOn, _navOn, _strobeOn, _cabinOn});
            
            yield return new WaitForSeconds(onTime);
            foreach (GameObject l in strobeLights) l.SetActive(_strobeOn);
            yield return new WaitForSeconds(offTime);
            foreach (GameObject l in strobeLights) l.SetActive(false);
        }
    }

    private void sendLightData(bool[] newOptions) {
        if (sync != null)
            sync.RPC("applyLights", RpcTarget.All, newOptions);
    }

    public void recieveLightData(bool[] lightOptions) {
        isMultiPlayer = false;
        for (int i = 0; i < lightOptions.Length; i++) {
            switch (i) {
                case 0:
                    if (lightOptions[i] != _landOn) switchLanding();
                    break;
                case 1:
                    if (lightOptions[i] != _navOn) switchNAV();
                    break;
                case 2:
                    if (lightOptions[i] != _strobeOn) switchStrobe();
                    break;
                case 3:
                    if (lightOptions[i] != _cabinOn) switchCabin();
                    break;
            }
        }
        isMultiPlayer = true;
    }
}
