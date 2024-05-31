using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundPlayerChecker : MonoBehaviour
{
    public GameObject insideSound, outsideSound;
    public bool isMultiplayer;

    [HideInInspector]
    public bool inTrigger;

    void OnTriggerEnter(Collider other)
    {
        if (s_player(other) || mp_player(other)) {
            outsideSound.SetActive(false);
            insideSound.SetActive(true);
        }
            
        if (s_player(other) || mp_any_player(other))
            other.gameObject.transform.SetParent(this.gameObject.transform);

        if (anyTrigger(other))
            inTrigger = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (s_player(other) || mp_player(other)) {
            outsideSound.SetActive(true);
            insideSound.SetActive(false);
        }

        if (s_player(other) || mp_any_player(other))
            other.gameObject.transform.SetParent(null);

        if (anyTrigger(other))
            inTrigger = false;
    }

    public void resetSounds () {
        outsideSound.SetActive(true);
        insideSound.SetActive(false);
    }

    private bool s_player (Collider obj) {
        return obj.gameObject.CompareTag("Player") && !isMultiplayer && !obj.gameObject.GetComponent<Rigidbody>().isKinematic;
    }

    private bool mp_player (Collider obj) {
        return obj.gameObject.name == "MinePlayer" && isMultiplayer && !obj.gameObject.GetComponent<Rigidbody>().isKinematic;
    }

    private bool mp_any_player (Collider obj) {
        return obj.gameObject.CompareTag("Player") && isMultiplayer && !obj.gameObject.GetComponent<Rigidbody>().isKinematic;
    }

    private bool anyTrigger (Collider obj) {
        return (obj.gameObject.name == "MinePlayer" && isMultiplayer) || (obj.gameObject.CompareTag("Player") && !isMultiplayer);
    }
}