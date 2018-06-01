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
    /// Returns the powerup input status. 1 using the power up, 0 not using the power up.
    /// </summary>
    bool PowerUpInput { get; }

    /// <summary>
    /// Updates the input status.
    /// </summary>
    void Update();
}
