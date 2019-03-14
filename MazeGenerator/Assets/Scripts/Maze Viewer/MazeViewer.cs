using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MazeViewer
{
	public static void DisplayGrid(IEnumerable maze, GameObject vertexPrefab) {
		foreach (GraphVertex gv in maze) {
			Vector2 gvPosition = gv.CoreCoordinates;
			GameObject objCore = GameObject.Instantiate (vertexPrefab, new Vector3 (gvPosition.x, gvPosition.y, vertexPrefab.transform.position.z), Quaternion.identity);

			// Displays the teleport, if any
			if (gv.TeleportCell != null) {
				objCore.GetComponent<SpriteRenderer> ().color = gv.TeleportColor;
			}

			// Displays the walls
			CellComponent cellViewer = objCore.GetComponent<CellComponent>();
			foreach (Wall w in gv.Walls) {
				cellViewer.DisplayWallWithPhysics (w);
			}
		}
	}
}

