using UnityEngine;
using System.Collections;

public class FireTriggerRandomizer : MonoBehaviour
{
    [Header("Fire VFX Objects (already placed in scene)")]
    public GameObject[] firePoints;

    [Header("Trigger Settings")]
    public float initialDelay = 30f;
    public float minInterval = 15f;
    public float maxInterval = 45f;
    [Header("Notification")]
    public FireWarningNotification fireWarning;

    [Header("Evacuation Timer")]
    public EvacuationTimer evacuationTimer;

    private bool firstFireTriggered = false;

    private int lastIndex = -1;
    private bool triggeringStopped = false;

    void Start()
    {
        Debug.Log("FireTriggerRandomizer Start() called. Fire points count: " + firePoints.Length);

        foreach (GameObject fire in firePoints)
        {
            if (fire != null) fire.SetActive(false);
        }

        StartCoroutine(FireTriggerLoop());
    }

    IEnumerator FireTriggerLoop()
    {
        yield return new WaitForSeconds(initialDelay);
        TriggerRandomFire();

        while (!triggeringStopped)
        {
            float waitTime = Random.Range(minInterval, maxInterval);
            float elapsed = 0f;
            GameObject currentFire = firePoints[lastIndex];

            while (elapsed < waitTime)
            {
                if (currentFire != null && !currentFire.activeSelf)
                {
                    Debug.Log("Fire was put out before the next one could spawn. Stopping fire trigger system.");
                    triggeringStopped = true;
                    if (evacuationTimer != null) evacuationTimer.StopTimer();
                    yield break;
                }

                elapsed += Time.deltaTime;
                yield return null;
            }

            TriggerRandomFire();
        }
    }
    void TriggerRandomFire()
    {
        if (firePoints.Length == 0)
        {
            Debug.LogWarning("No fire points assigned!");
            return;
        }

        int index = GetRandomIndexNoRepeat();
        Debug.Log("Triggering fire at index: " + index + " (" + firePoints[index].name + ")");
        firePoints[index].SetActive(true);
        if (!firstFireTriggered)
        {
            firstFireTriggered = true;
            if (fireWarning != null) fireWarning.ShowWarning();
            if (evacuationTimer != null) evacuationTimer.StartTimer();
        }
        
    }

    int GetRandomIndexNoRepeat()
    {
        int index;
        do
        {
            index = Random.Range(0, firePoints.Length);
        } while (index == lastIndex && firePoints.Length > 1);

        lastIndex = index;
        return index;
    }
}