using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour, IChatClientListener
{
	#region Setup

	ChatClient chatClient;
	bool isConnected;
	[SerializeField] string username;

	public void UsernameOnValueChange(string valueIn)
	{
		username = valueIn;
	}

	#endregion Setup

	#region General

	[SerializeField] GameObject chatPanel;
	string privateReceiver = "";
	string currentChat;
	[SerializeField] InputField chatField;
	[SerializeField] Text chatDisplay;

	// Start is called before the first frame update
	void Start()
	{
		isConnected = true;
		chatClient = new ChatClient(this);
		//chatClient.ChatRegion = "US";
		chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(username));
		Debug.Log("Connenting");
	}

	// Update is called once per frame
	void Update()
	{
		if (isConnected)
		{
			chatClient.Service();
		}

		if (chatField.text != "" && Input.GetKey(KeyCode.Return))
        {
			SubmitPublicChatOnClick();
		}
	}

	#endregion General

	#region PublicChat

	public void SubmitPublicChatOnClick()
	{
		if (privateReceiver == "")
		{
			chatClient.PublishMessage("RegionChannel", currentChat);
			chatField.text = "";
			currentChat = "";
		}
	}

	public void TypeChatOnValueChange(string valueIn)
	{
		currentChat = valueIn;
	}

	#endregion PublicChat

	#region Callbacks

	public void DebugReturn(DebugLevel level, string message)
	{
		//throw new System.NotImplementedException();
	}

	public void OnChatStateChange(ChatState state)
	{
		if (state == ChatState.Uninitialized)
		{
			isConnected = false;
			chatPanel.SetActive(false);
		}
	}

	public void OnConnected()
	{
		Debug.Log("Connected");
		chatClient.Subscribe(new string[] { "RegionChannel" });
	}

	public void OnDisconnected()
	{
		isConnected = false;
		chatPanel.SetActive(false);
	}

	public void OnGetMessages(string channelName, string[] senders, object[] messages)
	{
		string msgs = "";
		for (int i = 0; i < senders.Length; i++)
        {
			msgs = string.Format("{0}: {1}", senders[i], messages[i]);

			chatDisplay.text += "\n" + msgs;

			Debug.Log(msgs);
		}

	}

	public void OnPrivateMessage(string sender, object message, string channelName)
	{
		string msgs = "";

		msgs = string.Format("(Private) {0}: {1}", sender, message);

		chatDisplay.text += "\n " + msgs;

		Debug.Log(msgs);

	}

	public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
	{
		throw new System.NotImplementedException();
	}

	public void OnSubscribed(string[] channels, bool[] results)
	{
		chatPanel.SetActive(true);
	}

	public void OnUnsubscribed(string[] channels)
	{
		throw new System.NotImplementedException();
	}

	public void OnUserSubscribed(string channel, string user)
	{
		throw new System.NotImplementedException();
	}

	public void OnUserUnsubscribed(string channel, string user)
	{
		throw new System.NotImplementedException();
	}

	#endregion Callbacks
}