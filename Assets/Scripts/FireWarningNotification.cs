using UnityEngine;
using System.Collections;

public class FireWarningNotification : MonoBehaviour
{
    [Header("References")]
    public CanvasGroup warningPanel;

    [Header("Timing")]
    public float fadeInTime = 0.4f;
    public float holdTime = 2.5f;
    public float fadeOutTime = 1.0f;

    public void ShowWarning()
    {
        StopAllCoroutines();
        StartCoroutine(FadeSequence());
    }

    IEnumerator FadeSequence()
    {
        yield return Fade(0f, 1f, fadeInTime);
        yield return new WaitForSeconds(holdTime);
        yield return Fade(1f, 0f, fadeOutTime);
    }

    IEnumerator Fade(float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            warningPanel.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }
        warningPanel.alpha = to;
    }
}