using UnityEngine;

/// <summary>
/// HUD element used to display a big message to the player.
/// </summary>
public class HUDMainMessage : MonoBehaviour
{

    // Countdown: 3 | 2 | 1 
    // Start: GO!
    // Game over: YOU WIN | YOU LOSE | TIE | SERVER LEFT + dimming

    private TextMesh textMeshCountdown;

    private float blankOverlayFinalAlpha = 0.8f;
    private int fontBigSize = 130;
    private float standardLightPower;
    private GameObject blankOverlay;
    
    // Use this for initialization
    void Start () {

        textMeshCountdown = gameObject.GetComponent<TextMesh>();

        blankOverlay = Instantiate(Resources.Load("Prefabs/HUD/BlankOverlay")) as GameObject;
        blankOverlay.GetComponent<Renderer>().material.color = new Color(0,0,0,0);
        blankOverlay.transform.parent = transform;
        blankOverlay.transform.localPosition = Vector3.forward * 40f;
        blankOverlay.SetActive(false);
    }
    
    private void OnStart(object sender, int countdown)
    {

        if (countdown > 0)
        {
            textMeshCountdown.text = countdown.ToString();
        }
        else
        {

            textMeshCountdown.color = Color.red;
            textMeshCountdown.fontSize = fontBigSize;
            textMeshCountdown.text = "GO!";
            iTween.ValueTo(this.gameObject, iTween.Hash(
                "from", 1f,
                "to", 0f,
                "time", 2f,
                "onUpdate","ChangeAlphaColor"));
        }

    }

    private void ChangeAlphaColor(float alpha) {
        Color color = textMeshCountdown.color;
        color.a = alpha;
        textMeshCountdown.color = color;
    }

    private void OnGameOver(object sender, GameObject winner, int info) {
        blankOverlay.SetActive(true);
        iTween.FadeTo(blankOverlay, blankOverlayFinalAlpha, 2f);
        textMeshCountdown.text = "Game Over";
        iTween.ValueTo(this.gameObject, iTween.Hash(
            "from", 0f,
            "to", 1f,
            "time", 2f,
            "onUpdate","ChangeAlphaColor"));
    }

}
