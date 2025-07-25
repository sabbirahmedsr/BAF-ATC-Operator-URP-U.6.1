using ATC.Operator.MapView;
using InfoViewScript;
using StripViewScript;
using UnityEngine;
using CommandControllerScript;
using ATC.Operator.Networking;
using ATC.Global;
using ATC.Operator.FlightCreation;

namespace ATCManagerScript {

    public class ATCManager : MonoBehaviour {
        public OperatorType oepratorType = OperatorType.departure;
        [Space]
        [SerializeField] private Map_Controller radarMapController;
        [SerializeField] private Map_Controller surfaceMapController;
        [SerializeField] private StripController stripController;
        [SerializeField] private InfoController infoController;
        [SerializeField] private CommandController commandController;
        [SerializeField] private FlightCreation_Controller flightCreationController;
        [SerializeField] private NetworkManager_ForClient networkManager;

        [Header("Reference")]
        [SerializeField] private AirplaneData[] allAirplaneData;

        private void Awake() {
#if UNITY_EDITOR
            GlobalData.operatorType = oepratorType;
#endif
            GlobalData.allAirplaneData = allAirplaneData;
        }

        void Start() {
            //
            radarMapController.Initialize();
            surfaceMapController.Initialize();
            stripController.Initialize();
            infoController.Initialize();
            commandController.Initialize();
            flightCreationController.Initialize();
            //
            networkManager.Initialize();
        }

        private void OnDisable() {
            GlobalData.CrealAllListener();
            GlobalEvent.ClearAllListeners();
            GlobalNetwork.ClearAllListeners();
        }

    }

}