using AirplaneControllerScript;
using ATC.Global;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CommandControllerScript.Child { 
    public class CC_ArrCmdNode : MonoBehaviour {
        [Header("UI Refernce")]
        [SerializeField] private TMP_Text txtCallSign;
        [SerializeField] private Button btnMainAction;
        [SerializeField] private TMP_Text txtMainAction;
        [SerializeField] private Button btnMoreAction;
        private ArrivalCommandID mainArrCmdID = ArrivalCommandID.none;

        private CommandController cmdController;
        private AirplaneController apController;


        internal void Initialize(CommandController rCmdController, AirplaneController rAP__Controller) {
            cmdController = rCmdController;
            apController = rAP__Controller;
            txtCallSign.text = apController.myData.callSign.ToString();
            apController.apEvent.onAirplaneDestroyEvent.AddListener(DestroyMe);
            apController.apEvent.onUpdate_ArrivalCommandList.AddListener(OnUpdateArrivalCommandList);
            btnMainAction.onClick.AddListener(ActivateMainAction);
            btnMoreAction.onClick.AddListener(ShowMoreAction);
            OnUpdateArrivalCommandList(apController.allArrivalCommandID);
        }
        
        void OnUpdateArrivalCommandList(ArrivalCommandID[] rAllArrCmdID) {
            // at first reset the main button value
            mainArrCmdID = ArrivalCommandID.none;
            txtMainAction.text = "No Pending CMD";
            txtMainAction.color = Color.gray;
            btnMainAction.interactable = false;
            // now check if there is any main action
            for (int i = 0; i < rAllArrCmdID.Length; i++) {
                if ((ushort)rAllArrCmdID[i] <= 99) {
                    mainArrCmdID = rAllArrCmdID[i];
                    break;
                }
            }
            // if we have main action, update the required button
            if (mainArrCmdID != ArrivalCommandID.none) {
                if(cmdController.arrCommandData.TryGetCommand(mainArrCmdID, out ArrivalCommand arrCmd)){
                    txtMainAction.text = arrCmd.commandName;
                    txtMainAction.color = Color.green;
                    btnMainAction.interactable = true;
                }
            }
            // hide the extra buttons
            cmdController.extraCmdButton.HideMe();
        }


        void ActivateMainAction() {
            if (mainArrCmdID != ArrivalCommandID.none) {
                if(cmdController.arrCommandData.TryGetCommand(mainArrCmdID, out ArrivalCommand arrCommand)) {
                    if(arrCommand.allParameterID.Length > 0) {
                        cmdController.arrFlightCommand.Activate(apController, mainArrCmdID);
                    } else {
                        GlobalNetwork.actionSender.Send_AP_FlightCommand(apController.globalID, (ushort)mainArrCmdID, new ushort[] { });
                    }
                }                
            }
        }
        void ShowMoreAction() {
            bool hasMoreCmdId = false;
            for (int i = 0; i < apController.allArrivalCommandID.Length; i++) {
                if((ushort)apController.allArrivalCommandID[i] >= 99) {
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