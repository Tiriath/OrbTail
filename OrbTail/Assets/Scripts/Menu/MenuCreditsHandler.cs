using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuCreditsHandler : GUIMenuChoose {

	protected override void OnSelect (GameObject target)
	{
		base.OnSelect (target);
		if (target.tag == Tags.BackButton) {
			SceneManager.LoadScene("MenuMain");
		}
	}
}
