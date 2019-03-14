using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MazeViewer
{
	public static void DisplayGrid(IEnumerable maze, GameObject vertexPrefab) {
		foreach (GraphVertex gv in maze) {
			Vector2 gvPosition = gv.CoreCoordinates;
			GameObject objCore = GameObject.Instantiate (vertexPrefab, new Vector3 (gvPosition.x, gvPosition.y, vertexPrefab.transform.position.z), Quaternion.identity);
			if (gv.Mark == MarkType.Unvisited) {
				objCore.GetComponent<SpriteRenderer> ().color = Color.white;
			}

			// Displays the walls
			QuadCellViewer cellViewer = objCore.GetComponent<QuadCellViewer>();
			foreach (Wall w in gv.Walls) {
				cellViewer.DisplayWall (w);
			}
		}
	}
}

