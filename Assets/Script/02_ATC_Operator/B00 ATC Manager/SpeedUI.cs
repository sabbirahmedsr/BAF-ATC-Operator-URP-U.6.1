using ATC.Global;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ATCManagerScript {
    [System.Serializable]
    public class SpeedUI {
        [SerializeField] private TMP_Dropdown drdSpeed;
        private int[] allSpeed = new int[] { 0, 1, 5, 10, 25, 50, 100 };

        [SerializeField] private Button btnResetSpeed;
        [SerializeField] private Button btnGlobalPause;


        internal void Initialize() {
            // set initial variable
            List<string> allOptions = new List<string>();
            for (int i = 0; i < allSpeed.Length; i++) {
                allOptions.Add(allSpeed[i].ToString() + "x");
            }
            drdSpeed.options.Clear();
            drdSpeed.AddOptions(allOptions);
            drdSpeed.SetValueWithoutNotify(1);
            //
            /// AddButtonListener
            drdSpeed.onValueChanged.AddListener(OnDrdChange_Speed);
            btnResetSpeed.onClick.AddListener(ResetSpeed);
            btnGlobalPause.onClick.AddListener(GlobalPause);
        }



        private void OnDrdChange_Speed(int rIndex) {
            SetGlobalSpeed(allSpeed[rIndex]);
        }
        private void ResetSpeed() {
            SetGlobalSpeed(1);
            drdSpeed.SetValueWithoutNotify(1);
        }
        private void GlobalPause() {
            SetGlobalSpeed(0);
            drdSpeed.SetValueWithoutNotify(0);
        }

        private void SetGlobalSpeed(int speed) {
            GlobalNetwork.actionSender.Send_GlobalSpeed((ushort)speed);
        }

    }
}