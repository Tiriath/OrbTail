using System;

/// <summary>
/// Base interface for drivers.
/// A driver models a specific behavior of a ship, such as how orbs are attached or detached, how it moves, how much damage it does and takes and so on.
/// </summary>
public interface IDriver
{
    /// <summary>
    /// Deactivate this driver.
    /// </summary>
    void Deactivate();

    /// <summary>
    /// Check whether this driver is active or not.
    /// </summary>
    /// <returns>Returns true if the driver is active, returns false otherwise.</returns>
    bool IsActive();
}
