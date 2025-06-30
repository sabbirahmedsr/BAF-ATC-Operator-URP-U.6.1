using AirplaneControllerScript;
using ATC.Global;
using CommandControllerScript.Child;
using System.Collections.Generic;
using UnityEngine;

namespace CommandControllerScript {
    public class CommandController : MonoBehaviour {
        [Header("Data")]
        [SerializeField] internal ArrivalCommandData arrCommandData;
        [SerializeField] internal DepartureCommandData depCommandData;
        [SerializeField] internal ParkingPathData parkingPathData;
        [SerializeField] internal AirplaneData[] allAirplaneData;

        [Header("Child Script")]
        [SerializeField] internal CC_ArrFlightCreation arrFlightCreation;
        [SerializeField] internal CC_ArrFlightCommand arrFlightCommand;
        [SerializeField] internal CC_DepFlightCreation depFlightCreation;
        [SerializeField] internal CC_DepFlightCommand depFlightCommand;
        [SerializeField] internal CC_ExtraCommandButton extraCmdButton;

        [Header("Command Node")]
        [SerializeField] private GameObject arrCmdNodePrefab;
        [SerializeField] private GameObject depCmdNodePrefab;
        [SerializeField] private Transform tCmdContentRoot;
        private List<CC_ArrCmdNode> allArrCmdNode = new List<CC_ArrCmdNode>();
        private List<CC_DepCmdNode> allDepCmdNode = new List<CC_DepCmdNode>();


        internal void Initialize() {
            // initialize child script
            extraCmdButton.Initialize(this);
            arrFlightCreation.Initialize(this);
            arrFlightCommand.Initialize(this);
            depFlightCreation.Initialize(this);
            depFlightCommand.Initialize(this);
            
            // enable / disable required flight creation
            arrFlightCreation.MakeDisable(GlobalData.operatorType != OperatorType.arrival);
            depFlightCreation.MakeDisable(GlobalData.operatorType != OperatorType.departure);
            
            // add global listener
            GlobalEvent.onAirplaneCreatedEvent += OnAirplaneCreated;
            GlobalNetwork.onConnectionEvent += OnConnectionChange;
        }
        private void OnDisable() {
            GlobalEvent.onAirplaneCreatedEvent -= OnAirplaneCreated;
            GlobalNetwork.onConnectionEvent -= OnConnectionChange;
        }




        private void OnAirplaneCreated(ushort apID, AirplaneController rAPController) {
            if (rAPController.movementType == MovementType.arrival && GlobalData.operatorType == OperatorType.arrival) {
                GameObject newArrCmdNodeObject = Instantiate(arrCmdNodePrefab, tCmdContentRoot);
                CC_ArrCmdNode arrCmdNode = newArrCmdNodeObject.GetComponent<CC_ArrCmdNode>();
                arrCmdNode.Initialize(this, rAPController);
                allArrCmdNode.Add(arrCmdNode);
            }
            if (rAPController.movementType == MovementType.departure && GlobalData.operatorType == OperatorType.departure) {
                GameObject newDepCmdNodeObject = Instantiate(depCmdNodePrefab, tCmdContentRoot);
                CC_DepCmdNode depCmdNode = newDepCmdNodeObject.GetComponent<CC_DepCmdNode>();
                depCmdNode.Initialize(this, rAPController);
                allDepCmdNode.Add(depCmdNode);
            }
        }


        private void OnConnectionChange(ConnectionStatus rStatus, string rMsg, string rReason) {
            if (rStatus != ConnectionStatus.connected) {
                arrFlightCreation.MakeDisable(true);
                depFlightCreation.MakeDisable(true);
            }
        }

    }
}