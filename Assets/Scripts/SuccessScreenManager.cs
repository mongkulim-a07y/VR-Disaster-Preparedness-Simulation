using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class SuccessScreen : MonoBehaviour
{
    [Header("References")]
    public CanvasGroup successPanel;
    public TMP_Text subtitleText;

    [Header("Settings")]
    public float returnDelay = 5f;
    public string mainMenuSceneName = "MainMenu";

    private Coroutine countdownRoutine;

    public void ShowSuccessScreen()
    {
        if (successPanel != null)
        {
            successPanel.alpha = 1f;
            successPanel.blocksRaycasts = true;
            successPanel.interactable = true;
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