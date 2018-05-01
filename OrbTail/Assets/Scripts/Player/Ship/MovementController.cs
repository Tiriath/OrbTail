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
    /// Get the engine driver.
    /// </summary>
    /// <returns>Returns the engine driver.</returns>
    public DriverStack<IEngineDriver> EngineDriver { get; private set; }

    /// <summary>
    /// Get the wheel driver.
    /// </summary>
    /// <returns>Returns the steer driver.</returns>
    public DriverStack<ISteerDriver> SteerDriver { get; private set; }

    void Awake()
    {
        hover = this.GetComponent<FloatingObject>();
        rigid_body = this.GetComponent<Rigidbody>();
        input = this.GetComponent<InputProxy>();

        EngineDriver = new DriverStack<IEngineDriver>();
        SteerDriver = new DriverStack<ISteerDriver>();

        EngineDriver.SetDefaultDriver(new DefaultEngineDriver(speed, speed_smooth));
        SteerDriver.SetDefaultDriver(new DefaultSteerDriver(steer, steer_smooth));
    }

    // Update movement drivers.
    void Update()
    {
        EngineDriver.Top().Input = input.ThrottleInput;
        SteerDriver.Top().Input = input.SteerInput;
    }

    // Physics update.
    void FixedUpdate()
    {
        // Engine and steering.

        var steer = SteerDriver.Top().GetSteer(hover.AngularVelocity, Time.fixedDeltaTime) * hover.Up;
        var thrust = EngineDriver.Top().GetThrust(hover.ForwardVelocity, Time.fixedDeltaTime) * hover.Forward;

        rigid_body.AddForce(thrust, ForceMode.VelocityChange);
        rigid_body.AddTorque(steer, ForceMode.VelocityChange);

        // Rolling - The ship rolls as result of its steering.

        var rolling_up = Quaternion.AngleAxis(SteerDriver.Top().Input * roll_angle, -hover.Forward) * hover.Up;

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
