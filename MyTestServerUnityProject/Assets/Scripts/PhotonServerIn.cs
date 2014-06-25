using UnityEngine;
using System.Collections;

public class PhotonServerIn : MonoBehaviour 
{

	public string Name { get; set; }
	public GameObject PlayerPrefab;

	PhotonServer _photonServer;
	float _sendPosInterval = 0.25f;
	float _currentSendPosInterval = -10.0f;





	void Awake()
	{
		Name = "bulochka";

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
		_photonServer.CreatePlayer = CreatePlayer;
		_photonServer.MovePlayer = MovePlayer;

		Vector3 pos = transform.position;
		_photonServer.SendOperation_Position(10, pos.x, pos.y, pos.z);

		_currentSendPosInterval = _sendPosInterval;
	}

	void Update () 
	{
		_photonServer.PhotonPeer.Service();

		if (_currentSendPosInterval > 0.0f)
		{
			_currentSendPosInterval -= Time.deltaTime;
		}
		else
		{
			if (_currentSendPosInterval > -10.0f)
			{
				Vector3 pos = transform.position;
				_photonServer.SendOperation_Position(20, pos.x, pos.y, pos.z);
				_currentSendPosInterval = _sendPosInterval;
			}
		}
	}

	void OnApplicationQuit()
	{
		_photonServer.PhotonPeer.Disconnect();
	}





	void DebugLog(object message)
	{
		Debug.Log (message);
	}

	void CreatePlayer(string id, PhotonServer.MyVector3 position)
	{
		if (id != Name && PlayerPrefab != null)
		{
			GameObject obj = GameObject.Instantiate(PlayerPrefab);
			obj.name = id;
			obj.transform.position = new Vector3(position.X, position.Y, position.Z);
		}
	}

	void MovePlayer(string id, PhotonServer.MyVector3 position)
	{
		if (id!=Name)
		{
			GameObject obj = GameObject.Find(id);
			obj.transform.position = new Vector3(position.X, position.Y, position.Z);
		}
	}
	
}
