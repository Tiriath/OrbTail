using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Facade over the various input systems (AI, mobile, desktop, etc.)
/// </summary>
public class InputProxy : MonoBehaviour, IInputBroker
{
    /// <summary>
    /// Returns the acceleration command's status. 0 no acceleration, 1 maximum acceleration.
    /// </summary>
    public float ThrottleInput
    {
        get
        {
            return (InputBroker != null) ? InputBroker.ThrottleInput : 0.0f;
        }
    }

    /// <summary>
    /// Returns the steering command's status. -1 steer left, 0 no steering, 1 steer right
    /// </summary>
    public float SteerInput
    {
        get
        {
            return (InputBroker != null) ? InputBroker.SteerInput : 0.0f;
        }
    }

    /// <summary>
    /// Returns the fire input status. 0 not firing, 1 firing.
    /// </summary>
    public bool FireInput
    {
        get
        {
            return (InputBroker != null) ? InputBroker.FireInput : false;
        }
    }

    /// <summary>
    /// Returns the fire special input status. 0 not firing, 1 firing.
    /// </summary>
    public bool SpecialInput
    {
        get
        {
            return (InputBroker != null) ? InputBroker.SpecialInput : false;
        }
    }

    public void Start ()
    {
        var AI = GetComponent<PlayerAI>();

        if (AI)
        {
            InputBroker = AI;                               // AI.
        }
        else if (SystemInfo.supportsAccelerometer)
        {
            InputBroker = new MobileInputBroker();          // Mobile.
        }
        else
        {
            InputBroker = new DesktopInputBroker();         // Desktop.
        }
    }

    void Update()
    {
        UpdateInput();
    }

    public void UpdateInput ()
    {
        if (InputBroker != null)
        {
            InputBroker.UpdateInput();
        }
    }

    /// <summary>
    /// Concrete input handler.
    /// </summary>
    private IInputBroker InputBroker = null;
}
