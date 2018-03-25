using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// The gravity is always pointing at 0;0;0
/// </summary>
public class SphericalGravityField: IGravityField
{

    public const float hoverForce = 100.0f;
    public const float hoverDampen = 50.0f;

    /// <summary>
    /// The center of gravity
    /// </summary>
    public Vector3 Center { get; private set; }

    public SphericalGravityField(Vector3 center)
    {

        Center = center;

    }

    public void SetGravity(FloatingObject floatie)
    {

        floatie.hover_force = hoverForce;
        floatie.hover_dampen = hoverDampen;

        floatie.ArenaDown = Center - floatie.transform.position.normalized;


    }

}

