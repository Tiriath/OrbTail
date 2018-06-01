using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Reads the input data from a desktop platform
/// </summary>
class DesktopInputBroker: IInputBroker
{
    // Base action names.

    public const string Throttle = "Throttle";
    public const string Steer = "Steer";
    public const string PowerUp = "PowerUp";

    // Prefixes (device and player index)

    public const string KeyboardPrefix = "K1_";
    public static readonly string[] JoystickPrefix = new string[] { "J1_", "J2_", "J3_", "J4_" };

    /// <summary>
    /// Returns the throttle input status. -1 backwards, 0 still, +1 maximum throttle.
    /// </summary>
    public float ThrottleInput { get; private set; }

    /// <summary>
    /// Returns the steer input status. -1 steer left, +1 steer right.
    /// </summary>
    public float SteerInput { get; private set; }

    /// <summary>
    /// Returns the fire input status.
    /// </summary>
    public bool PowerUpInput { get; private set; }

    /// <summary>
    /// Create a new input broker used to read the input status on a desktop.
    /// </summary>
    /// <param name="controller_id">Id of the player to read the input from.</param>
    public DesktopInputBroker(short controller_id)
    {
        if(controller_id == 0)
        {
            throttle_axis.Add(KeyboardPrefix + Throttle);
            steer_axis.Add(KeyboardPrefix + Steer);
            powerup_button.Add(KeyboardPrefix + PowerUp);
        }

        throttle_axis.Add(JoystickPrefix[controller_id] + Throttle);
        steer_axis.Add(JoystickPrefix[controller_id] + Steer);
        powerup_button.Add(JoystickPrefix[controller_id] + PowerUp);
    }

    public void Update()
    {
        ThrottleInput = Mathf.Clamp(throttle_axis.Sum(axis => Input.GetAxis(axis)), -1.0f, 1.0f);
        SteerInput = Mathf.Clamp(steer_axis.Sum(axis => Input.GetAxis(axis)), -1.0f, 1.0f);
        PowerUpInput = powerup_button.Any(button => Input.GetButtonDown(button));
    }

    private List<string> throttle_axis = new List<string>();

    private List<string> steer_axis = new List<string>();

    private List<string> powerup_button = new List<string>();
}

