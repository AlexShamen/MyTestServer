using UnityEngine;
using System.Collections;

public class PhotonServerIn : MonoBehaviour 
{

	PhotonServer _photonServer;

	void Awake()
	{
		if(PhotonServer.Instance != null)
		{
			DestroyObject(gameObject);
		}

//		DontDestroyOnLoad(gameObject);
//		Application.runInBackground = true;
	}
	
	void Start () 
	{
		_photonServer = new PhotonServer();
		_photonServer.DebugLogHandle = DebugLog;
	}

	void Update () 
	{
		_photonServer.PhotonPeer.Service();
	}

	void OnApplicationQuit()
	{
		_photonServer.PhotonPeer.Disconnect();
	}

	void DebugLog(object message)
	{
		Debug.Log (message);
	}
	
}
