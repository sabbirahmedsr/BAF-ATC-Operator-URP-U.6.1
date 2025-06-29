using AirplaneControllerScript;
using UnityEngine;
namespace ATC.Operator.MapView {
    [System.Serializable]
    public class Map_Node : MonoBehaviour {

        internal AirplaneController apController;

        [SerializeField] private Map_Node_DraggableInfo draggableInfo;
        [SerializeField] private Map_Node_AirplaneIcon airplaneIcon;
        [SerializeField] private Map_Node_Connecting_Line connectingLine;

        internal Map_Node_Controller mapNodeController;

        internal void Initialize(Map_Node_Controller rMapNodeController, AirplaneController rApController) {
            mapNodeController = rMapNodeController;
            apController = rApController;

            draggableInfo.Initialize(this);
            airplaneIcon.Initialize(this);
            connectingLine.Initialize(this);

            // register callback
            apController.apEvent.onUpdate_MapNodePosRot.AddListener(OnUpdate_MapNodePosRot);
            apController.apEvent.onUpdate_VizHeadingSpeedFL.AddListener(OnUpdate_VizHeadingSpeedFL);
            apController.apEvent.onUpdate_AirplanePhaseNT.AddListener(OnUpdate_AirplaneStateNT);
        }



        /* External call back */
        private void OnUpdate_MapNodePosRot(Vector3 rWorldPos, Vector3 rFwd) {
            airplaneIcon.OnUpdate_MapNodePosRot(rWorldPos, rFwd);
        }
        internal void OnUpdate_VizHeadingSpeedFL(VizHeadSpeedFL rVizHeadSpeedFL) {
            draggableInfo.SetVizHeadSpeedHeight(rVizHeadSpeedFL);
        }
        internal void OnUpdate_AirplaneStateNT(NameAndTimeZ rLastNT, NameAndTimeZ rNextNT) {
            draggableInfo.Update_AirplanePhaseNT(rLastNT, rNextNT);
        }
        internal void OnChange_ActiveMapCamera(Camera rActiveMapCamera) {
            airplaneIcon.OnChange_ActiveMapCamera(rActiveMapCamera);
        }



        internal void OnChange_AirplaneIconPosition() {
            draggableInfo.OnAirplanePositionUpdate(airplaneIcon.localPos);
            connectingLine.UpdateConnectingLine();
        }
        internal void OnChange_DraggableInfoPosition() {
            connectingLine.UpdateConnectingLine();
        }


    }
}