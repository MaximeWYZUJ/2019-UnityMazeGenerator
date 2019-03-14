using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pair<T>
{
	// Elements of the pair
	private T ext1, ext2;

	// CONSTRUCTOR
	public Pair(T ext1, T ext2) {
		this.ext1 = ext1;
		this.ext2 = ext2;
	}



	// METHODS
	// Checks if this unordered pair is the same a the provided pair
	public bool IsEqual(Pair<T> other) {
		bool cond1 = (this.ext1.Equals (other.ext1) && this.ext2.Equals (other.ext2));
		bool cond2 = (this.ext1.Equals (other.ext2) && this.ext2.Equals (other.ext1));

		return cond1 || cond2;
	}


	// Checks if there is this unordered pair in the provided list
	public bool AlreadyExists(IEnumerable<Pair<T>> list) {
		foreach (Pair<T> w in list) {
			if (this.IsEqual (w)) {
				// This pair is already in the list
				return true;
			}
		}
		// This pair is not in the list
		return false;
	}


	// Checks if there is this ordered pair in the provided list
	public bool OrderedValueExistsIn(IEnumerable<Pair<T>> list) {
		foreach (Pair<T> w in list) {
			if (w.ext1.Equals (this.ext1) && w.ext2.Equals (this.ext2)) {
				return true;
			}
		}
		// This pair is not in the list
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
