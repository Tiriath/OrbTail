using UnityEngine;
using System.Collections;

/// <summary>
/// Default steering subsystem for a ship.
/// </summary>
public class DefaultSteerDriver : BaseDriver, ISteerDriver
{
    /// <summary>
    /// Get or set the current input value in the range [-1; +1].
    /// </summary>
    public float Input { get; set; }

    /// <summary>
    /// Get target ship steering, in radians per second.
    /// </summary>
    public virtual float Steer
    {
        get
        {
            return max_steer * Input;
        }
    }

    public DefaultSteerDriver(float max_steer, float steer_smooth)
    {
        this.max_steer = max_steer;
        this.steer_smooth = steer_smooth;
    }

    public float GetSteer(float current_steer, float delta_time)
    {
        var steer = (Steer - current_steer) * steer_smooth * delta_time;

        return Mathf.Clamp(steer, -max_steer, +max_steer);
    }

    /// <summary>
    /// Maximum steering, in radians per second.
    /// </summary>
    private float max_steer;

    /// <summary>
    /// Factor used to smooth out change in steering, in error percentage per second.
    /// </summary>
    private float steer_smooth;
}
