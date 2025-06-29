using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ATC.Operator.MapView {
    internal enum MapThemeEnum { none, day, night, satelite }

    [System.Serializable]
    public class Map_Model_Controller {
        internal List<string> MapThemeName = new List<string> { "NONE", "LITE", "DARK", "S.MAP" };

        [SerializeField] internal Map_Model_Holder[] allMapModelHolder;

        [Header("UI Reference")]
        [SerializeField] private TMP_Dropdown drdMapType = null;
        [SerializeField] private TMP_Dropdown drdMapTheme = null;

        private Map_Controller mapController;

        internal void Initialize(Map_Controller _mapController) {
            mapController = _mapController;

            // Setup initial variable
            /// Map type dropdown
            List<string> tmpMapTypeList = new List<string>();
            for (int i = 0; i < allMapModelHolder.Length; i++) {
                tmpMapTypeList.Add(allMapModelHolder[i].caption);
            }
            drdMapType.options.Clear();
            drdMapType.AddOptions(tmpMapTypeList);
            /// Map Theme dropdown
            drdMapTheme.options.Clear();
            drdMapTheme.AddOptions(MapThemeName);


            // Get set variable from PlayerPrefs
            int curMapType = PlayerPrefs.GetInt(mapController.mapType + nameof(drdMapType), 0);
            drdMapType.SetValueWithoutNotify(curMapType);
            int curMapTheme = PlayerPrefs.GetInt(mapController.mapType + nameof(drdMapTheme), 0);
            drdMapTheme.SetValueWithoutNotify(curMapTheme);
            SetMapTypeAndTheme(curMapType, curMapTheme);


            // Register Callback
            drdMapType.onValueChanged.AddListener(OnDrdChange_MapType);
            drdMapTheme.onValueChanged.AddListener(OnDrdChange_MapTheme);
        }

        private void OnDrdChange_MapType(int rIndex) {
            SetMapTypeAndTheme(rIndex, drdMapTheme.value);
        }
        private void OnDrdChange_MapTheme(int rIndex) {
            SetMapTypeAndTheme(drdMapType.value, rIndex);
        }

        internal void SetMapTypeAndTheme(int _mapTypeIndex, int _mapThemeIndex) {
            for (int i = 0; i < allMapModelHolder.Length; i++) {
                allMapModelHolder[i].Activate(i == _mapTypeIndex, _mapThemeIndex);
            }
            PlayerPrefs.SetInt(mapController.mapType + nameof(drdMapType), _mapTypeIndex);
            PlayerPrefs.SetInt(mapController.mapType + nameof(drdMapTheme), _mapThemeIndex);
            mapController.OnChange_ActiveMapCamera(allMapModelHolder[_mapTypeIndex].mapCamera);
        }
    }
}