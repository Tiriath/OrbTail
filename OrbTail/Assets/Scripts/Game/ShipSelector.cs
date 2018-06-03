using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

/// <summary>
/// Script used to generate the player configuration after each player confirmed his ship.
/// 
/// </summary>
public class ShipSelector : MonoBehaviour
{
    /// <summary>
    /// Name of the scene to load when the ships have been confirmed.
    /// </summary>
    public SceneField scene;

    public void Awake()
    {
        selectors = FindObjectsOfType<GUIShipSelector>();

        GUIShipSelector.ShipSelectedEvent += OnShipSelected;
    }

    public void OnDestroy()
    {
        GUIShipSelector.ShipSelectedEvent -= OnShipSelected;
    }

    /// <summary>
    /// Called whenever a ship is selected.
    /// </summary>
    /// <param name="sender"></param>
    private void OnShipSelected(GUIShipSelector sender)
    {
        var joined = selectors.Count(selector => selector.PlayerJoined);

        var confirmed = selectors.Where(selector => selector.Confirmed).OrderBy(selector => selector.player_index).ToArray();

        if(confirmed.Length == joined && joined > 0)
        {
            // Create the player configuration for each player.

            for(int index = 0; index < joined; ++index)
            {
                var player_configuration = GameLobby.Instance.gameObject.AddComponent<PlayerConfiguration>();

                player_configuration.player_controller_id = index;
                player_configuration.ship_prefab = confirmed[index].ShipPrefab;
                player_configuration.is_human = true;
            }

            // Load the next scene.

            SceneManager.LoadSceneAsync(scene);
        }
    }

    /// <summary>
    /// List of ship selectors.
    /// </summary>
    private GUIShipSelector[] selectors;
}

