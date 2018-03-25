using UnityEngine;
using System.Collections;

/// <summary>
/// Represents an object floating onto an arena.
/// </summary>
public class FloatingObject : MonoBehaviour
{
    /// <summary>
    /// Smooth factor used to adjust object forward direction and keep it tangent to the gravity field.
    /// </summary>
    public float pitch_smooth = 10.0f;

    /// <summary>
    /// Force attracting the object to the arena surface.
    /// </summary>
    public float hover_force = 20.0f;

    /// <summary>
    /// Dampen factor, used to reduce the "springiness" of the hovering force.
    /// </summary>
    public float hover_dampen = 0f;

    /// <summary>
    /// Hovering distance of the object from the arena surface.
    /// </summary>
    public float hover_distance = 5f;

    /// <summary>
    /// Get the forward vector accounting for gravity direction.
    /// </summary>
    public Vector3 Forward
    {
        get
        {
            return Vector3.Cross(Right, Up).normalized;
        }
    }

    /// <summary>
    /// Get the up vector accounting for gravity direction.
    /// </summary>
    public Vector3 Up
    {
        get
        {
            return -ArenaDown;
        }
    }

    /// <summary>
    /// Get the right vector accounting for gravity direction.
    /// </summary>
    public Vector3 Right
    {
        get
        {
            return Vector3.Cross(Up, this.transform.forward).normalized;
        }
    }

    /// <summary>
    /// Get the object forward velocity.
    /// </summary>
    public float ForwardVelocity
    {
        get
        {
            return Vector3.Dot(rigid_body.velocity, Forward);
        }
    }

    /// <summary>
    /// Get the object vertical velocity.
    /// </summary>
    public float VerticalVelocity
    {
        get
        {
            return Vector3.Dot(rigid_body.velocity, Up);
        }
    }

    /// <summary>
    /// Get the object angular velocity, relative to up axis.
    /// </summary>
    public float AngularVelocity
    {
        get
        {
            return Vector3.Dot(rigid_body.angularVelocity, Up);
        }
    }

    /// <summary>
    /// Current down direction.
    /// </summary>
    public Vector3 ArenaDown { get; set; }

    // Use this for initialization
    void Start ()
    {
        ArenaDown = Vector3.zero;
        rigid_body = GetComponent<Rigidbody>();
        gravity_field = GameObject.FindGameObjectWithTag(Tags.Arena).GetComponent<GravityField>();
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        gravity_field.SetGravity(this);

        // Handle hovering.

        RaycastHit hit;

        if(Physics.Raycast(transform.position, ArenaDown, out hit, Mathf.Infinity, Layers.Field))
        {
            rigid_body.AddForce(Up * (hover_force * (hover_distance - hit.distance) - hover_dampen * VerticalVelocity), ForceMode.Acceleration);
            
            Debug.DrawRay(transform.position, ArenaDown * hit.distance, Color.green);
        }
        else
        {
            rigid_body.AddForce(ArenaDown * hover_force, ForceMode.Acceleration);
        }

        // Adjust object pitch such that the object forward is tangent to the gravity field.

        rigid_body.rotation = Quaternion.Lerp(rigid_body.rotation, Quaternion.LookRotation(Forward, Up), pitch_smooth * Time.deltaTime);
    }

    /// <summary>
    /// Arena source of gravity.
    /// </summary>
    private GravityField gravity_field;

    /// <summary>
    /// Rigid body affected by the gravity field.
    /// </summary>
    private Rigidbody rigid_body;
}
