using UnityEngine;
using Riptide;
using AirplaneControllerScript;

namespace ATC.Operator.Networking { 
    public class Network_ActionSender : MonoBehaviour, INetwork_ActionSender_Operator {
        private AirplaneController tmpApController;
        NetworkManager_ForClient networkManager;

        internal void Initialize(NetworkManager_ForClient rNetworkManager) {
            networkManager = rNetworkManager;
        }



        public void SendHeartbeat() {
            Message heartBeatMsg = CreateNewMessage(MessageSendMode.Unreliable, MSG_Initial.heartbeat);
            networkManager.SendMessageToNetwork(heartBeatMsg);
        }

        public void Request_StartupInfo() {
            Message msg = CreateNewMessage(MessageSendMode.Reliable, MSG_Initial.request_startupInfo);
            networkManager.SendMessageToNetwork(msg);
        }



        // Create_Airplane_And_Node
        public void Create_ArrFlight_OnServer(CallSign rCallSign, ushort rStartIndex) {
            Message msg = CreateNewMessage(MessageSendMode.Reliable, MSG_Initial.create_ArrFlight_OnServer);
            msg.AddUShort((ushort)rCallSign);
            msg.AddUShort(rStartIndex);
            networkManager.SendMessageToNetwork(msg);
        }
        public void Create_DepFlight_OnServer(CallSign rCallSign, ParkingStandID rParkingStandID) {
            Message msg = CreateNewMessage(MessageSendMode.Reliable, MSG_Initial.create_DepFlight_OnServer);
            msg.AddUShort((ushort)rCallSign);
            msg.AddUShort((ushort)rParkingStandID);
            networkManager.SendMessageToNetwork(msg);
        }










        // Update_Airplane_ReliableData
        public void Send_AP_FlightCommand(ushort rGlobalAirplaneID, ushort rCommandID, ushort[] rParameterArray) {
            Message msg = CreateNewMessage(MessageSendMode.Reliable, MSG_Initial.send_AP_FlightCommand);
            msg.AddUShort(rGlobalAirplaneID);
            msg.AddUShort(rCommandID);
            msg.AddUShort((ushort)rParameterArray.Length);
            for (int i = 0; i < rParameterArray.Length; i++) {
                msg.AddUShort(rParameterArray[i]);
            }
            networkManager.SendMessageToNetwork(msg);
        }
        public void OnDestroy_AP_RadarMapNode(ushort rGlobalAirplaneID) {
            if (GlobalData.allActiveAirplane.TryGetValue(rGlobalAirplaneID, out tmpApController)) {
                tmpApController.OnDestroyRadapMapNode();
            }
        }
        public void OnDestroy_AP_SurfaceMapNode(ushort rGlobalAirplaneID) {
            if (GlobalData.allActiveAirplane.TryGetValue(rGlobalAirplaneID, out tmpApController)) {
                tmpApController.OnDestroySurfaceMapNode();
            }
        }
        public void OnDestroy_AP_Airplane_OnOperator(ushort rGlobalAirplaneID) {
            if (GlobalData.allActiveAirplane.TryGetValue(rGlobalAirplaneID, out tmpApController)) {
                tmpApController.DestroyMeAll();
            }
        }






       // Update_Other_ReliableData
        public void Send_GlobalSpeed(ushort rSpeed) {
            Message tmpMsg = CreateNewMessage(MessageSendMode.Reliable, MSG_Initial.update_globalSpeed);
            tmpMsg.AddUShort(rSpeed);
            networkManager.SendMessageToNetwork(tmpMsg);
        }







        private Message CreateNewMessage(MessageSendMode rSendMode, MSG_Initial rMsgInitial) {
            Message newMsg = Message.Create(rSendMode, MessageID.clientId);
            newMsg.AddUShort((ushort)rMsgInitial);
            return newMsg;
        }


    }
}