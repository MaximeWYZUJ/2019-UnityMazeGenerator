using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface GraphVertex {
	// Removes a wall between this cell and another adjacent cell
	// Does nothing if the 2 cells are not adjacent
	void RemoveBorderBetweenCells (GraphVertex otherGraphVertex);

	// Joins the cores of this cell and another and merges their common borders
	void ConnectToOtherGraphVertex (GraphVertex other);

	// Gets the coordinates of the core of this cell
	Vector2 CoreCoordinates { get; }

	// Gets the coordinates of all the cores of the adjacent cells
	List<Vector2> ConnectedGraphVerticesCoordinates { get; }

	// Gets the adjacents cells
	List<GraphVertex> Neighbours { get; }

	// Gets the pairs of coordinates which represent the extremities of the walls
	List<Wall> Walls { get; }

	// Sets this cell as visited
	void SetVisited();

	// Gets the current mark of this cell (visited or not)
	MarkType Mark { get; set; }

	// Gets a random adjacent cell with the specified mark
	// returns null if there is no cell with this mark
	GraphVertex GetRandomMarkedNeighbour (MarkType mark);

	// Gets a all the adjacent cells with the specified mark
	List<GraphVertex> GetMarkedNeighbours (MarkType mark);

	// Returns the cell connected by the teleport, or null if there is no teleport
	GraphVertex TeleportCell { get; set; }

	// Gets or sets the color of the teleport (if any)
	Color TeleportColor { get; set; }
}


public enum MarkType {Visited, Unvisited, TemporaryVisited};


public class UndirectedGraph
{

	// Generates a grid of QuadCell
	public static IEnumerable<GraphVertex> GridCellUndirectedGraph (float cellSize, int nbLines, int nbColumns, int nbBorders, bool putTeleporters) {
		RegularCell[,] vertices = new RegularCell[nbLines, nbColumns];


		// Initialization of each cell
		for (int i = 0; i < nbLines; i++) {
			for (int j = 0; j < nbColumns; j++) {
				// For each cell, we calculate its center according to the grid
				// and the optional offsets due to the shape (square/hexagon) and the line number (odd or even)
				Pair<float> angleOffsets = UndirectedGraph.DetermineAngleOffsets (nbBorders);
				Pair<float> axisOffsets = DetermineAxisOffsets (nbBorders, cellSize);

				float coreX = j * cellSize * 2 * Mathf.Cos (angleOffsets.Ext1);
				float coreY = i * cellSize * 2 * Mathf.Sin (angleOffsets.Ext2);

				coreY += axisOffsets.Ext2 * 2 * Mathf.Floor (i / 2);

				if (i % 2 == 0) {
					coreX += axisOffsets.Ext1;
					coreY += axisOffsets.Ext2;
				} else {
					coreY += 2 * axisOffsets.Ext2;
				}

				Vector2 coreCoo = new Vector2 (coreX, coreY);

				vertices [i, j] = new RegularCell (coreCoo, cellSize, nbBorders, angleOffsets.Ext1);
			}
		}

		// We define the number and random indexes of the teleporters
		List<Pair<int>> teleportersIndexes = new List<Pair<int>> ();
		if (putTeleporters) {
			// Sets the indexes of the cells with a teleporter and the number of pairs of teleporters
			int nbTeleporters = Mathf.FloorToInt(nbLines * nbColumns / 40);

			// Find the right amound of distinct indexes
			for (int k = 0; k < 2 * nbTeleporters; k++) {
				Pair<int> p = new Pair<int> (Random.Range (0, nbLines - 1), Random.Range (0, nbColumns - 1));

				while (p.OrderedValueExistsIn(teleportersIndexes)) {
					p = new Pair<int> (Random.Range (0, nbLines - 1), Random.Range (0, nbColumns - 1));
				}
				teleportersIndexes.Add (p);
			}

			// Add a teleporter in the data of the corresponding cells
			for (int k = 0; k < 2 * nbTeleporters; k += 2) {
				Pair<int> id1 = teleportersIndexes [k];
				Pair<int> id2 = teleportersIndexes [k + 1];

				vertices [id1.Ext1, id1.Ext2].TeleportCell = vertices [id2.Ext1, id2.Ext2];
				vertices [id2.Ext1, id2.Ext2].TeleportCell = vertices [id1.Ext1, id1.Ext2];

				Color c = Random.ColorHSV ();
				vertices [id1.Ext1, id1.Ext2].TeleportColor = c;
				vertices [id2.Ext1, id2.Ext2].TeleportColor = c;
			}
		}

		// Connexion of cells each other (regardless of the walls)
		if (nbBorders == 4) {
			SquareConnections (vertices, nbLines, nbColumns, 4);
		} else {
			HexagonConnections (vertices, nbLines, nbColumns, 6);
		}

		// The cast to IEnumerable<GraphVertex> doesn't work, that's why I use the ArrayExtension class (cf end of file)
		return ArrayExtensions.ToEnumerable<GraphVertex> (vertices);
	}



	// Returns the right angle offsets (orientation) according to the cell shape
	public static Pair<float> DetermineAngleOffsets(int nbBorders) {
		float off1 = 0;
		float off2 = 0;
		switch (nbBorders) {
		case 4:
			{
				off1 = Mathf.PI / 4;
				off2 = Mathf.PI / 4;
				break;
			}
		case 6:
			{
				off1 = Mathf.PI / 6;
				off2 = Mathf.PI / 2;
				break;
			}
		case 8:
			{
				off1 = Mathf.PI / 8;
				off2 = Mathf.PI * (3 / 8);
				break;
			}
		}
		Pair<float> offsets = new Pair<float> (off1, off2);

		return offsets;
	}


	// Returns the right axis offsets according to the cell shape (regardless of the even or odd line index)
	public static Pair<float> DetermineAxisOffsets(int nbBorders, float cellSize) {
		float offx = 0;
		float offy = 0;
		switch (nbBorders) {
		case 4:
			{
				offx = 0;
				offy = 0;
				break;
			}
		case 6:
			{
				float w = cellSize * 2 * Mathf.Cos (Mathf.PI / 6);
				float h = 2 * cellSize;
				offx = w / 2;
				offy = -h / 4;
				break;
			}
		case 8:
			{
				// TODO
				break;
			}
		}
		Pair<float> offsets = new Pair<float> (offx, offy);

		return offsets;
	}


	// Connects the cell with the 4 surrounding cells (merges the corners and handle the core neighbourhoods)
	public static void SquareConnections(RegularCell[,] vertices, int nbLines, int nbColumns, int nbBorders) {
		// First line : connexion to the cell at the left
		for (int j = 1; j < nbColumns; j++) {
			vertices [0, j].ConnectToOtherGraphVertex (vertices [0, j - 1]);
		}

		// First column : connexion to the up cell
		for (int i = 1; i < nbLines; i++) {
			vertices [i, 0].ConnectToOtherGraphVertex (vertices [i - 1, 0]);
		}

		// All the other elements :
		for (int i = 1; i < nbLines; i++) {
			for (int j = 1; j < nbColumns; j++) {
				// connect to the cells on the left and up
				vertices [i, j].ConnectToOtherGraphVertex (vertices [i - 1, j]);
				vertices [i, j].ConnectToOtherGraphVertex (vertices [i, j - 1]);
				}
			}
		}


	// Connects the cell with the 6 surrounding cells (merges the corners and handle the core neighbourhoods)
	public static void HexagonConnections(RegularCell[,] vertices, int nbLines, int nbColumns, int nbBorders) {
		// Vertical and horizontal connexions
		for (int i = 0; i < nbLines; i++) {
			for (int j = 0; j < nbColumns; j++) {
				// Horizontal
				if (j != 0) {
					vertices [i, j].ConnectToOtherGraphVertex (vertices [i, j - 1]);
					vertices [i, j - 1].ConnectToOtherGraphVertex (vertices [i, j]);
				}

				// Vertical
				if (i != 0) {
					vertices [i, j].ConnectToOtherGraphVertex (vertices [i - 1, j]);
					vertices [i - 1, j].ConnectToOtherGraphVertex (vertices [i, j]);
				}
			}
		}

		// Up right diagonal
		for (int i = 0; i < nbLines - 1; i ++) {
			for (int j = 0; j < nbColumns - 1; j++) {
				if (i % 2 == 0) {
					vertices [i, j].ConnectToOtherGraphVertex (vertices [i + 1, j + 1]);
					vertices [i + 1, j + 1].ConnectToOtherGraphVertex (vertices [i, j]);
				}
			}
		}

		// Up left diagonal
		for (int i = 1; i < nbLines - 1; i++) {
			for (int j = 1; j < nbColumns; j++) {
				if (i % 2 == 1) {
					vertices [i, j].ConnectToOtherGraphVertex (vertices [i + 1, j - 1]);
					vertices [i + 1, j - 1].ConnectToOtherGraphVertex (vertices [i, j]);
				}
			}
		}
	}

}



public static class ArrayExtensions
{
	public static IEnumerable<T> ToEnumerable<T>(this T[,] target)
	{
		foreach (var item in target)
			yield return item;
	}
}