using AirplaneControllerScript;
using ATC.Operator.Pathway;
using Pathway;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ParkingPathData", menuName = "Azmi Studio/ParkingPathData", order = 4)]
public class ParkingPathData : ScriptableObject {
    [SerializeField] internal Taxiway[] allTaxiway;
    [SerializeField] internal ParkingStand[] allParkingStand;
    [SerializeField] internal Midline[] allMidline;

#if UNITY_EDITOR
    [Header("Debug")]
    [SerializeField] internal ViaTaxiwayID debug_viaTaxiway;
    [SerializeField] internal ParkingStandID debug_parkingStand;
    [SerializeField] internal MovementType debug_movementType;
    [SerializeField] internal PushbackFacing debug_pushbackFacing;
    private Path[] tmpPathResult = null;
#endif

    private Path tmpPath = default;
    private ParkingStand tmpParkignStand = default;
    private Taxiway tmpTaxiway = default;
    private Midline tmpMidline = default;

#if UNITY_EDITOR
    public void RenameValirable() {
        for (int i = 0; i < allTaxiway.Length; i++) {
            allTaxiway[i]._name = allTaxiway[i].id.ToString().ToUpper();
        }
        for (int i = 0; i < allParkingStand.Length; i++) {
            for (int j = 0; j < allParkingStand[i].allNorthFacingTaxiway.Length; j++) {
                allParkingStand[i].allNorthFacingTaxiway[j]._name = allParkingStand[i].allNorthFacingTaxiway[j].id.ToString().ToUpper();
            }
            for (int j = 0; j < allParkingStand[i].allSouthFacingTaxiway.Length; j++) {
                allParkingStand[i].allSouthFacingTaxiway[j]._name = allParkingStand[i].allSouthFacingTaxiway[j].id.ToString().ToUpper();
            }
        }
        for (int i = 0; i < allMidline.Length; i++) {
            allMidline[i]._name = allMidline[i].id.ToString().ToUpper();
        }
        EditorUtility.SetDirty(this);
    }
#endif





    public bool TryGetParkingStand(ParkingStandID rID, out ParkingStand oParkingStand) {
        for (int i = 0; i < allParkingStand.Length; i++) {
            if (allParkingStand[i].id == rID) {
                oParkingStand = allParkingStand[i];
                return true;
            }
        }
        oParkingStand = default;
        return false;
    }
    public bool TryGetTaxiway(TaxiwayID rID, out Taxiway oTaxiway) {
        for (int i = 0; i < allTaxiway.Length; i++) {
            if (allTaxiway[i].id == rID) {
                oTaxiway = allTaxiway[i];
                return true;
            }
        }
        oTaxiway = default;
        return false;
    }
    public bool TryGetMidline(MidlineID rID, out Midline oMidline) {
        for (int i = 0; i < allMidline.Length; i++) {
            if (allMidline[i].id == rID) {
                oMidline = allMidline[i];
                return true;
            }
        }
        oMidline = default;
        return false;
    }
    public bool TryGetPushbackPath(ParkingStandID rPkgStandID, PushbackFacing rPbFacing, out PathSequence oResultPath) {
        if (TryGetParkingStand(rPkgStandID, out ParkingStand pkgStand)) {
            if (rPbFacing == PushbackFacing.facingNorth && pkgStand.facingNorth != null) {
                oResultPath = pkgStand.facingNorth.pathSequences; return true;
            } else if (rPbFacing == PushbackFacing.facingSouth && pkgStand.facingSouth != null) {
                oResultPath = pkgStand.facingSouth.pathSequences; return true;
            }
        }
        oResultPath = default;
        return false;
    }








    public bool TryGetDeparturePath(ParkingStandID rPkgStandID, PushbackFacing rPbFacing, ViaTaxiwayID rViaTaxiwayID, out Path[] oAllPath) {
        if (rViaTaxiwayID == ViaTaxiwayID.F) {
            if (TryGetDeparturePath(rPkgStandID, rPbFacing, TaxiwayID.TWY_FS, out Path[] recievePath)) {
                oAllPath = recievePath;
                return true;
            }
        } else if (rViaTaxiwayID == ViaTaxiwayID.C) {
            if (TryGetDeparturePath(rPkgStandID, rPbFacing, TaxiwayID.TWY_CS, out Path[] recievePath)) {
                oAllPath = recievePath;
                return true;
            } else if (TryGetDeparturePath(rPkgStandID, rPbFacing, TaxiwayID.TWY_CN, out Path[] recievePath2)) {
                oAllPath = recievePath2;
                return true;
            }
        } else if (rViaTaxiwayID == ViaTaxiwayID.V) {
            if (TryGetDeparturePath(rPkgStandID, rPbFacing, TaxiwayID.TWY_VN, out Path[] recievePath)) {
                oAllPath = recievePath;
                return true;
            }
        }
        oAllPath = new Path[] { };
        return false;
    }
    private bool TryGetDeparturePath(ParkingStandID rParkStandID, PushbackFacing rPbFacing, TaxiwayID rTaxiwayID, out Path[] oResultPath) {
        if (TryGetParkingStand(rParkStandID, out tmpParkignStand)) {
            if (rPbFacing == PushbackFacing.facingNorth && tmpParkignStand.facingNorth != null) {
                for (int i = 0; i < tmpParkignStand.allNorthFacingTaxiway.Length; i++) {
                    if (tmpParkignStand.allNorthFacingTaxiway[i].id == rTaxiwayID) {
                        if (TryGetTaxiway(rTaxiwayID, out tmpTaxiway)) {
                            oResultPath = JoinDeparturePath(tmpParkignStand.facingNorth, tmpTaxiway.outboundPath, tmpParkignStand.allNorthFacingTaxiway[i].allOutboundMidlineID);
                            return true;
                        }
                    }
                }
            }
            if (rPbFacing == PushbackFacing.facingSouth && tmpParkignStand.facingSouth != null) {
                for (int i = 0; i < tmpParkignStand.allSouthFacingTaxiway.Length; i++) {
                    if (tmpParkignStand.allSouthFacingTaxiway[i].id == rTaxiwayID) {
                        if (TryGetTaxiway(rTaxiwayID, out tmpTaxiway)) {
                            oResultPath = JoinDeparturePath(tmpParkignStand.facingSouth, tmpTaxiway.outboundPath, tmpParkignStand.allSouthFacingTaxiway[i].allOutboundMidlineID);
                            return true;
                        }
                    }
                }
            }
        }
        oResultPath = new Path[] { };
        return false;
    }
    private Path[] JoinDeparturePath(PathData rParkingStandPath, PathData rTaxiwayPath, MidlineID[] rAllMidlineID) {
        List<Path> newPathList = new List<Path>();
        Vector3 p0 = rParkingStandPath.pathSequences.GetP0();
        if (rAllMidlineID.Length > 0) {
            for (int i = 0; i < rAllMidlineID.Length; i++) {
                if (TryGetMidline(rAllMidlineID[i], out tmpMidline)) {
                    tmpPath.CreateLinearPath(p0, tmpMidline.pathData.pathSequences.GetP0());
                    newPathList.Add(tmpPath);
                    for (int j = 0; j < tmpMidline.pathData.pathSequences.allPath.Length; j++) {
                        newPathList.Add(tmpMidline.pathData.pathSequences.allPath[j]);
                    }
                    p0 = tmpMidline.pathData.pathSequences.GetP3();
                }
            }
        }
        tmpPath.CreateLinearPath(p0, rTaxiwayPath.pathSequences.GetP0());
        newPathList.Add(tmpPath);
        for (int i = 0; i < rTaxiwayPath.pathSequences.allPath.Length; i++) {
            newPathList.Add(rTaxiwayPath.pathSequences.allPath[i]);
        }
        return newPathList.ToArray();
    }









    public Path[] GetArrivalPath(ViaTaxiwayID rViaTaxiwayID, ParkingStandID rParkStandID) {
        Path[] tmpPath = new Path[] { };
        if (rViaTaxiwayID == ViaTaxiwayID.F) {
            tmpPath = GetArrivalPath(TaxiwayID.TWY_FS, rParkStandID);
        } else if (rViaTaxiwayID == ViaTaxiwayID.C) {
            tmpPath = GetArrivalPath(TaxiwayID.TWY_CS, rParkStandID);
            if (tmpPath == null) {
                tmpPath = GetArrivalPath(TaxiwayID.TWY_CN, rParkStandID);
            }
        } else if (rViaTaxiwayID == ViaTaxiwayID.V) {
            tmpPath = GetArrivalPath(TaxiwayID.TWY_VN, rParkStandID);
        }
        return tmpPath;
    }
    public Path[] GetArrivalPath(TaxiwayID rTaxiwayID, ParkingStandID rParkStandID) {
        if (TryGetParkingStand(rParkStandID, out tmpParkignStand)) {
            if (tmpParkignStand.facingSouth != null) {
                for (int i = 0; i < tmpParkignStand.allNorthFacingTaxiway.Length; i++) {
                    if (tmpParkignStand.allNorthFacingTaxiway[i].id == rTaxiwayID) {
                        if (TryGetTaxiway(rTaxiwayID, out tmpTaxiway)) {
                            return JoinArrivalPath(tmpTaxiway.inboudPath, tmpParkignStand.facingSouth, tmpParkignStand.allNorthFacingTaxiway[i].allInboundMidlineID);
                        }
                    }
                }
            }
            if (tmpParkignStand.facingNorth != null) {
                for (int i = 0; i < tmpParkignStand.allSouthFacingTaxiway.Length; i++) {
                    if (tmpParkignStand.allSouthFacingTaxiway[i].id == rTaxiwayID) {
                        if (TryGetTaxiway(rTaxiwayID, out tmpTaxiway)) {
                            return JoinArrivalPath(tmpTaxiway.inboudPath, tmpParkignStand.facingNorth, tmpParkignStand.allSouthFacingTaxiway[i].allInboundMidlineID);
                        }
                    }
                }
            }
        }
        return null;
    }
    private Path[] JoinArrivalPath(PathData rTaxiwayPath, PathData rParkingStandPath, MidlineID[] rAllMidlineID) {
        List<Path> newPathList = new List<Path>();
        for (int j = 0; j < rTaxiwayPath.pathSequences.allPath.Length; j++) {
            newPathList.Add(rTaxiwayPath.pathSequences.allPath[j]);
        }
        Vector3 p0 = rTaxiwayPath.pathSequences.GetP3();
        if (rAllMidlineID.Length > 0) {
            for (int i = 0; i < rAllMidlineID.Length; i++) {
                if (TryGetMidline(rAllMidlineID[i], out tmpMidline)) {
                    tmpPath.CreateLinearPath(p0, tmpMidline.pathData.pathSequences.GetP0());
                    newPathList.Add(tmpPath);
                    for (int j = 0; j < tmpMidline.pathData.pathSequences.allPath.Length; j++) {
                        newPathList.Add(tmpMidline.pathData.pathSequences.allPath[j]);
                    }
                    p0 = tmpMidline.pathData.pathSequences.GetP3();
                }
            }
        }
        tmpPath.CreateLinearPath(p0, rParkingStandPath.pathSequences.GetP0());
        newPathList.Add(tmpPath);
        for (int i = 0; i < rParkingStandPath.pathSequences.allPath.Length; i++) {
            newPathList.Add(rParkingStandPath.pathSequences.allPath[i]);
        }
        return newPathList.ToArray();
    }



     




    public ParkingStandID[] GetArrivalParkingStandList(ViaTaxiwayID rViaTaxiwayID) {
        List<ParkingStandID> resultList = new List<ParkingStandID>();
        for (int i = 0; i< allParkingStand.Length; i++) {
            if (allParkingStand[i].facingNorth != null) {
                if (IsViaTaxiwayAvailable(allParkingStand[i].allSouthFacingTaxiway, rViaTaxiwayID)) {
                    resultList.Add(allParkingStand[i].id);
                }
            }
            if (allParkingStand[i].facingSouth != null) {
                if (IsViaTaxiwayAvailable(allParkingStand[i].allNorthFacingTaxiway, rViaTaxiwayID)) {
                    resultList.Add(allParkingStand[i].id);
                }
            }
        }
        return resultList.ToArray();
    }
    private bool IsViaTaxiwayAvailable(ConnectedTaxiway[] allConnectedTaxiway, ViaTaxiwayID rViaTaxiwayID) {
        for (int i = 0; i < allConnectedTaxiway.Length; i++) {
            TaxiwayID txyID = allConnectedTaxiway[i].id;
            if (txyID == TaxiwayID.TWY_FS && rViaTaxiwayID == ViaTaxiwayID.F) {
                return true;
            } else if (txyID == TaxiwayID.TWY_CS && rViaTaxiwayID == ViaTaxiwayID.C) {
                return true;
            } else if (txyID == TaxiwayID.TWY_CN && rViaTaxiwayID == ViaTaxiwayID.C) {
                return true;
            } else if (txyID == TaxiwayID.TWY_VN && rViaTaxiwayID == ViaTaxiwayID.V) {
                return true;
            }
        }
        return false;
    }



#if UNITY_EDITOR
    public void CreateDebugPath() {
        if (debug_movementType == MovementType.arrival) {
            tmpPathResult = GetArrivalPath(debug_viaTaxiway, debug_parkingStand);
        } else if (debug_movementType == MovementType.departure) {
            if (TryGetDeparturePath(debug_parkingStand, debug_pushbackFacing, debug_viaTaxiway, out Path[] oAllPath)) {
                tmpPathResult = oAllPath;
            }
        }
        if (tmpPathResult != null) {
            GameObject newObject = new GameObject($"{debug_movementType} :: {debug_parkingStand} :: {debug_viaTaxiway}");
            TestPathVisual tpv = newObject.AddComponent<TestPathVisual>();
            tpv.Initialize(tmpPathResult);
        }
    }
#endif


}





public enum ParkingStandID {
    _01, _02, _02A, _03, _04, _05, _06, _07, _08, _09, _10, _11, _12S, _12, _12N, _13, _13N, _14N,
    _15, _16, _17, _18N, _18, _19, _20, _21, _22, _23, _24, _25, _26, _27, _28, _29
};
public static class ParkingStandIDExtension {
    public static string Caption(this ParkingStandID pkgStandID) {
        return pkgStandID.ToString().Replace("_","");
    }
}
public enum PushbackFacing { facingNorth, facingSouth }
[System.Serializable]   
public struct ParkingStand{
    [SerializeField] internal string _name;
    [SerializeField] internal ParkingStandID id;
    [SerializeField] internal PathData facingNorth;
    [SerializeField] internal PathData facingSouth;
    [SerializeField] internal ConnectedTaxiway[] allNorthFacingTaxiway;
    [SerializeField] internal ConnectedTaxiway[] allSouthFacingTaxiway;
}
[System.Serializable]
public struct ConnectedTaxiway {
    [SerializeField] internal string _name;
    [SerializeField] internal TaxiwayID id;
    [SerializeField] internal MidlineID[] allOutboundMidlineID;
    [SerializeField] internal MidlineID[] allInboundMidlineID;
}





public enum TaxiwayID { TWY_FS, TWY_CN, TWY_CS, TWY_VN }
public enum ViaTaxiwayID { F, C, V }
[System.Serializable]
public struct Taxiway {
    [SerializeField] internal string _name;
    [SerializeField] internal TaxiwayID id;
    [SerializeField] internal PathData inboudPath;
    [SerializeField] internal PathData outboundPath;
}






public enum MidlineID { MN, MS }
[System.Serializable]
public struct Midline {
    [SerializeField] internal string _name;
    [SerializeField] internal MidlineID id;
    [SerializeField] internal PathData pathData;
}




