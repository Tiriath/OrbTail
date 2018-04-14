using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class HUDMenuButtonHandler : MonoBehaviour
{
	private TextMesh exitMessage;
	private SpriteRenderer menuSprite;
	private float shortFadeTime = 0.1f;
	private float longFadeTime = 2f;
	private bool alreadyPressed = false;
	private Color originalButtonColor;

	// Use this for initialization
	public void Start () {
		exitMessage = transform.Find("ExitMessage").GetComponent<TextMesh>();
		menuSprite = transform.Find("MenuButtonSprite").GetComponent<SpriteRenderer>();
		originalButtonColor = menuSprite.color;
	}
	
	protected void OnSelect (GameObject target)
	{
		if (target.name == "MenuButtonSprite") {
			if (!alreadyPressed) {
				alreadyPressed = true;
				menuSprite.color = Color.white;

				iTween.ValueTo(this.gameObject, iTween.Hash(
					"from", 0f,
					"to", 1f,
					"time", shortFadeTime,
					"onUpdate","ChangeAlphaColor",
					"onComplete", "WaitSecondTap"));
			}
			else {
				Destroy(GameObject.FindGameObjectWithTag(Tags.Master));
				
				GameObjectFactory.Instance.Purge();
				
				//Okay, good game, let's go home...
				Network.Disconnect();
				
				SceneManager.LoadScene("MenuMain");
			}
		}
	}

	private void WaitSecondTap() {
		iTween.ValueTo(this.gameObject, iTween.Hash(
			"from", 1f,
			"to", 0f,
			"time", longFadeTime,
			"easetype", iTween.EaseType.easeInCubic,
			"onUpdate","ChangeAlphaColor",
			"onComplete", "ResetButton"));
	}

	private void ResetButton() {
		alreadyPressed = false;
		menuSprite.color = originalButtonColor;
	}

	private void ChangeAlphaColor(float alpha) {
		Color color = exitMessage.color;
		color.a = alpha;
		exitMessage.color = color;
	}
}
