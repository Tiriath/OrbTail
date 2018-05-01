using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Toroidal gravity field where object are pushed away or towards the torus walls.
/// </summary>
public class ToroidalGravityField : GravityField
{
    /// <summary>
    /// Torus center.
    /// </summary>
    public Vector3 center = Vector3.zero;

    /// <summary>
    /// Torus radius.
    /// </summary>
    public float radius = 70.0f;

    /// <summary>
    /// Whether the gravity pushes towards the torus (true) or away from it (false).
    /// </summary>
    public bool attract = false;

    public override Vector3 GetGravityAt(Vector3 position)
    {
        // The torus is aligned along the ZX plane.

        Vector3 projected_position = new Vector3(position.x, 0.0f, position.z);

        var circle_center = (projected_position - center).normalized * radius + center;

        var direction = attract ? (circle_center - position) : (position - circle_center);

        return direction.normalized;
    }

}

