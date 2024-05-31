using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class serverManager : MonoBehaviourPunCallbacks
{
    public Text title, buttontext;
    public Button but, create, join, random;
    public InputField createIn, joinIn, namefield;
    public GameObject canv, thisone;
    public menuController menuController;
    public centralAdvertisementManager adManager;
    bool connected = false, loading = false;

    void Start()
    {
        namefield.text = PlayerPrefs.GetString("name");
    }

    public void clickMult()
    {
        if (!connected) loading = true;
        else
        {
            thisone.SetActive(true);
            canv.SetActive(false);
        }
    }

    void FixedUpdate ()
    {
        if (!connected && loading)
        {
            PhotonNetwork.ConnectUsingSettings();
            but.interactable = false;
            buttontext.text = "Connecting...";
        }
        random.interactable = namefield.text != "";
        join.interactable = joinIn.text != "" && namefield.text != "";
        create.interactable = createIn.text != "" && namefield.text != "";

        if (connected) but.interactable = !(menuController.airlines[menuController.getSelected()].GetComponent<airlineSelecter>().isAd && !adManager.isFinished()) && menuController.SelectedMultiplayerablePlane();
    }

    public override void OnConnectedToMaster()
    {
        buttontext.text = "Tap to continue";
        connected = true;
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        title.text = message;
    }

    public void CreateRoom()
    {
        if (namefield.text.Length <= 20 && namefield.text.Length >= 3)
        {
            if (createIn.text != "")
            {
                title.text = "Creating new room...";
                RoomOptions ro = new RoomOptions();
                ro.MaxPlayers = 10;
                PhotonNetwork.CreateRoom(createIn.text, ro);
            }
        }
        else {
            title.text = "Uncorrect nickname!";
        }
    }

    public void JoinRandom()
    {
        if (namefield.text.Length <= 20 && namefield.text.Length >= 3)
        {
            PhotonNetwork.JoinRandomOrCreateRoom();
            title.text = "Loading...";
        }
        else {
            title.text = "Uncorrect nickname!";
        }
    }

    public void JoinRoom()
    {
        if (namefield.text.Length <= 20 && namefield.text.Length >= 3)
        {
            PhotonNetwork.JoinRoom(joinIn.text);
            title.text = "Loading...";
        }
        else {
            title.text = "Uncorrect nickname!";
        }
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.NickName = namefield.text;
        PlayerPrefs.SetString("name", namefield.text);
        thisone.SetActive(false);
        PhotonNetwork.LoadLevel("MPscene");
    }
}
