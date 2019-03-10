using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface GraphVertex {
	void ConnectToOtherGraphVertex (GraphVertex other);
	Vector2 CoreCoordinates { get; }
	// info to display the walls
}



public class UndirectedGraph
{

	// Generates a grid of QuadCell
	public static QuadCell[,] GridCellUndirectedGraph (float cellSize, int nbLines, int nbColumns) {
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


		return vertices;
	}
}


