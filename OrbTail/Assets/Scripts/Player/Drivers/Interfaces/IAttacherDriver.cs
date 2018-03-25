using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A driver used to determine how orbs are attached to a ship.
/// </summary>
public interface IAttacherDriver : IDriver
{

	/// <summary>
	/// Attach the provided orb to a tail.
	/// </summary>
	/// <param name="orb">The orb to attach.</param>
	/// <param name="tail">The tail to attach the orb to.</param>
	void AttachOrbs(GameObject orb, Tail tail);


	/// <summary>
	/// Attach any number of orbs to a tail.
	/// </summary>
	/// <param name="orbs">Orbs to attach.</param>
	/// <param name="tail">The tail to attach the orbs to.</param>
	void AttachOrbs(List<GameObject> orbs, Tail tail);

}
