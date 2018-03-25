using UnityEngine;
using System.Collections;

/// <summary>
/// Default steering subsystem for a ship.
/// </summary>
public class DefaultSteerDriver : BaseDriver, ISteerDriver
{
    public DefaultSteerDriver(float max_steer, float steer_smooth)
    {
        this.max_steer = max_steer;
        this.steer_smooth = steer_smooth;
    }

    public float GetMaxSteer()
    {
        return max_steer;
    }

    public float GetSteerSmooth()
    {
        return steer_smooth;
    }

    public virtual float GetSteer()
    {
        return steer;
    }

    public float GetSteerInput()
    {
        return steer_input;
    }

    public void Update(float current_steer, float steer_input)
    {
        this.steer_input = steer_input;

        var target_speed = max_steer * steer_input;

        steer = (target_speed - current_steer) * steer_smooth * Time.deltaTime;

        steer = Mathf.Clamp(steer, -max_steer, +max_steer);
    }

    /// <summary>
    /// Maximum steering, in radians per second.
    /// </summary>
    private float max_steer;

    /// <summary>
    /// Factor used to smooth out change in steering, in error percentage per second.
    /// </summary>
    private float steer_smooth;

    /// <summary>
    /// Current steering, in radians per second.
    /// </summary>
    private float steer;

    /// <summary>
    /// Current throttle.
    /// </summary>
    private float steer_input;
}
