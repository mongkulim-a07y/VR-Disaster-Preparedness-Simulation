using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;

public class FootstepSound : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] footstepClips;   // supports multiple sounds for variation

    [Header("Movement Reference")]
    public ContinuousMoveProvider moveProvider; // drag your Move Provider here

    [Header("Timing")]
    public float stepInterval = 0.5f;   // time between steps while walking
    public float minMoveThreshold = 0.1f; // ignore tiny stick drift

    private float stepTimer = 0f;

    void Update()
    {
        Vector2 input = moveProvider.leftHandMoveInput.ReadValue(); 
        // if using Right hand instead, swap this reference

        bool isMoving = input.magnitude > minMoveThreshold;

        if (isMoving)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                PlayFootstep();
                stepTimer = stepInterval;
            }
        }
        else
        {
            stepTimer = 0f; // reset so next move starts a step immediately
        }
    }

    void PlayFootstep()
    {
        if (footstepClips.Length == 0 || audioSource == null) return;

        AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
        audioSource.PlayOneShot(clip);
    }
}