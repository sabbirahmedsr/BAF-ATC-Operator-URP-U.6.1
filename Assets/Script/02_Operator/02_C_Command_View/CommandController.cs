
using UnityEngine;

namespace ATC.Operator.CommandView {
    public class CommandController : ATC_Operator_Sub_Controller {
        [Header("Data")]
        [SerializeField] internal ArrivalCommandData arrCommandData;
        [SerializeField] internal DepartureCommandData departureCommandData;
        [SerializeField] internal ParkingPathData parkingPathData;

        [Header("Child Script")]
        [SerializeField] internal Command_Node_Controller commandNodeController;
        [SerializeField] internal FlightCommandPopup flightCommandPopup;



        internal override void Initialize(ATC_Operator_Main_Controller _mainController) {
            base.Initialize(_mainController);

            commandNodeController.Initialize(this);
            flightCommandPopup.Initialize(this);
        }
    }
}