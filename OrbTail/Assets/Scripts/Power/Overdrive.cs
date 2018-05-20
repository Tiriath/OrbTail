using UnityEngine;

/// <summary>
/// Increases a ship speed.
/// </summary>
public class Overdrive : PowerUpEffect
{
    /// <summary>
    /// Speed increase factor.
    /// </summary>
    public float factor = 0.5f;

    public override void OnStartClient()
    {
        base.OnStartClient();

        movement_controller = Owner.GetComponent<MovementController>();

        transform.SetParent(Owner.transform);

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        movement_controller.Overdrive = factor;
    }

    public void OnDestroy()
    {
        movement_controller.Overdrive = 0.0f;
    }

    /// <summary>
    /// Movement controller the overdrive is applied to.
    /// </summary>
    private MovementController movement_controller;
}