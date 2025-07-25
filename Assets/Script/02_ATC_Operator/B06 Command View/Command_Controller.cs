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

        [Header("Child Script")]
        [SerializeField] internal CC_ArrFlightCommand arrFlightCommand;
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
            arrFlightCommand.Initialize(this);
            depFlightCommand.Initialize(this);            
          
            // add global listener
            GlobalEvent.onAirplaneCreatedEvent += OnAirplaneCreated;
        }
        private void OnDisable() {
            GlobalEvent.onAirplaneCreatedEvent -= OnAirplaneCreated;
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

    }
}