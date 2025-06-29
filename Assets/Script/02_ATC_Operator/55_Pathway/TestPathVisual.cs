
using Pathway;
using UnityEngine;

#if UNITY_EDITOR
namespace ATC.Operator.Pathway {
    [RequireComponent (typeof(BoxCollider))]
    public class TestPathVisual : MonoBehaviour {
        public PathData pathData;
        public PathSequence myPathSequence;
        [SerializeField] private PathGizmo pathGizmo;

        private void OnDrawGizmosSelected() {
            pathGizmo.DrawPathSequence(myPathSequence);
        }

        public void Initialize(PathData rPathData) {
            pathData = rPathData;
            myPathSequence = pathData.pathSequences;
            SetTransformAndBoxCollider(); 
        }

        public void Initialize(Path[] rAllPath) {
            myPathSequence.CreateFromPathArray(rAllPath);
            SetTransformAndBoxCollider();
        }

        private void SetTransformAndBoxCollider() {
            transform.position = myPathSequence.GetPointAtTime(0.5f);
            BoxCollider bCol = gameObject.AddComponent<BoxCollider>();
            bCol.size = Vector3.one * Vector3.Distance(myPathSequence.GetP0(), myPathSequence.GetP3()) * 0.5f;
            pathGizmo.sphereSize = myPathSequence.totalDistance * 0.1f;
        }
    }
}
#endif