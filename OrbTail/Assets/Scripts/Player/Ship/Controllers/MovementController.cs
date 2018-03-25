using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Controls the movement of a ship inside an arena.
/// </summary>
public class MovementController : MonoBehaviour
{
    /// <summary>
    /// Maximum banking while steering, in degrees.
    /// </summary>
    public float roll_angle = 80f;

    /// <summary>
    /// Smooth factor used to smooth out ship roll.
    /// </summary>
    public float roll_smooth = 8.0f;

    /// <summary>
    /// Floating body object used to determine how the ship floats over the arena.
    /// </summary>
    public FloatingObject FloatingBody {get; private set;}

    void Awake()
    {
        engine_driver = new DriverStack<IEngineDriver>();
        steer_driver = new DriverStack<ISteerDriver>();
    }

    // Use this for initialization
    void Start()
    {
        FloatingBody = this.GetComponent<FloatingObject>();

        rigid_body = this.GetComponent<Rigidbody>();
        input = this.GetComponent<InputProxy>();
    }
    
    // Update movement drivers.
    void Update()
    {
        engine_driver.Top().Update(FloatingBody.ForwardVelocity, input.Acceleration);
        steer_driver.Top().Update(FloatingBody.AngularVelocity, input.Steering);
    }

    // Physics update.
    void FixedUpdate()
    {
        // Engine and steering.

        var steer = steer_driver.Top().GetSteer();
        var speed = engine_driver.Top().GetSpeed();

        rigid_body.AddForce(FloatingBody.Forward * speed, ForceMode.VelocityChange);
        rigid_body.AddTorque(FloatingBody.Up * steer, ForceMode.VelocityChange);

        // Rolling - The ship rolls as result of its steering.

        var rolling_up = Quaternion.AngleAxis(steer_driver.Top().GetSteerInput() * roll_angle, -FloatingBody.Forward) * FloatingBody.Up;

        rigid_body.rotation = Quaternion.Lerp(rigid_body.rotation, Quaternion.LookRotation(transform.forward, rolling_up), roll_smooth * Time.deltaTime);
    }

    /// <summary>
    /// Get the engine driver.
    /// </summary>
    /// <returns>Returns the engine driver.</returns>
    public DriverStack<IEngineDriver> GetEngineDriver()
    {
        return engine_driver;
    }

    /// <summary>
    /// Get the wheel driver.
    /// </summary>
    /// <returns>Returns the steer driver.</returns>
    public DriverStack<ISteerDriver> GetSteerDriver()
    {
        return steer_driver;
    }

    /// <summary>
    /// Ship engine. Affects ship speed and acceleration.
    /// </summary>
    private DriverStack<IEngineDriver> engine_driver;

    /// <summary>
    /// Ship steering. Affects ship steering.
    /// </summary>
    private DriverStack<ISteerDriver> steer_driver;

    /// <summary>
    /// Proxy used to read user or AI input.
    /// </summary>
    private InputProxy input;

    /// <summary>
    /// Rigid body component.
    /// </summary>
    private Rigidbody rigid_body;
}
