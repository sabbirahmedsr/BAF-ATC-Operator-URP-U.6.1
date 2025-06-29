using Pathway;
using UnityEngine;

namespace ATC.Operator.Networking {    
    public interface INetworkAction_Server {
        // connection and startup
        public void OnRequest_StartupInfo();
        public void Send_StartupInfo_Result();
        // create airplane and other
        public void OnCreate_ArrFlight_OnServer(CallSign rCallSign, ushort rStartIndex);
        public void OnCreate_DepFlight_OnServer(CallSign rCallSign, ParkingStandID rParkingStandID);
        public void Create_Airplane_OnOperator(ushort rGlobalAirplaneID, NetworkAirplane_SpawnInfo rSpawnInfo);
        public void Create_RadarMapNode(ushort rGlobalAirplaneID, ushort rStartLineIndex);
        public void Create_SurfaceMapNode(ushort rGlobalAirplaneID, ushort rStartLineIndex);
        // update unreliable data
        public void Update_All_PosAndRot();
        public void Update_TimeAndVisualInfo();
        // update reliable data
        public void Send_AP_AirplanePhaseNT(ushort rGlobalAirplaneID, NameAndTimeZ rLastNT, NameAndTimeZ rNextNT);
        public void Send_AP_PathHistory(ushort rGlobalAirplaneID, Path[] rPathHisotory);
        public void Send_AP_ArrivalCommandList(ushort rGlobalAirplaneID, ArrivalCommandID[] rAllArrivalCommandID);
        public void Send_AP_DepartureCommandList(ushort rGlobalAirplaneID, DepartureCommandID[] rAllDepartureCommandID);
        public void OnRecieve_AP_FlightCommand(ushort rGlobalAirplaneID, ushort rCommandID, ushort[] rParameterArray);
        public void Destroy_AP_RadarMapNode(ushort rGlobalAirplaneID);
        public void Destroy_AP_SurfaceMapNode(ushort rGlobalAirplaneID);
        public void Destroy_AP_Airplane_OnOperator(ushort rGlobalAirplaneID);
        // update other reliable data
        public void Send_ArrAndDepScoreCount();
        public void OnRecieve_GlobalSpeed(ushort rSpeed);
    }


    public interface INetwork_ActionReciever_Operator{
        // on connection and startup
        public void OnRecieved_StartupInfo_Result(ushort rGlobalAirplaneID, NetworkAirplane_SpawnInfo spawnInfo);
        // on create airplane and other
        public void OnCreate_Airplane_OnOperator(ushort rGlobalAirplaneID, NetworkAirplane_SpawnInfo spawnInfo);
        public void OnCreate_RadarMapNode(ushort rGlobalAirplaneID, ushort rStartLineIndex);
        public void OnCreate_SurfaceMapNode(ushort rGlobalAirplaneID, ushort rStartLineIndex);
        // on update unreliable data
        public void OnUpdate_All_PosAndRot(ushort rGlobalAirplaneID, Vector3 rPos, Vector3 rFwd);
        public void OnUpdate_TimeAndVisualInfo(ushort rGlobalAirplaneID, VizHeadSpeedFL rVizHSFL);
        // on update reliable data
        public void OnRecieve_AP_AirplanePhaseNT(ushort rGlobalAirplaneID, NameAndTimeZ rLastNT, NameAndTimeZ rNextNT);
        public void OnRecieve_AP_PathHistory(ushort rGlobalAirplaneID, Path[] rPathHisotory);
        public void OnRecieve_AP_ArrivalCommandList(ushort rGlobalAirplaneID, ArrivalCommandID[] rAllArrivalCommandID);
        public void OnRecieve_AP_DepartureCommandList(ushort rGlobalAirplaneID, DepartureCommandID[] rAllDepartureCommandID);
        public void OnDestroy_AP_RadarMapNode(ushort rGlobalAirplaneID);
        public void OnDestroy_AP_SurfaceMapNode(ushort rGlobalAirplaneID);
        public void OnDestroy_AP_Airplane_OnOperator(ushort rGlobalAirplaneID);
        // on update other reliable data
        public void OnRecieve_ArrAndDepScoreCount();
        public void Send_GlobalSpeed(ushort rSpeed);
    }
    public interface INetwork_ActionSender_Operator{
        // connection and startup
        public void SendHeartbeat();
        public void Request_StartupInfo();
        // create airplane and other
        public void Create_ArrFlight_OnServer(CallSign rCallSign, ushort rStartIndex);
        public void Create_DepFlight_OnServer(CallSign rCallSign, ParkingStandID rParkingStandID);
        // update reliable data
        public void Send_AP_FlightCommand(ushort rGlobalAirplaneID, ushort rCommandID, ushort[] rParameterArray);
        // update other reliable data
        public void Send_GlobalSpeed(ushort rSpeed);
    }
}