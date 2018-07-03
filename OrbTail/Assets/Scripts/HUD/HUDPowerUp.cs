using UnityEngine;

/// <summary>
/// HUD element used to display a ship's current power up.
/// </summary>
public class HUDPowerUp : HUDElement
{
    public void Awake ()
    {
        renderer_component = GetComponent<SpriteRenderer>();

        renderer_component.enabled = false;
    }

    public void OnDestroy()
    {
        if(power_controller)
        {
            power_controller.PowerAcquiredEvent -= OnPowerAcquired;
        }

        if(power != null)
        {
            OnPowerUpDestroyed(power);
        }
    }

    protected override void OnOwnerChanged()
    {
        if (Owner)
        {
            power_controller = Owner.GetComponent<PowerController>();

            if(power_controller)
            {
                power_controller.PowerAcquiredEvent += OnPowerAcquired;

                OnPowerAcquired(power_controller);

                return;
            }
        }

        renderer_component.enabled = false;
    }

    /// <summary>
    /// Called whenever a new power up is acquired.
    /// </summary>
    /// <param name="sender"></param>
    private void OnPowerAcquired(PowerController sender)
    {
        power = sender.PowerUp;

        if (power != null)
        {
            renderer_component.sprite = power.icon;

            renderer_component.enabled = true;

            // #TODO Change the power icon.
            Debug.Log("Power-up: " + power.power_up_name);

            power.DestroyedEvent += OnPowerUpDestroyed;
        }
        else
        {
            renderer_component.enabled = false;
        }
    }

    /// <summary>
    /// Called whenever a power on this owner is destroyed.
    /// </summary>
    private void OnPowerUpDestroyed(PowerUp power)
    {
        if(this.power == power)
        {
            this.power = null;

            renderer_component.enabled = false;
        }

        power.DestroyedEvent -= OnPowerUpDestroyed;
    }

    /// <summary>
    /// Current power.
    /// </summary>
    private PowerUp power;

    /// <summary>
    /// Power-up controller on the current owner.
    /// </summary>
    private PowerController power_controller;

    /// <summary>
    /// Renderer component.
    /// </summary>
    private SpriteRenderer renderer_component;
}
