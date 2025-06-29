
using Pathway;
using UnityEngine;
using UnityEngine.Events;

namespace AirplaneControllerScript {
    [System.Serializable]
    public class AP_Events {
        private AirplaneController apController;
        internal void Initialize(AirplaneController rApController) {
            apController = rApController;
        }


        internal UnityEvent<Path[]> onUpdate_PathHistory= new UnityEvent<Path[]> { };
        internal void Update_PathHistory(Path[] rPathHistory) {
            onUpdate_PathHistory.Invoke(rPathHistory);
        }


        internal UnityEvent<VizHeadSpeedFL> onUpdate_VizHeadingSpeedFL = new UnityEvent<VizHeadSpeedFL> { };
        internal void Update_VizHeadingSpeedFL(VizHeadSpeedFL rVizHeadSpeedFL) {
            onUpdate_VizHeadingSpeedFL.Invoke(rVizHeadSpeedFL);
        }


        internal UnityEvent<Vector3, Vector3> onUpdate_MapNodePosRot = new UnityEvent<Vector3, Vector3>();
        internal void Update_MapNodePosRot(Vector3 rPos, Vector3 rFwd) {
            onUpdate_MapNodePosRot.Invoke(rPos, rFwd);
        }


        internal UnityEvent<NameAndTimeZ, NameAndTimeZ> onUpdate_AirplanePhaseNT = new UnityEvent<NameAndTimeZ, NameAndTimeZ> { };
        internal void Update_AirplanePhaseNT(NameAndTimeZ rLastPhase, NameAndTimeZ rNextPhase) {
            onUpdate_AirplanePhaseNT.Invoke(rLastPhase, rNextPhase);
        }

        internal UnityEvent<ArrivalCommandID[]> onUpdate_ArrivalCommandList = new UnityEvent<ArrivalCommandID[]> { };
        internal void Update_ArrivalCommandList(ArrivalCommandID[] rAllArrCmdID) {
            onUpdate_ArrivalCommandList.Invoke(rAllArrCmdID);
        }

        internal UnityEvent<DepartureCommandID[]> onUpdate_DepartureCommandList = new UnityEvent<DepartureCommandID[]> { };
        internal void Update_DepartureCommandList(DepartureCommandID[] rAllDepCmdID) {
            onUpdate_DepartureCommandList.Invoke(rAllDepCmdID);
        }


        internal UnityEvent onRadarMapNodeDestroyEvent = new UnityEvent();
        internal void DestroyRadarMapNode() {
            onRadarMapNodeDestroyEvent.Invoke();
        }

        internal UnityEvent onSurfaceMapNodeDestroyEvent = new UnityEvent();
        internal void DestroySurfaceMapNode() {
            onSurfaceMapNodeDestroyEvent.Invoke();
        }

        internal UnityEvent onAirplaneDestroyEvent = new UnityEvent();
        internal void DestroyThisAirplane() {
            onAirplaneDestroyEvent.Invoke();
        }
    }
}
