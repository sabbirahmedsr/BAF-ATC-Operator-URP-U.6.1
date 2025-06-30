using AirplaneControllerScript;
using UnityEngine;

namespace ATC.Operator.MapView {
    internal enum MapType { radarMap, surfaceMap}

    public class Map_Controller : MonoBehaviour {
        [Header("Basic")]
        [SerializeField] internal MapType mapType;
        [SerializeField] private Canvas uiCanvas;

        [Header("Child Script")]
        [SerializeField] internal Map_Model_Controller mapModelController;
        [SerializeField] internal Map_Node_Controller mapNodeController;
        internal Camera activeMapCamera;

        internal void Initialize() {

            // Initialize Child Script
            mapModelController.Initialize(this);
            mapNodeController.Initialize(this);

            // Add global listener
            if (mapType == MapType.radarMap) {
                GlobalEvent.onRadarMapNodeCreatedEvent += CreateMapNode;
            } else if (mapType == MapType.surfaceMap) {
                GlobalEvent.onSurfaceMapNodeCreatedEvent += CreateMapNode;
            }
        }

        private void CreateMapNode(AirplaneController rApController, ushort rStartIndex) {
            mapNodeController.CreateMapNode(rApController);            
        }

        internal void OnChange_ActiveMapCamera(Camera rActiveMapCamera) {
            activeMapCamera = rActiveMapCamera;
            mapNodeController.OnChange_ActiveMapCamera(rActiveMapCamera);
            // set approprite camera to render map canvas ui
            uiCanvas.worldCamera = activeMapCamera;
        }

        internal GameObject Instantiate_GO(GameObject rPrefab, Transform rParent) {
            GameObject newGo = Instantiate(rPrefab, rParent);
            return newGo;
        }
    }
}
