﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TailController : MonoBehaviour {

	private DriverStack<IAttacherDriver> attacherDriverStack;
	private DriverStack<IDetacherDriver> detacherDriverStack;
	private DriverStack<IOffenceDriver> offenceDriverStack;
	private DriverStack<IDefenceDriver> defenceDriverStack;

	private EventLogger eventLogger;

	public Tail Tail { get; set;}

	private float dotProductAttackThreshold = 0.2f;
	// Range [-1, 1] 
	private float velocityAttackThreshold = -0.3f;


	/// <summary>
	/// Gets the attacher driver stack.
	/// </summary>
	/// <returns>The attacher driver stack.</returns>
	public DriverStack<IAttacherDriver> GetAttacherDriverStack() {
		return attacherDriverStack;
	}


	/// <summary>
	/// Gets the detacher driver stack.
	/// </summary>
	/// <returns>The detacher driver stack.</returns>
	public DriverStack<IDetacherDriver> GetDetacherDriverStack() {
		return detacherDriverStack;
	}


	/// <summary>
	/// Gets the offence driver stack.
	/// </summary>
	/// <returns>The offence driver stack.</returns>
	public DriverStack<IOffenceDriver> GetOffenceDriverStack() {
		return offenceDriverStack;
	}


	/// <summary>
	/// Gets the defence driver stack.
	/// </summary>
	/// <returns>The defence driver stack.</returns>
	public DriverStack<IDefenceDriver> GetDefenceDriverStack() {
		return defenceDriverStack;
	}



	void Awake() {
		attacherDriverStack = new DriverStack<IAttacherDriver>();
		detacherDriverStack = new DriverStack<IDetacherDriver>();
		offenceDriverStack = new DriverStack<IOffenceDriver>();
		defenceDriverStack = new DriverStack<IDefenceDriver>();

		eventLogger = GameObject.FindGameObjectWithTag(Tags.Game).GetComponent<EventLogger>();
	}

	void Start () {
		Tail = GetComponent<Tail>();

		// Link proximity field
		ProximityHandler proximityField = GetComponentInChildren<ProximityHandler>();
		proximityField.EventOnProximityEnter += OnProximityEnter;
	}

	void OnCollisionEnter(Collision collision) {
		GameObject collidedObj = collision.gameObject;

		if (collidedObj.tag == Tags.Ship) {

			if (IsAttack(collidedObj)) {
				float damage = collidedObj.GetComponent<TailController>().GetOffenceDriverStack().GetHead().GetDamage(this.gameObject, collision);
				int nOrbsToDetach = defenceDriverStack.GetHead().DamageToOrbs(damage);
				List<GameObject> orbsDetached = detacherDriverStack.GetHead().DetachOrbs(nOrbsToDetach, this.Tail);

				eventLogger.NotifyFight(orbsDetached, collidedObj, this.gameObject);
			}

		}
	}

	void OnProximityEnter(object sender, Collider other) {
		GameObject collidedObj = other.gameObject;
		
		if (collidedObj.tag == Tags.Orb) {
			OrbController orbController = collidedObj.GetComponent<OrbController>();
			
			if (!orbController.IsAttached()) {
				attacherDriverStack.GetHead().AttachOrbs(collidedObj, Tail);
			}
			
		}
	}
	

	// Update is called once per frame
	void Update () {
	
	}


	/// <summary>
	/// Determines whether the specified attacker is attacking this instance.
	/// </summary>
	/// <returns><c>true</c> if the specified attacker is attacking this instance; otherwise, <c>false</c>.</returns>
	/// <param name="attacker">Attacker.</param>
	private bool IsAttack(GameObject attacker) {
		Vector3 relVector = this.transform.position - attacker.transform.position;
		float dotProduct = Vector3.Dot(attacker.transform.forward, relVector.normalized);

		float attackVelocity = Vector3.Dot (attacker.rigidbody.velocity.normalized, attacker.transform.forward);
		Debug.Log (attackVelocity);
		return dotProduct >= dotProductAttackThreshold && attackVelocity > velocityAttackThreshold;
	}
}
