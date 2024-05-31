using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class loaderClass : MonoBehaviour
{
    public string menuSceneName;
    public bool isMultiplayer;

    public void loadMenu () {
        if (isMultiplayer) PhotonNetwork.LeaveRoom();

        SceneManager.LoadScene(menuSceneName);
    }
}
