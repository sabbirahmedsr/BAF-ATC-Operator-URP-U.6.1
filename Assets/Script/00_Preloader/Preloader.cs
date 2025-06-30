using ATC.Global;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PreloaderScript {
    public class Preloader : MonoBehaviour {
        [SerializeField] private Sprite[] logos;
        [SerializeField] private Image imgLogo;
        //
        private Color colorAlpha = new Color(1, 1, 1, 0);
        private Color colorWhite = new Color(1, 1, 1, 1);

        private void Start() {
            Application.targetFrameRate = 60;

            {// activate display
                Display[] allDisplay = Display.displays;
                foreach (Display display in allDisplay) {
                    display.Activate();
                }
                if (allDisplay.Length >= 2) {
                    int width = allDisplay[1].systemWidth;
                    int height = width / 16 * 3;
                    allDisplay[1].SetParams(width, height, 0, 0);
                }
            }
            {
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
                StartCoroutine(StartAsync());
            }
        }


        IEnumerator StartAsync() {
            yield return LogoTransition();
            SceneManager.LoadScene(GlobalSceneName.home);
        }

        IEnumerator LogoTransition() {
            yield return new WaitForSeconds(1f);

            imgLogo.color = colorAlpha;
            for (int i = 0; i < logos.Length; i++) {
                imgLogo.sprite = logos[i];

                var r = 0f;
                while (r < 1.1f) {
                    if (r < 0.16f) {
                        imgLogo.color = Color.Lerp(colorAlpha, colorWhite, r / 0.15f);
                    } else if (r > 0.75f) {
                        imgLogo.color = Color.Lerp(colorAlpha, colorWhite, (1f - r) / 0.15f);
                    }

                    r += Time.deltaTime * 0.2f;
                    yield return new WaitForEndOfFrame();
                }
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForEndOfFrame();
        }
    }
}