using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Radial gravity field where objects are attracted towards a point in space or pushed away from it.
/// </summary>
public class RadialGravityField: GravityField
{
    /// <summary>
    /// Gravity center.
    /// </summary>
    public Vector3 center = Vector3.zero;

    /// <summary>
    /// Whether the gravity pushes towards the center (true) or away from it (false).
    /// </summary>
    public bool attract = false;

    public override Vector3 GetGravityAt(Vector3 position)
    {
        var direction = attract ? (center - position) : (position - center);

        return direction.normalized;
    }
}

