using System.Collections;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using TestPhotonLib.Common;
using System;



/// <summary>
/// 
/// </summary>
public class PhotonServer : IPhotonPeerListener
{

    #region

    [Serializable]
    public class MyVector3
    {
        public float X;
        public float Y;
        public float Z;

        public MyVector3()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }

        public MyVector3(float x, float y, float z)
        {
            X = x;
            Y = y; 
            Z = z;
        }

    }

    public delegate void PlayerPositionDelegate(string id, MyVector3 position);

    #endregion


    private string ConnectionString = "192.168.1.75:4530";
	private string AppName = "MyTestServer";

	private static PhotonServer _instance;
	public static PhotonServer Instance 
	{
		get { return _instance; }
	}

	public PhotonPeer PhotonPeer { get; set; }

    public string ClientName { get; set; }
    public MyVector3 Position { get; set; }

    public PlayerPositionDelegate CreatePlayer;
    public PlayerPositionDelegate MovePlayer;





    public PhotonServer()
    {
        _instance = this;

        PhotonPeer = new PhotonPeer(this, ConnectionProtocol.Tcp);
        if (PhotonPeer != null)
            PhotonPeer.Connect(ConnectionString, AppName);

        ErrorCode errorCode = new ErrorCode();
    }

    
    
    
    
    #region IPhotonPeerListener implementation

    public void DebugReturn(DebugLevel level, string message)
    {
        
    }

    public void OnEvent(EventData eventData)
    {
        switch (eventData.Code)
        {
            case 1:
                if (eventData.Parameters.ContainsKey(1))
                {
                    DebugLog("Receive type=1 parameter:   " + eventData.Parameters[1]);
                }
                break;

            case 10:
                if (CreatePlayer != null)
                {
                    CreatePlayer((string)eventData.Parameters[1], (MyVector3)eventData.Parameters[2]);
                }
                break;

            case 20:
                if (MovePlayer != null)
                {
                    MovePlayer((string)eventData.Parameters[1], (MyVector3)eventData.Parameters[2]);
                }
                break;

            default:
                DebugLog("Unknown event:   " + eventData.Code);
                break;
        }
    }

    /// <summary>
    /// Данные приходят с сервера.
    /// </summary>
    /// <param name="operationResponse"></param>
    public void OnOperationResponse(OperationResponse operationResponse)
    {
        switch (operationResponse.OperationCode)
        {
            case 1:
                if (operationResponse.Parameters.ContainsKey(1))
                {
                    DebugLog("Receive type=1 parameter:   " + operationResponse.Parameters[1]);
                    SendOperation(2);
                }
                break;
            default:
                DebugLog("Unknown OperationCode:   " + operationResponse.OperationCode);
                break;
        }
    }

    public void OnStatusChanged(StatusCode statusCode)
    {
        DebugLog(statusCode.ToString());

        if (statusCode == StatusCode.Connect)
        {
            SendOperation(1);
        }
    }

    public void SendOperation(byte operationCode)
    {
        switch(operationCode)
        {
            case 1:
            case 2:
                PhotonPeer.OpCustom(operationCode, new Dictionary<byte, object> { { 1, "parameter" } }, false);
                break;
        }
    }

    public void SendOperation_Position(byte operationCode, float x, float y, float z)
    {
        switch (operationCode)
        {
            case 10:
            case 20:
                PhotonPeer.OpCustom(operationCode, new Dictionary<byte, object> { { 1, ClientName }, { 2, new MyVector3(x, y, z) } }, false);
                break;
        }
    }

    #endregion




    #region realization Debug.Log from Unity3D

    public delegate void DelegateDebugLog(object message);
    public DelegateDebugLog DebugLogHandle;

    public void DebugLog(object message)
    {
        if (DebugLogHandle != null)
            DebugLogHandle(message);
    }

    /// <summary>
    /// 192.168.1.75:4530
    /// windowsseven1:4530
    /// </summary>
    /// <param name="conStr"></param>
    public void SetConnectionString(string conStr)
    {
        ConnectionString = conStr;
    }

    #endregion

}
