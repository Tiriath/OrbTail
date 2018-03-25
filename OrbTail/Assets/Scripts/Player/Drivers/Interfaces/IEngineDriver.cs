using UnityEngine;
using System.Collections;

/// <summary>
/// Represents a ship engine subsystem.
/// </summary>
public interface IEngineDriver : IDriver
{
    /// <summary>
    /// Get ship maximum speed, in units per second.
    /// </summary>
    /// <returns>Returns ship maximum speed, in units per second.</returns>
    float GetMaxSpeed();

    /// <summary>
    /// Get the factor used to smooth out change in speed, in error percentage per second.
    /// </summary>
    /// <returns>Returns the factor used to smooth out change in speed.</returns>
    float GetSpeedSmooth();

    /// <summary>
    /// Get current ship speed, in units per second.
    /// </summary>
    /// <returns>Returns current ship speed, in units per second.</returns>
    float GetSpeed();

    /// <summary>
    /// Get current speed input value. Range [-1;1].
    /// </summary>
    /// <returns>Returns current speed input value.</returns>
    float GetSpeedInput();

    /// <summary>
    /// Update engine status.
    /// </summary>
    /// <param name="current_speed">Current ship speed.</param>
    /// <param name="speed_input">Current speed input. Range [-1;1].</param>
    void Update(float current_speed, float speed_input);
}
