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

    private int lastIndex = -1;

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
        Debug.Log("Waiting initial delay: " + initialDelay + " seconds...");
        yield return new WaitForSeconds(initialDelay);
        TriggerRandomFire();
        while (true)
        {
            float waitTime = Random.Range(minInterval, maxInterval);
            Debug.Log("Waiting " + waitTime + " seconds before next fire trigger...");
            yield return new WaitForSeconds(waitTime);
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