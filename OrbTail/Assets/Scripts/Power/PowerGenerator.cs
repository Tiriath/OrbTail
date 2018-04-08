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
        orbs = new List<OrbController>(GameObject.FindGameObjectsWithTag(Tags.Orb).Select(orb => orb.GetComponent<OrbController>()));

        GameObjectFactory.Instance.Preload(OrbController.glow_path, 10);                    // Preload VFX. #TODO Remove this from here!
    }

    void Update ()
    {
        if (Time.time > imbue_timestamp + delta_generation)
        {
            imbue_timestamp = Time.time;

            ImbueRandomOrb();
        }
    }

    /// <summary>
    /// Imbue a random orb with a random power.
    /// </summary>
    private void ImbueRandomOrb()
    {
        int left_index = 0;
        int right_index = orbs.Count - 1;

        // O(#orbs) - Sort the orbs array such that orbs on the "right" side are the orbs that can be used to spawn a power upon.

        while (left_index < right_index)
        {
            while (left_index < orbs.Count && (orbs[left_index].IsLinked || orbs[left_index].GetImbuedPower() != null))
            {
                ++left_index;
            }

            while (right_index > 0 && !orbs[right_index].IsLinked && orbs[right_index].GetImbuedPower() == null)
            {
                --right_index;
            }

            if (left_index < right_index)
            {
                var temp = orbs[left_index];
                orbs[left_index] = orbs[right_index];
                orbs[right_index] = temp;
            }
        }

        if (left_index < orbs.Count)
        {
            var index = rng.Next(left_index, orbs.Count);

            orbs[index].ImbuePower(PowerFactory.Instance.RandomPower);
        }
    }

    /// <summary>
    /// Last time an orb was imbued with a power.
    /// </summary>
    private float imbue_timestamp;

    /// <summary>
    /// Random number generator used to spawn random powers.
    /// </summary>
    private System.Random rng = new System.Random();

    /// <summary>
    /// List of orbs in the arena.
    /// </summary>
    private IList<OrbController> orbs = null;
}
