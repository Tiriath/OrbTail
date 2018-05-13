using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

/// <summary>
/// Controls ship fights.
/// </summary>
public class FightController : NetworkBehaviour
{
    public delegate void DelegateFight(GameObject attacker, GameObject defender, IList<GameObject> orbs);

    public event DelegateFight FightEvent;

    /// <summary>
    /// The offence power of the ship, as multiplicative factor of dealt damage.
    /// </summary>
    public float offence = 1.0f;

    /// <summary>
    /// The defence power of the ship, as damage required to detach each orb.
    /// </summary>
    public float defence = 15.0f;
    
    /// <summary>
    /// Determine the amount of damage dealt by the ship after a collision.
    /// </summary>
    public DriverStack<IOffenceDriver> OffenceDriver { get; private set; }

    /// <summary>
    /// Determine the amount of damage received by the ship after a collision.
    /// </summary>
    public DriverStack<IDefenceDriver> DefenceDriver { get; private set; }

    void Awake()
    {
        OffenceDriver = new DriverStack<IOffenceDriver>();
        DefenceDriver = new DriverStack<IDefenceDriver>();
        
        OffenceDriver.SetDefaultDriver(new DefaultOffenceDriver(offence));
        DefenceDriver.SetDefaultDriver(new DefaultDefenceDriver(defence));
    }

    /// <summary>
    /// Called whenever the ship hits another ship.
    /// </summary>
    /// <param name="collision">Collision data.</param>
    private void OnCollisionEnter(Collision collision)
    {
        if(isServer)
        {
            // This ship is the defender, whereas the other is the attacker. This event is triggered on both ships simultaneously with reversed roles.

            GameObject game_object = collision.gameObject;

            if (game_object.tag == Tags.Ship)
            {
                var defender = GetComponent<Ship>();

                var damage = game_object.GetComponent<FightController>().OffenceDriver.Top().GetDamage(collision);      // Damage dealt to this ship.

                var orbs_count = DefenceDriver.Top().ReceiveDamage(damage);                                             // Orbs lost by this ship.

                //Debug.Log(game_object.name + " hit " + gameObject.name + " causing " + damage + "damage.");

                for (;orbs_count > 0 && defender != null; --orbs_count)
                {
                    defender.RpcDetachOrb();

                    //if (FightEvent != null)
                    //{
                    //    FightEvent(game_object, this.gameObject, lost_orbs);                                            // Notify.
                    //}
                }
            }
        }
    }
}
