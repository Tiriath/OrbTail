
/// <summary>
/// Set a ship status to "ready".
/// </summary>
public class HUDShipSetReady : HUDElement
{
    public override void OnInputConfirm()
    {
        var ship = Owner.GetComponent<Ship>();

        ship.SetReady();
    }
}
