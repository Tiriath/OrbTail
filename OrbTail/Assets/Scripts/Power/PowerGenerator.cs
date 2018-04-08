using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Game component used to generate random powers on existing unlinked orbs.
/// </summary>
public class PowerGenerator : MonoBehaviour
{
    /// <summary>
    /// Time between power generation.
    /// </summary>
    public float delta_generation = 20.0f;

    void Start()
    {
        foreach(var orb in GameObject.FindGameObjectsWithTag(Tags.Orb))
        {
            orbs.Add(orb.GetComponent<OrbController>());                                    // Cache orbs.
        }

        GameObjectFactory.Instance.Preload(OrbController.glow_path, 10);                    // Preload VFX.
    }

    void Update ()
    {
        if (NetworkHelper.IsServerSide())
        {
            if (Time.time > generation_timestamp + delta_generation)
            {
                generation_timestamp = Time.time;

                var orb = RandomOrb;

                if(orb)
                {
                    SpawnPower(orb);
                }
            }
        }
    }

    /// <summary>
    /// Spawn a new power on the provided orb.
    /// </summary>
    /// <param name="orb">Orb to spawn the power on.</param>
    private void SpawnPower(GameObject orb)
    {
        orb.GetComponent<OrbController>().PowerEnabled = true;

        if (Network.isServer)
        {
            GetComponent<NetworkView>().RPC("RPCSpawnPower", RPCMode.OthersBuffered, orb.GetComponent<NetworkView>().viewID);
        }
    }

    /// <summary>
    /// Spawn a new power on the provided orb. RPC.
    /// </summary>
    /// <param name="orb_view_id"></param>
    [RPC]
    private void RPCSpawnPower(NetworkViewID orb_view_id)
    {
        SpawnPower(NetworkView.Find(orb_view_id).gameObject);
    }

    /// <summary>
    /// Get a random orb that can be used to spawn a power.
    /// </summary>
    private GameObject RandomOrb
    {
        get
        {
            int left_index = 0;
            int right_index = orbs.Count - 1;

            // O(#orbs) - Sort the orbs array such that orbs on the "right" side are the orbs that can be used to spawn a power upon.

            while(left_index < right_index)
            {
                while(left_index < orbs.Count && (orbs[left_index].IsLinked || orbs[left_index].PowerEnabled))
                {
                    ++left_index;
                }
                
                while(right_index > 0 && !orbs[right_index].IsLinked && !orbs[right_index].PowerEnabled)
                {
                    --right_index;
                }

                if(left_index < right_index)
                {
                    var temp = orbs[left_index];
                    orbs[left_index] = orbs[right_index];
                    orbs[right_index] = temp;
                }
            }

            if(left_index < orbs.Count)
            {
                var index = rng.Next(left_index, orbs.Count);

                return orbs[index].gameObject;
            }

            return null;
        }
    }

    /// <summary>
    /// Last time a power was generated.
    /// </summary>
    private float generation_timestamp;

    /// <summary>
    /// Random number generator used to spawn random powers.
    /// </summary>
    private System.Random rng = new System.Random();

    /// <summary>
    /// List of orbs in the arena.
    /// </summary>
    private IList<OrbController> orbs = new List<OrbController>();
}
