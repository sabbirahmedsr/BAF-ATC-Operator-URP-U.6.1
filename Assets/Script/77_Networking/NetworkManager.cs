using ATC.Global;
using Riptide;
using Riptide.Utils;
using System;
using UnityEngine;

namespace ATC.Operator.Networking {

    public enum MessageID : ushort {
        serverId = 0,
        clientId = 1,
    }

    [RequireComponent(typeof(Network_Heartbeat))]
    [RequireComponent(typeof(Network_ConnectionUI))]
    [RequireComponent(typeof(Network_ActionReceiver))]
    [RequireComponent(typeof(Network_ActionSender))]

    public class NetworkManager_ForClient : MonoBehaviour {

        private Client client = new Client();
        private int timeOutTimeInMili = 600000;

        [Header("Update Rate")]
        [Tooltip("Number of frame per seconds [network update frame]")]
        [SerializeField] private int frameRate = 5;
        private float frameWaitingTime = 10f;
        private float counter = 0;

        [Header("Child Script")]
        [SerializeField] private Network_Heartbeat heartbeat;
        [SerializeField] private Network_ConnectionUI connectionUI;
        [SerializeField] private Network_ActionReceiver actionReceiver;
        [SerializeField] private Network_ActionSender actionSender;

        internal void Initialize() {
            // Initialize Riptide Logger
            RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

            // Calculate variable 
            frameWaitingTime = 1f / frameRate;          

            // register call back
            client.Connected += OnSelfConnected;
            client.Disconnected += OnSelfDisconnected;
            client.ConnectionFailed += OnSelfConnectionFailed;

            // initialize child script
            connectionUI.Initialize(this);
            actionReceiver.Initialize(this);
            actionSender.Initialize(this);

            // set global variable
            GlobalNetwork.networkManager = this;
            GlobalNetwork.actionReciever = actionReceiver;
            GlobalNetwork.actionSender = actionSender;

            // finally start the client
            StartOrRetryConnection();
        }


        public void StartOrRetryConnection() {
            GlobalNetwork.OnConnectionChange(ConnectionStatus.attemptConnection, "Connecting", "Attempting Connection...");
            client.Connect(GlobalNetwork.ip + ":" + GlobalNetwork.port);
            client.TimeoutTime = timeOutTimeInMili;
        }

        public void SyncSceneWithServer() {
            // request for startup info to the server
            actionSender.Request_StartupInfo();
        }




        internal void Update() {
            counter += Time.deltaTime;
            if (counter >= frameWaitingTime) {
                client.Update();
                heartbeat.ManualUpdate();
                counter = 0f;
            }
        }

        public void OnDisable() {
            client.Disconnect();
        }
        public void Disconnect() {
            client.Disconnect();            
        }











        // ConnectionCheck
        private void OnSelfConnected(object sender, EventArgs e) {
            GlobalNetwork.clientId = client.Id;
            GlobalNetwork.OnConnectionChange(ConnectionStatus.connected, "Connected", e.ToString());
            SyncSceneWithServer();
        }
        private void OnSelfDisconnected(object sender, DisconnectedEventArgs e) {
            GlobalNetwork.OnConnectionChange(ConnectionStatus.disconnected, "Server Lost", e.Reason.ToString());
        }
        private void OnSelfConnectionFailed(object sender, ConnectionFailedEventArgs e) {
            GlobalNetwork.OnConnectionChange(ConnectionStatus.connectionFailed, "No server found", e.Reason.ToString());
        }
        private void OnMessageRecieve(object sender, MessageReceivedEventArgs e) {
            actionReceiver.OnRecieveMessage(e.Message);
            Debug.Log(e.Message);
        }
        public void SendMessageToNetwork(Message rMsg) {
            client.Send(rMsg);
        }









        // SendOrReceiveMessage
        [MessageHandler((ushort)MessageID.clientId)]
        private static void HandleMessageFromClient(ushort fromClientID, Message message) {
            if (fromClientID == GlobalNetwork.clientId)
                return;
            OnRecievedMessage(message);
        }
        [MessageHandler((ushort)MessageID.serverId)]
        private static void HandleMessageFromServer(Message message) {
            ushort fromClientID = message.GetUShort();
            if (fromClientID == GlobalNetwork.clientId)
                return;
            OnRecievedMessage(message);
        }

        

        private static void OnRecievedMessage(Message message) {
            GlobalNetwork.actionReciever.OnRecieveMessage(message);
        }

    }
}
