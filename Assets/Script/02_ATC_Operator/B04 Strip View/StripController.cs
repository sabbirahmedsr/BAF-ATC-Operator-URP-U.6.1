using AirplaneControllerScript;
using StripViewScript.Child;
using System.Collections.Generic;
using UnityEngine;

namespace StripViewScript {
    public class StripController : MonoBehaviour {
        [SerializeField] private Camera infoCamera;
        [SerializeField] private Canvas infoUICanvas;

        [Header("Strip Node")]
        [SerializeField] private GameObject stripNodePrefab;
        [SerializeField] private Transform tStripContentRoot;
        private List<StripNode> allStripNode = new List<StripNode>();

        internal void Initialize() {
            GlobalEvent.onAirplaneCreatedEvent += OnAirplaneCreated;
        }
        private void OnDisable() {
            GlobalEvent.onAirplaneCreatedEvent -= OnAirplaneCreated;
        }



        private void OnAirplaneCreated(ushort apID, AirplaneController rAPController) {
            if (rAPController.movementType == MovementType.arrival && GlobalData.operatorType == OperatorType.departure)
                return;
            if (rAPController.movementType == MovementType.departure && GlobalData.operatorType == OperatorType.arrival)
                return;
            GameObject newStripNodeObject = Instantiate(stripNodePrefab, tStripContentRoot);
            StripNode stripNode = newStripNodeObject.GetComponent<StripNode>();
            stripNode.Initialize(this, rAPController);
            allStripNode.Add(stripNode);
        }
    }
}