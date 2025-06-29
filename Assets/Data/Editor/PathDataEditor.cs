using ATC.Operator.Pathway;
using UnityEditor;
using UnityEngine;

namespace Pathway {
#if UNITY_EDITOR
    [CustomEditor(typeof(PathData))]
    public class PathDataEditor : Editor {
        PathData pd;
        private bool drawPathToggle;

        private void OnEnable() {
            pd = (PathData)target;
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            if (GUILayout.Button("Create Test Path Visual", GUILayout.Height(30))) {
                GameObject newObject = new GameObject(pd.name);
                TestPathVisual tpv = newObject.AddComponent<TestPathVisual>();
                tpv.Initialize(pd);
            }
        }
    }
#endif
}