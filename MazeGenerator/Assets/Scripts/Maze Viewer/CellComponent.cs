using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CellComponent : MonoBehaviour {

	private float size;
	private Vector3 teleportOtherCell;

	public GameObject wallPrefab;
	public GameObject teleporter;



	// Displays the walls on scene and adds colliders too
	public void DisplayWallWithPhysics(Wall w) {
		GameObject newWall = GameObject.Instantiate (wallPrefab, gameObject.transform);

		// Draws a line between the extremeties of the wall using the LineRenderer
		LineRenderer lr = newWall.GetComponent<LineRenderer> ();
		lr.SetPosition (0, new Vector3 (w.Ext1.x, w.Ext1.y, gameObject.transform.position.z-2));
		lr.SetPosition (1, new Vector3 (w.Ext2.x, w.Ext2.y, gameObject.transform.position.z-2));

		// Sets the collider extremeties
		EdgeCollider2D collider = newWall.GetComponent<EdgeCollider2D> ();
		Vector2[] newPoints = new Vector2[2];
		newPoints [0] = w.Ext1;
		newPoints [1] = w.Ext2;
		collider.points = newPoints;
		collider.offset = -new Vector2 (transform.position.x, transform.position.y);
	}


	// Adds a teleporter on the cell in the scene
	public void AddTeleporter(){
		GameObject.Instantiate (teleporter, gameObject.transform);
	}


	// Gets or sets the coordinates to go in case we use the teleporter
	public Vector3 TeleportOtherCell {
		get {
			return teleportOtherCell;
		}
		set {
			teleportOtherCell = value;
		}
	}
}
