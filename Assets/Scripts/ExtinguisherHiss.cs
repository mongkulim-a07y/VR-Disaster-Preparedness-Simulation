using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ExtinguisherHiss : MonoBehaviour
{
    [Header("Audio Settings")]
    [Range(0f, 1f)]
    public float volume = 0.2f;

    [Header("Air Hiss Filter Settings")]
    [Range(100f, 10000f)]
    public float cutoffFrequency = 3000f; // High frequency filter for realistic air hiss

    private System.Random random = new System.Random();
    private AudioSource audioSource;
    private bool isSpraying = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1.0f; // 3D sound in VR
    }

    // Call this from your VR Activate Event (Trigger Press)
    public void StartHiss()
    {
        isSpraying = true;
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    // Call this from your VR Deactivate Event (Trigger Release)
    public void StopHiss()
    {
        isSpraying = false;
        audioSource.Stop();
    }

    // Unity audio thread: Generates procedural high-pressure white noise
    void OnAudioFilterRead(float[] data, int channels)
    {
        if (!isSpraying)
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = 0f;
            }
            return;
        }

        // Generate filtered air pressure noise
        for (int i = 0; i < data.Length; i += channels)
        {
            float noiseSample = (float)(random.NextDouble() * 2.0 - 1.0) * volume;

            for (int channel = 0; channel < channels; channel++)
            {
                data[i + channel] = noiseSample;
            }
        }
    }
}