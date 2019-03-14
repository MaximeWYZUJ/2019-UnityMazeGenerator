using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This class is deprecated.
public class QuadCell : GraphVertex {

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



	// GETTERS AND SETTERS
	public List<GraphVertex> Neighbours {
		get {
			List<GraphVertex> list = new List<GraphVertex> ();
			foreach (QuadCell cell in connectedCells.Values) {
				list.Add ((GraphVertex)cell);
			}
			return list;
		}
	}

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

	public List<Wall> Walls {
		get {
			List<Wall> list = new List<Wall> ();

			foreach (Vertex cp1 in cornerPoints.Values) {
				foreach (Vertex cp2 in cornerPoints.Values) {
					if (!cp1.Equals (cp2)) {
						Wall wall = new Wall (cp1.Coo, cp2.Coo);
						if (cp1.Neighbours.Contains (cp2) && !wall.AlreadyExists (list)) {
							list.Add (wall);
						}
					}
				}
			}
			return list;
		}
	}


	public void SetVisited() {
		this.core.Mark = MarkType.Visited;
	}

	public MarkType Mark {
		get {
			return core.Mark;
		}
		set {
			this.core.Mark = value;
		}
	}

	// Returns a random neighbour of the cell with a specified mark, or null if there is no such adjacent cell
	public GraphVertex GetRandomMarkedNeighbour(MarkType m) {
		// Construction of the list of marked neighbours
		List<GraphVertex> markedNeighbours = GetMarkedNeighbours(m);

		// Selection of a random marked neighbour
		int length = markedNeighbours.Count;
		if (length > 0) {
			return markedNeighbours[Random.Range (0, length)];
		} else {
			return null;
		}
	}

	// Gets all the adjacent cells with the specified mark
	public List<GraphVertex> GetMarkedNeighbours(MarkType m) {
		List<GraphVertex> markedNeighbours = new List<GraphVertex> ();
		foreach (QuadCell c in connectedCells.Values) {
			if (c.core.Mark == m) {
				markedNeighbours.Add ((GraphVertex)c);
			}
		}
		return markedNeighbours;
	}


	// Returns the cell connected by the teleport, or null if there is no teleport
	public GraphVertex TeleportCell {
		get {
			return null;
		}
		set { }
	}

	public Color TeleportColor {
		get {
			return Color.white;
		}
		set { }
	}
}
