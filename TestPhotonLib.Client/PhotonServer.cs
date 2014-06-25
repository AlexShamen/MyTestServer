using System.Collections;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using TestPhotonLib.Common;
using System;
using UnityEngine;



/// <summary>
/// 
/// </summary>
public class PhotonServer : IPhotonPeerListener
{

    public delegate void PlayerPositionDelegate(string id, Vector3 position);
    private string ConnectionString;
	private string AppName;

	private static PhotonServer _instance;
	public static PhotonServer Instance 
	{
		get { return _instance; }
	}

	public PhotonPeer PhotonPeer { get; set; }

    public string ClientName { get; set; }
    public Vector3 Position { get; set; }

    public PlayerPositionDelegate CreatePlayer;
    public PlayerPositionDelegate MovePlayer;




    /// <summary>
    /// 192.168.1.75:4530
    /// windowsseven1:4530
    /// </summary>
    public PhotonServer(string conStr = "192.168.1.75:4530", string appName = "MyTestServer")
    {
        ConnectionString = conStr;
        AppName = appName;

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
                    CreatePlayer((string)eventData.Parameters[1], (Vector3)eventData.Parameters[2]);
                }
                break;

            case 20:
                if (MovePlayer != null)
                {
                    MovePlayer((string)eventData.Parameters[1], (Vector3)eventData.Parameters[2]);
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

    public void SendOperation_Position(byte operationCode, Vector3 position)
    {
        switch (operationCode)
        {
            case 10:
            case 20:
                PhotonPeer.OpCustom(operationCode, new Dictionary<byte, object> { { 1, ClientName }, { 2, position } }, false);
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

    #endregion

}
