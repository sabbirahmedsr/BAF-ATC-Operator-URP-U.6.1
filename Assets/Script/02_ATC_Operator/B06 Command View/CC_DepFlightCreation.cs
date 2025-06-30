using ATC.Global;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.TMP_Dropdown;

namespace CommandControllerScript.Child {
    /// <summery>
    /// Departure Command Controller, Create Departure Aircraft/Flight
    /// </summery>
    [System.Serializable]   
    public struct CC_DepFlightCreation {
        [SerializeField] private Button btnCreateDepAircraft;

        [Header("Create Popup UI")]
        [SerializeField] private Transform tFlightCreateionRoot;
        [SerializeField] private TMP_Dropdown drdAircraftCategory;
        [SerializeField] private TMP_Dropdown drdAircraftCallSign;
        [SerializeField] private TMP_Dropdown drdParkingStand;
        [SerializeField] private Button btnCreateConfirm;
        [SerializeField] private Button btnCreateCancel;
        private List<TypeOfAircraft> result_category;
        private List<AirplaneData> result_apData;
        private List<OptionData> tmpDrdOptionData;

        private  CommandController cmdController;
        internal void Initialize(CommandController rCmdController) {
            cmdController = rCmdController;
            result_apData = new List<AirplaneData>();
            result_category = new List<TypeOfAircraft>();
            tmpDrdOptionData = new List<OptionData>();
            btnCreateDepAircraft.onClick.AddListener(Activate);
            btnCreateConfirm.onClick.AddListener(CreateConfirm);
            btnCreateCancel.onClick.AddListener(CreateCancel);
            drdAircraftCategory.onValueChanged.AddListener(OnCategoryChange);
            drdAircraftCallSign.onValueChanged.AddListener(OnCallSignChange);
            drdParkingStand.onValueChanged.AddListener(OnParkingStandChange);
            SetDrdOptions_Aircraft_Category();
            OnCategoryChange(0);
        }
        internal void Activate() {
            tFlightCreateionRoot.gameObject.SetActive(true);
        }
        internal void MakeDisable(bool rBool) {
            btnCreateDepAircraft.gameObject.SetActive(!rBool);
        }



        private void OnCategoryChange(int rIndex) {
            SetDrdOptions_Aircraft_CallSign();
            OnCallSignChange(0);
        }
        private void OnCallSignChange(int rIndex) {
            if (cmdController.allAirplaneData[rIndex].aircraftType == TypeOfAircraft.Military) {
                drdParkingStand.ClearOptions();
            } else {
                SetDrdOptions_ParkingStand();
            }
        }
        private void OnParkingStandChange(int rIndex) {
            // we need to do nothing here, the drop down value will be sent over network if needed
        }




        /// <summary>
        /// This will create aircraft category [international, domestic, militery] dynamically, to be used as drop down option
        /// </summary>
        private void SetDrdOptions_Aircraft_Category() {
            TypeOfAircraft[] allCategory = (TypeOfAircraft[])Enum.GetValues(typeof(TypeOfAircraft));
            tmpDrdOptionData = new List<OptionData>();
            result_category.Clear();
            // Note : we have to skip the first type as it is set as none [thats why, i = 1]
            // Note 2 : We will also skip the last catergory [other], beacure, there is no departure system available right now
            for (int i = 1; i < allCategory.Length - 1; i++) {
                result_category.Add(allCategory[i]);
                tmpDrdOptionData.Add(new OptionData(allCategory[i].ToString()));
            }
            drdAircraftCategory.ClearOptions();
            drdAircraftCategory.options = tmpDrdOptionData;
        }
        /// <summary>
        /// this will create a list of airplane call sign based on selected airplane category, to be used as drop down option
        /// </summary>
        private void SetDrdOptions_Aircraft_CallSign() {
            result_apData.Clear();
            tmpDrdOptionData = new List<OptionData>();
            TypeOfAircraft selectedCategory = result_category[drdAircraftCategory.value];
            for (int i = 0; i < cmdController.allAirplaneData.Length; i++) {
                if (cmdController.allAirplaneData[i].aircraftType == selectedCategory) {
                    result_apData.Add(cmdController.allAirplaneData[i]);
                    tmpDrdOptionData.Add(new OptionData(cmdController.allAirplaneData[i].callSign.Caption()));
                }
            }
            drdAircraftCallSign.ClearOptions();
            drdAircraftCallSign.options = tmpDrdOptionData;
        }
        /// <summary>
        /// this function will create parking stand id drop down list based on selected aircraft call sign
        /// </summary>
        private void SetDrdOptions_ParkingStand() {
            AirplaneData apData = result_apData[drdAircraftCallSign.value];
            tmpDrdOptionData = new List<OptionData>();
            for (int i = 0; i < apData.allParkingStandID.Length; i++) {
                tmpDrdOptionData.Add(new OptionData(apData.allParkingStandID[i].Caption()));
            }
            drdParkingStand.ClearOptions();
            drdParkingStand.options = tmpDrdOptionData;
        }






        private void CreateConfirm() {
            CallSign selectedCallSign = result_apData[drdAircraftCallSign.value].callSign;
            Span<ParkingStandID> allPkgStandID = result_apData[drdAircraftCallSign.value].allParkingStandID;
            ParkingStandID selectedParkingStandID = allPkgStandID.Length > 0 ? allPkgStandID[drdParkingStand.value] : ParkingStandID._05;
            GlobalNetwork.actionSender.Create_DepFlight_OnServer(selectedCallSign, selectedParkingStandID);
            tFlightCreateionRoot.gameObject.SetActive(false);
        }
        private void CreateCancel() {
            tFlightCreateionRoot.gameObject.SetActive(false);
        }
    }
}