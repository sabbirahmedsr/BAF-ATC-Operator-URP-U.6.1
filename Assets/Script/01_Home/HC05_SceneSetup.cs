using ATC.Global;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ATC.Home {
    public class HC05_SceneSetup : HC_SubClass {
        [SerializeField] private TMP_Dropdown drdAirport;
        [SerializeField] private Button btnBack;
        [SerializeField] private Button btnStart;
        [SerializeField] private SceneLoader sceneLoader;

        internal override void Initialize(HomeController _mc) {
            base.Initialize(_mc);

            // set variable
            drdAirport.options.Clear();
            drdAirport.AddOptions(new List<string>() { "VGHS", "VGJR" });

            // add button listener
            drdAirport.onValueChanged.AddListener(OnDrdChange_Airport);
            btnStart.onClick.AddListener(OnClick_Start);
            btnBack.onClick.AddListener(OnClick_Back);
        }
        private void OnDrdChange_Airport(int rIndex) {

        }
        private void OnClick_Start() {
            sceneLoader.LoadSceneAsync(GlobalSceneName.atcOperator_vghs);    
            Activate(false);
        }
        private void OnClick_Back() {
            mainController.Toggle_UIPanel(UIPanelType.mainMenu);
        }
    }
}