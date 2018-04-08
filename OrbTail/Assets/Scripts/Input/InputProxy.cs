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
    public float Acceleration { get; private set; }

    /// <summary>
    /// Returns the steering command's status. -1 steer left, 0 no steering, 1 steer right
    /// </summary>
    public float Steering { get; private set; }

    /// <summary>
    /// Returns the fire input status. 0 not firing, 1 firing.
    /// </summary>
    public bool Fire { get; private set; }

    /// <summary>
    /// Returns the fire special input status. 0 not firing, 1 firing.
    /// </summary>
    public bool FireSpecial { get; private set; }

    public void Start ()
    {
        if (!GetComponent<PlayerIdentity>().IsHuman)
        {
            InputBroker = GetComponent<PlayerAI>().GetInputBroker();        // AI.
        }
        else if (SystemInfo.supportsAccelerometer)
        {
            InputBroker = new MobileInputBroker();                          // Mobile.
        }
        else
        {
            InputBroker = new DesktopInputBroker();                         // Desktop.
        }
    }
    
    public void Update ()
    {
        if (InputBroker != null)
        {
            InputBroker.Update();

            Acceleration = InputBroker.Acceleration;
            Steering = InputBroker.Steering;
            Fire = InputBroker.Fire;
            FireSpecial = InputBroker.FireSpecial;
        }
    }

    /// <summary>
    /// The input broker used to read user's or AI's input.
    /// </summary>
    private IInputBroker InputBroker { get; set; }
}
