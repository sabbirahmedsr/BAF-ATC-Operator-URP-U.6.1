using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[System.Serializable]
internal class DropdownHolder {
    [SerializeField] internal ArrParameterID arrParameterID;
    [SerializeField] internal DepParameterID depParameterID;
    [SerializeField] internal TMP_Text txtCaption;
    [SerializeField] internal TMP_Dropdown dropdown;
    internal void ToggleActivation(bool activate) {
        txtCaption.gameObject.SetActive(activate);
        dropdown.gameObject.SetActive(activate);
    }

    internal void SetWithParameter(ArrParameterID rParameterID, AirplaneData apData) {
        arrParameterID = rParameterID;
        string tmpCaption = "";
        List<string> tmpDropdownOption = new List<string>();
        //
        if (arrParameterID == ArrParameterID.altitude) {
            tmpCaption = "Altitude";
            tmpDropdownOption = System.Enum.GetNames(typeof(Altitude)).ToList();
        } else if (arrParameterID == ArrParameterID.qnh) {
            tmpCaption = "QNH";
            tmpDropdownOption = System.Enum.GetNames(typeof(QNH)).ToList();
        } else if (arrParameterID == ArrParameterID.arrivalApproach) {
            tmpCaption = "Arrival Approach";
            for (int i = 0; i < apData.allArrivalApproach.Length; i++) {
                tmpDropdownOption.Add(apData.allArrivalApproach[i].ToString());
            }
        } else if (arrParameterID == ArrParameterID.eatTime) {
            tmpCaption = "EAT";
            for (int i = 0; i < 20; i++) {
                tmpDropdownOption.Add(ATCTime.GetFutureClockTimeInHHMM(i * 60));
            }
            tmpDropdownOption[0] = "No Delay";
        } else if (arrParameterID == ArrParameterID.arrivalTaxiway) {
            tmpCaption = "Arrival Taxiway";        
            for (int i = 0; i < apData.allArrTaxiwayID.Length; i++){
                tmpDropdownOption.Add(apData.allArrTaxiwayID[i].ToString());
            }
        } else if (arrParameterID == ArrParameterID.viaTaxiway) {
            tmpCaption = "Via Taxiway";
            tmpDropdownOption = System.Enum.GetNames(typeof(ViaTaxiwayID)).ToList();
        } else if (arrParameterID == ArrParameterID.parkingStand) {
            tmpCaption = "Parking Stand";
            tmpDropdownOption.Add("None");
        }
        //
        txtCaption.text = tmpCaption;
        dropdown.ClearOptions();
        dropdown.AddOptions(tmpDropdownOption);
    }



    internal void SetWithParameter(DepParameterID rParameterID) {
        depParameterID = rParameterID;
        string tmpCaption = "";
        List<string> tmpDropdownOption = new List<string>();
        //
        if (depParameterID == DepParameterID.altitude) {
            tmpCaption = "Altitude";
            tmpDropdownOption = System.Enum.GetNames(typeof(Altitude)).ToList();
        } else if (depParameterID == DepParameterID.qnh) {
            tmpCaption = "QNH";
            tmpDropdownOption = System.Enum.GetNames(typeof(QNH)).ToList();
        } else if (depParameterID == DepParameterID.time) {
            tmpCaption = "Time";
            for (int i = 0; i < 20; i++) {
                tmpDropdownOption.Add(ATCTime.GetFutureClockTimeInHHMM((-2 + i) * 60));
            }
        } else if (depParameterID == DepParameterID.pushbackFacingNS) {
            tmpCaption = "Pushback Facing";
            string[] allOptions = new string[2] { "North", "South" };
            tmpDropdownOption = allOptions.ToList();
        } else if (depParameterID == DepParameterID.viaTaxiway) {
            tmpCaption = "Via Taxiway";
            tmpDropdownOption = System.Enum.GetNames(typeof(ViaTaxiwayID)).ToList();
        }
        //
        txtCaption.text = tmpCaption;
        dropdown.ClearOptions();
        dropdown.AddOptions(tmpDropdownOption);
    }
}