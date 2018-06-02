using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Reads the input data from a desktop platform
/// </summary>
class DesktopInputBroker: IInputBroker
{
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
        input_manager = new InputManager(controller_id);
    }

    public void Update()
    {
        ThrottleInput = input_manager.Throttle;
        SteerInput = input_manager.Steer;
        PowerUpInput = input_manager.PowerUp;
    }

    /// <summary>
    /// Input manager for the player controlling this ship.
    /// </summary>
    private InputManager input_manager;
}

