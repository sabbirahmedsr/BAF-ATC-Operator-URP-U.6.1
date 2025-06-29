using UnityEngine;
using UnityEngine.UI;

namespace ATC.Home {
    public class HC04_ExitWarning : HC_SubClass {

        [SerializeField] private Button btnExitConfirm;
        [SerializeField] private Button btnExitCancel;

        internal override void Initialize(HomeController _mc) {
            base.Initialize(_mc);

            btnExitConfirm.onClick.AddListener(OnClick_ExitConfirm);
            btnExitCancel.onClick.AddListener(OnClick_ExitCancel);
        }

        private void OnClick_ExitConfirm() {
            Application.Quit();
        }
        private void OnClick_ExitCancel() {
            mainController.Toggle_UIPanel(UIPanelType.mainMenu);
        }
    }
}