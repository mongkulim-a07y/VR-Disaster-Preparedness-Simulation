using UnityEngine;

public class FireExtinguisherController : MonoBehaviour
{
    [Header("Spray Settings")]
    public Transform nozzle;              // empty GameObject at the tip of the extinguisher, forward = spray direction
    public float range = 5f;
    public LayerMask fireLayer;
    public ParticleSystem sprayVFX;        // optional: the visual spray effect

    private bool isSpraying = false;

    public void StartSpray()
    {
        isSpraying = true;
        if (sprayVFX != null) sprayVFX.Play();
    }

    public void StopSpray()
    {
        isSpraying = false;
        if (sprayVFX != null) sprayVFX.Stop();
    }

    void Update()
    {
        if (!isSpraying) return;

        RaycastHit hit;
        if (Physics.Raycast(nozzle.position, nozzle.forward, out hit, range, fireLayer))
        {
            ExtinguishableFire fire = hit.collider.GetComponent<ExtinguishableFire>();
            if (fire != null)
            {
                fire.ApplyExtinguish();
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (nozzle == null) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(nozzle.position, nozzle.position + nozzle.forward * range);
    }
}