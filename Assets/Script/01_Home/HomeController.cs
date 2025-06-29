using UnityEngine;

namespace ATC.Home {
    internal enum UIPanelType { mainMenu, setting, about, exitWarning, sceneSetup }

    public class HomeController : MonoBehaviour {
        private UIPanelType panelType;

        [SerializeField] private HC01_MainMenu mainMenu;
        [SerializeField] private HC02_Setting setting;
        [SerializeField] private HC03_About about;
        [SerializeField] private HC04_ExitWarning exitWarning;
        [SerializeField] private HC05_SceneSetup sceneSetup;

        private void Start() {
            // initialize child script
            mainMenu.Initialize(this);
            setting.Initialize(this);
            about.Initialize(this);
            exitWarning.Initialize(this);
            sceneSetup.Initialize(this);

            // activate main menu only
            Toggle_UIPanel(UIPanelType.mainMenu);
        }
        internal void Toggle_UIPanel(UIPanelType rUIPanelType) {
            panelType = rUIPanelType;
            mainMenu.Activate(panelType == UIPanelType.mainMenu);
            setting.Activate(panelType == UIPanelType.setting);
            about.Activate(panelType == UIPanelType.about);
            exitWarning.Activate(panelType == UIPanelType.exitWarning);
            sceneSetup.Activate(panelType == UIPanelType.sceneSetup);
        } 

    }
}