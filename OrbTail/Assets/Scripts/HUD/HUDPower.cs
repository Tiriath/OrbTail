using UnityEngine;

/// <summary>
/// HUD element used to display a ship's current power.
/// Powers can be filtered by group.
/// </summary>
public class HUDPower : HUDElement
{
    /// <summary>
    /// Power group this element reacts to.
    /// </summary>
    public int power_group;

    public void Start ()
    {
        GetComponent<Renderer>().enabled = false;
    }

    public void Update ()
    {
    
    }

    protected override void OnOwnerChanged()
    {
        //var power_controller = Owner.Ship.GetComponent<PowerController>();

        //power_controller.PowerAcquiredEvent += OnPowerAcquired;
    }

    private void OnPowerAcquired(PowerController sender, PowerUp power)
    {
        if (power.Group == power_group)
        {
            if(this.power != null)
            {
                OnPowerDeactivated(this.power);         // Forget about the old power.
            }

            this.power = power;

            power.DeactivatedEvent += OnPowerDeactivated;

            //power.Name == "Missile"
            //GetComponent<Renderer>().enabled = true;
        }
    }

    private void OnPowerDeactivated(PowerUp power)
    {
        this.power = null;

        power.DeactivatedEvent -= OnPowerDeactivated;

        //GetComponent<Renderer>().enabled = false;
    }

    /// <summary>
    /// Current power.
    /// </summary>
    private PowerUp power;
}
