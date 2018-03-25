using UnityEngine;
using System.Collections;

/// <summary>
/// Driver used to calculate standard damage when two ships collide.
/// </summary>
public class DefaultOffenceDriver : BaseDriver, IOffenceDriver
{
    public DefaultOffenceDriver(int offence)
    {
        this.offence = offence;
        this.normalized_offence = Mathf.Sqrt(offence / 5.0f);
    }

    public int GetOffence()
    {
        return offence;
    }

    public float GetDamage(GameObject defender, Collision col)
    {
        // #TODO Magic formula!

        var velocity = Mathf.Min(col.relativeVelocity.magnitude, max_velocity);

        return (velocity / max_velocity) * max_orbs * normalized_offence;
    }

    /// <summary>
    /// Ship offence value. Range [1;5].
    /// </summary>
    private int offence;

    /// <summary>
    /// Normalize ship offence value. Range [0; 1].
    /// </summary>
    private float normalized_offence;

    /// <summary>
    /// Maximum velocity to consider.
    /// </summary>
    private const float max_velocity = 5.0f;

    /// <summary>
    /// Maximum number of orbs that can be lost in a single collision.
    /// </summary>
    private const float max_orbs = 10.0f;
}
