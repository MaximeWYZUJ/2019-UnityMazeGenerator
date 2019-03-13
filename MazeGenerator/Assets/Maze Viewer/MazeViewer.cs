using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MazeViewer
{
	public static void DisplayGrid(IEnumerable maze, GameObject vertexPrefab) {
		List<Vertex> allPoints = new List<Vertex> ();
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

			RegularCell r = (RegularCell)gv;
			foreach (Vertex v in r.cornerPoints) {
				if (!allPoints.Contains (v)) {
					allPoints.Add (v);
				}
			}
		}
		/*Debug.Log ("Nb points :  " + allPoints.Count);
		foreach (Vertex v in allPoints) {
			if (v.Neighbours.Count == 6) {
				Debug.Log ("Coo :  " + v.Coo);
				foreach (Vertex v2 in v.Neighbours) {
					Debug.Log (v2.Coo);
				}
			}
		}*/


	}
}

