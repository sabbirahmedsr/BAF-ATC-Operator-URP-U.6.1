using TMPro;
using UnityEngine;

namespace InfoViewScript {
    public class InfoController : MonoBehaviour {
        [SerializeField] private Camera infoCamera;

        [Header("Date & Time")]
        [SerializeField] private TMP_Text txtATCTime;
        [SerializeField] private TMP_Text txtElapsedTime;
        [SerializeField] private TMP_Text txtDate;

        [Header("Flight Info")]
        [SerializeField] private TMP_Text txtHeading;
        [SerializeField] private TMP_Text txtActive;
        [SerializeField] private TMP_Text txtComplete;
        [SerializeField] private TMP_Text txtFail;
        [SerializeField] private TMP_Text txtTotal;


        internal void Initialize() {
            ATCTime.onATC_SecondUpdate += OnATCSecondsUpdate;
            GlobalEvent.onScoreCountChangedEvent += OnScoreCountChange;
        }
        private void OnDisable() {
            ATCTime.onATC_SecondUpdate += OnATCSecondsUpdate;
            GlobalEvent.onScoreCountChangedEvent -= OnScoreCountChange;
        }
        private void Start() {
            txtDate.text = System.DateTime.Now.ToString("dd/MM/yyyy");
            txtHeading.text = GlobalData.operatorType == OperatorType.arrival ? "Arrival Score" : "Departure Score";
        }


        private void OnATCSecondsUpdate() {
            txtATCTime.text = ATCTime.GetClockTimeInHHMMSS();
            txtElapsedTime.text = ATCTime.GetElapsedTimeInHHMMSS();
        }

        private void OnScoreCountChange() {
            if (GlobalData.operatorType == OperatorType.arrival) {
                txtActive.text = GlobalData.arrScoreCount.activeAircraftCount.ToString("00");
                txtComplete.text = GlobalData.arrScoreCount.completedAircraftCount.ToString("00");
                txtFail.text = GlobalData.arrScoreCount.failedAircraftCount.ToString("00");
                txtTotal.text = GlobalData.arrScoreCount.totalAircraftCount.ToString("00");
            } else if (GlobalData.operatorType == OperatorType.departure) {
                txtActive.text = GlobalData.depScoreCount.activeAircraftCount.ToString("00");
                txtComplete.text = GlobalData.depScoreCount.completedAircraftCount.ToString("00");
                txtFail.text = GlobalData.depScoreCount.failedAircraftCount.ToString("00");
                txtTotal.text = GlobalData.depScoreCount.totalAircraftCount.ToString("00");
            }
        }
    }
}