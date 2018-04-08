using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Handles physical appearance of the tail and how orbs are attached to each othes, as well as proper replication mechanics.
/// </summary>
public class Tail : MonoBehaviour
{
    public delegate void OnOrbAttachedDelegate(object sender, GameObject orb, GameObject ship);

    public delegate void OnOrbDetachedDelegate(object sender, GameObject ship, int count);

    /// <summary>
    /// Called whenever an orb is attached to the tail.
    /// </summary>
    public event OnOrbAttachedDelegate OnEventOrbAttached;

    /// <summary>
    /// Called whenever an orb is detached from the tail.
    /// </summary>
    public event OnOrbDetachedDelegate OnEventOrbDetached;

    void Awake()
    {
        default_orb_material = Resources.Load<Material>("Materials/OrbMat");

        orb_material = new Material(default_orb_material);

        default_orb_color = default_orb_material.color;
    }
    
    void Start ()
    {
        game_identity = GetComponent<GameIdentity>();

        UpdateTailColor();

        game_identity.EventIdSet += OnIdSet;
    }
    
    /// <summary>
    /// Attach an orb to the tail.
    /// </summary>
    /// <param name="orb">The orb to attach</param>
    public void AttachOrb(GameObject orb)
    {
        var orb_controller = orb.GetComponent<OrbController>();

        // Fancy upward force while attaching the orb.

        orb.GetComponent<Rigidbody>().AddForce(orb.GetComponent<FloatingObject>().Up * attach_impulse, ForceMode.Impulse);

        // Attach the orb to the existing ones (or the ship itself).

        GameObject target = orbs.Count > 0 ? orbs.Peek() : gameObject;

        orbs.Push(orb);

        orb_controller.Link(target);

        // Update tail color and notify the game of the attachment.

        orb.GetComponent<Renderer>().material = orb_material;

        UpdateTailColor();

        if (OnEventOrbAttached != null)
        {
            OnEventOrbAttached(this, orb, gameObject);
        }
    }

    /// <summary>
    /// Detach any number of orbs from the tail.
    /// </summary>
    /// <returns>Returns the list of the orbs detached.</returns>
    /// <param name="count">Maximum number of orbs to deatch.</param>
    public List<GameObject> DetachOrbs(int count)
    {
        List<GameObject> detached_orbs = new List<GameObject>();
        
        while (count > 0 && orbs.Count > 0)
        {
            GameObject orb = orbs.Pop();

            orb.GetComponent<OrbController>().Unlink();

            // Fire the detached orb in a random direction and restore its original color.

            orb.GetComponent<Rigidbody>().AddForce(Random.onUnitSphere * detach_impulse, ForceMode.Impulse);

            orb.GetComponent<Renderer>().material = default_orb_material;

            detached_orbs.Add(orb);
        }

        UpdateTailColor();

        if (OnEventOrbDetached != null)
        {
            OnEventOrbDetached(this, gameObject, detached_orbs.Count);
        }

        return detached_orbs;
    }

    /// <summary>
    /// Gets the number of the orbs in the tail.
    /// </summary>
    /// <returns>The number of the orbs in the tail.</returns>
    public int GetOrbCount()
    {
        return orbs.Count;
    }

    /// <summary>
    /// Update current tail color according to the current number or orbs on the tail.
    /// </summary>
    private void UpdateTailColor()
    {
        const float kMaxOrbs = 10;            //Number of orbs to have for a full colored tail.

        orb_material.color = Color.Lerp(default_orb_color, game_identity.Color, Mathf.Min(1.0f, orbs.Count / kMaxOrbs));
    }

    /// <summary>
    /// Called whenever the id of the player is set.
    /// </summary>
    private void OnIdSet(object sender, int id)
    {
        UpdateTailColor();
    }

    /// <summary>
    /// Orbs in this tail.
    /// </summary>
    private Stack<GameObject> orbs = new Stack<GameObject>();

    /// <summary>
    /// Identity of the player owning this tail.
    /// </summary>
    private GameIdentity game_identity = null;

    /// <summary>
    /// Default orbs material.
    /// </summary>
    private Material default_orb_material;

    /// <summary>
    /// Current orbs material.
    /// </summary>
    private Material orb_material;

    /// <summary>
    /// Default orb color.
    /// </summary>
    private Color default_orb_color;

    /// <summary>
    /// Impulse to apply to the orbs when attached. VFX purposes only.
    /// </Impulse>
    private float attach_impulse = 0.03f;

    /// <summary>
    /// Impulse to apply to the orbs when detached. VFX purposes only.
    /// </summary>
    private float detach_impulse = 0.06f;
}
