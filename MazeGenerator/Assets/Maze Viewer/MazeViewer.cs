using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MazeViewer
{
	public static void DisplayGrid(IEnumerable maze, GameObject vertexPrefab, float deltaTime) {
		foreach (GraphVertex gv in maze) {
			//Vector2 gvPosition = gv.CoreCoordinates;
			//GameObject.Instantiate (vertexPrefab, new Vector3 (gvPosition.x, gvPosition.y, 0), Quaternion.identity);

			// Displays the connexions between the cell cores
			/*foreach (Vector2 connectedGvCoo in gv.ConnectedGraphVerticesCoordinates) {
				Debug.DrawLine (gv.CoreCoordinates, connectedGvCoo, Color.green, 2000);
			}*/


			// Displays the walls
			foreach (Wall w in gv.Walls) {
				Debug.DrawLine (w.Ext1, w.Ext2, Color.yellow, deltaTime);
			}
		}
	}
}

