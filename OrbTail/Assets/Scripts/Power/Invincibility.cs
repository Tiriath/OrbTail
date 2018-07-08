using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Prevents any orb from being detached from the ship when this power is active.
/// </summary>
public class Invincibility : PowerUpEffect
{
    /// <summary>
    /// Color of the ship when this powerup is enabled.
    /// </summary>
    public Color RimColor = new Color(1.0f, 0.9f, 0.8f, 1.0f);

    /// <summary>
    /// Power of the rim-shader effect.
    /// </summary>
    public float RimPower = 2.0f;

    /// <summary>
    /// Scale value for the rim-shader effect.
    /// </summary>
    public float RimValue = 2.0f;

    public override void OnStartClient()
    {
        base.OnStartClient();

        transform.SetParent(Owner.transform);

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        var material = Owner.GetComponentInChildren<MeshRenderer>().material;

        material.SetColor("_RimColor", RimColor);
        material.SetFloat("_RimPower", RimPower);
        material.SetFloat("_RimScale", RimValue);
    }

    public void OnDestroy()
    {
        if(Owner)
        {
            var material = Owner.GetComponentInChildren<MeshRenderer>().material;

            material.SetFloat("_RimScale", 0.0f);                       // Turn the VFX off.
        }
    }
}
