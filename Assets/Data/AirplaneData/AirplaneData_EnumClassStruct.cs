using Riptide;
using UnityEngine;

#region Basic_Identity

public enum CallSign : byte {
    NONE,
    THAI_321, EK_584, BG_367, IGO_1113, QR_638, CX_667, SQ_446, BG_602, BG_492, BG_472, NVQ_946, AWA_446, UBG_108,
    GFN_391, BJR_471, UNC_1031, AJX_1483, S2_AGP
}
public static class EnumExtensions {
    public static string Caption(this CallSign callSign) {
       return callSign.ToString().Replace("_"," ");
    }
}

public enum TypeOfAircraft: byte { none, International, Domestic, Military, Other }

#endregion






[System.Serializable]
public struct NameAndTimeZ {
    [SerializeField] internal string name;
    [SerializeField] internal string timeZ;
    public NameAndTimeZ(string rName, string rTimeZ) {
        name = rName; timeZ = rTimeZ;
    }
    public void GetFromMessage(Message msg) {
        this.name = msg.GetString();
        this.timeZ = msg.GetString();
    }
    public void AddToMessage(Message msg) {
        msg.AddString(this.name);
        msg.AddString(this.timeZ);
    }
}


public enum FLDirection : byte { flat, upward, downward }
[System.Serializable]
public struct VizHeadSpeedFL {
    [SerializeField] internal short heading;
    [SerializeField] internal short speed;
    [SerializeField] internal short FL;
    [SerializeField] internal FLDirection flDirection;
    public void AddToMessage(Message msg) {
        msg.AddShort(this.heading);
        msg.AddShort(this.speed);
        msg.AddShort(this.FL);
        msg.AddUShort((ushort)this.flDirection);
    }     
    public void GetFromMessage(Message msg) {
        this.heading = msg.GetShort();
        this.speed = msg.GetShort();
        this.FL = msg.GetShort();
        this.flDirection = (FLDirection)msg.GetUShort();
    }
}





[System.Serializable]
public struct InitialCall {
    [Tooltip("If provided, this will be shown in operator command list as a button")]
    [SerializeField] internal ArrivalCommandID arrCmdId;
    [Tooltip("If provided, this will be shown in operator command list as a button")]
    [SerializeField] internal DepartureCommandID depCmdId;
}

[System.Serializable]
public struct FeedbackCall {
    [Tooltip("If provided, this will be shown in operator command list as a button")]
    [SerializeField] internal ArrivalCommandID arrCmdId;
    [Tooltip("If provided, this will be shown in operator command list as a button")]
    [SerializeField] internal DepartureCommandID depCmdId;
}



public enum QNH : byte {
    QNH1013, QNH1012, QNH1011, QNH1010, QNH999, QNH760, QNH758, QNH756, QNH755, QNH30d08, QNH30d02, QNH29d87, QNH29d85,
}

public enum Altitude : byte {
    _4000ft, _3000ft, _2000ft, _1500ft
}

public enum ArrivalApproach : byte {
    none, vor, arc, delta, eat, militery_circuit
}

public enum ArrTaxiwayID : byte { S1, S2, S3, SA, HA }