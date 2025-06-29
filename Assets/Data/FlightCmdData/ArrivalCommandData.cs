using UnityEngine;

public enum ArrivalCommandID : byte {
    none, playOnAwake, approveInboundApproach, approveVorOutbound, approve12DMEArc, approveR300, approveR355,
    approveLocalizer, approve6DME, approveLanding, giveLandToRwyVacate, giveTaxiToPark, giveTaxiToTermarc,
    // this command is for helicopter and circuite
    approve3Mile, approveTransfer, approveCctState,
    // xtra command
    changeFlightLevel = 100, freezeAircraft = 101, resumeAircraft = 102, destroy = 250
}
public enum ArrParameterID : byte { altitude, eatTime, arrivalTaxiway, viaTaxiway, parkingStand, arrivalApproach, qnh}


[CreateAssetMenu(fileName = "ArrivalCommandData", menuName = "Azmi Studio/ArrivalComandData", order = 5)]
public class ArrivalCommandData : ScriptableObject {
    [SerializeField] internal ArrivalCommand[] allArrivalCommand = new ArrivalCommand[] { };

    internal bool TryGetCommand(ArrivalCommandID rArrCmdId, out ArrivalCommand oArrCmd) {
        for (int i = 0; i < allArrivalCommand.Length; i++) {
            if (allArrivalCommand[i].commandID == rArrCmdId) {
                oArrCmd = allArrivalCommand[i];
                return true;
            }
        }
        oArrCmd = default;
        return false;
    }
}



[System.Serializable]
internal struct ArrivalCommand {
    [SerializeField] internal string commandName;
    [SerializeField] internal ArrivalCommandID commandID;
    [SerializeField] internal ArrParameterID[] allParameterID;
}

