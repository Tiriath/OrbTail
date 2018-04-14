using UnityEngine;
using System.Collections;

public class HUDButtonsHandler : MonoBehaviour
{
	public delegate void DelegateOnMissileButtonSelect(object sender, GameObject button);
	
	public event DelegateOnMissileButtonSelect EventOnMissileButtonSelect;

	public delegate void DelegateOnBackButtonSelect(object sender, GameObject button);
	
	public event DelegateOnBackButtonSelect EventOnBackButtonSelect;


	protected void OnSelect(GameObject target) {
		if (target.tag == Tags.PowerButton)
		{
			if (EventOnMissileButtonSelect != null) {
				EventOnMissileButtonSelect(this, target);
			}
		}
		else if (target.tag == Tags.BackButton) {
			if (EventOnBackButtonSelect != null) {
				EventOnBackButtonSelect(this, target);
			}
		}
	}
}
