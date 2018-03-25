using UnityEngine;
using System.Collections;

/// <summary>
/// Driver used to calculate the default amount of orbs lost by a ship as a result of an impact with another ship.
/// </summary>
public class DefaultDefenceDriver : BaseDriver, IDefenceDriver
{
    public DefaultDefenceDriver(int defence)
    {
        this.defence = defence;
        this.normalized_defence = (defence / 5.0f) + 1.0f;
    }

    public int GetDefence()
    {
        return defence;
    }

    public int DamageToOrbs(float damage)
    {
        // #TODO Magic formula!

        return Mathf.FloorToInt(damage / normalized_defence);
    }

    /// <summary>
    /// Ship defence value. Range [1; 5]
    /// </summary>
    private int defence;

    /// <summary>
    /// Normalize ship defence value. Range [0; 1].
    /// </summary>
    private float normalized_defence;
}
