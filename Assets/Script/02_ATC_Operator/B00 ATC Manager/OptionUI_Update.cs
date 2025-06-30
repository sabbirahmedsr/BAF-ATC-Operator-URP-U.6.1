using UnityEngine;
using UnityEngine.EventSystems;

public class OptionUI_Update : MonoBehaviour{
    private enum AnimationState { disabled, transit_InAnimation, isFullyVisible, transit_OutAnimation }

    [Header("Basic Variable")]
    private AnimationState animState = AnimationState.disabled;
    private RectTransform myRectTransform;
    private RectTransform buttonRectTransform;
    private float counter = 0f;
    private float fadeSpeed = 5f;

    [Header("transition in out animation")]
    [SerializeField] private bool inverseDirection = false;
    private Vector3 defaultPosition;
    private Vector3 hidePosition;

    internal void ManualStart(RectTransform rButtonRect) {
        myRectTransform = GetComponent<RectTransform>();
        buttonRectTransform = rButtonRect;
        //
        defaultPosition = myRectTransform.anchoredPosition;
        int inverseMult = inverseDirection ? -1 : 1;
        float width = myRectTransform.rect.width + 10f;
        hidePosition = defaultPosition + Vector3.left * width * inverseMult;
        myRectTransform.anchoredPosition = hidePosition;

        gameObject.SetActive(false);
    }

    internal void ToggleActivation(bool rIsActive) {
        if (rIsActive) {
            if (animState == AnimationState.disabled) {
                gameObject.SetActive(true);
                animState = AnimationState.transit_InAnimation;
            }
        } else {
            if (animState == AnimationState.isFullyVisible || animState == AnimationState.transit_InAnimation) {
                animState = AnimationState.transit_OutAnimation;
            }
        }
    }

    private void Update() {
        if (animState == AnimationState.disabled) {
            return;
        } else if (animState == AnimationState.transit_InAnimation) {
            counter += Time.deltaTime * fadeSpeed;
            AnimationUpdate();
            ListenForOutsideClick();
            if (counter >= 1f) {
                counter = 1f;
                animState = AnimationState.isFullyVisible;
            }
        } else if (animState == AnimationState.isFullyVisible) {
            ListenForOutsideClick();
        } else if (animState == AnimationState.transit_OutAnimation) {
            counter -= Time.deltaTime * fadeSpeed;
            AnimationUpdate();
            if (counter <= 0f) {
                counter = 0f;
                animState = AnimationState.disabled;
                gameObject.SetActive(false);
            }
        }
    }

    private void AnimationUpdate() {
        myRectTransform.anchoredPosition = Vector3.Lerp(hidePosition, defaultPosition, counter);
    }

    private void ListenForOutsideClick() {
        if (Input.GetMouseButtonDown(0)) {
            // Is the mouse pointer over a UI element?
            // This is crucial to prevent false positives when clicking on other UI elements.
            if (!EventSystem.current.IsPointerOverGameObject()) {
                bool clickInsideMyRect = RectTransformUtility.RectangleContainsScreenPoint(myRectTransform, Input.mousePosition);
                if (!clickInsideMyRect) {
                    ToggleActivation(false);
                }
            }

        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ToggleActivation(false);
        }
    }

}
