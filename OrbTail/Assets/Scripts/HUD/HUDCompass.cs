using UnityEngine;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// Compass used to track opponents position in space.
/// </summary>
public class HUDCompass : HUDElement
{
    /// <summary>
    /// Prefab of the object to use as needle.
    /// </summary>
    public GameObject needle;

    /// <summary>
    /// Length of each needle.
    /// </summary>
    public float needle_length = 3.0f;

    /// <summary>
    /// Minimum directin for which the needle fades away, as dot product relative to the forward direction.
    /// </summary>
    public float alpha_direction = 0.9f;

    public void Awake ()
    {
        BaseGameMode.Instance.MatchCountdownEvent += OnMatchCountdown;
    }

    public void OnDestroy()
    {
        var game_mode = BaseGameMode.Instance;

        if(game_mode)
        {
            game_mode.MatchCountdownEvent -= OnMatchCountdown;
        }
        
        Ship.ShipDestroyedEvent -= OnShipDestroyed;

        foreach (var needle in needles)
        {
            needle.OnDestroy();
        }
    }

    // Update is called once per frame
    void Update ()
    {
        foreach (var needle in needles)
        {
            needle.Update(needle_length, alpha_direction);
        }
    }

    /// <summary>
    /// Called whenever the countdown starts.
    /// </summary>
    private void OnMatchCountdown(BaseGameMode game_mode)
    {
        BaseGameMode.Instance.MatchCountdownEvent -= OnMatchCountdown;

        Ship.ShipDestroyedEvent += OnShipDestroyed;

        // Add a needle for each other ship.

        var owner_ship = Owner.GetComponent<Ship>();

        foreach (var ship in FindObjectsOfType<Ship>())
        {
            if(ship.gameObject != Owner)
            {
                needles.Add(new HUDCompassNeedle(owner_ship, ship, needle));
            }
        }
    }

    /// <summary>
    /// Called whenever a ship is destroyed.
    /// </summary>
    private void OnShipDestroyed(Ship ship)
    {
        if (ship.gameObject == Owner)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// List of needles in the compass.
    /// </summary>
    private List<HUDCompassNeedle> needles = new List<HUDCompassNeedle>();
}

/// <summary>
/// A single needle pointing to a ship.
/// </summary>
struct HUDCompassNeedle
{
    /// <summary>
    /// Create a new indicator.
    /// </summary>
    /// <param name="owner">Ship owning this indicator.</param>
    /// <param name="target">Ship pointed by this indicator.</param>
    public HUDCompassNeedle(Ship owner, Ship target, GameObject prefab)
    {
        this.needle = GameObject.Instantiate(prefab, owner.transform);
        this.target = target;
        this.owner = owner;
        this.renderer = needle.GetComponent<Renderer>();
        this.color = target.LobbyPlayer.Color;

        Ship.ShipDestroyedEvent += OnShipDestroyed;
    }

    /// <summary>
    /// Destroy the needle.
    /// </summary>
    public void OnDestroy()
    {
        Ship.ShipDestroyedEvent -= OnShipDestroyed;

        if(needle)
        {
            GameObject.Destroy(needle);
        }
    }

    /// <summary>
    /// Update the needle position.
    /// </summary>
    public void Update(float length, float alpha_direction)
    {
        if(target)
        {
            var direction = (target.transform.position - owner.transform.position).normalized;      // In world space.

            direction = owner.transform.InverseTransformDirection(direction);                       // In owner space.

            float alpha = Mathf.Clamp(-direction.z + alpha_direction, 0.0f, 1.0f);

            needle.transform.localPosition = direction * length;
            needle.transform.localRotation = Quaternion.identity;

            renderer.material.color = new Color(color.r, color.g, color.b, alpha);
        }
    }

    /// <summary>
    /// Called whenever a ship is destroyed.
    /// </summary>
    private void OnShipDestroyed(Ship ship)
    {
        if (ship.gameObject == target)
        {
            GameObject.Destroy(needle);

            this.target = null;
        }
    }

    /// <summary>
    /// Target of the indicator.
    /// </summary>
    private Ship target;

    /// <summary>
    /// Owner of this needle.
    /// </summary>
    private Ship owner;

    /// <summary>
    /// Color of the indicator.
    /// </summary>
    private Color color;

    /// <summary>
    /// Needle object.
    /// </summary>
    private GameObject needle;

    /// <summary>
    /// Indicator renderer.
    /// </summary>
    private Renderer renderer;
}
