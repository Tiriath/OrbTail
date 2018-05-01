using UnityEngine;

/// <summary>
/// Gravity field whose value is equal for each point in space.
/// </summary>
public class ConstantGravityField: GravityField
{
    /// <summary>
    /// Gravity direction.
    /// </summary>
    public Vector3 gravity = Vector3.down;

    public override Vector3 GetGravityAt(Vector3 position)
    {
        return gravity;
    }
}
