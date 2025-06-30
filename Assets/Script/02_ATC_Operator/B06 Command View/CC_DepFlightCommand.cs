using AirplaneControllerScript;
using ATC.Global;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CommandControllerScript.Child {
    public class CC_DepFlightCommand : MonoBehaviour {

        [Header("Root Variable")]
        [SerializeField] private TMP_Text txtCallSign;
        [SerializeField] private TMP_Text txtCommandCaption;

        [Header("Command Variable")]
        [SerializeField] private DropdownHolder[] allDropDownHolder;

        [Header("Action Button")]
        [SerializeField] private Button btnConfirm;
        [SerializeField] private Button btnCancel;


        CommandController cmdController;
        AirplaneController targetApController;
        DepartureCommandID depCommandID;
        ViaTaxiwayID[] resultViaTaxiwayID = new ViaTaxiwayID[] { };

        internal void Initialize(CommandController rCMDController) {
            cmdController = rCMDController;
            for (int i = 0; i < allDropDownHolder.Length; i++) {
                int index = i;
                allDropDownHolder[i].dropdown.onValueChanged.AddListener(delegate { OnDrdValueChange(index); });
            }
            btnConfirm.onClick.AddListener(ConfirmCommand);
            btnCancel.onClick.AddListener(HideAll);
        }

        internal void Activate(AirplaneController rAPController, DepartureCommandID rDepCmdID) {
            depCommandID = rDepCmdID;
            targetApController = rAPController;
            if (cmdController.depCommandData.TryGetCommand(rDepCmdID, out DepartureCommand depCommand)) {
                txtCallSign.text = targetApController.callSign.ToString();
                txtCommandCaption.text = depCommand.commandName;
                for (int i = 0; i < allDropDownHolder.Length; i++) {
                    allDropDownHolder[i].ToggleActivation(i < depCommand.allParameterID.Length);
                    if (i < depCommand.allParameterID.Length) {
                        allDropDownHolder[i].SetWithParameter(depCommand.allParameterID[i]);
                    }
                }
            } 
            gameObject.SetActive(true);
        }

        private void HideAll() {
            gameObject.SetActive(false);
        }


        private void OnDrdValueChange(int rDrdIndex) {
            //  DepParameterID parameterID = allDropDownHolder[rDrdIndex].depParameterID;
            // ushort value = (ushort)allDropDownHolder[rDrdIndex].dropdown.value;
        }



        private void ConfirmCommand() {
            SendCommandToTargetAirplane();
            HideAll();
        }
        private void SendCommandToTargetAirplane() {
            if (targetApController == null)
                return;
            List<ushort> parameterList = new List<ushort>();
            foreach (DropdownHolder drdHolder in allDropDownHolder) {
                if (drdHolder.dropdown.gameObject.activeSelf) {
                    if (drdHolder.depParameterID == DepParameterID.pushbackFacingNS) {
                        PushbackFacing pbFacing = (PushbackFacing)drdHolder.dropdown.value;
                        parameterList.Add((ushort)pbFacing);
                    } else if (drdHolder.depParameterID == DepParameterID.viaTaxiway) {
                        ViaTaxiwayID viaTaxiwayID = (ViaTaxiwayID)drdHolder.dropdown.value;
                        parameterList.Add((ushort)viaTaxiwayID);
                    } else if (drdHolder.depParameterID == DepParameterID.altitude) {
                        Altitude altitude = (Altitude)drdHolder.dropdown.value;
                        parameterList.Add((ushort)altitude);
                    } else if (drdHolder.depParameterID == DepParameterID.qnh) {
                        QNH qnh = (QNH)drdHolder.dropdown.value;
                        parameterList.Add((ushort)qnh);
                    } else if (drdHolder.depParameterID == DepParameterID.time) {
                        if (ushort.TryParse(drdHolder.dropdown.captionText.text, out ushort time)) {
                            parameterList.Add(time);
                        } else {
                            parameterList.Add(ushort.Parse(ATCTime.GetClockTimeInHHMM()));
                        }
                    }
                }
            }
            GlobalNetwork.actionSender.Send_AP_FlightCommand(targetApController.globalID, (ushort)depCommandID, parameterList.ToArray());
        }
    }
}