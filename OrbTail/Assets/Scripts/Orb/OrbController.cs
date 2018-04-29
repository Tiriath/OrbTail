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
    /// Get the default material assigned to the orb.
    /// </summary>
    public Material DefaultMaterial { get; private set; }

    void Awake ()
    {
        DefaultMaterial = GetComponent<Renderer>().material;

        IsLinked = (gameObject.GetComponent<SpringJoint>() != null);       // An orb may start already linked, just check if it has some existing SpringJoint.
    }

    /// <summary>
    /// Links this orb to another object via a SpringJoint.
    /// </summary>
    /// <param name="target">Object to attach this orb to.</param>
    /// <param name="material">Material to assign to the orb. May be left null.</param>
    public void Link(GameObject target, Material material = null)
    {
        Unlink();

        IsLinked = true;

        SpringJoint joint = this.gameObject.AddComponent<SpringJoint>();

        joint.connectedBody = target.GetComponent<Rigidbody>();
        joint.damper = kSpringDamper;
        joint.spring = kSpringStiffness;
        joint.minDistance = kSpringMinLength;
        joint.maxDistance = kSpringMaxLength;

        if (target.tag == Tags.Ship)
        {
            joint.minDistance += kShipDistance;
            joint.maxDistance += kShipDistance;
        }

        joint.autoConfigureConnectedAnchor = false;
        joint.anchor = Vector3.zero;
        joint.connectedAnchor = Vector3.zero;

        if(material != null)
        {
            GetComponent<Renderer>().material = material;
        }
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

        GetComponent<Rigidbody>().AddForce(Random.onUnitSphere * kDetachImpulse, ForceMode.Impulse);

        GetComponent<Renderer>().material = DefaultMaterial;
    }

    /// <summary>
    /// Imbue the orb with a power.
    /// </summary>
    /// <param name="power">Power to imbue.</param>
    public void ImbuePower(Power power)
    {
        imbued_power = power;

        if(imbued_power != null && vfx == null)
        {
            vfx = GameObjectFactory.Instance.Instantiate(glow_path, gameObject.transform.position, Quaternion.identity);
            vfx.transform.parent = gameObject.transform;
        }
        else if(imbued_power == null && vfx != null)
        {
            GameObjectFactory.Instance.Destroy(glow_path, vfx);
            vfx = null;
        }
    }

    /// <summary>
    /// Get the power currently imbued in this orb.
    /// </summary>
    /// <returns></returns>
    public Power GetImbuedPower()
    {
        return imbued_power;
    }

    /// <summary>
    /// Spring dampening factor.
    /// </summary>
    private const float kSpringDamper = 0.4f;

    /// <summary>
    /// Spring stiffness.
    /// </summary>
    private const float kSpringStiffness = 3f;

    /// <summary>
    /// Minimum spring length.
    /// </summary>
    private const float kSpringMinLength = 1.0f;

    /// <summary>
    /// Maximum spring length.
    /// </summary>
    private const float kSpringMaxLength = 1.0f;

    /// <summary>
    /// Additional length to both minimum and maximum spring length when the orb is attached to a ship.
    /// </summary>
    private const float kShipDistance = 2.5f;

    /// <summary>
    /// Impulse to apply to the orbs when detached. Cosmetic purposes only.
    /// </summary>
    private const float kDetachImpulse = 0.06f;

    /// <summary>
    /// Power imbued in this orb.
    /// </summary>
    private Power imbued_power = null;

    /// <summary>
    /// VFX associated to the orb.
    /// </summary>
    private GameObject vfx = null;
}
