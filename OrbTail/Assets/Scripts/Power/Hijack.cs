using UnityEngine;

/// <summary>
/// Hijack the ship making it impossible to control.
/// </summary>
public class Hijack : PowerUpEffect
{
    /// <summary>
    /// Hijack force, in radians per second.
    /// </summary>
    public float torque = 20.0f;
    public void Awake()
    {
        // The hijack will remain dropped forever until picked up.

        original_duration = duration;
        duration = 0.0f;
    }

    public void OnDestroy()
    {
        if(movement_controller)
        {
            movement_controller.EnableInput = true;
        }
    }

    public void FixedUpdate()
    {
        if (target)
        {
            target.localRotation *= Quaternion.Euler(torque * Time.fixedDeltaTime, torque * Time.fixedDeltaTime, torque * Time.fixedDeltaTime);
        }
    }

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
                    RefreshDuration(original_duration);

                    target = ship.transform;
                    movement_controller = ship.GetComponent<MovementController>();

                    movement_controller.EnableInput = false;
                }
            }
        }
    }

    /// <summary>
    /// Target of the hijack.
    /// </summary>
    private Transform target;

    /// <summary>
    /// Target movement controller.
    /// </summary>
    private MovementController movement_controller;

    /// <summary>
    /// Actual duration of the hijack.
    /// </summary>
    private float original_duration = 0.0f;
}