using System.Collections;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using TestPhotonLib.Common;



/// <summary>
/// 
/// </summary>
public class PhotonServer : IPhotonPeerListener
{

    private const string ConnectionString = "192.168.1.75:4530";
	private const string AppName = "MyTestServer";

	private static PhotonServer _instance;
	public static PhotonServer Instance 
	{
		get { return _instance; }
	}

	public PhotonPeer PhotonPeer { get; set; }





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
        PhotonPeer.OpCustom(operationCode, new Dictionary<byte, object> { { 1, "parameter" } }, false);
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
