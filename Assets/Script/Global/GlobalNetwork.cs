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

        internal static NetworkManager_ForClient networkManager = null;
        internal static Network_ActionSender actionSender = null;
        internal static Network_ActionReceiver actionReciever = null;

        // client event
        public static event Action<ConnectionStatus, string, string> onConnectionEvent = delegate { };
        public static event Action<Message> onMessageReceivedEvent = delegate { };

        internal static void OnConnectionChange(ConnectionStatus rConnectionStatue, string rShortMsg, string rReason) {
            connectionStatus = rConnectionStatue;
            onConnectionEvent.Invoke(rConnectionStatue, rShortMsg, rReason);
        }

        internal static void ClearAllListeners() {
            onConnectionEvent = delegate { };
            onMessageReceivedEvent = delegate { };
        }
    }
}