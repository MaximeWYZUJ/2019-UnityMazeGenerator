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
}


public enum MarkType {Visited, Unvisited, TemporaryVisited};


public class UndirectedGraph
{

	// Generates a grid of QuadCell
	public static IEnumerable<GraphVertex> GridCellUndirectedGraph (float cellSize, int nbLines, int nbColumns, int nbBorders) {
		RegularCell[,] vertices = new RegularCell[nbLines, nbColumns];

		// Initialization of each cell
		for (int i = 0; i < nbLines; i++) {
			for (int j = 0; j < nbColumns; j++) {
				Pair<float> angleOffsets = UndirectedGraph.DetermineAngleOffsets (nbBorders);
				Pair<float> axisOffsets = DetermineAxisOffsets (nbBorders, cellSize, i);

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
				//vertices [i, j] = new QuadCell (new Vector2 (j * cellSize, i * cellSize), cellSize);
			}
		}


		// Connection of cells each other
		// First line : connexion to the cell at the left
		for (int j = 1; j < nbColumns; j++) {
			vertices [0, j].ConnectToOtherGraphVertex (vertices [0, j - 1]);
		}

		// First column : connexion to the up cell
		for (int i = 1; i < nbLines; i++) {
			vertices [i, 0].ConnectToOtherGraphVertex (vertices [i - 1, 0]);
		}

		// All the other elements : connect to the cells on the left and up
		for (int i = 1; i < nbLines; i++) {
			for (int j = 1; j < nbColumns; j++) {
				vertices [i, j].ConnectToOtherGraphVertex (vertices [i - 1, j]);
				vertices [i, j].ConnectToOtherGraphVertex (vertices [i, j - 1]);
			}
		}

		// The cast to IEnumerable<GraphVertex> doesn't work, that's why I use the ArrayExtension class
		return ArrayExtensions.ToEnumerable<GraphVertex> (vertices);
		//return (IEnumerable<GraphVertex>) vertices;
	}



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


	public static Pair<float> DetermineAxisOffsets(int nbBorders, float cellSize, int lineIndex) {
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

}



public static class ArrayExtensions
{
	public static IEnumerable<T> ToEnumerable<T>(this T[,] target)
	{
		foreach (var item in target)
			yield return item;
	}
}