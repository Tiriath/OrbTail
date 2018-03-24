using System;

/// <summary>
/// Base class for drivers.
/// </summary>
public class BaseDriver : IDriver
{
    public void Deactivate()
    {
        is_active = false;
    }

    public bool IsActive()
    {
        return is_active;
    }

    /// <summary>
    /// Whether this driver is active or not.
    /// Once deactivated a driver cannot be reactivated.
    /// </summary>
    private bool is_active = true;
}
