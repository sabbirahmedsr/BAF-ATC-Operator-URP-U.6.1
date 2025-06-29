using UnityEngine;
using UnityEngine.UI;

namespace ATC.Home {
    public class HC01_MainMenu : HC_SubClass {
        [SerializeField] private Button btnStart;
        [SerializeField] private Button btnSetting;
        [SerializeField] private Button btnAbout;
        [SerializeField] private Button btnExit;

        internal override void Initialize(HomeController _mc) {
            base.Initialize(_mc);

            //Add Button Listener
            btnStart.onClick.AddListener(OnClick_Start);
            btnSetting.onClick.AddListener(OnClick_Setting);
            btnAbout.onClick.AddListener(OnClick_About);
            btnExit.onClick.AddListener(OnClick_Exit);
        }

        private void OnClick_Start() {
            mainController.Toggle_UIPanel(UIPanelType.sceneSetup);
        }

        private void OnClick_Setting() {
            mainController.Toggle_UIPanel(UIPanelType.setting);
        }

        private void OnClick_About() {
            mainController.Toggle_UIPanel(UIPanelType.about);
        }

        private void OnClick_Exit() {
            mainController.Toggle_UIPanel(UIPanelType.exitWarning);
        }
    }
}