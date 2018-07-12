using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Facade over the various input systems (AI, mobile, desktop, etc.)
/// </summary>
public class InputProxy : NetworkBehaviour, IInputBroker
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
    /// Returns the power input status.
    /// </summary>
    public bool PowerUpInput
    {
        get
        {
            var input_value = (InputBroker != null) ? InputBroker.PowerUpInput : false;

            input_value |= PowerUpSignal;

            PowerUpSignal = false;              // Consume the signal at the first read.

            return input_value;
        }
    }

    /// <summary>
    /// Value used to signal the power input status externally (from HUD, for example)
    /// </summary>
    public bool PowerUpSignal{ private get; set; }

    public override void OnStartLocalPlayer()
    {
        var AI = GetComponent<PlayerAI>();

        if (AI)
        {
            InputBroker = AI;                                               // AI.
        }
        else if (SystemInfo.supportsAccelerometer)
        {
            InputBroker = new MobileInputBroker();                          // Mobile.
        }
        else
        {
            InputBroker = new DesktopInputBroker(playerControllerId);       // Desktop.
        }
    }

    public void Update()
    {
        if (InputBroker != null)
        {
            InputBroker.Update();
        }
    }

    /// <summary>
    /// Concrete input handler.
    /// </summary>
    private IInputBroker InputBroker = null;
}
