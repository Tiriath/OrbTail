using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// A bubble that protects from incoming projectiles.
/// </summary>
public class Bubble : PowerUpEffect
{
    public override void OnStartClient()
    {
        base.OnStartClient();

        transform.SetParent(Owner.transform);

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    /// <summary>
    /// Called whenever the missile hits something.
    /// </summary>
    void OnTriggerEnter(Collider collision)
    {
        if (isServer)
        {
            // Destroy the barrier upon impact with projectiles and drops.

            var projectile = collision.gameObject.GetComponent<Projectile>();
            var hijack = collision.gameObject.GetComponent<Hijack>();

            if((projectile && projectile.Owner != Owner) ||
               (hijack && hijack.Owner != Owner))
            {
                Destroy(gameObject);
            }
        }
    }
}
