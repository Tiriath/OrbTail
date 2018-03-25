using UnityEngine;
using System.Collections;

/// <summary>
/// Default engine that uses an acceleration input to propel the ship forward.
/// </summary>
public class DefaultEngineDriver : BaseDriver, IEngineDriver
{
    public DefaultEngineDriver(float max_speed, float speed_smooth)
    {
        this.max_speed = max_speed;
        this.speed_smooth = speed_smooth;
    }

    public float GetMaxSpeed()
    {
        return max_speed;
    }

    public float GetSpeedSmooth()
    {
        return speed_smooth;
    }

    public virtual float GetSpeed()
    {
        return speed;
    }

    public float GetSpeedInput()
    {
        return speed_input;
    }

    public void Update(float current_speed, float speed_input)
    {
        this.speed_input = speed_input;

        var target_speed = max_speed * speed_input;

        speed = (target_speed - current_speed) * speed_smooth * Time.deltaTime;

        speed = Mathf.Clamp(speed, -max_speed, +max_speed);
    }

    /// <summary>
    /// Maximum speed, in units per second.
    /// </summary>
    private float max_speed;

    /// <summary>
    /// Factor used to smooth out change in speed, in error percentage per second.
    /// </summary>
    private float speed_smooth;

    /// <summary>
    /// Current speed, in units per second.
    /// </summary>
    private float speed;

    /// <summary>
    /// Current throttle.
    /// </summary>
    private float speed_input;
}
