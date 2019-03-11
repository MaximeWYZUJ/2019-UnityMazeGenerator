using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MazeViewer
{
	public static void DisplayGrid(IEnumerable maze, GameObject vertexPrefab) {
		foreach (GraphVertex gv in maze) {
			Vector2 gvPosition = gv.CoreCoordinates;
			GameObject.Instantiate (vertexPrefab, new Vector3 (gvPosition.x, gvPosition.y, 0), Quaternion.identity);

			foreach (Vector2 connectedGvCoo in gv.ConnectedGraphVerticesCoordinates) {
				Debug.DrawLine (gv.CoreCoordinates, connectedGvCoo, Color.green, 2000);
			}


			foreach (KeyValuePair<Vector2, Vector2> kv in gv.ConnectedWalls) {
				Debug.DrawLine (kv.Key, kv.Value, Color.yellow, 2000);
			}
		}
	}
}

