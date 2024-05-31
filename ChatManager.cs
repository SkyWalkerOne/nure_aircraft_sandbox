using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ChatManager : MonoBehaviour
{
    public List<string> _messages = new List<string>();
    public int _maximumMessages;

    private PhotonView _photon;
    private string[] prohibited;

    void Start()
    {
        _photon = GetComponent<PhotonView>();
        prohibited = new string[] { "бля", "хуй", "хер", "хрен", "фиг", "еба", "ебат", "сук", "пидо", "гонд", "срат", "сран", "говн", "fuck", "bitch", "slave", "shit", " hoe" };
    }

    [PunRPC]
    void RPC_AddNewMessage(string msg)
    {
        _messages.Add(msg);
    }

    public void SendChat(string msg, bool nickname)
    {
        _photon.RPC("RPC_AddNewMessage", RpcTarget.All, PhotonNetwork.NickName + ": " + msg);
    }

    public void SubmitChat(InputField ChatInput)
    {
        string blankCheck = ChatInput.text;

        if (blankCheck == "" || blankCheck.Length > 200)
            return;

        foreach (string word in prohibited)
        {
            string newstr = "";
            for (int i = 0; i < word.Length; i++) newstr += "*";

            blankCheck = blankCheck.ToLower().Replace(word, newstr);
        }

        _photon.RPC("RPC_AddNewMessage", RpcTarget.All, PhotonNetwork.NickName + ": " + blankCheck);
        ChatInput.text = "";

    }

    public string getFullChat()
    {
        string NewContents = "";
        foreach (string s in _messages)
            NewContents += s + "\n\n";

        return NewContents;
    }

    void Update()
    {
        if (PhotonNetwork.InRoom)
        {
            if (_messages.Count > _maximumMessages)
            {
                _messages.RemoveAt(0);
            }
        }
        else if (_messages.Count > 0)
        {
            _messages.RemoveAt(0);
        }
    }
}
