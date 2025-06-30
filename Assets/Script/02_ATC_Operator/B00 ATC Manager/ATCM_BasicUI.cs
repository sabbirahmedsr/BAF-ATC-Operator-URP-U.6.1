using ATC.Global;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ATCManagerScript {
    public class ATCM_BasicUI : MonoBehaviour {

        [Header("Option UI")]
        [SerializeField] private Button btnOption;
        [SerializeField] private OptionUI_Update optionUIUpdate;

        [Header("Speed")]
        [SerializeField] private SpeedUI speedUI;

        [Header("Exit")]
        [SerializeField] private Button btnExitRequest;
        [Space]
        [SerializeField] private Transform tExitWarningRoot;
        [SerializeField] private Button btnExitConfirm;
        [SerializeField] private Button btnExitCancel;

        private void Awake() {
            RectTransform btnRect = btnOption.GetComponent<RectTransform>();
            optionUIUpdate.ManualStart(btnRect);
            AddButtonListener();
            speedUI.Initialize();
        }

        private void AddButtonListener() {
            btnExitRequest.onClick.AddListener(ExitRequest);
            btnExitConfirm.onClick.AddListener(ExitConfirm);
            btnExitCancel.onClick.AddListener(ExitCancel);
            btnOption.onClick.AddListener(delegate {
                if (optionUIUpdate.gameObject.activeSelf) {
                    optionUIUpdate.ToggleActivation(false);
                } else {
                    optionUIUpdate.ToggleActivation(true);
                }
            });
        }


        // ExitAndStopServer
        private void ExitRequest() {
            tExitWarningRoot.gameObject.SetActive(true);
        }
        private void ExitConfirm() {
            GlobalNetwork.networkManager.Disconnect();
            SceneManager.LoadScene(GlobalSceneName.home);
        }
        private void ExitCancel() {
            tExitWarningRoot.gameObject.SetActive(false);
        }

    }
}