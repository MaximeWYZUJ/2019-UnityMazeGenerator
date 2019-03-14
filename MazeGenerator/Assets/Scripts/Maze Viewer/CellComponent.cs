using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CellComponent : MonoBehaviour {

	private float size;
	public GameObject wallPrefab;


	public void DisplayWall(Wall w) {
		GameObject newWall = GameObject.Instantiate (wallPrefab, gameObject.transform);
		LineRenderer lr = newWall.GetComponent<LineRenderer> ();
		lr.SetPosition (0, new Vector3 (w.Ext1.x, w.Ext1.y, gameObject.transform.position.z-2));
		lr.SetPosition (1, new Vector3 (w.Ext2.x, w.Ext2.y, gameObject.transform.position.z-2));
	}

	public void DisplayWallWithPhysics(Wall w) {
		GameObject newWall = GameObject.Instantiate (wallPrefab, gameObject.transform);
		LineRenderer lr = newWall.GetComponent<LineRenderer> ();
		lr.SetPosition (0, new Vector3 (w.Ext1.x, w.Ext1.y, gameObject.transform.position.z-2));
		lr.SetPosition (1, new Vector3 (w.Ext2.x, w.Ext2.y, gameObject.transform.position.z-2));

		EdgeCollider2D collider = newWall.GetComponent<EdgeCollider2D> ();
		Vector2[] newPoints = new Vector2[2];
		newPoints [0] = w.Ext1;
		newPoints [1] = w.Ext2;
		collider.points = newPoints;
		collider.offset = -new Vector2 (transform.position.x, transform.position.y);
	}
}
