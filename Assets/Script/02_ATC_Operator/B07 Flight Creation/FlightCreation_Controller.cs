
using ATC.Global;
using ATC.Operator.FlightCreation.Child;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ATC.Operator.FlightCreation {
    public class FlightCreation_Controller : MonoBehaviour {
        [SerializeField] private Button btnCreateArrAircraft;
        [SerializeField] private Button btnCreateDepAircraft;
        [Space]
        [SerializeField] internal FC_ArrFlightCreation arrFlightCreation;
        [SerializeField] internal FC_DepFlightCreation depFlightCreation;


        internal void Initialize() {
            // initialize child script
            arrFlightCreation.Initialize(this);
            depFlightCreation.Initialize(this);

            btnCreateArrAircraft.onClick.AddListener(arrFlightCreation.Activate);
            btnCreateDepAircraft.onClick.AddListener(depFlightCreation.Activate);

            // enable / disable required flight creation
            btnCreateArrAircraft.gameObject.SetActive(GlobalData.operatorType == OperatorType.arrival);
            btnCreateDepAircraft.gameObject.SetActive(GlobalData.operatorType == OperatorType.departure);
            
            // add global listener
            GlobalNetwork.onConnectionEvent += OnConnectionChange;
        }
        private void OnDisable() {
            GlobalNetwork.onConnectionEvent -= OnConnectionChange;
        }


        private void OnConnectionChange(ConnectionStatus rStatus, string rMsg, string rReason) {
            if (rStatus != ConnectionStatus.connected) {
                btnCreateArrAircraft.gameObject.SetActive(false);
                btnCreateDepAircraft.gameObject.SetActive(false);
            }
        }

    }
}