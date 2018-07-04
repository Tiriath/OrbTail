using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script used to automatically rotate the game object.
/// </summary>
public class Rotator : MonoBehaviour
{
    /// <summary>
    /// Rotation to add per second, in degrees for each axis.
    /// </summary>
    public Vector3 rotation;
    
    void Update ()
    {
        var delta_rotation = rotation * Time.deltaTime;

        transform.localRotation *= Quaternion.Euler(delta_rotation.x, delta_rotation.y, delta_rotation.z);
    }
}
