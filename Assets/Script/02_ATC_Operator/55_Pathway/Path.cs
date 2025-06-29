// take idea and code from "catlikecoding"
// Link: catlikecoding.com/unity/tutorials/curves-and-splines

using Riptide;
using UnityEngine;

namespace Pathway {

    public enum PathType : ushort { linear, bezier }

    [System.Serializable]
    public struct Path {
        [SerializeField] internal PathType pathType;
        [SerializeField] internal Vector3 p0;
        [SerializeField] internal Vector3 p1;
        [SerializeField] internal Vector3 p2;
        [SerializeField] internal Vector3 p3;
        [SerializeField] internal float dist;
        [SerializeField] internal float distFraction;
        [SerializeField] internal Vector3 fwd_p0;
        [SerializeField] internal Vector3 fwd_p3;
        [SerializeField] internal Vector3 fwd_linear;
        [SerializeField] internal float totalAngle;
        private const ushort curveDistResolution = 50;
        private const ushort curveAngleResolution = 50;
        [SerializeField] internal ushort drawResolution;

        public Vector3 GetP0() { return p0; }
        public Vector3 GetP3() { return p3;}
        public Vector3 GetFWD_P0() { return fwd_p0; }
        public Vector3 GetFWD_P3() { return fwd_p3; }

        public void UpdateVariable(Vector3 rP0, Vector3 rP1, Vector3 rP2, Vector3 rP3, PathType rPathType) {
            this.p0 = rP0;
            this.p1 = rP1;
            this.p2 = rP2;
            this.p3 = rP3;
            pathType = rPathType;
            CalculateDistance();
        }



        public void CreateFromPathNode(PathNode pn01, PathNode pn02, PathType rPathType) {
            this.p0 = pn01.pos; p3 = pn02.pos;
            this.p1 = p0 + pn01.fwd * pn01.nextCtrlPointDist;
            this.p2 = p3 - pn02.fwd * pn02.prevCtrlPointDist;
            this.pathType = rPathType;
            CalculateDistance();
        }
        public void CreateFromPathNodeInverse(PathNode pn01, PathNode pn02, PathType rPathType) {
            this.p0 = pn01.pos; p3 = pn02.pos;
            this.p1 = p0 - pn01.fwd * pn01.prevCtrlPointDist;
            this.p2 = p3 + pn02.fwd * pn02.nextCtrlPointDist;
            this.pathType = rPathType;
            CalculateDistance();
        }


        public void CreateLinearPath(Vector3 fromPos, Vector3 toPos) {
            UpdateVariable(fromPos, fromPos, toPos, toPos, PathType.linear);
        }

        /*
        public void CreateFromPathToPos(IAPPath rFromPath, Vector3 rTgtPos, Vector3 rTgtDir, float rDist, PathType rPathType) {
            this.p0 = rFromPath.GetP3(); this.p3 = rTgtPos;
            this.p1 = p0 - rFromPath.GetFWD_P3() * rDist;
            this.p2 = p3 + rTgtDir * rDist;
            this.pathType = rPathType;
            CalculateDistance();
        }
        public void CreateFromPosToPath(Vector3 rTgtPos, Vector3 rTgtDir, float rDist, IAPPath rToPath, PathType rPathType) {
            this.p0 = rTgtPos; this.p3 = rToPath.GetP0();
            this.p1 = p0 + rTgtDir * rDist;
            this.p2 = p3 - rToPath.GetFWD_P0() * rDist;
            this.pathType = rPathType;
            CalculateDistance();
        }
        public void CreateFromPathToPath(IAPPath rFromPath, IAPPath rToPath, float rDist, PathType rPathType) {
            this.p0 = rFromPath.GetP3(); this.p3 = rToPath.GetP0();
            this.p1 = p0 + rFromPath.GetFWD_P3() * rDist;
            this.p2 = p3 - rToPath.GetFWD_P0() * rDist;
            this.pathType = rPathType;
            CalculateDistance();
        }

        */



        public void CalculateDistance() {
            if (pathType == PathType.linear) {
                dist = Vector3.Distance(p0, p3);
                distFraction = 1f / dist;
                drawResolution = 2;
            } else if (pathType == PathType.bezier) {
                dist = 0f;
                for (int i = 0; i < curveDistResolution; i++) {
                    Vector3 lp0 = BezierPos(i / (float)curveDistResolution);
                    Vector3 lp1 = BezierPos((i + 1) / (float)curveDistResolution);
                    dist += Vector3.Distance(lp0, lp1);
                }
                distFraction = 1f / dist;
                //
                totalAngle = 0f;
                for (int i = 0; i < curveAngleResolution; i++) {
                    Quaternion fwd0 = GetRotationAtTime(i / (float)curveAngleResolution);
                    Quaternion fwd1 = GetRotationAtTime((i + 1) / (float)curveAngleResolution);
                    totalAngle += Mathf.Abs(Quaternion.Angle(fwd0, fwd1));
                }
                totalAngle = totalAngle / curveAngleResolution;
                drawResolution = (ushort)(2 + Mathf.Round(5* totalAngle));
            }
            fwd_p0 = (p1 - p0).normalized;
            fwd_p3 = (p3 - p2).normalized;
            fwd_linear = (p3 - p0).normalized;
        }







        public Vector3 GetPointAtDistance(float rDist) {
            float t = rDist * distFraction;
            return GetPointAtTime(t);
        }
        public Quaternion GetRotationAtDistance(float rDist) {
            float t = rDist * distFraction;
            return GetRotationAtTime(t);
        }
        public Vector3 GetPointAtTime(float r) {
            if (pathType == PathType.bezier) {
                return BezierPos(r);
            } else {
                return Vector3.Lerp(p0, p3, r);
            }
        }
        public Quaternion GetRotationAtTime(float r) {
            if (pathType == PathType.bezier) {
                return BezierRot(r);
            } else {
               // Vector3 lookDir = Vector3.Lerp(fwd_p0, fwd_p3, r);
                //return Quaternion.LookRotation(lookDir, Vector3.up);
                return Quaternion.LookRotation(fwd_linear, Vector3.up);
            }
        }








        private Vector3 BezierPos(float r) {
            float t = Mathf.Clamp01(r);
            float tt = t * t;
            float ttt = t * t * t;
            //
            float d = 1 - t;
            float dd = d * d;
            float ddd = d * d * d;
            //
            Vector3 bezierPos = (ddd * p0) + (3 * dd * t * p1) + (3 * d * tt * p2) + (ttt * p3);
            return bezierPos;
        }

        private Quaternion BezierRot(float r) {
            float t = Mathf.Clamp01(r);
            //
            float d = (1f - t);
            float dd = d * d;
            //
            Vector3 derivative = (3f * dd * (p1 - p0)) + (6f * d * t * (p2 - p1)) + (3f * t * t * (p3 - p2));
            return Quaternion.LookRotation(derivative.normalized, Vector3.up);
        }







        internal void AddToMessage(Message msg) {
            msg.AddVector3(p0); msg.AddVector3(p1);
            msg.AddVector3(p2); msg.AddVector3(p3);
            msg.AddUShort((ushort)pathType);
        }
        internal void GetFromMessage(Message msg) {
            this.p0 = msg.GetVector3(); this.p1 = msg.GetVector3();
            this.p2 = msg.GetVector3(); this.p3 = msg.GetVector3();
            pathType = (PathType)msg.GetUShort();
            CalculateDistance();
        }
    }

}