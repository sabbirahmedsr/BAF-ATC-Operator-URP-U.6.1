using ATC.Operator.MapView;
using InfoViewScript;
using StripViewScript;
using UnityEngine;
using CommandControllerScript;
using ATC.Operator.Networking;
using ATC.Global;

namespace ATCManagerScript {

    public class ATCManager : MonoBehaviour {
        public OperatorType oepratorType = OperatorType.departure;
        [Space]
        [SerializeField] private Map_Controller radarMapController;
        [SerializeField] private Map_Controller surfaceMapController;
        [SerializeField] private StripController stripController;
        [SerializeField] private InfoController infoController;
        [SerializeField] private CommandController commandController;
        [SerializeField] private NetworkManager_ForClient networkManager;

        private void Awake() {
#if UNITY_EDITOR
            GlobalData.operatorType = oepratorType;
#endif
        }

        void Start() {
            GlobalEvent.ClearAllListeners();
            GlobalNetwork.ClearAllListeners();
            //
            radarMapController.Initialize();
            surfaceMapController.Initialize();
            stripController.Initialize();
            infoController.Initialize();
            commandController.Initialize();
            //
            networkManager.Initialize();
        }

    }

}