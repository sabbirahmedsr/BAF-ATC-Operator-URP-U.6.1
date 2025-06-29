using UnityEditor;
using UnityEngine;

namespace AirplaneControllerScript {

    [CustomEditor(typeof(AirplaneData))]
    public class AirplaneLifecycleEditor : Editor {
        AirplaneData alc;

        private void OnEnable() {
            alc = (AirplaneData)target;
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            if (GUILayout.Button("Set Stage Name", GUILayout.Height(30))) {
                Debug.Log("Do Nothing");
            }

        }
    }

}