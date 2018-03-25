using UnityEngine;
using System.Collections;

/// <summary>
/// Represents a ship steering subsystem.
/// </summary>
public interface ISteerDriver : IDriver
{
    /// <summary>
    /// Get ship maximum steering, in radians per second.
    /// </summary>
    /// <returns>Returns ship maximum steering, in radians per second.</returns>
    float GetMaxSteer();

    /// <summary>
    /// Get the factor used to smooth out change in steering, in error percentage per second.
    /// </summary>
    /// <returns>Returns the factor used to smooth out change in steering.</returns>
    float GetSteerSmooth();

    /// <summary>
    /// Get current ship steering, in radians per second.
    /// </summary>
    /// <returns>Returns current ship steering, in radians per second.</returns>
    float GetSteer();

    /// <summary>
    /// Get current steer input value. Range [-1;1].
    /// </summary>
    /// <returns>Returns current steer input value.</returns>
    float GetSteerInput();

    /// <summary>
    /// Update engine status.
    /// </summary>
    /// <param name="current_steer">Current ship steer.</param>
    /// <param name="steer_input">Current steer input. Range [-1;1].</param>
    void Update(float current_steer, float steer_input);
}
