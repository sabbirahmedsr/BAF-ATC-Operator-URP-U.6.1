using UnityEngine;
using System.Collections.Generic;
using AirplaneControllerScript;

namespace ATC.Operator.MapView {
    [System.Serializable]
    public class Map_Node_Controller {
        [SerializeField] internal GameObject mapNodePrefab;
        [SerializeField] internal RectTransform mapNodeContainer;

        private MapNodeReference mapNodeReference = new MapNodeReference();

        internal Map_Controller mapController;

        internal void Initialize(Map_Controller _mapController) {
            mapController = _mapController;
        }

        internal void OnChange_ActiveMapCamera(Camera rActiveMapCamera) {
            foreach (Map_Node mapNode in mapNodeReference.activeMapNode) {
                 mapNode.OnChange_ActiveMapCamera(rActiveMapCamera);
            }
        }

        internal void CreateMapNode(AirplaneController rApController) {
            GameObject newMapNodeObj = mapController.Instantiate_GO(mapNodePrefab, mapNodeContainer);
            Map_Node newMapNode = newMapNodeObj.GetComponent<Map_Node>();
            newMapNode.Initialize(this, rApController);
            //
            mapNodeReference.activeMapNode.Add(newMapNode);
        }
    }

    internal class MapNodeReference {
        internal List<Map_Node> activeMapNode = new List<Map_Node>();
        internal List<Map_Node> mapNodePool = new List<Map_Node>();
    }
}