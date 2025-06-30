using AirplaneControllerScript;
using ATC.Global;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CommandControllerScript.Child { 
    public class CC_DepCmdNode : MonoBehaviour {
        [Header("UI Refernce")]
        [SerializeField] private TMP_Text txtCallSign;
        [SerializeField] private Button btnMainAction;
        [SerializeField] private TMP_Text txtMainAction;
        [SerializeField] private Button btnMoreAction;
        private DepartureCommandID mainDepCmdID = DepartureCommandID.none;

        private CommandController cmdController;
        private AirplaneController apController;


        internal void Initialize(CommandController rCmdController, AirplaneController rAP__Controller) {
            cmdController = rCmdController;
            apController = rAP__Controller;
            txtCallSign.text = apController.myData.callSign.ToString();
            apController.apEvent.onAirplaneDestroyEvent.AddListener(DestroyMe);
            apController.apEvent.onUpdate_DepartureCommandList.AddListener(OnUpdateDepartureCommandList);
            btnMainAction.onClick.AddListener(ActivateMainAction);
            btnMoreAction.onClick.AddListener(ShowMoreAction);
            OnUpdateDepartureCommandList(apController.allDepartureCommandID);
            enabled = true;
        }
        
        void OnUpdateDepartureCommandList(DepartureCommandID[] rAllDepCmdID) {
            // at first reset the main button value
            mainDepCmdID = DepartureCommandID.none;
            txtMainAction.text = "No Pending CMD";
            txtMainAction.color = Color.gray;
            btnMainAction.interactable = false;
            // now check if there is any main action
            for (int i = 0; i < rAllDepCmdID.Length; i++) {
                if ((ushort)rAllDepCmdID[i] <= 99) {
                    mainDepCmdID = rAllDepCmdID[i];
                    break;
                }
            }
            // if we have main action, update the required button
            if (mainDepCmdID != DepartureCommandID.none) {
                if(cmdController.depCommandData.TryGetCommand(mainDepCmdID, out DepartureCommand depCmd)){
                    txtMainAction.text = depCmd.commandName;
                    txtMainAction.color = Color.green;
                    btnMainAction.interactable = true;
                }
            }
            // hide the extra buttons
            cmdController.extraCmdButton.HideMe();
        }


        void ActivateMainAction() {
            if (mainDepCmdID != DepartureCommandID.none) {
                if (cmdController.depCommandData.TryGetCommand(mainDepCmdID, out DepartureCommand depCommand)) {
                    if (depCommand.allParameterID.Length > 0) {
                        cmdController.depFlightCommand.Activate(apController, mainDepCmdID);
                    } else {
                        GlobalNetwork.actionSender.Send_AP_FlightCommand(apController.globalID, (ushort)mainDepCmdID, new ushort[] { });
                    }
                }                
            }
        }
        void ShowMoreAction() {
            bool hasMoreCmdId = false;
            for (int i = 0; i < apController.allDepartureCommandID.Length; i++) {
                if((ushort)apController.allDepartureCommandID[i] >= 99) {
                    hasMoreCmdId = true;
                }
            }
            if (hasMoreCmdId) {
                cmdController.extraCmdButton.Activate(apController, transform.GetSiblingIndex());
            }
        }


        internal void DestroyMe() {
            Destroy(gameObject);
        }
    }
}