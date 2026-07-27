using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning; // Required for XRI 3.0+ turning providers

public class SetTurnTypeFromPlayerPref : MonoBehaviour
{
    public SnapTurnProvider snapTurn;
    public ContinuousTurnProvider continuousTurn;

    // Start is called before the first frame update
    void Start()
    {
        ApplyPlayerPref();
    }

    public void ApplyPlayerPref()
    {
        if (PlayerPrefs.HasKey("turn"))
        {
            int value = PlayerPrefs.GetInt("turn");

            if (value == 0) // Snap Turn
            {
                if (snapTurn != null) snapTurn.enabled = true;
                if (continuousTurn != null) continuousTurn.enabled = false;
            }
            else if (value == 1) // Continuous Turn
            {
                if (snapTurn != null) snapTurn.enabled = false;
                if (continuousTurn != null) continuousTurn.enabled = true;
            }
        }
    }
}