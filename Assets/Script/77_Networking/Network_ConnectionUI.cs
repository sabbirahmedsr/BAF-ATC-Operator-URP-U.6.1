using ATC.Global;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ATC.Operator.Networking {
    /// <summary>
    /// Show UI at each state of
    /// OnConnectionChange event
    /// </summary>

    [System.Serializable]
    public class Network_ConnectionUI : MonoBehaviour {
        [SerializeField] private RectTransform canvasRoot;

        [Header("Text Reference")]
        [SerializeField] private RectTransform tConnectingRoot;
        [SerializeField] private TMP_Text txtIPandPort;

        [Header("Connection Success")]
        [SerializeField] private RectTransform tConnectionSuccessRoot;
        [SerializeField] private Button btnOk;

        [Header("Button Reference")]
        [SerializeField] private RectTransform tConnectionFailureRoot;
        [SerializeField] private TMP_Text txtReason;
        [SerializeField] private Button btnRetry;
        [SerializeField] private Button btnExit;

        private NetworkManager_ForClient networkManager;
        internal void Initialize(NetworkManager_ForClient rNetworkManager) {
            networkManager = rNetworkManager;

            // register callback
            btnOk.onClick.AddListener(OnClick_Ok);
            btnRetry.onClick.AddListener(OnClick_RetryNow);
            btnExit.onClick.AddListener(OnClick_Exit);

            GlobalNetwork.onConnectionEvent += OnConnectionChange;
        }



        private void OnClick_Ok() {
            canvasRoot.gameObject.SetActive(false);
        }
        private void OnClick_RetryNow() {
            networkManager.StartOrRetryConnection();
        }
        private void OnClick_Exit() {
            SceneManager.LoadScene(GlobalSceneName.home);
        }



        internal void OnConnectionChange(ConnectionStatus rConnectionStatus, string rHeadingString, string rReason) {
            // enable disable connection ui
            tConnectingRoot.gameObject.SetActive(rConnectionStatus==ConnectionStatus.attemptConnection);
            tConnectionSuccessRoot.gameObject.SetActive(rConnectionStatus == ConnectionStatus.connected);
            bool isFailure = rConnectionStatus == ConnectionStatus.disconnected || rConnectionStatus == ConnectionStatus.connectionFailed;
            tConnectionFailureRoot.gameObject.SetActive(isFailure);

            if (rConnectionStatus == ConnectionStatus.attemptConnection) {
                txtIPandPort.text = "Server IP   : " + GlobalNetwork.ip + "\n";
                txtIPandPort.text += "Server Port : " + GlobalNetwork.port;
            } else if (isFailure) {
                txtReason.text = rHeadingString + " : " + rReason;
            }
            canvasRoot.gameObject.SetActive(true);
        }

    }
}