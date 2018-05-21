using UnityEngine;

/// <summary>
/// Script used to handle camera movement.
/// </summary>
public class FollowCamera : MonoBehaviour
{
    /// <summary>
    /// Camera pitch relative to camera's view target.
    /// </summary>
    public float camera_pitch = 15.0f;

    /// <summary>
    /// Camera radius used to prevent the camera from compenetrating the environment.
    /// </summary>
    public float camera_radius = 5.0f;

    /// <summary>
    /// Distance of the camera from the target.
    /// </summary>
    public float camera_distance = 10.0f;

    /// <summary>
    /// Movement smoothing factor.
    /// </summary>
    public float movement_smooth = 2.0f;

    /// <summary>
    ///  Rotation smoothing factor.
    /// </summary>
    public float rotation_smooth = 2.0f;

    /// <summary>
    /// Distance smoothing factor.
    /// </summary>
    public float distance_smooth = 2.0f;

    /// <summary>
    /// Get or set the current camera target.
    /// </summary>
    public GameObject ViewTarget
    {
        get
        {
            return view_target_transform.gameObject;
        }
        set
        {
            view_target_transform = value.transform;
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
            var camera = GetComponentInChildren<Camera>();

            // Set the cull mask for the objects that can only be seen by this lobby player.

            if(lobby_player)
            {
                camera.cullingMask &= ~Layers.GetPlayerLayerBitmask(lobby_player);
            }
            
            lobby_player = value;

            camera.cullingMask |= Layers.GetPlayerLayerBitmask(lobby_player);
        }
    }

    /// <summary>
    /// Get the transform of the camera attached at the end of the boom.
    /// </summary>
    public Transform CameraTransform { get; private set; }

    /// <summary>
    /// Get the view target position.
    /// </summary>
    public Vector3 ViewTargetPosition
    {
        get
        {
            return view_target_transform.position;
        }
    }

    /// <summary>
    /// Get the view target rotation.
    /// </summary>
    public Quaternion ViewTargetRotation
    {
        get
        {
            return gravity_field.TangentRotation(view_target_transform) * Quaternion.Euler(camera_pitch, 0.0f, 0.0f);
        }
    }

    public void Awake()
    {
        gravity_field = FindObjectOfType<GravityField>();

        CameraTransform = GetComponentInChildren<Camera>().transform;
    }

    public void FixedUpdate ()
    {
        if(view_target_transform != null)
        {
            // Move smoothly towards the target.

            current_position = Vector3.Lerp(current_position, ViewTargetPosition, movement_smooth * Time.fixedDeltaTime);
            current_rotation = Quaternion.Lerp(current_rotation, ViewTargetRotation, rotation_smooth * Time.fixedDeltaTime);
            current_length = Mathf.Lerp(current_length, camera_distance, distance_smooth * Time.fixedDeltaTime);

            // Retract the camera instantly to prevent it from compenetrating the arena.

            RaycastHit hit;

            if (Physics.SphereCast(current_position, camera_radius, (CameraTransform.position - current_position).normalized, out hit, camera_distance, Layers.Obstacles | Layers.Field))
            {
                current_length = hit.distance;
            }

            // Update camera position and rotation.

            transform.position = current_position;
            transform.rotation = current_rotation;

            CameraTransform.localPosition = new Vector3(0.0f, 0.0f, -current_length);
            CameraTransform.localRotation = Quaternion.identity;
        }
    }

    /// <summary>
    /// Snap the camera instantly to its target position.
    /// </summary>
    public void Snap()
    {
        current_position = ViewTargetPosition;
        current_rotation = ViewTargetRotation;
        current_length = camera_distance;
    }

    /// <summary>
    /// Arena source of gravity.
    /// </summary>
    private GravityField gravity_field;

    /// <summary>
    /// View target transform.
    /// </summary>
    private Transform view_target_transform;

    /// <summary>
    /// Current camera target position.
    /// </summary>
    private Vector3 current_position;

    /// <summary>
    /// Current camera target rotation.
    /// </summary>
    private Quaternion current_rotation;

    /// <summary>
    /// Lobby player this camera refers to.
    /// </summary>
    private LobbyPlayer lobby_player;

    /// <summary>
    /// Current camera distance from the target.
    /// </summary>
    private float current_length = 0.0f;
}
