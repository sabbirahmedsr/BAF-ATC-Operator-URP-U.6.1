using Pathway;
using UnityEngine;

namespace Pathway {
    [System.Serializable]
    public struct PathSequence {
        [SerializeField] internal Path[] allPath;
        [SerializeField] internal float[] allDistFraction;
        [SerializeField] internal float totalDistance;



        public Vector3 GetP0() {
            return allPath[0].GetP0();
        }
        public Vector3 GetP3() {
            return allPath[allPath.Length - 1].GetP3();
        }
        public Vector3 GetFWD_P0() {
            return allPath[0].GetFWD_P0();
        }
        public Vector3 GetFWD_P3() {
            return allPath[allPath.Length - 1].GetFWD_P3();
        }





        public Vector3 GetPointAtTime(float r) {
            for (int i = 0; i < allDistFraction.Length; i++) {
                if (r <= allDistFraction[i]) {
                    float ratio = r / allDistFraction[i];
                    return allPath[i].GetPointAtTime(ratio);
                }
                r -= allDistFraction[i];
            }
            return allPath[allPath.Length - 1].GetPointAtTime(1f);
        }

        public Quaternion GetRotationAtTime(float r) {
            for (int i = 0; i < allDistFraction.Length; i++) {
                if (r <= allDistFraction[i]) {
                    float ratio = r / allDistFraction[i];
                    return allPath[i].GetRotationAtTime(ratio);
                }
                r -= allDistFraction[i];
            }
            return allPath[allPath.Length - 1].GetRotationAtTime(1f);
        }







        public void CreateFromPathNode(PathNode[] rAllPathNode, PathType rPathType) {
            allPath = new Path[rAllPathNode.Length - 1];
            for (int i = 0; i < allPath.Length; i++) {
                allPath[i].CreateFromPathNode(rAllPathNode[i], rAllPathNode[i + 1], rPathType);
            }
            CalculateDistanceFraction();
        }
        public void CreateFromPathArray(Path[] rAllPath) {
            allPath = rAllPath;
            CalculateDistanceFraction();
        }
        private void CalculateDistanceFraction() {
            totalDistance = 0f;
            for (int i = 0; i < allPath.Length; i++) {
                totalDistance += allPath[i].dist;
            }
            allDistFraction = new float[allPath.Length];
            float additiveFraction = 0f;
            for (int i = 0; i < allDistFraction.Length - 1; i++) {
                allDistFraction[i] = allPath[i].dist / totalDistance;
                additiveFraction += allDistFraction[i];
            }
            allDistFraction[allDistFraction.Length - 1] = 1f - additiveFraction;
        }
    }
}