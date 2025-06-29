using UnityEngine;

namespace ATC.Home {
    public class HC_SubClass : MonoBehaviour {
        internal Transform myRoot;
        internal HomeController mainController;

        internal virtual void Initialize(HomeController _mc) {
            myRoot = transform;
            mainController = _mc;
        }

        internal void Activate(bool rBool) {
            myRoot.gameObject.SetActive(rBool);
        }

    }
}