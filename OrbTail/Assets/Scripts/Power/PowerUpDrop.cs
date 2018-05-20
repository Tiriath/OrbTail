using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Script used to attach a power-up effect on a ship upon collision.
/// </summary>
public class PowerUpDrop : PowerUpEffect
{
    /// <summary>
    /// Power-up effect prefab.
    /// </summary>
    public GameObject pickup_effect;

    /// <summary>
    /// Called whenever the powerup hits something.
    /// </summary>
    void OnTriggerEnter(Collider collision)
    {
        if (isServer)
        {
            var ship = collision.gameObject.GetComponent<Ship>();

            if (ship && ship != Owner)
            {
                if (!ship.GetComponent<Bubble>())
                {
                    var effect = Instantiate(pickup_effect, transform.position, transform.rotation).GetComponent<PowerUpEffect>();

                    effect.Owner = Owner;
                    effect.Target = ship;

                    NetworkServer.Spawn(effect.gameObject);
                }

                Destroy(gameObject);
            }
        }
    }
}