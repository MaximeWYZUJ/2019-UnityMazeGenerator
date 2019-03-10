using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MazeViewer
{
	public static void DisplayGrid(GraphVertex[,] maze, GameObject vertexPrefab) {
		foreach (GraphVertex gv in maze) {
			Vector2 gvPosition = gv.CoreCoordinates;
			GameObject.Instantiate (vertexPrefab, new Vector3 (gvPosition.x, gvPosition.y, 0), Quaternion.identity);
		}
	}
}

