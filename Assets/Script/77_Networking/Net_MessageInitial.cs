
namespace ATC.Operator.Networking {
    public enum MSG_Initial : ushort {
        /// heartbeat for connection continuety
        heartbeat,

        /// <summary> format :: initial </summary>
        request_startupInfo,
        /// <summary> format :: initial, map id, array length, spawnInfo[]</summary>
        startupInfo_result,


        // create airplane and node
        /// <summary> format :: initial, call sign id, startIndex </summary>
        create_ArrFlight_OnServer,
        /// <summary> format :: initial, call sign id, parking stand id </summary>
        create_DepFlight_OnServer,
        /// <summary> format :: initial, airplane id, callSignID, pos, fwd, lastNT, nextNT </summary>
        create_ArrFlight_OnOperator,
        /// <summary> format :: initial, airplane id, startLineIndex </summary>
        create_SurfaceMapNode,
        /// <summary> format :: initial, airplane id, startLineIndex </summary>
        create_RadarMapNode,


        // update all data [Array-Batching] [Unreliable]
        /// <summary> format :: initial, array length, airplane id[], position[], forward[] </summary>
        update_all_PosAndRot,
        /// <summary> Base format :: initial, ATC Time, VizHeadingSpeedFL Array :::
        /// Detail format :: initial, atcTimeInSeconds, elapsedTimeInSeconds, array length, airplane id[], vizHeadingSpeedFL[] </summary>
        update_TimeAndVisualInfo,


        // update airplane data [reliable]
        /// <summary> format :: initial, airplane id, lastNameAndTimeZ, nextNameAndTimeZ </summary>
        update_AP_AirplanePhaseNT,
        /// <summary> format :: initial, airplane id, array length, pathHistory[]  </summary>
        update_AP_PathHisotry,
        /// <summary> format :: initial, airplane id, array length, arrivalCommandID[]  </summary>
        update_AP_ArrivalCommandList,
        /// <summary> format :: initial, airplane id, array length, departureCommandID[]  </summary>
        update_AP_DepartureCommandList,
        /// <summary> format :: initial, airplane id, commandId, array length, parameter[] </summary>
        send_AP_FlightCommand,
        /// <summary> format :: initial, airplaneId </summary>
        destroy_AP_RadarMapNode,
        /// <summary> format :: initial, airplaneId </summary>
        destroy_AP_SurfaceMapNode,
        /// <summary> format :: initial, airplaneId </summary>
        destroy_AP_Airplane_OnOperator,


        // update other data [reliable] 
        /// <summary> format :: initial, arrivalScoreCount, departureScoreCount </summary>
        update_ArrAndDepScoreCount,
        /// <summary> format :: initial, speed </summary>
        update_globalSpeed
    }

}