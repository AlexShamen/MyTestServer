/*
 * using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using System.Collections.Generic;

public class PhotonServer : MonoBehaviour, IPhotonPeerListener
{

	private const string ConnectionString = "192.168.1.75:4530";
	private const string AppName = "MyTestServer";

	private static PhotonServer _instance;
	public static PhotonServer Instance 
	{
		get { return _instance; }
	}

	private PhotonPeer PhotonPeer { get; set; }





	void Awake()
	{
		if(Instance != null)
		{
			DestroyObject(gameObject);
		}

//		DontDestroyOnLoad(gameObject);
//		Application.runInBackground = true;
//		_instance = this;
	}

	void Start()
	{
		PhotonPeer = new PhotonPeer(this, ConnectionProtocol.Tcp);
		Connect();
	}

	void Update()
	{
		if (PhotonPeer != null)
		{
			PhotonPeer.Service();
		}
	}

	void OnApplicationQuit()
	{
		Disconnect();
	}

	private void Connect()
	{
		if (PhotonPeer != null)
		{
			PhotonPeer.Connect(ConnectionString, AppName);
		}
	}

	private void Disconnect()
	{
		if (PhotonPeer != null)
		{
			PhotonPeer.Disconnect();
		}
	}





	#region IPhotonPeerListener implementation

	public void DebugReturn (DebugLevel level, string message)
	{

	}

	public void OnOperationResponse (OperationResponse operationResponse)
	{
		// что то приходит с сервера
		switch (operationResponse.OperationCode)
		{
		case 1:
			if (operationResponse.Parameters.ContainsKey(1))
			{
				Debug.Log ("Receive type=1 parameter:   " + operationResponse.Parameters[1]);
				SendOperation(2);
			}
			break;
		default:
			Debug.Log ("Unknown OperationCode:   " + operationResponse.OperationCode);
			break;
		}
	}

	public void OnStatusChanged (StatusCode statusCode)
	{
		Debug.Log (statusCode.ToString());

		if (statusCode == StatusCode.Connect)
		{
			SendOperation(1);
		}
	}

	public void OnEvent (EventData eventData)
	{
		switch(eventData.Code)
		{
		case 1:
			if(eventData.Parameters.ContainsKey(1))
			{
				Debug.Log ("Receive type=1 parameter:   " + eventData.Parameters[1]);
			}
			break;
		default:
			Debug.Log ("Unknown event:   " + eventData.Code);
			break;
		}
	}

	public void SendOperation(byte operationCode)
	{
		PhotonPeer.OpCustom(operationCode, new Dictionary<byte, object> { { 1, "parameter" } }, false);
	}

	#endregion


}
*/
