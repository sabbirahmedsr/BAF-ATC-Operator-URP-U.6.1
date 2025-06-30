using UnityEngine;
using UnityEngine.UI;

namespace ATC.Home {
    public class HC03_About : HC_SubClass {

        [SerializeField] private Button btnOk;

        internal override void Initialize(HomeController _mc) {
            base.Initialize(_mc);

            btnOk.onClick.AddListener(OnClick_OK);
        }

        private void OnClick_OK() {
            mainController.Toggle_UIPanel(UIPanelType.mainMenu);
        }
    }
}