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
    public bool FireInput { get; private set; }

    /// <summary>
    /// Returns the special input status.
    /// </summary>
    public bool SpecialInput { get; private set; }

    public void UpdateInput()
    {
        ThrottleInput = Input.GetAxis(Inputs.Throttle);
        SteerInput = Input.GetAxis(Inputs.Steer);
        FireInput = Input.GetButtonDown(Inputs.Fire);
        SpecialInput = Input.GetButtonDown(Inputs.Special);
    }
}

