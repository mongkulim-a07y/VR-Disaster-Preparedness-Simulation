using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    [Header("References")]
    public CanvasGroup gameOverPanel;
    public TMP_Text subtitleText;

    [Header("Settings")]
    public float returnDelay = 5f;
    public string mainMenuSceneName = "1 Start Scene";

    private Coroutine countdownRoutine;

    public void ShowGameOverScreen()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.alpha = 1f;
            gameOverPanel.blocksRaycasts = true;
            gameOverPanel.interactable = true;
        }

        if (countdownRoutine != null) StopCoroutine(countdownRoutine);
        countdownRoutine = StartCoroutine(CountdownAndReturn());
    }

    IEnumerator CountdownAndReturn()
    {
        float remaining = returnDelay;

        while (remaining > 0f)
        {
            if (subtitleText != null)
                subtitleText.text = "Returning to main menu in " + Mathf.CeilToInt(remaining) + "s";

            yield return null;
            remaining -= Time.deltaTime;
        }

        SceneManager.LoadScene(mainMenuSceneName);
    }
}