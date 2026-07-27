using UnityEngine;
using UnityEngine.InputSystem;

public class AnimatimeHandOnInput : MonoBehaviour
{
    public InputActionProperty triggerValue;
    public InputActionProperty gribValue;
    

    public Animator handAnimator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float grip = gribValue.action.ReadValue<float>();
        float trigger = triggerValue.action.ReadValue<float>();

        handAnimator.SetFloat("Grip", grip);
        handAnimator.SetFloat("Trigger", trigger);
        
    }
}
