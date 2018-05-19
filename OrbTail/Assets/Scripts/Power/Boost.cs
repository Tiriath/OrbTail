using UnityEngine;

/// <summary>
/// Grants a temporary boost in speed which constantly propels the ship forward.
/// </summary>
public class Boost : PowerUpEffect
{
    /// <summary>
    /// Boost force.
    /// </summary>
    public float boost = 20.0f;

    public override void OnStartClient()
    {
        base.OnStartClient();

        rigid_body = Owner.GetComponent<Rigidbody>();

        transform.SetParent(Owner.transform);

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public override void Update()
    {
        base.Update();

        if(rigid_body)
        {
            rigid_body.AddForce(Owner.transform.forward * boost, ForceMode.VelocityChange);
        }
    }

    /// <summary>
    /// Rigid body to propel forward.
    /// </summary>
    private Rigidbody rigid_body;
}