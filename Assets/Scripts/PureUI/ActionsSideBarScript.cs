using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActionsSideBarScript : MonoBehaviour
{
    [SerializeField] private RectTransform panel;          // The panel to slide
    [SerializeField] Vector2 hiddenPosition;       // Where the panel is when hidden
    [SerializeField] Vector2 shownPosition;        // Where the panel is when shown
    [SerializeField] float slideDuration = 0.5f;   // Duration of the slide animation

    private Coroutine slideCoroutine;
    private bool isVisible = false;

    // Call this on Action Button click
    public void OnClickActionSideBarButton()
    {
        if (!isVisible)
        {
            if (slideCoroutine != null) StopCoroutine(slideCoroutine);
            slideCoroutine = StartCoroutine(SlidePanel(panel.anchoredPosition, shownPosition));
            isVisible = true;
        }
    }

    // Call this on Close Button click
    public void OnClickActionSideBarCloseButton()
    {
        if (isVisible)
        {
            if (slideCoroutine != null) StopCoroutine(slideCoroutine);
            slideCoroutine = StartCoroutine(SlidePanel(panel.anchoredPosition, hiddenPosition));
            isVisible = false;
        }
    }

    private IEnumerator SlidePanel(Vector2 from, Vector2 to)
    {
        float elapsed = 0f;
        while (elapsed < slideDuration)
        {
            panel.anchoredPosition = Vector2.Lerp(from, to, elapsed / slideDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        panel.anchoredPosition = to;
    }
}


