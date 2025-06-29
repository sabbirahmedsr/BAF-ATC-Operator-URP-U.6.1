using ATC.Global;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ATC.Home {
    public class HC02_Setting : HC_SubClass {
        [SerializeField] private TMP_InputField inpfServerIP;
        [SerializeField] private TMP_InputField inpfServerPort;
        [SerializeField] private Button btnConfirmSetting;
        [SerializeField] private Button btnCancelSetting;

        internal override void Initialize(HomeController _mc) {
            base.Initialize(_mc);

            // add button listener
            btnConfirmSetting.onClick.AddListener(OnClick_ConfirmSetting);
            btnCancelSetting.onClick.AddListener(OnClick_CancelSetting);
            LoadSettingData();
        }


        private void OnClick_ConfirmSetting() {
            SaveSettingData();
            mainController.Toggle_UIPanel(UIPanelType.mainMenu);
        }
        private void OnClick_CancelSetting() {
            LoadSettingData();
            mainController.Toggle_UIPanel(UIPanelType.mainMenu);
        }



        private void LoadSettingData() {
            GlobalNetwork.ip = PlayerPrefs.GetString("ServerIP", GlobalNetwork.ip);
            GlobalNetwork.port = (ushort)PlayerPrefs.GetInt("ServerPort", GlobalNetwork.port);
            inpfServerIP.text = GlobalNetwork.ip.ToString();
            inpfServerPort.text = GlobalNetwork.port.ToString();
        }
        private void SaveSettingData() {
            GlobalNetwork.ip = inpfServerIP.text;
            PlayerPrefs.SetString("ServerIP", GlobalNetwork.ip);
            GlobalNetwork.port = ushort.Parse(inpfServerPort.text);
            PlayerPrefs.SetInt("ServerPort", GlobalNetwork.port);
        }
    }
}