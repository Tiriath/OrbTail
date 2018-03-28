using UnityEngine;
using System.Collections;

/// <summary>
/// Component used to detect when an object enters the proximity field of a ship.
/// Can be used to gather orbs that are close to the ship.
/// </summary>
public class ProximityHandler : MonoBehaviour
{
    public delegate void OnProximityDelegate(object sender, Collider other);

    /// <summary>
    /// Called whenever an object enters the proximity field.
    /// </summary>
    public event OnProximityDelegate OnProximityEvent;
    
    void Start ()
    {
        proximity_field = GetComponent<SphereCollider>();
    }

    /// <summary>
    /// Gets or sets the radius of the proximity field.
    /// </summary>
    /// <value>The radius, in units.</value>
    public float Radius
    {
        get
        {
            return proximity_field.radius;
        }

        set
        {
            proximity_field.radius = value;
        }
    }

    /// <summary>
    /// Called whenever a new object enters the proximity field.
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (OnProximityEvent != null)
        {
            OnProximityEvent(this, other);
        }
    }

    /// <summary>
    /// Proximity field collider around the ship.
    /// </summary>
    private SphereCollider proximity_field;
}
