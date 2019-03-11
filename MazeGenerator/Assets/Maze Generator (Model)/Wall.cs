using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Wall
{

	private Vector2 ext1, ext2;

	// CONSTRUCTOR
	public Wall (Vector2 ext1, Vector2 ext2) {
		this.ext1 = ext1;
		this.ext2 = ext2;
	}



	// METHODS
	public bool IsEqual(Wall other) {
		bool cond1 = (this.ext1.Equals (other.ext1) && this.ext2.Equals (other.ext2));
		bool cond2 = (this.ext1.Equals (other.ext2) && this.ext2.Equals (other.ext1));

		return cond1 || cond2;
	}

	public bool AlreadyExists(IEnumerable<Wall> list) {
		foreach (Wall w in list) {
			if (this.IsEqual (w)) {
				// This wall is already in the list
				return true;
			}
		}

		// This wall is not in the list
		return false;
	}



	// GETTERS
	public Vector2 Ext1 {
		get{
			return this.ext1;
		}
	}

	public Vector2 Ext2 {
		get{
			return this.ext2;
		}
	}
}
