using UnityEngine;
using System.Collections;

/// <summary>
/// Represents a ship engine subsystem.
/// </summary>
public interface IEngineDriver : IDriver
{
    /// <summary>
    /// Get or set the current input value in the range [-1; +1].
    /// </summary>
    float Input { get; set; }

    /// <summary>
    /// Get target ship speed, in units per second.
    /// </summary>
    float Speed { get; }

    /// <summary>
    /// Get effective engine thrust given the current ship speed.
    /// </summary>
    /// <param name="current_speed">Current ship speed.</param>
    /// <returns>Returns the effective engine thrust.</returns>
    float GetThrust(float current_speed, float delta_time);
}
