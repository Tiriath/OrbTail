using UnityEngine;
using System.Collections;

/// <summary>
/// Component used to build a ship with necessary components.
/// This component sets ship default drivers.
/// </summary>
public class ShipPrototype : MonoBehaviour
{
    /// <summary>
    /// The defence power of the ship. Range [1; 5].
    /// </summary>
    [Range(1,5)]
    public int defence;

    /// <summary>
    /// The offence power of the ship. Range [1; 5].
    /// </summary>
    [Range(1, 5)]
    public int offence;

    /// <summary>
    /// The steering of the ship. Range [1; 5].
    /// </summary>
    [Range(1, 5)]
    public int steering;

    /// <summary>
    /// The speed of the ship. Range [1; 5].
    /// </summary>
    [Range(1, 5)]
    public int speed;

    void Update ()
    {
        this.enabled = false;
    }

    void Awake()
    {
        // Add ship controllers.

        gameObject.AddComponent<Tail>();

        gameObject.AddComponent<PowerController>();

        //Server side controls the collisions.

        if (NetworkHelper.IsServerSide())
        {

            TailController tail_controller = gameObject.AddComponent<TailController>();

            tail_controller.GetOffenceDriverStack().SetDefaultDriver(new DefaultOffenceDriver(offence));
            tail_controller.GetDefenceDriverStack().SetDefaultDriver(new DefaultDefenceDriver(defence));
            tail_controller.GetAttacherDriverStack().SetDefaultDriver(new DefaultAttacherDriver());
            tail_controller.GetDetacherDriverStack().SetDefaultDriver(new DefaultDetacherDriver());
        }

        //Client side controls the movement.

        if (NetworkHelper.IsOwnerSide(GetComponent<NetworkView>()))
        {

            MovementController movement_controller = gameObject.AddComponent<MovementController>();

            movement_controller.enabled = false;

            movement_controller.GetEngineDriverStack().SetDefaultDriver(new DefaultEngineDriver(speed));
            movement_controller.GetWheelDriverStack().SetDefaultDriver(new DefaultWheelDriver(steering));
        }

    }
    void Start()
    {
        builder = GameObject.FindGameObjectWithTag(Tags.Master).GetComponent<GameBuilder>();

        builder.EventGameBuilt += OnGameBuilt;
    }

    /// <summary>
    /// Called whenever a game is built.
    /// </summary>
    /// <param name="sender">Object who raised the event.</param>
    void OnGameBuilt(object sender)
    {
        //Colorize this ship. The material is shared to reduce the draw calls.

        Material material = null;

        foreach (var renderer in GetComponentsInChildren<MeshRenderer>())
        {
            if (renderer.gameObject.tag.Equals(Tags.ShipDetail))
            {
                if (material == null)
                {
                    material = renderer.material;
                    material.color = GetComponent<GameIdentity>().Color * 0.7f;
                }

                renderer.material = material;
            }
        }

        builder.EventGameBuilt -= OnGameBuilt;
    }

    private GameBuilder builder;
}
