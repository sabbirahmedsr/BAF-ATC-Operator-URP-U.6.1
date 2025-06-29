using UnityEditor;
using UnityEngine;

namespace Pathway {
#if UNITY_EDITOR
    [CustomEditor(typeof(ParkingPathData))]
    public class ParkingPathDataEditor : Editor {
        ParkingPathData ppd;

        private void OnEnable() {
            ppd = (ParkingPathData)target;
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            if (GUILayout.Button("Rename Variable", GUILayout.Height(30))) {
                ppd.RenameValirable();
            }
            if (GUILayout.Button("Create Debug Path", GUILayout.Height(30))) {
                ppd.CreateDebugPath();
            }
        }
    }
#endif
}