using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Object used to instantiate and cache powers and powers VFX.
/// </summary>
public class PowerFactory
{
    /// <summary>
    /// Get the singleton instance.
    /// </summary>
    public static PowerFactory Instance
    {
        get
        {
            if (singleton == null)
            {
                singleton = new PowerFactory();
            }

            return singleton;
        }
    }

    /// <summary>
    /// Generate a random power.
    /// </summary>
    public Power RandomPower
    {
        get
        {
            int value = random.Next(total_drop_rate);

            return power_table.Values.SkipWhile((Power power) =>
            {
                value -= power.DropRate;
                return value >= 0;

            }).First().Generate();
        }
    }

    /// <summary>
    /// Generate a power archetype from name.
    /// </summary>
    /// <param name="name">Name of the power to get.</param>
    /// <returns>Returns the power matching the provided name.</returns>
    public Power GetPower(string name)
    {
        return power_table[name];
    }

    /// <summary>
    /// Register a new power
    /// </summary>
    private void RegisterPower(Power power)
    {
        power_table.Add(power.Name, power);
        total_drop_rate += power.DropRate;
    }

    private PowerFactory()
    {
        RegisterPower(new Boost());
        RegisterPower(new Missile());
        RegisterPower(new Invincibility());
        RegisterPower(new OrbSteal());
        RegisterPower(new Magnet());
        RegisterPower(new Jam());

        // Preload VFX stuffs so they don't have to be allocated during active gameplay.

        GameObjectFactory.Instance.Preload(Power.kPowerPrefabPath + "Boost", 4);
        GameObjectFactory.Instance.Preload(Power.kPowerPrefabPath + "Missile", 4);
        GameObjectFactory.Instance.Preload(Power.kPowerPrefabPath + "Invincibility", 4);
        GameObjectFactory.Instance.Preload(Power.kPowerPrefabPath + "OrbSteal", 4);
        GameObjectFactory.Instance.Preload(Power.kPowerPrefabPath + "Magnet", 4);
        GameObjectFactory.Instance.Preload(Power.kPowerPrefabPath + "Jam", 3);
        GameObjectFactory.Instance.Preload(MissileBehavior.explosion_prefab_path, 4);
    }

    /// <summary>
    /// Random number generator
    /// </summary>
    private Random random = new System.Random();

    /// <summary>
    /// Sum of all powers' drop rate.
    /// </summary>
    private int total_drop_rate = 0;

    /// <summary>
    /// The table containing all the power archetypes.
    /// </summary>
    private static IDictionary<string, Power> power_table = new Dictionary<string, Power>();

    /// <summary>
    /// Singleton instance.
    /// </summary>
    private static PowerFactory singleton = null;
}