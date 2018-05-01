using UnityEngine;
using System.Collections;

/// <summary>
/// Represents a ship steering subsystem.
/// </summary>
public interface ISteerDriver : IDriver
{
    /// <summary>
    /// Get or set the current input value in the range [-1; +1].
    /// </summary>
    float Input { get; set; }

    /// <summary>
    /// Get target ship steering, in radians per second.
    /// </summary>
    float Steer { get; }

    /// <summary>
    /// Get effective steer given the current ship's one.
    /// </summary>
    /// <param name="current_steer">Current ship steer.</param>
    /// <returns>Returns the effective steer.</returns>
    float GetSteer(float current_steer, float delta_time);
}
