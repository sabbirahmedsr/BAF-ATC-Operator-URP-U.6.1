using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ATC.Operator.MapView {
    public class Map_Node_DraggableInfo : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

        [Header("Text Variable")]
        [SerializeField] private TMP_Text txtCallSign;
        [SerializeField] private TMP_Text txtHeading;
        [SerializeField] private TMP_Text txtSpeed;
        [SerializeField] private TMP_Text txtHeight;
        [SerializeField] private TMP_Text txtNameAndTimeZ;

        [Header("Icon Variable")]
        [SerializeField] private RectTransform icnUpArrow;
        [SerializeField] private RectTransform icnDownArrow;
        [SerializeField] private RectTransform icnFlatArrow;

        [Header("Offset from Airplane Icon")]
        private Vector2 draggableOffset = new Vector2(-200f, -200f);
        private Vector2 lastAirplanePos;
        private bool _isDragging;


        [Header("Clamp Boundary")]
        private float minX = -300;
        private float maxX = 300;
        private float minY = -300;
        private float maxY = 300;


        [Header("Root Object")]
        private Canvas canvas;
        private RectTransform myRoot;
        private RectTransform mapNodeContainer;
        private Map_Node mapNode;





        internal void Initialize(Map_Node rMapNode) {
            mapNode = rMapNode;
            myRoot = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
            mapNodeContainer = rMapNode.mapNodeController.mapNodeContainer;
            CalculateClampBoundary();

            // set initial variable
            txtCallSign.text = rMapNode.apController.callSign.ToString();
            Update_AirplanePhaseNT(rMapNode.apController.lastNameAndTimeZ, rMapNode.apController.nextNameAndTimeZ);
            draggableOffset = Random.insideUnitCircle.normalized * Mathf.Lerp(50, 200, Random.value);
        }
        private void CalculateClampBoundary() {
            Vector3[] clampingAreaCorners = new Vector3[4];
            mapNodeContainer.GetLocalCorners(clampingAreaCorners);

            minX = clampingAreaCorners[0].x;
            maxX = clampingAreaCorners[2].x;
            minY = clampingAreaCorners[0].y;
            maxY = clampingAreaCorners[2].y;
        }








        public void OnBeginDrag(PointerEventData eventData) {
            _isDragging = true;
            myRoot.parent.SetAsLastSibling(); // Still good practice to bring to front
        }

        public void OnDrag(PointerEventData eventData) {
            // Convert screen point to local position within the clamping area's rect transform
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(mapNodeContainer, eventData.position,
                eventData.pressEventCamera, out Vector2 localPointerPos)) {

                // Calculate the new desired local position
                Vector2 desiredLocation = (Vector2)myRoot.localPosition + eventData.delta / canvas.scaleFactor;
                SetClampPosition(desiredLocation);
            }

            mapNode.OnChange_DraggableInfoPosition();
        }

        public void OnEndDrag(PointerEventData eventData) {
            if (_isDragging) {
                _isDragging = false;
                // RecordDraggableOffset
                draggableOffset = (Vector2)myRoot.localPosition - lastAirplanePos;
            }
        }







        private void SetClampPosition(Vector2 rDesiredPos) {
            Vector2 pivot = myRoot.pivot;
            Vector2 halfSize = myRoot.rect.size / 2f;
            float clampedX = Mathf.Clamp(rDesiredPos.x, minX + (pivot.x - 0.5f) * myRoot.rect.width, maxX + (pivot.x - 0.5f) * myRoot.rect.width);
            float clampedY = Mathf.Clamp(rDesiredPos.y, minY + (pivot.y - 0.5f) * myRoot.rect.height, maxY + (pivot.y - 0.5f) * myRoot.rect.height);
            // Apply the clamped position
            myRoot.localPosition = new Vector2(clampedX, clampedY);
        }






        internal void OnAirplanePositionUpdate(Vector2 _airplaneCenterPosition) {
            lastAirplanePos = _airplaneCenterPosition;
            if (!_isDragging) {
                SetClampPosition(lastAirplanePos + draggableOffset);
            }
        }








        internal void SetVizHeadSpeedHeight(VizHeadSpeedFL rVizHeadSpeedFL) {
            // set text
            txtHeading.text = $"{rVizHeadSpeedFL.heading}°";
            txtSpeed.text = $"{rVizHeadSpeedFL.speed}kts";
            txtHeight.text = $"{rVizHeadSpeedFL.FL}f";
            if (rVizHeadSpeedFL.flDirection == FLDirection.flat) {
                txtHeight.color = Color.white;
            } else if (rVizHeadSpeedFL.flDirection == FLDirection.upward) {
                txtHeight.color = Color.green;
            } else if (rVizHeadSpeedFL.flDirection == FLDirection.downward) {
                txtHeight.color = Color.red;
            }

            // set icon
            icnUpArrow.gameObject.SetActive(rVizHeadSpeedFL.flDirection == FLDirection.upward);
            icnDownArrow.gameObject.SetActive(rVizHeadSpeedFL.flDirection == FLDirection.downward);
            icnFlatArrow.gameObject.SetActive(rVizHeadSpeedFL.flDirection == FLDirection.flat);
        }


        internal void Update_AirplanePhaseNT(NameAndTimeZ rLastNT, NameAndTimeZ rNextNT) {
            txtNameAndTimeZ.text = $"Last: {rLastNT.name}, {rLastNT.timeZ}\n";
            txtNameAndTimeZ.text += $"Next: {rNextNT.name}, {rNextNT.timeZ}";
        }

    }
}