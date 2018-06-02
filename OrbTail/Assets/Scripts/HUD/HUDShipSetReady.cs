using System.Linq;

/// <summary>
/// Set local ship status to "ready".
/// </summary>
public class HUDShipSetReady : HUDElement
{
    public override void OnInputConfirm()
    {
        foreach(var ship in FindObjectsOfType<Ship>().Where(ship => ship.isLocalPlayer))
        {
            ship.SetReady();
        }
    }
}
