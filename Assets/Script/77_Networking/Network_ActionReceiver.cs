using UnityEngine;
using Riptide;
using Pathway;
using AirplaneControllerScript;

namespace ATC.Operator.Networking { 
    public class Network_ActionReceiver : MonoBehaviour {
        [SerializeField] private AirplaneData[] allAirplaneData;
        private AirplaneController tmpApController;

        private NetworkMessageConverter netMsgConverter = new NetworkMessageConverter(); 

        internal void Initialize(NetworkManager_ForClient rNetworkManager) {
            netMsgConverter.Initialize(this);
        }

        internal void OnRecieveMessage(Message message) {
            netMsgConverter.ConvertMessageToAction(message);
        }





        // Connection_And_StartupInfo
        public void OnRecieved_StartupInfo_Result(ushort rGlobalAirplaneID, NetworkAirplane_SpawnInfo rSpawnInfo) {
            OnCreate_Airplane_OnOperator(rGlobalAirplaneID, rSpawnInfo);
        }



        // Create_Airplane_And_Node
        public void OnCreate_Airplane_OnOperator(ushort rGlobalAirplaneID, NetworkAirplane_SpawnInfo rSpawnInfo) {
            if (GlobalData.allActiveAirplane.ContainsKey(rGlobalAirplaneID)) {
                Debug.Log("We already have this airplane");
                return;
            }

            foreach (var airplaneData in allAirplaneData) {
                if (airplaneData.callSign == rSpawnInfo.callSign) {
                    GameObject newAirplane = Instantiate(airplaneData.airplanePrefab);
                    AirplaneController newAPController = newAirplane.GetComponent<AirplaneController>();
                    newAPController.Initialize(rGlobalAirplaneID, rSpawnInfo);
                }
            }
        }
        public void OnCreate_RadarMapNode(ushort rGlobalAirplaneID, ushort rStartLineIndex) {
            if (GlobalData.allActiveAirplane.TryGetValue(rGlobalAirplaneID, out tmpApController)) {
                GlobalEvent.OnRadarMapNodeCreated(tmpApController, rStartLineIndex);
            }
        }
        public void OnCreate_SurfaceMapNode(ushort rGlobalAirplaneID, ushort rStartLineIndex) {
            if (GlobalData.allActiveAirplane.TryGetValue(rGlobalAirplaneID, out tmpApController)) {
                GlobalEvent.OnSurfaceMapNodeCreated(tmpApController, rStartLineIndex);
            }
        }





        // Update_All_UnrealiableData
        public void OnUpdate_All_PosAndRot(ushort rGlobalAirplaneID, Vector3 rPos, Vector3 rFwd) {
            if (GlobalData.allActiveAirplane.TryGetValue(rGlobalAirplaneID, out tmpApController)) {
                tmpApController.Update_PosRot(rPos, rFwd);
            }
        }

        public void OnUpdate_TimeAndVisualInfo(ushort rGlobalAirplaneID, VizHeadSpeedFL rVizHSFL) {
            if (GlobalData.allActiveAirplane.TryGetValue(rGlobalAirplaneID, out tmpApController)) {
                tmpApController.Update_VizHeadSpeedFL(rVizHSFL);
            }
        }







       // Update_Airplane_ReliableData
        public void OnRecieve_AP_PathHistory(ushort rGlobalAirplaneID, Path[] rPathHistory) {
            if (GlobalData.allActiveAirplane.TryGetValue(rGlobalAirplaneID, out tmpApController)) {
                tmpApController.Update_PathHistory(rPathHistory);
            }
        }

        public void OnRecieve_AP_AirplanePhaseNT(ushort rGlobalAirplaneID, NameAndTimeZ rLastNT, NameAndTimeZ rNextNT) {
            if (GlobalData.allActiveAirplane.TryGetValue(rGlobalAirplaneID, out tmpApController)) {
                tmpApController.Update_AirplanePhaseNT(rLastNT, rNextNT);
            }
        }
        public void OnRecieve_AP_ArrivalCommandList(ushort rGlobalAirplaneID, ArrivalCommandID[] rAllArrivalCommandID) {
            if (GlobalData.allActiveAirplane.TryGetValue(rGlobalAirplaneID, out tmpApController)) {
                tmpApController.Update_ArrivalCommandList(rAllArrivalCommandID);
            }
        }
        public void OnRecieve_AP_DepartureCommandList(ushort rGlobalAirplaneID, DepartureCommandID[] rAllDepartureCommandID) {
            if (GlobalData.allActiveAirplane.TryGetValue(rGlobalAirplaneID, out tmpApController)) {
                tmpApController.Update_DepartureCommandList(rAllDepartureCommandID);
            }
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
        public void OnRecieve_ArrAndDepScoreCount() {
            GlobalEvent.OnScoreCountChange();
        }
    }
}