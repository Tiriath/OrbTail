using UnityEngine;
using System.Collections;

/// <summary>
/// Driver that inverts any steering value.
/// </summary>
public class InvertedSteerDriver : BaseDriver, ISteerDriver
{
    /// <summary>
    /// Get or set the current input value in the range [-1; +1].
    /// </summary>
    public float Input
    {
        get
        {
            return driver.Input;
        }
        set
        {
            driver.Input = value;
        }
    }

    /// <summary>
    /// Get target ship steering, in radians per second.
    /// </summary>
    public virtual float Steer
    {
        get
        {
            return -driver.Steer;
        }
    }

    /// <summary>
    /// Create a new inverted steer driver.
    /// </summary>
    public InvertedSteerDriver(ISteerDriver driver)
    {
        this.driver = driver;
    }

    public float GetSteer(float current_steer, float delta_time)
    {
        return -driver.GetSteer(current_steer, delta_time);
    }

    private ISteerDriver driver;
}
