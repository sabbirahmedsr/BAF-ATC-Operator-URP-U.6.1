using UnityEngine;

namespace Pathway {
    public class PathNode : MonoBehaviour {
        public Vector3 pos { get { return transform.position; } }
        public Vector3 fwd { get { return transform.forward; } }

        public float prevCtrlPointDist = 500f;
        public float nextCtrlPointDist = 500f;

        [Space()]
        public float handleSize = 50f;
    }
}