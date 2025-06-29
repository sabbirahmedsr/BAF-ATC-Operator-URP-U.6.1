using UnityEngine;
using Riptide;
using ATC.Global;

namespace ATC.Operator.Networking{
    /// <summary>
    /// Send Heartbeat at fixed interval
    /// for loseless connection 
    /// </summary>
    [System.Serializable]
    public class Network_Heartbeat : MonoBehaviour {
        [Tooltip("Hearbeat interval in seconds")]
        private float heartbeatInterval = 2f;
        private float lastHeartbeatTime;

        internal void ManualUpdate() {
            if (Time.time - lastHeartbeatTime >= heartbeatInterval) {
                if (GlobalNetwork.connectionStatus == ConnectionStatus.connected) {
                    GlobalNetwork.actionSender.SendHeartbeat();
                } 
                lastHeartbeatTime = Time.time;                
            }
        }


    }
}