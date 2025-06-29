using UnityEngine;

namespace ATC.Operator.MapView {
    public class Map_Node_AirplaneIcon : MonoBehaviour {
        internal Vector2 localPos { get { return iconRoot.localPosition; } }

        private Camera activeMapCamera;
        private RectTransform container;
        private RectTransform iconRoot;
        private float rotationOffset;

        [Header("For Optimization")]
        private Vector3 lastWorldPos;
        private Vector3 lastFwd;
        private Vector2 lastScreenPos;

        // reference of parent script
        private Map_Node mapNode;

        internal void Initialize(Map_Node rMapNode) {
            mapNode = rMapNode;
            iconRoot = GetComponent<RectTransform>();
            container = rMapNode.mapNodeController.mapNodeContainer;
            activeMapCamera = rMapNode.mapNodeController.mapController.activeMapCamera;
            rotationOffset = activeMapCamera.transform.localEulerAngles.y;
        }

        internal void OnUpdate_MapNodePosRot(Vector3 rWorldPos, Vector3 rFwd) {
            // if we have same location and rotation of the previous frame, skip the further operation
            if (rWorldPos == lastWorldPos && rFwd == lastFwd) {
                return;
            }
            // store the position and fwd for comparing in next frame
            lastWorldPos = rWorldPos;
            lastFwd = rFwd;

            // get screen potision
            Vector3 screenPos = activeMapCamera.WorldToScreenPoint(rWorldPos);
            // if the screen position has not changed a pixel, we will skip further calculation
            if (Vector2.Distance(screenPos, lastScreenPos) < 2) {
                return;
            }
            // store the screen position for next frame comparison
            lastScreenPos = screenPos;

            // set position
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(container, screenPos, activeMapCamera, out Vector2 localPos)) {
                iconRoot.localPosition = localPos;
            }

            // set icon rotation
            float angle = Mathf.Atan2(rFwd.x, rFwd.z) * Mathf.Rad2Deg;
            float finalAngle = -angle + rotationOffset;
            iconRoot.localEulerAngles = new Vector3(0, 0, finalAngle);

            // share self position change info with parent script
            mapNode.OnChange_AirplaneIconPosition();
        }

        internal void OnChange_ActiveMapCamera(Camera rActiveMapCamera) {
            activeMapCamera = rActiveMapCamera;
            lastWorldPos = Vector3.positiveInfinity; lastFwd = Vector3.positiveInfinity;
            OnUpdate_MapNodePosRot(mapNode.apController.position, mapNode.apController.fwd);
        }
    }
}