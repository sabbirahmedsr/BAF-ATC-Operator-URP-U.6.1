using ATC.Global;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.TMP_Dropdown;

namespace CommandControllerScript.Child {
    /// <summery>
    /// Arrival Command Controller Create Arrival Aircraft/Flight
    /// </summery>
    [System.Serializable]   
    public struct CC_ArrFlightCreation {
        [SerializeField] private Button btnCreateArrAircraft;

        [Header("Create Popup UI")]
        [SerializeField] private Transform tFlightCreateionRoot;
        [SerializeField] private TMP_Dropdown drdAircraftCategory;
        [SerializeField] private TMP_Dropdown drdAircraftCallSign;
        [SerializeField] private TMP_Dropdown drdReportingPoint;
        [SerializeField] private Button btnCreateConfirm;
        [SerializeField] private Button btnCreateCancel;
        private List<TypeOfAircraft> result_category;
        private List<AirplaneData> result_apData;
        private List<OptionData> tmpDrdOptionData;

        private CommandController cmdController;
        internal void Initialize(CommandController rCommandController) {
            cmdController = rCommandController;
            result_apData = new List<AirplaneData>();
            result_category = new List<TypeOfAircraft>();
            tmpDrdOptionData = new List<OptionData>();
            btnCreateArrAircraft.onClick.AddListener(Activate);
            btnCreateConfirm.onClick.AddListener(CreateConfirm);
            btnCreateCancel.onClick.AddListener(CreateCancel);
            drdAircraftCategory.onValueChanged.AddListener(OnCategoryChange);
            drdAircraftCallSign.onValueChanged.AddListener(OnCallSignChange);
            drdReportingPoint.onValueChanged.AddListener(OnReportingPointChange);
            SetDrdOptions_Aircraft_Category();
            OnCategoryChange(0);
        }
        internal void Activate() {
            tFlightCreateionRoot.gameObject.SetActive(true);
        }
        internal void MakeDisable(bool rBool) {
            btnCreateArrAircraft.gameObject.SetActive(!rBool);
        }



        private void OnCategoryChange(int rIndex) {
            SetDrdOptions_Aircraft_CallSign();
            OnCallSignChange(0);
        }
        private void OnCallSignChange(int rIndex) {
            SetDrdOptions_ReportingPoint();
        }
        private void OnReportingPointChange(int rIndex) {
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
            for (int i = 1; i < allCategory.Length; i++) {
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
        /// this function will create reporting point drop down list based on selected aircraft call sign
        /// </summary>
        private void SetDrdOptions_ReportingPoint() {
            AirplaneData apData = result_apData[drdAircraftCallSign.value];
            tmpDrdOptionData = new List<OptionData>();
            for (int i = 0; i < apData.allPreArrivalPoints.Length; i++) {
                tmpDrdOptionData.Add(new OptionData(apData.allPreArrivalPoints[i]));
            }
            drdReportingPoint.ClearOptions();
            drdReportingPoint.options = tmpDrdOptionData;
            drdReportingPoint.value = drdReportingPoint.options.Count - 1;
        }






        private void CreateConfirm() {
            CallSign selectedCallSign = result_apData[drdAircraftCallSign.value].callSign;
            GlobalNetwork.actionSender.Create_ArrFlight_OnServer(selectedCallSign, (ushort)drdReportingPoint.value);
            tFlightCreateionRoot.gameObject.SetActive(false);
        }
        private void CreateCancel() {
            tFlightCreateionRoot.gameObject.SetActive(false);
        }
    }
}