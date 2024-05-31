using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class chatReciever : MonoBehaviour
{
    public ChatManager chat;
    public InputField inputChat;
    private float _buildDelay = 0f;

    string chatContent;

    void Start() {
        if (chat == null)
            chat = GameObject.Find("serverSync").GetComponent<ChatManager>();
    }

    void Update()
    {
        if (_buildDelay < Time.time)
        {
            if (chat.getFullChat() != chatContent) {
                chatContent = chat.getFullChat();
                GetComponent<Text>().text = chatContent;
            }  

            _buildDelay = Time.time + 0.5f;
        }
    }

    public void sendMessage() => chat.SubmitChat(inputChat);
}
