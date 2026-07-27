using UnityEngine;

public class ExtinguishableFire : MonoBehaviour
{
    [Header("Extinguish Settings")]
    public float health = 100f;
    public float extinguishRatePerSecond = 25f;

    private bool isExtinguished = false;

    public void ApplyExtinguish()
    {
        if (isExtinguished) return;

        health -= extinguishRatePerSecond * Time.deltaTime;

        if (health <= 0f)
        {
            isExtinguished = true;
            Debug.Log(gameObject.name + " extinguished!");
            gameObject.SetActive(false); // FireTriggerRandomizer detects this automatically
        }
    }
}