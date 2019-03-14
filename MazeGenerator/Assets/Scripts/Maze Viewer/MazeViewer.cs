using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MazeViewer
{
	public static void DisplayGrid(IEnumerable maze, GameObject vertexPrefab) {
		foreach (GraphVertex gv in maze) {
			Vector2 gvPosition = gv.CoreCoordinates;
			GameObject objCore = GameObject.Instantiate (vertexPrefab, new Vector3 (gvPosition.x, gvPosition.y, vertexPrefab.transform.position.z), Quaternion.identity);

			// Displays the teleport, if any, and stores the coordinates of the teleport-connected cell
			CellComponent cellViewer = objCore.GetComponent<CellComponent>();
			if (gv.TeleportCell != null) {
				
				// Stores the teleport coordinates into the CellComponent of gv
				Vector3 teleportCellCoo = new Vector3 (gv.TeleportCell.CoreCoordinates.x, gv.TeleportCell.CoreCoordinates.y, -2);
				cellViewer.TeleportOtherCell = teleportCellCoo;

				// Sets the color of the cell according to the teleporter
				objCore.GetComponent<SpriteRenderer> ().color = gv.TeleportColor;

				// Instantiates the teleporter
				cellViewer.AddTeleporter();
			}


			// Displays the walls
			foreach (Wall w in gv.Walls) {
				cellViewer.DisplayWallWithPhysics (w);
			}
		}
	}
}

