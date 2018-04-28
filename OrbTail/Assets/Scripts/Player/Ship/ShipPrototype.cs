using UnityEngine;
using System.Collections;

/// <summary>
/// Component used to build a ship with necessary components.
/// This component sets ship default drivers.
/// </summary>
public class ShipPrototype : MonoBehaviour
{
    public delegate void DelegateShipEvent(ShipPrototype sender);

    public static event DelegateShipEvent ShipCreatedEvent;
    public static event DelegateShipEvent ShipDestroyedEvent;

    /// <summary>
    /// Maximum steering speed, in radians per second.
    /// </summary>
    public float steer = 3.0f;

    /// <summary>
    /// Maximum steering acceleration, in radians per second squared.
    /// </summary>
    public float steer_smooth = 0.7f;

    /// <summary>
    /// Maximum speed, in units per second.
    /// </summary>
    public float speed = 10.0f;

    /// <summary>
    /// Maximum acceleration, in units per second squared.
    /// </summary>
    public float speed_smooth = 0.7f;

    /// <summary>
    /// The offence power of the ship, as multiplicative factor of dealt damage.
    /// </summary>
    public float offence = 1.0f;

    /// <summary>
    /// The defence power of the ship, as damage required to detach each orb.
    /// </summary>
    public float defence = 15.0f;

    /// <summary>
    /// Get or set the details color.
    /// </summary>
    public Color  DetailsColor{ get; set; }

    void Awake()
    {
        // Add ship controllers.

        gameObject.AddComponent<Tail>();
        gameObject.AddComponent<PowerController>();

        //Server side controls the collisions.

        TailController tail_controller = gameObject.AddComponent<TailController>();

        tail_controller.OffenceDriver.SetDefaultDriver(new DefaultOffenceDriver(offence));
        tail_controller.DefenceDriver.SetDefaultDriver(new DefaultDefenceDriver(defence));
        tail_controller.AttachDriver.SetDefaultDriver(new DefaultAttacherDriver());
        tail_controller.DetachDriver.SetDefaultDriver(new DefaultDetacherDriver());

        //Client side controls the movement.

        MovementController movement_controller = gameObject.AddComponent<MovementController>();

        movement_controller.enabled = false;

        movement_controller.GetEngineDriver().SetDefaultDriver(new DefaultEngineDriver(speed, speed_smooth));
        movement_controller.GetSteerDriver().SetDefaultDriver(new DefaultSteerDriver(steer, steer_smooth));

        //Colorize this ship. The material is shared to reduce the draw calls.

        Material material = null;

        foreach (var renderer in GetComponentsInChildren<MeshRenderer>())
        {
            if (renderer.gameObject.tag.Equals(Tags.ShipDetail))
            {
                if (material == null)
                {
                    material = renderer.material;
                    material.color = DetailsColor;
                }

                renderer.material = material;
            }
        }

        // Done!

        if (ShipCreatedEvent != null)
        {
            ShipCreatedEvent(this);
        }
    }

    public void OnDestroy()
    {
        if (ShipDestroyedEvent != null)
        {
            ShipDestroyedEvent(this);
        }
    }
}
