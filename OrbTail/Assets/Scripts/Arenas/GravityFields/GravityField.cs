using UnityEngine;

/// <summary>
/// Base component for gravity fields.
/// </summary>
public abstract class GravityField : MonoBehaviour
{
    /// <summary>
    /// Hover force exerted by this field.
    /// </summary>
    public float hover_force = 100.0f;

    /// <summary>
    /// Hover dampen exerted by this field.
    /// </summary>
    public float hover_dampen = 50.0f;

    /// <summary>
    /// Get the gravity position at given coordinates.
    /// </summary>
    /// <param name="position">Coordinates to get the gravity of, in world space.</param>
    /// <returns>Returns the gravity position at given coordinates.</returns>
    public abstract Vector3 GetGravityAt(Vector3 position);

    /// <summary>
    /// Get the rotation of a forward vector that is tangent to a point in this gravity field.
    /// </summary>
    public Quaternion TangentRotation(Transform transform)
    {
        Vector3 up = -GetGravityAt(transform.position);
        Vector3 right = Vector3.Cross(up, transform.forward).normalized;
        Vector3 forward = Vector3.Cross(right, up).normalized;

        return Quaternion.LookRotation(forward, up);
    }

}

