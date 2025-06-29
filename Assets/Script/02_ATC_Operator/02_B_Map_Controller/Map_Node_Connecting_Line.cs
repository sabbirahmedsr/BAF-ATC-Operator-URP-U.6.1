using UnityEngine;

namespace ATC.Operator.MapView {
    public class Map_Node_Connecting_Line : MonoBehaviour {
        [SerializeField] private RectTransform capRoot;
        [SerializeField] private RectTransform infoRoot;
        [SerializeField] private RectTransform apIconRoot; 

        private RectTransform myRoot; // The RectTransform of the Map Node (the source of the line)
        private SimpleUILineRenderer uiLineRenderer; // Assign your UILineRenderer component here

        Map_Node mapNode;

        internal void Initialize(Map_Node rMapNode) {
            mapNode = rMapNode;
            myRoot = GetComponent<RectTransform>();
            uiLineRenderer = GetComponent<SimpleUILineRenderer>();
        }

        public void UpdateConnectingLine() {
            Vector2 edgePoint = FindEdgePoint();
            capRoot.localPosition = edgePoint;
            uiLineRenderer.SetPoints(edgePoint, apIconRoot.localPosition);
        }


        // This method finds the intersection point of a ray originating from the center of infoRoot
        private Vector3 FindEdgePoint() {

            // nodeCenter and targetPoint are in the local space of MapNode
            Vector3 nodeCenter = infoRoot.localPosition;
            Vector3 targetPoint = apIconRoot.localPosition;
            Vector3 direction = (targetPoint - nodeCenter).normalized;


            // Let's get the bounds of infoRoot in its parent's local space (MapNode's local space)
            Bounds infoRootBounds = RectTransformUtility.CalculateRelativeRectTransformBounds(infoRoot.parent as RectTransform, infoRoot);

            float minX = infoRootBounds.min.x;
            float maxX = infoRootBounds.max.x;
            float minY = infoRootBounds.min.y;
            float maxY = infoRootBounds.max.y;

            // For ray-AABB intersection, `nodeCenter` should be the origin of the ray.
            // `direction` is already normalized.

            float tNear = float.MinValue;
            float tFar = float.MaxValue;

            // X-axis intersection
            if (Mathf.Abs(direction.x) < Mathf.Epsilon) { // Ray is parallel to Y-axis
                if (nodeCenter.x < minX || nodeCenter.x > maxX) {
                    // Ray is parallel to Y-axis and outside the X bounds, no intersection
                    return nodeCenter; // Fallback, no intersection
                }
            } else {
                float t1 = (minX - nodeCenter.x) / direction.x;
                float t2 = (maxX - nodeCenter.x) / direction.x;

                tNear = Mathf.Max(tNear, Mathf.Min(t1, t2));
                tFar = Mathf.Min(tFar, Mathf.Max(t1, t2));
            }

            // Y-axis intersection
            if (Mathf.Abs(direction.y) < Mathf.Epsilon) { // Ray is parallel to X-axis
                if (nodeCenter.y < minY || nodeCenter.y > maxY) {
                    // Ray is parallel to X-axis and outside the Y bounds, no intersection
                    return nodeCenter; // Fallback, no intersection
                }
            } else {
                float t1 = (minY - nodeCenter.y) / direction.y;
                float t2 = (maxY - nodeCenter.y) / direction.y;

                tNear = Mathf.Max(tNear, Mathf.Min(t1, t2));
                tFar = Mathf.Min(tFar, Mathf.Max(t1, t2));
            }

            // If tNear > tFar, there's no intersection along the positive ray direction.
            // If tFar < 0, the box is entirely behind the ray origin.
            if (tNear > tFar || tFar < 0) {
                return nodeCenter; // No valid forward intersection, return original center or other fallback
            }

            // Determine the final 't' value.
            // If the ray origin (nodeCenter) is inside the rectangle, tNear will be negative.
            // In that case, we want the point where the ray *exits* the rectangle, which is at tFar.
            // If the ray origin is outside, tNear will be the first positive intersection point.
            float finalT = tNear;
            if (finalT < 0) {
                finalT = tFar;
            }

            // Calculate the intersection point in MapNode's local space
            Vector3 intersectionPoint = nodeCenter + direction * finalT;
            return intersectionPoint;
        }




    }   

}