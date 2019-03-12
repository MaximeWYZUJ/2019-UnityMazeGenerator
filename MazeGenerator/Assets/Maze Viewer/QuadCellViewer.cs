using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QuadCellViewer : MonoBehaviour {

	private float size;
	public GameObject wallPrefab;


	public void DisplayWall(Wall w) {
		GameObject newWall = GameObject.Instantiate (wallPrefab, gameObject.transform);
		LineRenderer lr = newWall.GetComponent<LineRenderer> ();
		lr.SetPosition (0, new Vector3 (w.Ext1.x, w.Ext1.y, gameObject.transform.position.z-2));
		lr.SetPosition (1, new Vector3 (w.Ext2.x, w.Ext2.y, gameObject.transform.position.z-2));
	}
}
