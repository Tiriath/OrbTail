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
}

