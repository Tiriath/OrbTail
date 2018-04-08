using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Reads the input data from a desktop platform
/// </summary>
class DesktopInputBroker: IInputBroker
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

    public void Update()
    {
        Acceleration = Input.GetAxis(Inputs.Throttle);
        Steering = Input.GetAxis(Inputs.Steer);
        Fire = Input.GetAxis(Inputs.Fire) > 0.0f;
        FireSpecial = Input.GetAxis(Inputs.FireSpecial) > 0.0f;
    }
}

