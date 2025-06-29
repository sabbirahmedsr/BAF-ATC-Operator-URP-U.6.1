using Pathway;
using Riptide;
using UnityEngine;

namespace ATC.Operator.Networking {
    public class NetworkMessageConverter {
        private ushort globalAirplaneID = 0;
        private NetworkAirplane_SpawnInfo tmpSpawnInfo = new NetworkAirplane_SpawnInfo();
        private NameAndTimeZ tmpLastNT = new NameAndTimeZ();
        private NameAndTimeZ tmpNextNT = new NameAndTimeZ();
        private VizHeadSpeedFL tmpVizHSFL = new VizHeadSpeedFL();
        private ushort tmpStartLineIndex = 0;
        private Path[] tmpPathHistory;
        private ArrivalCommandID[] tmpAllArrivalCommandID;
        private DepartureCommandID[] tmpAllDepartureCommandID;

        private Network_ActionReceiver actionReceiver;

        internal void Initialize(Network_ActionReceiver rActionReciever) {
            actionReceiver = rActionReciever;
        }

        internal void ConvertMessageToAction(Message msg) {
            MSG_Initial msgInitial = (MSG_Initial)msg.GetUShort();
            switch (msgInitial) {
                case MSG_Initial.startupInfo_result: {
                    ushort mapID = msg.GetUShort();
                    ushort arrayLength = msg.GetUShort();
                    for (int i = 0; i < arrayLength; i++) {
                        globalAirplaneID = msg.GetUShort();
                        tmpSpawnInfo.GetFromMessage(msg);
                        actionReceiver.OnRecieved_StartupInfo_Result(globalAirplaneID, tmpSpawnInfo);
                    }
                }
                break;





                case MSG_Initial.create_ArrFlight_OnOperator: {
                    globalAirplaneID = msg.GetUShort();
                    tmpSpawnInfo.GetFromMessage(msg);
                    actionReceiver.OnCreate_Airplane_OnOperator(globalAirplaneID, tmpSpawnInfo);
                }
                break;
                case MSG_Initial.create_RadarMapNode: {
                    globalAirplaneID = msg.GetUShort();
                    tmpStartLineIndex = msg.GetUShort();
                    actionReceiver.OnCreate_RadarMapNode(globalAirplaneID, tmpStartLineIndex);
                }
                break;
                case MSG_Initial.create_SurfaceMapNode: {
                    globalAirplaneID = msg.GetUShort();
                    tmpStartLineIndex = msg.GetUShort();
                    actionReceiver.OnCreate_SurfaceMapNode(globalAirplaneID, tmpStartLineIndex);
                }
                break;






                case MSG_Initial.update_all_PosAndRot: {
                    ushort arrayLength = msg.GetUShort();
                    for (int i = 0; i < arrayLength; i++) {
                        ushort globalAirplaneID = msg.GetUShort();
                        Vector3 pos = msg.GetVector3();
                        Vector3 fwd = msg.GetVector3();
                        actionReceiver.OnUpdate_All_PosAndRot(globalAirplaneID, pos, fwd);
                    }
                }
                break;
                case MSG_Initial.update_TimeAndVisualInfo: {
                    ATCTime.SetATCTime(msg.GetInt(), msg.GetInt());
                    ushort arrayLength = msg.GetUShort();
                    for (int i = 0; i < arrayLength; i++) {
                        ushort globalAirplaneID = msg.GetUShort();
                        tmpVizHSFL.GetFromMessage(msg);
                        actionReceiver.OnUpdate_TimeAndVisualInfo(globalAirplaneID, tmpVizHSFL);
                    }
                }
                break;






                case MSG_Initial.update_AP_AirplanePhaseNT: {
                    globalAirplaneID = msg.GetUShort();
                    tmpLastNT.GetFromMessage(msg);
                    tmpNextNT.GetFromMessage(msg);
                    actionReceiver.OnRecieve_AP_AirplanePhaseNT(globalAirplaneID, tmpLastNT, tmpNextNT);
                }
                break;
                case MSG_Initial.update_AP_PathHisotry: {
                    globalAirplaneID = msg.GetUShort();
                    ushort arrayLength = msg.GetUShort();
                    tmpPathHistory = new Path[arrayLength];
                    for (int i = 0; i < tmpPathHistory.Length; i++) {
                        tmpPathHistory[i].GetFromMessage(msg);
                    }
                    actionReceiver.OnRecieve_AP_PathHistory(globalAirplaneID, tmpPathHistory);
                }
                break;
                case MSG_Initial.update_AP_ArrivalCommandList: {
                    globalAirplaneID = msg.GetUShort();
                    ushort arrayLength = msg.GetUShort();
                    tmpAllArrivalCommandID = new ArrivalCommandID[arrayLength];
                    for (int i = 0; i < tmpAllArrivalCommandID.Length; i++) {
                        tmpAllArrivalCommandID[i] = (ArrivalCommandID)msg.GetUShort();
                    }
                    actionReceiver.OnRecieve_AP_ArrivalCommandList(globalAirplaneID, tmpAllArrivalCommandID);
                }
                break;
                case MSG_Initial.update_AP_DepartureCommandList: {
                    globalAirplaneID = msg.GetUShort();
                    ushort arrayLength = msg.GetUShort();
                    tmpAllDepartureCommandID = new DepartureCommandID[arrayLength];
                    for (int i = 0; i < tmpAllDepartureCommandID.Length; i++) {
                        tmpAllDepartureCommandID[i] = (DepartureCommandID)msg.GetUShort();
                    }
                    actionReceiver.OnRecieve_AP_DepartureCommandList(globalAirplaneID, tmpAllDepartureCommandID);
                }
                break;





                case MSG_Initial.destroy_AP_RadarMapNode: {
                    globalAirplaneID = msg.GetUShort();
                    actionReceiver.OnDestroy_AP_RadarMapNode(globalAirplaneID);
                }
                break;
                case MSG_Initial.destroy_AP_SurfaceMapNode: {
                    globalAirplaneID = msg.GetUShort();
                    actionReceiver.OnDestroy_AP_SurfaceMapNode(globalAirplaneID);
                }
                break;
                case MSG_Initial.destroy_AP_Airplane_OnOperator: {
                    globalAirplaneID = msg.GetUShort();
                    actionReceiver.OnDestroy_AP_Airplane_OnOperator(globalAirplaneID);
                }
                break;




                case MSG_Initial.update_ArrAndDepScoreCount: {
                    GlobalData.arrScoreCount.GetFromMessage(msg);
                    GlobalData.depScoreCount.GetFromMessage(msg);
                    actionReceiver.OnRecieve_ArrAndDepScoreCount();
                }
                break;

            }

            //  Debug.Log("MSG :: " +  msgInitial);
            msg.Release();
        }

    }
}