using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Controls the movement of a ship inside an arena.
/// </summary>
public class MovementController : NetworkBehaviour
{
    /// <summary>
    /// Maximum steering speed, in radians per second.
    /// </summary>
    public float steer = 20.0f;

    /// <summary>
    /// Maximum steering acceleration, in radians per second squared.
    /// </summary>
    public float steer_smooth = 0.7f;

    /// <summary>
    /// Maximum speed, in units per second.
    /// </summary>
    public float speed = 250.0f;

    /// <summary>
    /// Maximum acceleration, in units per second squared.
    /// </summary>
    public float speed_smooth = 0.7f;

    /// <summary>
    /// Maximum banking while steering, in degrees.
    /// </summary>
    public float roll_angle = 80f;

    /// <summary>
    /// Smooth factor used to smooth out ship roll.
    /// </summary>
    public float roll_smooth = 8.0f;

    /// <summary>
    /// Get the current ship thrust.
    /// </summary>
    public float Thrust
    {
        get
        {
            var value = (speed * ThrottleInput - hover.ForwardVelocity) * speed_smooth * Time.fixedDeltaTime;

            return Mathf.Clamp(value, -speed, +speed);
        }
    }

    /// <summary>
    /// Get the current ship steering.
    /// </summary>
    public float Steer
    {
        get
        {
            var value = (steer * SteerInput - hover.AngularVelocity) * steer_smooth * Time.fixedDeltaTime;

            return Mathf.Clamp(value, -steer, +steer);
        }
    }

    /// <summary>
    /// Current throttle input.
    /// </summary>
    public float ThrottleInput { get; private set; }

    /// <summary>
    /// Current steer input.
    /// </summary>
    public float SteerInput { get; private set; }

    void Awake()
    {
        hover = this.GetComponent<FloatingObject>();
        rigid_body = this.GetComponent<Rigidbody>();
        input = this.GetComponent<InputProxy>();
    }

    // Update movement drivers.
    void Update()
    {
        if(!isLocalPlayer)
        {
            return;
        }

        ThrottleInput = input.ThrottleInput;
        SteerInput = input.SteerInput;
    }

    // Physics update.
    void FixedUpdate()
    {
        // Forward thrust.

        var thrust = Thrust * hover.Forward;

        rigid_body.AddForce(thrust, ForceMode.VelocityChange);

        // Steering.

        var steer = Steer * hover.Up;

        rigid_body.AddTorque(steer, ForceMode.VelocityChange);

        // Rolling - The ship rolls as result of its steering.

        var rolling_up = Quaternion.AngleAxis(SteerInput * roll_angle, -hover.Forward) * hover.Up;

        rigid_body.rotation = Quaternion.Lerp(rigid_body.rotation, Quaternion.LookRotation(transform.forward, rolling_up), roll_smooth * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Proxy used to read user or AI input.
    /// </summary>
    private InputProxy input;

    /// <summary>
    /// Rigid body component.
    /// </summary>
    private Rigidbody rigid_body;

    /// <summary>
    /// Handles the hovering of the ship.
    /// </summary>
    private FloatingObject hover;
}
