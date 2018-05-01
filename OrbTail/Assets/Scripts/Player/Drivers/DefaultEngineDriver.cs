using UnityEngine;
using System.Collections;

/// <summary>
/// Default engine that uses an acceleration input to propel the ship forward.
/// </summary>
public class DefaultEngineDriver : BaseDriver, IEngineDriver
{
    /// <summary>
    /// Set the current input value in the range [-1; +1].
    /// </summary>
    public float Input { get; set; }

    /// <summary>
    /// Get target ship speed, in units per second.
    /// </summary>
    public float Speed
    {
        get
        {
            return max_speed * Input;
        }
    }

    public DefaultEngineDriver(float max_speed, float speed_smooth)
    {
        this.max_speed = max_speed;
        this.speed_smooth = speed_smooth;
    }

    /// <summary>
    /// Get effective engine thrust given the current ship speed.
    /// </summary>
    /// <param name="current_speed">Current ship speed.</param>
    /// <returns>Returns the effective engine thrust.</returns>
    public float GetThrust(float current_speed, float delta_time)
    {
        var thrust = (Speed - current_speed) * speed_smooth * delta_time;

        return Mathf.Clamp(thrust, -max_speed, +max_speed);
    }

    /// <summary>
    /// Maximum thrust, in units per second.
    /// </summary>
    private float max_speed;

    /// <summary>
    /// Factor used to smooth out change in speed, in error percentage per second.
    /// </summary>
    private float speed_smooth;
}
