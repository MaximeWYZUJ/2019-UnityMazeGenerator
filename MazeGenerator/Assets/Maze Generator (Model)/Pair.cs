using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pair<T>
{

	private T ext1, ext2;

	// CONSTRUCTOR
	public Pair(T ext1, T ext2) {
		this.ext1 = ext1;
		this.ext2 = ext2;
	}



	// METHODS
	public bool IsEqual(Pair<T> other) {
		bool cond1 = (this.ext1.Equals (other.ext1) && this.ext2.Equals (other.ext2));
		bool cond2 = (this.ext1.Equals (other.ext2) && this.ext2.Equals (other.ext1));

		return cond1 || cond2;
	}

	public bool AlreadyExists(IEnumerable<Pair<T>> list) {
		foreach (Pair<T> w in list) {
			if (this.IsEqual (w)) {
				// This wall is already in the list
				return true;
			}
		}

		// This wall is not in the list
		return false;
	}



	// GETTERS
	public T Ext1 {
		get{
			return this.ext1;
		}
	}

	public T Ext2 {
		get{
			return this.ext2;
		}
	}
}
