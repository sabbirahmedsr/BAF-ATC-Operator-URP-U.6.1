using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace CommandControllerScript {
    public class DynamicButton : MonoBehaviour {
        [Header("Root Variable")]
        [SerializeField] internal RectTransform rectTransform;
      //  [SerializeField] internal Button myButton;

        [Header("Interaction Variable")]
        [SerializeField] private TMP_Text txtCaption;
        [SerializeField] internal UnityEvent<ushort, ushort> onClick = new UnityEvent<ushort, ushort>();
        [SerializeField] internal UnityEvent<ushort, ushort> onHover = new UnityEvent<ushort, ushort>();
        private ushort index;
        private ushort value;

        internal void SetTextAndValue(string rCaption, ushort rIndex, ushort rValue) {
            txtCaption.text = rCaption;
            index = rIndex; value = rValue;
        }

        public void OnHover() {
            onHover.Invoke(index, value);
         //   myButton.Select();
        }
        public void OnClick() {
            onClick.Invoke(index, value);
        }
    }
}