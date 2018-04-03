using System;
using System.Collections.Generic;
   
/// <summary>
/// Interface implemented by all the powers.
/// </summary>
public abstract class PowerView
{
    public delegate void DelegateDestroyed(object sender, int group);

    /// <summary>
    /// Fired when the power has been destroyed and cannot be used anymore
    /// </summary>
    public event DelegateDestroyed EventDestroyed;
    
    /// <summary>
    /// Get the group of this power.
    /// </summary>
    public abstract int Group { get; }
    
    /// <summary>
    /// Get the readyness of this power. 1 if ready, less than 1 otherwise.
    /// </summary>
    public abstract float IsReady{ get; }

    /// <summary>
    /// Called whenever the power is destroyed.
    /// </summary>
    protected void Destroy()
    {
        if (EventDestroyed != null)
        {
            EventDestroyed(this, Group);
        }
    }

}
