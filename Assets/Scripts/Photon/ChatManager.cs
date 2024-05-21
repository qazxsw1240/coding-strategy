using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Chat;
using ExitGames.Client.Photon;
using Photon.Pun;

public class ChatManager : MonoBehaviour, IChatClientListener
{
	ChatClient chatClient;
	[SerializeField] string userID;

	// Start is called before the first frame update
	void Start()
	{
		chatClient = new ChatClient(this);
		chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(userID));
	}

	// Update is called once per frame
	void Update()
	{
		chatClient.Service();
	}

	public void DebugReturn(DebugLevel level, string message)
	{
		throw new System.NotImplementedException();
	}

	public void OnChatStateChange(ChatState state)
	{
		throw new System.NotImplementedException();
	}

	public void OnConnected()
	{
		throw new System.NotImplementedException();
	}

	public void OnDisconnected()
	{
		throw new System.NotImplementedException();
	}

	public void OnGetMessages(string channelName, string[] senders, object[] messages)
	{
		throw new System.NotImplementedException();
	}

	public void OnPrivateMessage(string sender, object message, string channelName)
	{
		throw new System.NotImplementedException();
	}

	public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
	{
		throw new System.NotImplementedException();
	}

	public void OnSubscribed(string[] channels, bool[] results)
	{
		throw new System.NotImplementedException();
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


}
