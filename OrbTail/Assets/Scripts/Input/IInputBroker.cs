using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Brokes the input of various players
/// </summary>
public interface IInputBroker
{

    /// <summary>
    /// Returns the acceleration command's status. 0 no acceleration, 1 maximum acceleration.
    /// </summary>
    float ThrottleInput { get; }

    /// <summary>
    /// Returns the steering command's status. -1 steer left, 0 no steering, 1 steer right
    /// </summary>
    float SteerInput { get; }

    /// <summary>
    /// Returns the fire input status. 0 not firing, 1 firing.
    /// </summary>
    bool FireInput { get; }

    /// <summary>
    /// Returns the fire special input status. 0 not firing, 1 firing.
    /// </summary>
    bool SpecialInput { get; }

    /// <summary>
    /// Updates the input status.
    /// </summary>
    void UpdateInput();
}
