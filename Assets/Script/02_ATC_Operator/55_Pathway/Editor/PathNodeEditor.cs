// take idea and code from "catlikecoding"
// Link: catlikecoding.com/unity/tutorials/curves-and-splines

using UnityEditor;
using UnityEngine;

namespace Pathway {
    [CustomEditor(typeof(PathNode))]
    public class PathNodeEditor : Editor {
        private float snapValue = 0.1f;

        private void OnSceneGUI() {
            PathNode pathNode = target as PathNode;
            Quaternion handleRotation = pathNode.transform.rotation;

            Vector3 cpd0 = pathNode.pos - pathNode.fwd * pathNode.prevCtrlPointDist;
            Vector3 cpd1 = pathNode.pos + pathNode.fwd * pathNode.nextCtrlPointDist;

            EditorGUI.BeginChangeCheck();
            cpd0 = Handles.Slider(cpd0, -pathNode.fwd, pathNode.handleSize, Handles.CubeHandleCap, snapValue);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(pathNode, "Move Point");
                EditorUtility.SetDirty(pathNode);
                pathNode.prevCtrlPointDist = Vector3.Distance(pathNode.pos, cpd0);
            }

            EditorGUI.BeginChangeCheck();
            cpd1 = Handles.Slider(cpd1, pathNode.fwd, pathNode.handleSize, Handles.CubeHandleCap, snapValue);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(pathNode, "Move Point");
                EditorUtility.SetDirty(pathNode);
                pathNode.nextCtrlPointDist = Vector3.Distance(pathNode.pos, cpd1);
            }
        }
    }
}