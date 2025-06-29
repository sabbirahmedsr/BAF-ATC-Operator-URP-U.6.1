
using AirplaneControllerScript;
using Pathway;
using Riptide;
using UnityEngine;

namespace ATC.Operator.Networking {

    public struct NetworkAirplane_SpawnInfo {
        [Header("Basic")]
        internal CallSign callSign;
        internal MovementType movementType;

        [Header("Movement")]
        internal Vector3 pos;
        internal Vector3 fwd;

        [Header("State Name & Time")]
        internal NameAndTimeZ lastNT;
        internal NameAndTimeZ nextNT;

        [Header("Map Node")]
        internal ushort startLineIndex_radarMap;
        internal ushort startLineIndex_surfaceMap;
        internal bool hasRadarMap;
        internal bool hasSurfaceMap;

        [Header("Path History")]
        internal Path[] pathHistory;

        [Header("Command ID")]
        internal ArrivalCommandID[] allArrivalCommandID;
        internal DepartureCommandID[] allDepartureCommandID;


        public void AddToMessage(Message msg) {
            msg.AddUShort((ushort)this.callSign);
            msg.AddUShort((ushort)this.movementType);
            msg.AddVector3(this.pos);
            msg.AddVector3(this.fwd);
            lastNT.AddToMessage(msg);
            nextNT.AddToMessage(msg);
            msg.AddUShort(this.startLineIndex_radarMap);
            msg.AddUShort(this.startLineIndex_surfaceMap);
            msg.AddBool(this.hasRadarMap);
            msg.AddBool(this.hasSurfaceMap);
            //
            msg.AddUShort((ushort)this.pathHistory.Length);
            for (int i = 0; i < pathHistory.Length; i++) {
                Path path = pathHistory[i];
                path.AddToMessage(msg);
            }
            msg.AddUShort((ushort)this.allArrivalCommandID.Length);
            for (int i = 0; i < allArrivalCommandID.Length; i++) {
                msg.AddUShort((ushort)allArrivalCommandID[i]);
            }
            msg.AddUShort((ushort)this.allDepartureCommandID.Length);
            for (int i = 0; i < allDepartureCommandID.Length; i++) {
                msg.AddUShort((ushort)allDepartureCommandID[i]);
            }
        }

        public void GetFromMessage(Message msg) {
            this.callSign = (CallSign)msg.GetUShort();
            this.movementType = (MovementType)msg.GetUShort();
            this.pos = msg.GetVector3();
            this.fwd = msg.GetVector3();
            this.lastNT.GetFromMessage(msg);
            this.nextNT.GetFromMessage(msg);
            this.startLineIndex_radarMap = (ushort)msg.GetUShort();
            this.startLineIndex_surfaceMap = (ushort)msg.GetUShort();
            this.hasRadarMap = msg.GetBool();
            this.hasSurfaceMap = msg.GetBool();
            //
            ushort arrayLength = msg.GetUShort();
            this.pathHistory = new Path[arrayLength];
            for (int i = 0; i < pathHistory.Length; i++) {
                pathHistory[i].GetFromMessage(msg);
            }
            arrayLength = msg.GetUShort();
            allArrivalCommandID = new ArrivalCommandID[arrayLength];
            for (int i = 0; i < allArrivalCommandID.Length; i++) {
                allArrivalCommandID[i] = (ArrivalCommandID)msg.GetUShort();
            }
            arrayLength = msg.GetUShort();
            allDepartureCommandID = new DepartureCommandID[arrayLength];
            for (int i = 0; i < allDepartureCommandID.Length; i++) {
                allDepartureCommandID[i] = (DepartureCommandID)msg.GetUShort();
            }
        }

    }

}