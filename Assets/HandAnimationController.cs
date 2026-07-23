using UnityEngine;
using UnityEngine.XR;

public class HandAnimationController : MonoBehaviour
{
    public XRNode handNode;

    private InputDevice targetDevice;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        targetDevice = InputDevices.GetDeviceAtXRNode(handNode);
    }

    void Update()
    {
        if (!targetDevice.isValid)
        {
            targetDevice = InputDevices.GetDeviceAtXRNode(handNode);
        }

        // Grip
        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            animator.SetFloat("Grip", gripValue);
        }

        // Trigger
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            animator.SetFloat("Trigger", triggerValue);
        }
    }
}