using UnityEngine;
using System.Collections;

/// <summary>
/// Driver that acts as a default wheel driver except that it returns a steering value which is opposite to the provided input.
/// Can be used to model a debuff requiring the ship to deal with inverted commands!
/// </summary>
public class InvertedSteerDriver : DefaultSteerDriver
{
    /// <summary>
    /// Create a new inverted steer driver.
    /// </summary>
    /// <param name="max_steering_speed">Maximum steering speed in radians per second</param>
    /// <param name="max_steering_acceleration">Maximum steering acceleration in radians per second squared</param>
    public InvertedSteerDriver(float max_steering_speed, float max_steering_acceleration)
        : base(max_steering_speed, max_steering_acceleration)
    {

    }

    public override float GetSteer()
    {
        return -base.GetSteer();
    }
}
