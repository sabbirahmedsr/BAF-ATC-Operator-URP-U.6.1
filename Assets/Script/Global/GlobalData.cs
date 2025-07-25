using AirplaneControllerScript;
using Riptide;
using System.Collections.Generic;
public enum OperatorType { arrival, departure}

public static class GlobalData {
    internal static OperatorType operatorType = OperatorType.departure;

    internal static Dictionary<ushort, AirplaneController> allActiveAirplane = new Dictionary<ushort, AirplaneController>();
    internal static AirplaneData[] allAirplaneData;

    // scoring
    internal static ScoreCount arrScoreCount = new ScoreCount();
    internal static ScoreCount depScoreCount = new ScoreCount();

    internal static void CrealAllListener() {
        allActiveAirplane.Clear();
        allAirplaneData = new AirplaneData[] { };
        arrScoreCount = new ScoreCount();
        depScoreCount = new ScoreCount();
    }
}

[System.Serializable]
public struct ScoreCount {
    internal ushort activeAircraftCount;
    internal ushort completedAircraftCount;
    internal ushort failedAircraftCount;
    internal ushort totalAircraftCount;

    internal void AddToMessage(Message msg) {
        msg.AddUShort(activeAircraftCount);
        msg.AddUShort(completedAircraftCount);
        msg.AddUShort(failedAircraftCount);
        msg.AddUShort(totalAircraftCount);
    }

    internal void GetFromMessage(Message msg) {
        activeAircraftCount = msg.GetUShort();
        completedAircraftCount = msg.GetUShort();
        failedAircraftCount = msg.GetUShort();
        totalAircraftCount = msg.GetUShort();
    }
}