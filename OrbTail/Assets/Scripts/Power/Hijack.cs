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

    public override void OnStartClient()
    {
        base.OnStartClient();

        transform.SetParent(Target.transform);

        movement_controller = Target.GetComponent<MovementController>();

        movement_controller.EnableInput = false;
    }

    public void OnDestroy()
    {
        movement_controller.EnableInput = true;
    }

    public void FixedUpdate()
    {
        Target.transform.localRotation *= Quaternion.Euler(torque * Time.fixedDeltaTime, torque * Time.fixedDeltaTime, torque * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Target movement controller.
    /// </summary>
    private MovementController movement_controller;
}