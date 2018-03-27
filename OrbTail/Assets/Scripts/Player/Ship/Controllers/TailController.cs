using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Controls how orbs are attached to a tail and detached as result of a collision.
/// </summary>
public class TailController : MonoBehaviour
{
    public delegate void OnFightDelegate(object sender, IList<GameObject> orbs, GameObject attacker, GameObject defender);

    /// <summary>
    /// Notify that two ships collided.
    /// </summary>
    /// <param name="orbs">The list of the orbs lost by the defender</param>
    /// <param name="attacker">The attacker's ship</param>
    /// <param name="defender">The defender's ship</param>
    public event OnFightDelegate OnEventFight;

    /// <summary>
    /// Tail controlled by this object.
    /// </summary>
    public Tail Tail { get; set;}

    /// <summary>
    /// Determine how orbs are attached to the tail.
    /// </summary>
    public DriverStack<IAttacherDriver> AttachDriver { get; private set; }

    /// <summary>
    /// Determine how orbs are detached from the tail.
    /// </summary>
    public DriverStack<IDetacherDriver> DetachDriver { get; private set; }

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
        AttachDriver = new DriverStack<IAttacherDriver>();
        DetachDriver = new DriverStack<IDetacherDriver>();
        OffenceDriver = new DriverStack<IOffenceDriver>();
        DefenceDriver = new DriverStack<IDefenceDriver>();
    }

    void Start ()
    {
        Tail = GetComponent<Tail>();

        GetComponentInChildren<ProximityHandler>().EventOnProximityEnter += OnProximityEnter;       // Event used to collect orbs.
    }

    /// <summary>
    /// Called whenever the ship hits another ship.
    /// </summary>
    /// <param name="collision">Collision data.</param>
    void OnCollisionEnter(Collision collision)
    {
        GameObject opponent = collision.gameObject;

        if (opponent.tag == Tags.Ship)
        {
            var damage = opponent.GetComponent<TailController>().OffenceDriver.Top().GetDamage(collision);          // Damage dealt to this ship.

            var orbs_count = DefenceDriver.Top().ReceiveDamage(damage);                                              // Orbs lost by this ship.

            var lost_orbs = DetachDriver.Top().DetachOrbs(orbs_count, this.Tail);

            if (lost_orbs.Count > 0 && OnEventFight != null)
            {
                Debug.Log(opponent.name + " hit " + gameObject.name + " causing " + damage + "damage.");
                
                OnEventFight(this, lost_orbs, opponent, this.gameObject);
            }
        }
    }

    /// <summary>
    /// Called whenever a new orb cross the proximity boundaries.
    /// </summary>
    void OnProximityEnter(object sender, Collider other)
    {
        GameObject orb = other.gameObject;
        
        if (orb.tag == Tags.Orb)
        {
            OrbController orb_controller = orb.GetComponent<OrbController>();
            
            if (!orb_controller.IsAttached())
            {
                AttachDriver.Top().AttachOrbs(orb, Tail);
            }
            
        }
    }
}
