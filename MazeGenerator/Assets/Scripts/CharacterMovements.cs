using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovements : MonoBehaviour {

	public float speed = 10;
	public float seuil = 10;

	private Rigidbody2D rb;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();
	}
	
	void Update () {
		Vector3 direction = Input.mousePosition - Camera.main.WorldToScreenPoint (transform.position);
		float distance = direction.magnitude;

		if (distance > seuil) {
			rb.velocity = new Vector2 (direction.x, direction.y).normalized * speed;
		} else {
			rb.velocity = Vector2.zero;
		}
	}
}
