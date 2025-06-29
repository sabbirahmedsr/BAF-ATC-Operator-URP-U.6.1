using UnityEngine;

#if UNITY_EDITOR
namespace Pathway {
    [System.Serializable]   
    public struct PathGizmo {
        [Range(0f, 1f)]
        [SerializeField] internal float testRatio;
        [SerializeField] internal float sphereSize;

        internal void DrawPathSequence(PathSequence pathSequence) {
            for (int i = 0; i < pathSequence.allPath.Length; i++) {
                Gizmos.color = Color.white;
                Path path = pathSequence.allPath[i];
                if (path.pathType == PathType.linear) {
                    Gizmos.DrawLine(path.GetPointAtTime(0f), path.GetPointAtTime(1f));
                } else if (path.pathType == PathType.bezier) {
                    float fraction = 1f / (path.drawResolution - 1);
                    for (int j = 1; j < path.drawResolution; j++) {
                        Vector3 lp0 = path.GetPointAtTime(fraction * (j - 1));
                        Vector3 lp1 = path.GetPointAtTime(fraction * j);
                        Gizmos.DrawLine(lp0, lp1);
                    }
                }
                Gizmos.DrawWireSphere(path.GetPointAtTime(0), sphereSize);
                Gizmos.DrawWireSphere(path.GetPointAtTime(1), sphereSize / 2f);
            }
            Gizmos.color = Color.green;
            Vector3 testP0 = pathSequence.GetPointAtTime(testRatio);
            Vector3 testP1 = testP0 + pathSequence.GetRotationAtTime(testRatio) * Vector3.forward * pathSequence.totalDistance * 0.3f;
            Gizmos.DrawLine(testP0, testP1);
            Gizmos.DrawWireSphere(testP0, sphereSize * 0.5f);

        }
    }
}
#endif