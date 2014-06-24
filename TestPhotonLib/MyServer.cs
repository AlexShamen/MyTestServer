using log4net.Config;
using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using Photon.SocketServer;
using System;
using System.IO;

namespace TestPhotonLib
{
    public class MyServer: ApplicationBase
    {
        private readonly ILogger Log = LogManager.GetCurrentClassLogger();

        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            return new UnityClient(initRequest.Protocol, initRequest.PhotonPeer);
        }

        protected override void Setup()
        {
            FileInfo fileInfo = new FileInfo(Path.Combine(BinaryPath, "log4net.config"));
            if(fileInfo.Exists)
            {
                LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
                XmlConfigurator.ConfigureAndWatch(fileInfo);
            }

            Log.Info("Server is ready!");
        }

        protected override void TearDown()
        {
            Log.Debug("Server was stoped!");
        }
    }
}
