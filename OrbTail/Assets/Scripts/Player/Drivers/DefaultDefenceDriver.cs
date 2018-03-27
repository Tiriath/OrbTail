using UnityEngine;
using System.Collections;

/// <summary>
/// Driver used to calculate the default amount of orbs lost by a ship as a result of an impact with another ship.
/// </summary>
public class DefaultDefenceDriver : BaseDriver, IDefenceDriver
{
    public DefaultDefenceDriver(float defence)
    {
        this.defence = defence;
    }

    public float GetDefence()
    {
        return defence;
    }

    public int ReceiveDamage(float damage)
    {
        return Mathf.FloorToInt(damage / defence);
    }

    /// <summary>
    /// Ship defence value. Amount of damage required to detach a single orb.
    /// </summary>
    private float defence = 15.0f;
}
