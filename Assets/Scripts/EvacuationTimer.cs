using UnityEngine;
using System.Collections;
using TMPro;

public class EvacuationTimer : MonoBehaviour
{
    [Header("Evacuation UI")]
    public CanvasGroup evacuationPanel;
    public CanvasGroup graceTimerCorner;
    public TMP_Text graceTimerText;

    [Header("Game Over UI")]
    public CanvasGroup gameOverPanel;

    [Header("Success UI")]
    public CanvasGroup successPanel;

    [Header("Settings")]
    public float timeLimit = 100f;
    public float evacuationGraceTime = 10f;

    [Header("Evacuation Panel Fade Timing")]
    public float fadeInTime = 0.3f;
    public float holdTime = 2.5f;
    public float fadeOutTime = 1.0f;

    private float timeRemaining;
    private float graceTimeRemaining;
    private bool timerRunning = false;
    private bool graceRunning = false;
    private bool hasEnded = false;

    void Start()
    {
        SetPanelInstant(evacuationPanel, false);
        SetPanelInstant(graceTimerCorner, false);
        SetPanelInstant(gameOverPanel, false);
        SetPanelInstant(successPanel, false);
    }

    public void StartTimer()
    {
        timeRemaining = timeLimit;
        timerRunning = true;
        graceRunning = false;
        hasEnded = false;

        SetPanelInstant(evacuationPanel, false);
        SetPanelInstant(graceTimerCorner, false);
        SetPanelInstant(gameOverPanel, false);
        SetPanelInstant(successPanel, false);
    }

    public void StopTimer()
    {
        timerRunning = false;
        graceRunning = false;
        CancelInvoke(nameof(TriggerGameOver));
        StopAllCoroutines();
        SetPanelInstant(evacuationPanel, false);
        SetPanelInstant(graceTimerCorner, false);
    }

    public void PlayerEvacuated()
    {
        if (hasEnded) return;
        hasEnded = true;

        StopTimer();
        SetPanelInstant(gameOverPanel, false);
        SetPanelInstant(successPanel, true);
        Debug.Log("Player evacuated successfully!");
    }

    void Update()
    {
        if (timerRunning)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0f)
            {
                timeRemaining = 0f;
                timerRunning = false;
                ShowEvacuationWarning();
            }
        }
        else if (graceRunning)
        {
            graceTimeRemaining -= Time.deltaTime;

            if (graceTimeRemaining <= 0f)
            {
                graceTimeRemaining = 0f;
                graceRunning = false;
            }

            if (graceTimerText != null)
                graceTimerText.text = Mathf.CeilToInt(graceTimeRemaining).ToString();
        }
    }

    void ShowEvacuationWarning()
    {
        // Center panel: brief fade in/hold/out
        StartCoroutine(FadeSequence(evacuationPanel));

        // Corner timer: stays visible the whole grace period
        SetPanelInstant(graceTimerCorner, true);

        graceTimeRemaining = evacuationGraceTime;
        graceRunning = true;

        Invoke(nameof(TriggerGameOver), evacuationGraceTime);
    }

    IEnumerator FadeSequence(CanvasGroup panel)
    {
        if (panel == null) yield break;

        yield return Fade(panel, 0f, 1f, fadeInTime);
        yield return new WaitForSeconds(holdTime);
        yield return Fade(panel, 1f, 0f, fadeOutTime);
    }

    IEnumerator Fade(CanvasGroup panel, float from, float to, float duration)
    {
        float elapsed = 0f;
        panel.blocksRaycasts = to > 0f;
        panel.interactable = to > 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            panel.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }
        panel.alpha = to;
    }

    void TriggerGameOver()
    {
        if (hasEnded) return;
        hasEnded = true;
        graceRunning = false;

        SetPanelInstant(evacuationPanel, false);
        SetPanelInstant(graceTimerCorner, false);
        SetPanelInstant(gameOverPanel, true);
        Debug.Log("GAME OVER: Player failed to evacuate in time.");
    }

    void SetPanelInstant(CanvasGroup panel, bool visible)
    {
        if (panel == null) return;
        panel.alpha = visible ? 1f : 0f;
        panel.blocksRaycasts = visible;
        panel.interactable = visible;
    }
}