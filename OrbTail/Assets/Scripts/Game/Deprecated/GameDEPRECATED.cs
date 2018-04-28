using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameDEPRECATED : MonoBehaviour
{
    public const string explosion_prefab_path = "Prefabs/Power/Explosion";

    private void RemoveShip(GameObject ship)
    {
        //         //Enables the spectator mode
        //         if (ship == ActivePlayer)
        //         {
        //             Camera.GetComponent<SpectatorMode>().enabled = true;
        //         }

        //         StartCoroutine(ShipExplosion(ship.transform.position));
    }

    private IEnumerator ShipExplosion(Vector3 position)
    {
        //         GameObject explosion = GameObject.Instantiate(explosion_resource_, position, Quaternion.identity) as GameObject;
        // 
        //         AudioSource.PlayClipAtPoint(explosion_sound_, position);
        // 
        //         // Delayed for GFX
        //         yield return new WaitForSeconds(1.0f);
        // 
        //         Destroy(explosion);

        return null;
    }

    void master_EventPlayerLeft(object sender, int id)
    {
        //         //Restores the orbs
        // 
        //         var disconnected_player = (from player in ShipsInGame
        //                                     where player.GetComponent<GameIdentity>().Id == id
        //                                     select player).First();
        // 
        //         //Detaches all orbs from the player's tail
        // 
        //         disconnected_player.GetComponent<Tail>().DetachOrbs(int.MaxValue);
        // 
        //         //Removes the disconnected player
        //         RemoveShip(disconnected_player);
    }

    /// <summary>
    /// Restarts the game. Temporary method
    /// </summary>
    private IEnumerator RestartGame()
    {
        //         yield return new WaitForSeconds(restart_time);
        // 
        //         Destroy(GameObject.FindGameObjectWithTag(Tags.Master));
        // 
        //         GameObjectFactory.Instance.Purge();
        // 
        //         //Okay, good game, let's go home...
        //         Network.Disconnect();
        // 
        //         SceneManager.LoadScene("MenuMain");

        return null;
    }
}
