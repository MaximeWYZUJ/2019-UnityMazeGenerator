using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface CellInterface : GraphVertex {
	// TODO
}


/*public class Cell : CellInterface {

	private Vertex core;
	private List<Vertex> borderPoints;


	// CONSTRUCTORS
	// Creates an initial cell with n vertices on its border
	// n muste be greater than 3
	public Cell(Vector2 coreCoo, int n) {
		this.core = new Vertex (coreCoo);
		this.borderPoints = new List<Vertex> ();

		for (int i = 1; i <= n; i++) {
			float angle = Mathf.PI / n;
			Vertex newBP = coreCoo + new Vector2 (Mathf.Cos (angle), Mathf.Sin (angle));
			borderPoints.Add (newBP);
		}
	}



	// METHODS
	// Divides the cell into two new cells
	public Cell[] Divide() {
		// TODO
	}



	// GETTERS AND SETTERS
	public Vertex Core {
		get {
			return this.core;
		}
	}

	public List<Vertex> Border {
		get {
			return this.borderPoints;
		}
	}
}*/



public class QuadCell : CellInterface {

	private Vertex core;
	private float cellSize;
	private Dictionary<string, Vertex> cornerPoints;
	private Dictionary<string, QuadCell> connectedCells;


	// CONSTRUCTORS
	// Creates a cell without connected cell
	public QuadCell(Vector2 coreCoo, float size) {
		this.core = new Vertex (coreCoo);
		this.cellSize = size;
		this.cornerPoints = new Dictionary<string, Vertex> () {
			{ "up left", new Vertex (new Vector2 (coreCoo.x - size / 2, coreCoo.y + size / 2)) },
			{ "up right", new Vertex (new Vector2 (coreCoo.x + size / 2, coreCoo.y + size / 2)) },
			{ "down left", new Vertex (new Vector2 (coreCoo.x - size / 2, coreCoo.y - size / 2)) },
			{ "down right", new Vertex (new Vector2 (coreCoo.x + size / 2, coreCoo.y - size / 2)) },
		};

		this.connectedCells = new Dictionary<string, QuadCell> ();
	}



	// METHODS
	// Replaces a corner by another and unites both neighborhoods
	private void ReplaceCorner(string corner, Vertex replacementCorner) {
		Vertex currentCorner = this.cornerPoints [corner];

		// Fusion of the neighborhoods
		foreach (Vertex v in currentCorner.Neighbours) {
			replacementCorner.AddNeighbour (v);
		}

		// Replacement
		this.cornerPoints[corner] = replacementCorner;
	}

	// Connects 2 cells (connects the corner points each other and the cores)
	public void ConnectToOtherGraphVertex(GraphVertex other) {
		QuadCell cell = (QuadCell)other;

		if (cell.core.Coo.x < this.core.Coo.x - cellSize/2) {
			// The cell is on the left, so we connect the appropriate corners
			ReplaceCorner("up left", cell.cornerPoints["up left"]);
			ReplaceCorner ("down left", cell.cornerPoints ["down left"]);

			this.connectedCells.Add ("left", cell);
			cell.connectedCells.Add ("right", this);


		} else if (cell.core.Coo.x > this.core.Coo.x + cellSize/2) {
			// The cell is on the right
			ReplaceCorner("up right", cell.cornerPoints["up right"]);
			ReplaceCorner ("down right", cell.cornerPoints ["down right"]);

			this.connectedCells.Add ("right", cell);
			cell.connectedCells.Add ("left", this);


		} else if (cell.core.Coo.y < this.core.Coo.y - cellSize/2) {
			// The cell is down
			ReplaceCorner("down right", cell.cornerPoints["down right"]);
			ReplaceCorner ("down left", cell.cornerPoints ["down left"]);

			this.connectedCells.Add ("down", cell);
			cell.connectedCells.Add ("up", this);


		} else if (cell.core.Coo.y > this.core.Coo.y + cellSize/2) {
			// The cell is up
			ReplaceCorner("up left", cell.cornerPoints["up left"]);
			ReplaceCorner ("up right", cell.cornerPoints ["up right"]);
		
			this.connectedCells.Add ("up", cell);
			cell.connectedCells.Add ("down", this);
		}

		// Now we connect the cores
		this.core.AddNeighbour (cell.core);
		cell.core.AddNeighbour (this.core);
	}


	/*// Adds corner points to the dictionary
	public void AddCornersWithoutConnecting(Vertex upleft, Vertex upright, Vertex downleft, Vertex downright) {
		this.cornerPoints.Add ("up left", upleft);
		this.cornerPoints.Add ("up right", upright);
		this.cornerPoints.Add ("down left", downleft);
		this.cornerPoints.Add ("down right", downright);
	}

	// Adds the corner points and connects them each other (neighborhood)
	public void AddCorners(Vertex upleft, Vertex upright, Vertex downleft, Vertex downright) {
		this.cornerPoints.Add ("up left", upleft);
		this.cornerPoints.Add ("up right", upright);
		this.cornerPoints.Add ("down left", downleft);
		this.cornerPoints.Add ("down right", downright);

		upleft.AddNeighbour (upright);
		upleft.AddNeighbour (downleft);

		upright.AddNeighbour (upleft);
		upright.AddNeighbour (downright);

		downleft.AddNeighbour (upleft);
		downleft.AddNeighbour (downright);

		downright.AddNeighbour (downleft);
		downright.AddNeighbour (upright);
	}

	// Removes a corner and disconnects it from its neighbours
	public void RemoveCorner(Vertex v) {
		if (cornerPoints.ContainsValue (v)) {
			if (v.Equals (cornerPoints ["up left"])) {
				//v.RemoveNeighbour (cornerPoints ["up right"]);
				//v.RemoveNeighbour (cornerPoints ["down left"]);
				cornerPoints.Remove ("up left");

			} else if (v.Equals (cornerPoints ["up right"])) {
				//v.RemoveNeighbour (cornerPoints ["up left"]);
				//v.RemoveNeighbour (cornerPoints ["down right"]);
				cornerPoints.Remove ("up right");

			} else if (v.Equals (cornerPoints ["down left"])) {
				//v.RemoveNeighbour (cornerPoints ["down right"]);
				//v.RemoveNeighbour (cornerPoints ["up left"]);
				cornerPoints.Remove ("down left");

			} else if (v.Equals (cornerPoints ["down right"])) {
				//v.RemoveNeighbour (cornerPoints ["up right"]);
				//v.RemoveNeighbour (cornerPoints ["down left"]);
				cornerPoints.Remove ("down right");
			}
		}
	}*/

}
