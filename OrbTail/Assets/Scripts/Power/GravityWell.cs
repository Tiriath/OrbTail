using UnityEngine;
using System.Collections;

/// <summary>
/// Moves collectables towards the center of the gravity well.
/// </summary>
public class GravityWell : PowerUpEffect
{
    /// <summary>
    /// Attractive force.
    /// </summary>
    public float force = 2.0f;

    public override void OnStartClient()
    {
        base.OnStartClient();

        transform.SetParent(Owner.transform);

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    void OnTriggerStay(Collider collider)
    {
        var orb = collider.gameObject.GetComponent<OrbController>();

        if(orb && !orb.IsLinked)
        {
            collider.attachedRigidbody.AddForce((Owner.transform.position - collider.transform.position) * force, ForceMode.Acceleration);
        }
    }
}
