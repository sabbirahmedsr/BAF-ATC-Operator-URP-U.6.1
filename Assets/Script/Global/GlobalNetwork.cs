using ATC.Operator.Networking;
using Riptide;
using System;

namespace ATC.Global {

    public enum ConnectionStatus { hasNoResult, attemptConnection, connected, connectionFailed, disconnected }

    public class GlobalNetwork {
        public static string ip = "127.0.0.1";
        public static ushort port = 7777;
        public static ushort clientId = 0;
        public static ConnectionStatus connectionStatus = ConnectionStatus.hasNoResult;
        public static NetworkManager_ForClient networkManager = null;

        internal static Network_ActionSender actionSender = null;
        internal static Network_ActionReceiver actionReciever = null;

        // client event
        public static event Action<ConnectionStatus, string, string> onConnectionEvent = delegate { };
        public static event Action<Message> onMessageReceivedEvent = delegate { };

        internal static void OnConnectionChange(ConnectionStatus rConnectionStatue, string rShortMsg, string rReason) {
            connectionStatus = rConnectionStatue;
            onConnectionEvent.Invoke(rConnectionStatue, rShortMsg, rReason);
        }

    }
}
/*
using NetworkingScript;
using Riptide;
using System;

public enum ConnectionStatus { hasNoResult, connected, connectionFailed }

public class GlobalNetwork {
    public static string ip = "127.0.0.1";
    public static ushort port = 7777;
    public static ConnectionStatus connectionStatus = ConnectionStatus.hasNoResult;
    public static NetworkAction NetworkAction;

    // client event
    public static ClientManager clientManager = null;
    public static event Action<bool, string> onConnectionEvent = delegate { };
    public static event Action<Message> onMessageReceivedEvent = delegate { };

    internal static void StartClient() {
        clientManager.StartClient();
    }
    internal static void StopClient() {
        clientManager.StopClient();
    }

    internal static void OnConnection(bool rSuccessBool, string rMsg) {
        connectionStatus = rSuccessBool ? ConnectionStatus.connected : ConnectionStatus.connectionFailed;
        onConnectionEvent.Invoke(rSuccessBool, rMsg);
    }

    internal static void OnMessageRecieved(Message rMsg) {
        onMessageReceivedEvent.Invoke(rMsg);
    }

    internal static void SendMessageToNetwork(Message rMsg) {
        clientManager.SendMessageToNetwork(rMsg);
    }

}
*/