using UnityEngine;

/// <summary>
/// Script used to handle the HUD of a player.
/// </summary>
public class HUDHandler : GUIInputHandler
{
    /// <summary>
    /// Movement smoothing factor.
    /// </summary>
    public float movement_smooth = 30f;

    /// <summary>
    ///  Rotation smoothing factor.
    /// </summary>
    public float rotation_smooth = 30f;

    /// <summary>
    /// Distance of the object from the camera.
    /// </summary>
    public float distance = 10.7f;

    /// <summary>
    /// Set the owner of this HUD.
    /// </summary>
    public GameObject Owner
    {
        get
        {
            return owner;
        }
        set
        {
            owner = value;

            foreach(var hud_element in GetComponentsInChildren<HUDElement>(true))
            {
                hud_element.Owner = owner;
            }
        }
    }

    /// <summary>
    /// Get or set the lobby player this camera belongs to.
    /// </summary>
    public LobbyPlayer LobbyPlayer
    {
        get
        {
            return lobby_player;
        }
        set
        {
            lobby_player = value;

            if (lobby_player)
            {
                // The HUD is visible and reacts only to the lobby player.

                var player_layer = Layers.GetPlayerLayer(lobby_player);

                Layers.SetLayerRecursive(gameObject, player_layer);

                GUILayer = player_layer;
            }
            else
            {
                // The HUD is visible only via the global camera.

                var global_hud_layer = Layers.GetGlobalLayer();

                Layers.SetLayerRecursive(gameObject, global_hud_layer);

                GUILayer = global_hud_layer;
            }

            // Propagate to the elements.
            foreach (var hud_element in GetComponentsInChildren<HUDElement>(true))
            {
                hud_element.LobbyPlayer = lobby_player;
            }
        }
    }

    /// <summary>
    /// Set the camera this HUD component is attached to.
    /// </summary>
    public Camera Camera
    {
        set
        {
            if(value)
            {
                camera_transform = value.transform;

                transform.position = camera_transform.position;
                transform.rotation = camera_transform.rotation;

                OwningCamera = value;
            }
            else
            {
                camera_transform = null;
                OwningCamera = null;
            }
        }
    }

    void FixedUpdate ()
    {
        if(camera_transform != null)
        {
            transform.position = Vector3.Lerp(transform.position, camera_transform.position + camera_transform.forward * distance, Time.fixedDeltaTime * movement_smooth);
            transform.rotation = Quaternion.Lerp(transform.rotation, camera_transform.rotation, Time.fixedDeltaTime * rotation_smooth);
        }
    }

    /// <summary>
    /// Transform of the camera this component is bound to.
    /// </summary>
    private Transform camera_transform;

    /// <summary>
    /// Owner of this HUD.
    /// </summary>
    private GameObject owner;

    /// <summary>
    /// Lobby player this HUD refers to.
    /// </summary>
    private LobbyPlayer lobby_player;
}
