using UnityEngine;

public class ExitZone : MonoBehaviour
{
    public EvacuationTimer evacuationTimer;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            evacuationTimer.PlayerEvacuated();
        }
    }
}