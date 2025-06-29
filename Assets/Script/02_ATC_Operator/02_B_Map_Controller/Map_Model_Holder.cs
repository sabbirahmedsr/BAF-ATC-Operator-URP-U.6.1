using UnityEngine;

namespace ATC.Operator.MapView {
    public class Map_Model_Holder : MonoBehaviour {
        [Tooltip("UI Dropdown option name")]
        [SerializeField] internal string caption;

        [Header("Theme variation object")]
        [Tooltip("map model in no theme ")]
        [SerializeField] internal Transform tNoThemeObject;
        [Tooltip("map model in day theme ")]
        [SerializeField] internal Transform tDayThemeObject;
        [Tooltip("map model in night theme ")]
        [SerializeField] internal Transform tNightThemeObject;
        [Tooltip("map model in satelite theme ")]
        [SerializeField] internal Transform tSateliteThemeObject;

        [Header("Camera")]
        [Tooltip("Top view camera of the map")]
        [SerializeField] internal Camera mapCamera;

        internal void Activate(bool rBool, int themeIndex) {
            gameObject.SetActive(rBool);
            if (rBool) {
                tNoThemeObject.gameObject.SetActive(themeIndex == (int)MapThemeEnum.none);
                tDayThemeObject.gameObject.SetActive(themeIndex == (int)MapThemeEnum.day);
                tNightThemeObject.gameObject.SetActive(themeIndex == (int)MapThemeEnum.night);
                tSateliteThemeObject.gameObject.SetActive(themeIndex == (int)MapThemeEnum.satelite);
            }
        }
    }
}