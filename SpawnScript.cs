using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnScript : MonoBehaviour
{
    public GameObject player;
    
    void Start()
    {
        Vector3 pos = this.gameObject.transform.position;
        PhotonNetwork.Instantiate(player.name, pos, Quaternion.identity);
    }
}
