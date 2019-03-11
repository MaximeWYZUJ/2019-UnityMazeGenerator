﻿using System.Collections;
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
		ConnectCorners ();

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
			ReplaceCorner("up left", cell.cornerPoints["up right"]);
			ReplaceCorner ("down left", cell.cornerPoints ["down right"]);

			this.connectedCells.Add ("left", cell);
			cell.connectedCells.Add ("right", this);


		} else if (cell.core.Coo.x > this.core.Coo.x + cellSize/2) {
			// The cell is on the right
			ReplaceCorner("up right", cell.cornerPoints["up left"]);
			ReplaceCorner ("down right", cell.cornerPoints ["down left"]);

			this.connectedCells.Add ("right", cell);
			cell.connectedCells.Add ("left", this);


		} else if (cell.core.Coo.y < this.core.Coo.y - cellSize/2) {
			// The cell is down
			ReplaceCorner("down right", cell.cornerPoints["up right"]);
			ReplaceCorner ("down left", cell.cornerPoints ["up left"]);

			this.connectedCells.Add ("down", cell);
			cell.connectedCells.Add ("up", this);


		} else if (cell.core.Coo.y > this.core.Coo.y + cellSize/2) {
			// The cell is up
			ReplaceCorner("up left", cell.cornerPoints["down left"]);
			ReplaceCorner ("up right", cell.cornerPoints ["down right"]);
		
			this.connectedCells.Add ("up", cell);
			cell.connectedCells.Add ("down", this);
		}

		// Now we connect the cores
		this.core.AddNeighbour (cell.core);
		cell.core.AddNeighbour (this.core);
	}

	// Connects the corners of 1 cell
	public void ConnectCorners() {
		Vertex upleft = cornerPoints ["up left"];
		Vertex upright = cornerPoints ["up right"];
		Vertex downleft = cornerPoints ["down left"];
		Vertex downright = cornerPoints ["down right"];


		upleft.AddNeighbour (upright);
		upleft.AddNeighbour (downleft);

		upright.AddNeighbour (upleft);
		upright.AddNeighbour (downright);

		downleft.AddNeighbour (upleft);
		downleft.AddNeighbour (downright);

		downright.AddNeighbour (downleft);
		downright.AddNeighbour (upright);
	}

	public void RemoveBorderBetweenCells (GraphVertex otherGraphVertex) {
		QuadCell otherCell = (QuadCell)otherGraphVertex;

		foreach (QuadCell c in this.connectedCells.Values) {
			// We look for the adjacent cell to remove the border
			if (c.Equals (otherCell)) {
				// We look for the corner points which are common to the two cells
				List<Vertex> commonVertices = new List<Vertex> ();
				foreach (Vertex v1 in this.cornerPoints.Values) {
					foreach (Vertex v2 in otherCell.cornerPoints.Values) {
						if (v1.Equals (v2)) {
							commonVertices.Add (v1);
						}
					}
				}

				// We disconnect all the border points which are common to the 2 cells so that the way is open
				foreach (Vertex v1 in commonVertices) {
					foreach (Vertex v2 in commonVertices) {
						v1.RemoveNeighbour (v2);
					}
				}
			}
		}
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



	// GETTERS AND SETTERS
	public Vector2 CoreCoordinates{
		get {
			return this.core.Coo;
		}
	}

	public List<Vector2> ConnectedGraphVerticesCoordinates{
		get {
			List<Vector2> l = new List<Vector2> ();
			foreach (QuadCell c in this.connectedCells.Values) {
				l.Add (c.CoreCoordinates);
			}

			return l;
		}
	}

	public List<KeyValuePair<Vector2, Vector2>> ConnectedWalls {
		get {
			List<KeyValuePair<Vector2, Vector2>> l = new List<KeyValuePair<Vector2, Vector2>> ();
			/*foreach (Vertex cp in cornerPoints.Values) {
				foreach (Vertex v in cp.Neighbours) {
					l.Add (new KeyValuePair<Vector2, Vector2> (cp.Coo, v.Coo));
				}
			}*/

			foreach (Vertex cp1 in cornerPoints.Values) {
				foreach (Vertex cp2 in cornerPoints.Values) {
					if (!cp1.Equals (cp2)) {
						KeyValuePair<Vector2, Vector2> reversePair = new KeyValuePair<Vector2, Vector2> (cp2.Coo, cp1.Coo);
						if (cp1.Neighbours.Contains (cp2) && !l.Contains (reversePair)) {
							l.Add (new KeyValuePair<Vector2, Vector2> (cp1.Coo, cp2.Coo));
						}
					}
				}
			}

			return l;
		}
	}

	public void SetVisited() {
		this.core.Mark = MarkType.Visited;
	}

	public MarkType Mark {
		get {
			return core.Mark;
		}
	}

	// Returns a random unvisited neighbours of the cell, or null if there is no unvisited neigbour
	public GraphVertex GetRandomUnvisitedNeighbour() {
		// Construction of the list of unvisited neighbours
		List<QuadCell> unvisitedNeighbours = new List<QuadCell> ();
		foreach (QuadCell c in connectedCells.Values) {
			if (c.core.Mark == MarkType.Unvisited) {
				unvisitedNeighbours.Add (c);
			}
		}

		// Selection of a random unvisited neighbour
		int length = unvisitedNeighbours.Count;
		if (length > 0) {
			return unvisitedNeighbours[Random.Range (0, length)];
		} else {
			return null;
		}
	}
}
