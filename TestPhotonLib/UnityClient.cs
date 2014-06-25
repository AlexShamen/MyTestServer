using ExitGames.Logging;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestPhotonLib
{
    public class UnityClient: PeerBase
    {
        private readonly ILogger Log = LogManager.GetCurrentClassLogger();

        public UnityClient(IRpcProtocol rpcProtocol, IPhotonPeer unmanagedPeer)
            : base(rpcProtocol, unmanagedPeer)
        {
            Log.Info("Player connection ip: " + unmanagedPeer.GetLocalIP());
        }

        protected override void OnDisconnect(PhotonHostRuntimeInterfaces.DisconnectReason reasonCode, string reasonDetail)
        {
            Log.Debug("Disconnected!");
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            // здесь клиент что то присылает
            switch(operationRequest.OperationCode)
            {
                case 1:
                    if (operationRequest.Parameters.ContainsKey(1))
                    {
                        Log.Debug("Receive type=1 parameters:   " + operationRequest.Parameters[1]);
                        OperationResponse response = new OperationResponse()
                        {
                            OperationCode = operationRequest.OperationCode,
                            Parameters = new Dictionary<byte, object> { { 1, "response message"} }
                        };
                        SendOperationResponse(response, sendParameters);
                    }
                    break;
                case 2:
                    if (operationRequest.Parameters.ContainsKey(1))
                    {
                        Log.Debug("Receive type=1 parameters:   " + operationRequest.Parameters[1]);
                        EventData eventData = new EventData()
                        {
                            Code = 1,
                            Parameters = new Dictionary<byte, object> { { 1, "response message for event" } }
                        };
                        SendEvent(eventData, sendParameters);
                    }
                    break;
                case 10: // CREATE
                    if (operationRequest.Parameters.Count == 2)
                    {
                        EventData eventData = new EventData()
                        {
                            Code = 10,
                            Parameters = operationRequest.Parameters
                        };
                        
                        ApplicationBase.Instance.BroadCastEvent(eventData, MyServer.Peers, sendParameters);
                    }
                    break;
                case 20: // MOVE
                    if (operationRequest.Parameters.Count == 2)
                    {
                        EventData eventData = new EventData()
                        {
                            Code = 20,
                            Parameters = operationRequest.Parameters
                        };
                        
                        ApplicationBase.Instance.BroadCastEvent(eventData, MyServer.Peers, sendParameters);
                    }
                    break;
                default:
                    Log.Debug("Unknown operation code:   " + operationRequest.OperationCode);
                    break;
            }
        }

    }

}
