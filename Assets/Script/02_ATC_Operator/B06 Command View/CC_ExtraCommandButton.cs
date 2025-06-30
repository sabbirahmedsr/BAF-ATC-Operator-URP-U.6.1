using AirplaneControllerScript;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CommandControllerScript.Child {
    public class CC_ExtraCommandButton : MonoBehaviour {
        [SerializeField] private ExtraButton[] allExtraButton;
        private int xtraButtonCount;
        internal bool isVisible;

        private CommandController cmdController;
        private AirplaneController apController;
        internal void Initialize(CommandController rCmdController) {
            cmdController = rCmdController;
            for (int i = 0; i < allExtraButton.Length; i++) {
                int index = i;
                allExtraButton[i].button.onClick.AddListener(delegate { ActivateFlightCommand(index); });
            }
        }





        internal void Activate(AirplaneController rApController, int rSiblingIndex) {
            // if this apController is already active, then hide me
            if (isVisible && rApController == apController) {
                HideMe(); return;
            }
            // set my sibling index, and reset other variable
            int tgtSiblingIndex = rSiblingIndex > transform.GetSiblingIndex() ? rSiblingIndex : rSiblingIndex + 1;
            transform.SetSiblingIndex(tgtSiblingIndex);
            apController = rApController; xtraButtonCount = 0;
            // now refresh extra button variable
            RefreshExtraButtonVariable();
            // all set, now make me visible
            gameObject.SetActive(true);
            isVisible = true;
        }
        internal void RefreshExtraButtonVariable() {
            // crate xtra command buttons
            if (GlobalData.operatorType == OperatorType.arrival) {
                CreateExtraArrivalButton(apController.allArrivalCommandID);
            } else if (GlobalData.operatorType == OperatorType.departure) {
                CreateExtraDepartureButton(apController.allDepartureCommandID);
            }
            // Enable used button & disabled un used button
            for (int i = 0; i < allExtraButton.Length; i++) {
                bool shouldActive = i < xtraButtonCount;
                allExtraButton[i].button.gameObject.SetActive(shouldActive);
            }
        }
        private void CreateExtraArrivalButton(ArrivalCommandID[] rAllArrCmdID) {
            for (int i = 0; i < rAllArrCmdID.Length; i++) {
                if ((byte)rAllArrCmdID[i] <= 99)
                    continue;
                if (cmdController.arrCommandData.TryGetCommand(rAllArrCmdID[i], out ArrivalCommand arrCmd)) {
                    allExtraButton[xtraButtonCount].caption.text = arrCmd.commandName;
                    allExtraButton[xtraButtonCount].arrCmdID = rAllArrCmdID[i];
                    xtraButtonCount += 1;
                }
            }
        }
        private void CreateExtraDepartureButton(DepartureCommandID[] rAllDepCmdID) {
            for (int i = 0; i < rAllDepCmdID.Length; i++) {
                if ((byte)rAllDepCmdID[i] <= 99) 
                    continue;                
                if (cmdController.depCommandData.TryGetCommand(rAllDepCmdID[i], out DepartureCommand depCmd)) {
                    allExtraButton[xtraButtonCount].caption.text = depCmd.commandName;
                    allExtraButton[xtraButtonCount].depCmdID = rAllDepCmdID[i];                    
                    xtraButtonCount += 1;
                }
            }
        }






        internal void ActivateFlightCommand(int rIndex) {
            if (GlobalData.operatorType == OperatorType.arrival) {
                cmdController.arrFlightCommand.Activate(apController, allExtraButton[rIndex].arrCmdID);
            } else if (GlobalData.operatorType == OperatorType.departure) {
                cmdController.depFlightCommand.Activate(apController, allExtraButton[rIndex].depCmdID);
            }
            HideMe();
        }
        internal void HideMe() {
            gameObject.SetActive(false);
            isVisible = false;
        }
    }



    [System.Serializable]
    public struct ExtraButton {
        [SerializeField] internal Button button;
        [SerializeField] internal TMP_Text caption;
        [SerializeField] internal ArrivalCommandID arrCmdID;
        [SerializeField] internal DepartureCommandID depCmdID;
    }
}