using AirplaneControllerScript;
using ATC.Global;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CommandControllerScript.Child {
    public class CC_ArrFlightCommand : MonoBehaviour {

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
        ArrivalCommandID arrCommandID;
        ParkingStandID[] resultParkingStandID = new ParkingStandID[] { };

        internal void Initialize(CommandController rCmdController) {
            cmdController = rCmdController;
            for (int i = 0; i < allDropDownHolder.Length; i++) {
                int index = i;
                allDropDownHolder[i].dropdown.onValueChanged.AddListener(delegate { OnDrdValueChange(index); });
            }
            btnConfirm.onClick.AddListener(ConfirmCommand);
            btnCancel.onClick.AddListener(HideAll);
        }

        internal void Activate(AirplaneController rAPController, ArrivalCommandID rArrCmdID) {
            arrCommandID = rArrCmdID;
            targetApController = rAPController;            
            if (cmdController.arrCommandData.TryGetCommand(rArrCmdID, out ArrivalCommand arrCommand)) {
                txtCallSign.text = targetApController.callSign.ToString();
                txtCommandCaption.text = arrCommand.commandName;
                for (int i = 0; i < allDropDownHolder.Length; i++) {
                    allDropDownHolder[i].ToggleActivation(i < arrCommand.allParameterID.Length);
                    if (i < arrCommand.allParameterID.Length) {
                        allDropDownHolder[i].SetWithParameter(arrCommand.allParameterID[i], rAPController.myData);
                    }
                }
                if (rArrCmdID == ArrivalCommandID.giveTaxiToPark) {
                    foreach (DropdownHolder drdHolder in allDropDownHolder) {
                        if (drdHolder.arrParameterID == ArrParameterID.viaTaxiway) {
                            drdHolder.dropdown.value = (int)ViaTaxiwayID.C;
                        }
                    }
                    RefreshParkingStandDropdown(ViaTaxiwayID.C);
                }
            }
            gameObject.SetActive(true);
        }

        private void HideAll() {
            gameObject.SetActive(false);
        }


        private void OnDrdValueChange(int rDrdIndex) {
            ArrParameterID parameterID = allDropDownHolder[rDrdIndex].arrParameterID;
            ushort value = (ushort)allDropDownHolder[rDrdIndex].dropdown.value;
            if (parameterID == ArrParameterID.viaTaxiway) {
                RefreshParkingStandDropdown((ViaTaxiwayID)value);
            }
        }
        private void RefreshParkingStandDropdown(ViaTaxiwayID rViaTaxiwayID) {
            foreach (DropdownHolder drdHolder in allDropDownHolder) {
                if (drdHolder.arrParameterID == ArrParameterID.parkingStand) {
                    resultParkingStandID = cmdController.parkingPathData.GetArrivalParkingStandList(rViaTaxiwayID);
                    drdHolder.dropdown.ClearOptions();
                    List<string> tmpDropDownOptions = new List<string>();
                    for (int i = 0; i < resultParkingStandID.Length; i++) {
                        tmpDropDownOptions.Add(resultParkingStandID[i].ToString().ToUpper());
                    }
                    drdHolder.dropdown.AddOptions(tmpDropDownOptions);
                }
            }
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
                    if (drdHolder.arrParameterID == ArrParameterID.altitude) {
                        Altitude altitude = (Altitude)drdHolder.dropdown.value;
                        parameterList.Add((ushort)altitude);
                    } else if (drdHolder.arrParameterID == ArrParameterID.qnh) {
                        QNH qnh = (QNH)drdHolder.dropdown.value;
                        parameterList.Add((ushort)qnh);
                    } else if (drdHolder.arrParameterID == ArrParameterID.arrivalApproach) {
                        ArrivalApproach arrApproach = targetApController.myData.allArrivalApproach[drdHolder.dropdown.value];
                        parameterList.Add((ushort)arrApproach);
                    } else if (drdHolder.arrParameterID == ArrParameterID.eatTime) {
                        if (ushort.TryParse(drdHolder.dropdown.captionText.text, out ushort eatTime)) {
                            parameterList.Add(eatTime);
                        } else {
                            parameterList.Add(ushort.Parse(ATCTime.GetClockTimeInHHMM()));
                        }                       
                    } else if (drdHolder.arrParameterID == ArrParameterID.arrivalTaxiway) {
                        ArrTaxiwayID arrTaxiwayID = targetApController.myData.allArrTaxiwayID[drdHolder.dropdown.value];
                        parameterList.Add((ushort)arrTaxiwayID);
                    } else if (drdHolder.arrParameterID == ArrParameterID.viaTaxiway) {
                        ViaTaxiwayID viaTaxiwayID = (ViaTaxiwayID)drdHolder.dropdown.value;
                        parameterList.Add((ushort)viaTaxiwayID);
                    } else if (drdHolder.arrParameterID == ArrParameterID.parkingStand) {
                        ParkingStandID parkingStandID = resultParkingStandID[drdHolder.dropdown.value];
                        parameterList.Add((ushort)parkingStandID);
                    }
                }
            }
            GlobalNetwork.actionSender.Send_AP_FlightCommand(targetApController.globalID, (ushort)arrCommandID, parameterList.ToArray());
        }
    }
}