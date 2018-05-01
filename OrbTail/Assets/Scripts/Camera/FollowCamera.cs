using UnityEngine;

/// <summary>
/// Script used to handle camera movement.
/// </summary>
public class FollowCamera : MonoBehaviour
{
    /// <summary>
    /// Distance of the camera from the target.
    /// </summary>
    public float boom_length = 10.0f;

    /// <summary>
    /// Camera pitch relative to camera's view target.
    /// </summary>
    public float camera_pitch = 15.0f;

    /// <summary>
    /// Camera radius used to prevent the camera from compenetrating the environment.
    /// </summary>
    public float camera_radius = 5.0f;

    /// <summary>
    /// Camera movement smoothing factor.
    /// </summary>
    public float movement_smooth = 2.0f;

    /// <summary>
    ///  Camera rotation smoothing factor.
    /// </summary>
    public float rotation_smooth = 2.0f;

    /// <summary>
    /// Boom length smoothing factor.
    /// </summary>
    public float boom_smooth = 2.0f;
    
    /// <summary>
    /// Get or set the lobby player owning this camera.
    /// </summary>
    public LobbyPlayer Owner
    {
        get
        {
            return lobby_player;
        }
        set
        {
            lobby_player = value;

            // #TODO Set the proper viewport for the player according to the number of local players in the lobby.
        }
    }

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

    public void Awake()
    {
        gravity_field = FindObjectOfType<GravityField>();

        camera_transform = GetComponentInChildren<Camera>().gameObject.transform;
    }

    public void FixedUpdate ()
    {
        // Move smoothly towards the target.

        current_position = Vector3.Lerp(current_position, view_target_transform.position, movement_smooth * Time.fixedDeltaTime);

        current_rotation = Quaternion.Lerp(gravity_field.TangentRotation(view_target_transform) * Quaternion.Euler(camera_pitch, 0.0f, 0.0f), current_rotation, rotation_smooth * Time.fixedDeltaTime);

        // Retract the boom instantly to prevent it from compenetrating the arena.

        RaycastHit hit;
        
        if(Physics.SphereCast(current_position, camera_radius, (camera_transform.position - current_position).normalized, out hit, boom_length + camera_radius, Layers.Obstacles | Layers.Field))
        {
            current_length = hit.distance;
        }
        else
        {
            current_length = Mathf.Lerp(current_length, boom_length, boom_smooth * Time.fixedDeltaTime);
        }

        // Update camera position and rotation.

        transform.position = current_position;
        transform.rotation = current_rotation;

        camera_transform.localPosition = new Vector3(0.0f, 0.0f, -current_length);
        camera_transform.localRotation = Quaternion.identity;
    }

    /// <summary>
    /// Snap the camera instantly to its target position.
    /// </summary>
    public void Snap()
    {
        current_position = view_target_transform.position;
        current_rotation = gravity_field.TangentRotation(view_target_transform) * Quaternion.Euler(camera_pitch, 0.0f, 0.0f);
        current_length = boom_length;
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
    /// Camera attached at the end of the camera boom.
    /// </summary>
    private Transform camera_transform;

    /// <summary>
    /// Current camera target position.
    /// </summary>
    private Vector3 current_position;

    /// <summary>
    /// Current camera target rotation.
    /// </summary>
    private Quaternion current_rotation;

    /// <summary>
    /// Target boom length.
    /// </summary>
    private float current_length = 0.0f;

    /// <summary>
    /// Player owning this camera.
    /// </summary>
    LobbyPlayer lobby_player;
}
