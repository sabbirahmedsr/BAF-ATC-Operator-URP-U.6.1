using UnityEngine;



public enum DepartureCommandID : byte {
    none, playOnAwake, reqAirfieidInfo, giveAirfieldInfo, reqDepClearance, giveDepClearance,
    reqStartup, approveStartup, reqPushback, approvePushback, reqStartAndPush, approveStartAndPush,
    reqTaxi, approveTaxi, approveMilTaxi, giveAtcClearance, reqLineUp, aproveLineUp,
    confirmDeparture, approveAirborne, approveTransfer, approveCctState,
    // xtra command
    freezeAircraft = 101, resumeAircraft = 102, destroy = 250
}
public enum DepParameterID : byte { pushbackFacingNS, viaTaxiway, altitude, time, qnh}


[CreateAssetMenu(fileName = "DepartureCommandData", menuName = "Azmi Studio/DepartureCommandData", order = 6)]
public class DepartureCommandData : ScriptableObject {
    [SerializeField] internal DepartureCommand[] allDepartureCommand = new DepartureCommand[] { };

    internal bool TryGetCommand(DepartureCommandID rDepCmdId, out DepartureCommand oDepCmd) {
        for (int i = 0; i < allDepartureCommand.Length; i++) {
            if (allDepartureCommand[i].commandID == rDepCmdId) {
                oDepCmd = allDepartureCommand[i];
                return true;
            }
        }
        oDepCmd = default;
        return false;
    }
}

[System.Serializable]
internal struct DepartureCommand {
    [SerializeField] internal string commandName;
    [SerializeField] internal DepartureCommandID commandID;
    [SerializeField] internal DepParameterID[] allParameterID;
}
