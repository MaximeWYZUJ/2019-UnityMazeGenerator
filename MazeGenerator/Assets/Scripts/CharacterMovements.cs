using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovements : MonoBehaviour {

	public float speed = 10;
	public float seuil = 10;

	private Rigidbody2D rb;
	private CellComponent currentTeleportCell;



	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		currentTeleportCell = null;
	}


	void Update () {
		// Direction towards the mouse position
		Vector3 direction = Input.mousePosition - Camera.main.WorldToScreenPoint (transform.position);
		float distance = direction.magnitude;


		if (distance > seuil) {
			// The mouse is not on the character
			rb.velocity = new Vector2 (direction.x, direction.y).normalized * speed;
		} else {
			// The mouse/finger is on the character
			rb.velocity = Vector2.zero;
			if (currentTeleportCell != null) {
				// The player is on a teleport
				Vector3 teleportCoordinates = currentTeleportCell.TeleportOtherCell;
				transform.position = teleportCoordinates;
			}
		}
	}



	// COLLISION HANDLERS

	// If the character is on a teleport, we store this teleport in case the mouse gets on the character
	void OnTriggerEnter2D(Collider2D c) {
		if (c.gameObject.tag.Equals ("teleporter")) {
			currentTeleportCell = c.gameObject.GetComponentInParent<CellComponent> ();
		}
	}

	// If the character leaves the teleport, we set the currentTeleportCell as null
	void OnTriggerExit2D(Collider2D c) {
		if (c.gameObject.tag.Equals ("teleporter")) {
			currentTeleportCell = null;
		}
	}
}
