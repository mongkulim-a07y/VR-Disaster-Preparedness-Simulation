using UnityEngine;
using System.Collections;

public class FireExtinguisherController : MonoBehaviour
{
    [Header("Spray Settings")]
    public Transform nozzle;
    public float range = 5f;
    public LayerMask fireLayer;
    public ParticleSystem sprayVFX;

    [Header("Audio")]
    public AudioSource blastAudio;   // one-shot burst at start
    public AudioSource sprayLoopAudio; // continuous loop after blast

    private bool isSpraying = false;
    private Coroutine spraySequenceRoutine;

    public void StartSpray()
    {
        isSpraying = true;
        if (sprayVFX != null) sprayVFX.Play();

        // Stop anything currently playing/queued, then start fresh from the blast
        if (spraySequenceRoutine != null) StopCoroutine(spraySequenceRoutine);
        spraySequenceRoutine = StartCoroutine(PlaySpraySequence());
    }

    public void StopSpray()
    {
        isSpraying = false;
        if (sprayVFX != null) sprayVFX.Stop();

        if (spraySequenceRoutine != null)
        {
            StopCoroutine(spraySequenceRoutine);
            spraySequenceRoutine = null;
        }

        if (blastAudio != null) blastAudio.Stop();
        if (sprayLoopAudio != null) sprayLoopAudio.Stop();
    }

    IEnumerator PlaySpraySequence()
    {
        float blastLength = 0f;

        if (blastAudio != null && blastAudio.clip != null)
        {
            blastAudio.Play();
            blastLength = blastAudio.clip.length;
        }

        // Wait for the blast to finish before starting the loop
        yield return new WaitForSeconds(blastLength);

        if (sprayLoopAudio != null)
        {
            sprayLoopAudio.Play();
        }
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