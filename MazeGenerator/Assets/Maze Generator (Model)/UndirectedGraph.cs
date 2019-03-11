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

	// Gets the pairs of coordinates which represent the extremities of the walls
	List<Wall> Walls { get; }

	// Sets this cell as visited
	void SetVisited();

	// Gets the current mark of this cell (visited or not)
	MarkType Mark { get; }

	// Gets a random adjacent cell with the specified mark
	// returns null if there is no cell with this mark
	GraphVertex GetRandomMarkedNeighbour (MarkType mark);

	// Gets a all the adjacent cells with the specified mark
	List<GraphVertex> GetMarkedNeighbours (MarkType mark);
}


public enum MarkType {Visited, Unvisited};


public class UndirectedGraph
{

	// Generates a grid of QuadCell
	public static IEnumerable<GraphVertex> GridCellUndirectedGraph (float cellSize, int nbLines, int nbColumns) {
		QuadCell[,] vertices = new QuadCell[nbLines, nbColumns];

		// Initialization of each cell
		for (int i = 0; i < nbLines; i++) {
			for (int j = 0; j < nbColumns; j++) {
				vertices [i, j] = new QuadCell (new Vector2 (j * cellSize, i * cellSize), cellSize);
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
}



public static class ArrayExtensions
{
	public static IEnumerable<T> ToEnumerable<T>(this T[,] target)
	{
		foreach (var item in target)
			yield return item;
	}
}