using UnityEngine;
using UnityEngine.EventSystems;

public class SpriteDragScript : MonoBehaviour, IDragHandler
{
    [SerializeField] Canvas canvas;
    private RectTransform rectTransform;
    private Vector2 originalAnchoredPosition;

    [SerializeField] private float scaleSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalAnchoredPosition = rectTransform.anchoredPosition;
    }

    void IDragHandler.OnDrag(UnityEngine.EventSystems.PointerEventData eventData) {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void ResetPosition() {
        rectTransform.anchoredPosition = originalAnchoredPosition;
        rectTransform.localScale = new Vector3(1, 1, 1);
    }

    private void Update() {
        
        if (Input.GetAxis("Mouse ScrollWheel") > 0) {
            rectTransform.localScale += new Vector3(scaleSpeed, scaleSpeed, scaleSpeed);
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0) {
            rectTransform.localScale -= new Vector3(scaleSpeed, scaleSpeed, scaleSpeed);
        }
    }
}
