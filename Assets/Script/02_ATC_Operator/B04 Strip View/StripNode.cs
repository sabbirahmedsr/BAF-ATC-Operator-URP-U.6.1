using AirplaneControllerScript;
using TMPro;
using UnityEngine;

namespace StripViewScript.Child {
    public class StripNode : MonoBehaviour {
        [Header("UI Refernce")]
        [SerializeField] private TMP_Text txtCallSign;
        [SerializeField] private TMP_Text txtHeading;
        [SerializeField] private TMP_Text txtSpeed;
        [SerializeField] private TMP_Text txtFL;

        [Header("Name & Time Z")]
        [SerializeField] private TMP_Text txtLastName;
        [SerializeField] private TMP_Text txtLastTimeZ;
        [SerializeField] private TMP_Text txtNextName;
        [SerializeField] private TMP_Text txtNextTimeZ;

        private AirplaneController apController;
        private StripController stripController;

        internal void Initialize(StripController rStripController, AirplaneController rAP__Controller) {
            stripController = rStripController;
            apController = rAP__Controller;
            apController.apEvent.onUpdate_VizHeadingSpeedFL.AddListener(Update_VizHeadingSpeedAndFL);
            apController.apEvent.onUpdate_AirplanePhaseNT.AddListener(Update_AirplanePhaseNT);
            apController.apEvent.onAirplaneDestroyEvent.AddListener(DestroyMe);
            txtCallSign.text = apController.myData.callSign.ToString();
            Update_AirplanePhaseNT(apController.lastNameAndTimeZ, apController.nextNameAndTimeZ);
            enabled = true;
        }


        internal void Update_VizHeadingSpeedAndFL(VizHeadSpeedFL rVizHeadSpeedFL) {
            txtHeading.text = $"{rVizHeadSpeedFL.heading}°";
            txtSpeed.text = $"{rVizHeadSpeedFL.speed}N";
            if (rVizHeadSpeedFL.flDirection == FLDirection.flat) {
                txtFL.text = $"{rVizHeadSpeedFL.FL}FL";
                txtFL.color = Color.white;
            } else if (rVizHeadSpeedFL.flDirection == FLDirection.upward) {
                txtFL.text = $"▲{rVizHeadSpeedFL.FL}FL";
                txtFL.color = Color.green;
            } else if (rVizHeadSpeedFL.flDirection == FLDirection.downward) {
                txtFL.text = $"▼{rVizHeadSpeedFL.FL}FL";
                txtFL.color = Color.red;
            }
        }
        internal void Update_AirplanePhaseNT(NameAndTimeZ rLastNT, NameAndTimeZ rNextNT) {
            txtLastName.text = rLastNT.name;
            txtLastTimeZ.text = rLastNT.timeZ;
            txtNextName.text = rNextNT.name;
            txtNextTimeZ.text = rNextNT.timeZ;
        }


        internal void DestroyMe() {
            Destroy(gameObject);
        }
    }
}