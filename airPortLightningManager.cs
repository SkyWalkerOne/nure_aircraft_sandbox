using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class airPortLightningManager : MonoBehaviour
{
    [SerializeField] private GameObject lights;

    void Start()
    {
        lights.SetActive(PlayerPrefs.GetInt("time") == 1);
    }
}
