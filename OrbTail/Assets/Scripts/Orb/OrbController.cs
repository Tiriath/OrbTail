using UnityEngine;
using System.Collections;

/// <summary>
/// Script used to link orbs together forming a tail.
/// </summary>
public class OrbController : MonoBehaviour
{
    /// <summary>
    /// Path of the prefab used to make the orb glow.
    /// </summary>
    public const string glow_path = "Prefabs/Power/PowerGlow";

    /// <summary>
    /// Whether the orb is currently linked to something else.
    /// </summary>
    public bool IsLinked { get; private set; }

    /// <summary>
    /// Enable or disable a power on this orb.
    /// </summary>
    public bool PowerEnabled
    {
        get
        {
            return power_enabled;
        }

        set
        {
            power_enabled = value;

            if(glow_vfx == null && value)
            {
                // Add a VFX when a power is enabled.

                glow_vfx = GameObjectFactory.Instance.Instantiate(glow_path, gameObject.transform.position, Quaternion.identity);
                glow_vfx.transform.parent = gameObject.transform;
            }
            else if(glow_vfx != null && !value)
            {
                // Remove the VFX when a power is disabled.

                GameObjectFactory.Instance.Destroy(glow_path, glow_vfx);
                glow_vfx = null;
            }
        }
    }

    void Start ()
    {
        IsLinked = (gameObject.GetComponent<SpringJoint>() != null);       // An orb may start already linked, just check if it has some existing SpringJoint.
    }

    /// <summary>
    /// Links this orb to another object via a SpringJoint.
    /// </summary>
    /// <param name="target">Object to attach this orb to.</param>
    public void Link(GameObject target)
    {
        Unlink();

        IsLinked = true;

        SpringJoint joint = this.gameObject.AddComponent<SpringJoint>();

        joint.connectedBody = target.GetComponent<Rigidbody>();
        joint.damper = spring_damper;
        joint.spring = spring_stiffness;
        joint.minDistance = spring_min_length;
        joint.maxDistance = spring_max_length;

        if (target.tag == Tags.Ship)
        {
            joint.minDistance += ship_distance;
            joint.maxDistance += ship_distance;
        }

        joint.autoConfigureConnectedAnchor = false;
        joint.anchor = Vector3.zero;
        joint.connectedAnchor = Vector3.zero;
    }

    /// <summary>
    /// Unlink this orb from the current object.
    /// Does nothing if the orb is not linked to anything.
    /// </summary>
    public void Unlink()
    {
        foreach (SpringJoint joint in GetComponents<SpringJoint>())
        {
            Destroy(joint);
        }

        IsLinked = false;
    }

    /// <summary>
    /// Spring dampening factor.
    /// </summary>
    private const float spring_damper = 0.4f;

    /// <summary>
    /// Spring stiffness.
    /// </summary>
    private const float spring_stiffness = 3f;

    /// <summary>
    /// Minimum spring length.
    /// </summary>
    private const float spring_min_length = 1.0f;

    /// <summary>
    /// Maximum spring length.
    /// </summary>
    private const float spring_max_length = 1.0f;

    /// <summary>
    /// Additional length to both minimum and maximum spring length when the orb is attached to a ship.
    /// </summary>
    private const float ship_distance = 2.5f;

    /// <summary>
    /// Whether there's a power enabled on this orb.
    /// </summary>
    private bool power_enabled = false;

    /// <summary>
    /// Glow effect.
    /// </summary>
    private GameObject glow_vfx;
}
