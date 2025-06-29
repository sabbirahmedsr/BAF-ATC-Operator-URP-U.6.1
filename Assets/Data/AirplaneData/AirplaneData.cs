using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "AirplaneData", menuName = "Azmi Studio/AirplaneData", order = 2)]
public class AirplaneData : ScriptableObject {
    [SerializeField] internal GameObject airplanePrefab;
    [SerializeField] internal CallSign callSign;
    [SerializeField] internal TypeOfAircraft aircraftType;
    [SerializeField] internal string[] allPreArrivalPoints;
    [SerializeField] internal ArrivalApproach[] allArrivalApproach;

    [Header("Basic Taxi Ref")]
    [SerializeField] internal ParkingPathData parkingPathData;
    [SerializeField] internal ArrTaxiwayID[] allArrTaxiwayID;
    [SerializeField] internal ParkingStandID[] allParkingStandID;
}
