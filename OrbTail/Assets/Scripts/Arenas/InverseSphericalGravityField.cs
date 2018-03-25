using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Gravity field which pushes from the inside of a sphere to its surface
/// </summary>
public class InverseSphericalGravityField: IGravityField
{
       
    public const float hoverForce = 50f;
    public const float hoverDampen = 5f;

    /// <summary>
    /// The center of gravity
    /// </summary>
    public Vector3 Center { get; private set; }

    public InverseSphericalGravityField(Vector3 center)
    {

        Center = center;

    }

    public void SetGravity(FloatingObject floatie)
    {

        floatie.hover_force = hoverForce;
        floatie.hover_dampen = hoverDampen;

        floatie.ArenaDown = -Center + floatie.transform.position.normalized;

    }

}
