using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ATC.Home {
    public class SceneLoader : MonoBehaviour {

        [SerializeField] private TMP_Text txtLoadingCaption;
        [SerializeField] private Image imgLoadingBar;
        private string[] allLoadingString = new string[] {
            "Loading Airport...", "Loading Radar Map...", "Loading Surface Map...", "Loading Data..."
        };


        [Header("Animation & Time")]
        [SerializeField] private AnimationCurve airportLoadingCurve;
        private float asyncTime = 0f;
        private float loadingTime = 3f;
        private float asyncThreshold = 0f;


        private string targetSceneName = "";
        internal void LoadSceneAsync(string rSceneName) {
            targetSceneName = rSceneName;
            gameObject.SetActive(true);
            StartCoroutine(StartOperator_Async());
        }



        private IEnumerator StartOperator_Async() {
            yield return new WaitForEndOfFrame();

            float totalSegment = allLoadingString.Length + 1;
            float segmentRatio = 1f / totalSegment;

            for (int i = 0; i < allLoadingString.Length; i++) {
                txtLoadingCaption.text = allLoadingString[i];
                asyncThreshold = segmentRatio * (i + 1);
                while (asyncTime < asyncThreshold) {
                    asyncTime += Time.deltaTime / loadingTime;
                    imgLoadingBar.fillAmount = airportLoadingCurve.Evaluate(asyncTime);
                    yield return new WaitForSeconds(Time.deltaTime);
                }
            }

            txtLoadingCaption.text = "Getting Ready...";
            AsyncOperation async = SceneManager.LoadSceneAsync(targetSceneName);
            while (!async.isDone) {
                float ratio = asyncTime + async.progress * segmentRatio;
                imgLoadingBar.fillAmount = airportLoadingCurve.Evaluate(ratio);
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
    }
}