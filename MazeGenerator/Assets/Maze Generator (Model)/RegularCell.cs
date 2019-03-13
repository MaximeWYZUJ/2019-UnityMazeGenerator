using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RegularCell : GraphVertex
{
	private Vertex core;
	private float cellSize;
	private List<Vertex> cornerPoints;
	private List<RegularCell> connectedCells;
	private int nbBorders;
	private float angleOffset;


	// CONSTRUCTOR
	// Creates a cell without connected cell
	public RegularCell(Vertex core, float cellSize, int nbBorders, float angleOffset) {
		this.core = core;
		this.cellSize = cellSize;
		this.nbBorders = nbBorders;
		this.angleOffset = angleOffset;

		this.cornerPoints = new List<Vertex> ();
		for (int i = 0; i < nbBorders; i++) {
			float angle = i * Mathf.PI / nbBorders + angleOffset;
			cornerPoints.Add (new Vertex (cellSize * new Vector2 (Mathf.Cos (angle), Mathf.Sin (angle))));
		}
		ConnectCorners ();

		this.connectedCells = new List<RegularCell> ();
	}



	// METHODS
	// Connects the corners of this cell
	private void ConnectCorners() {
		// Connects the current corner point with the previous one in the list
		for (int i = 1; i < nbBorders; i++) {
			cornerPoints [i].AddNeighbour (cornerPoints [i - 1]);
			cornerPoints [i - 1].AddNeighbour (cornerPoints [i]);
		}
		// Connects the first and last corner points in the list
		cornerPoints [0].AddNeighbour (cornerPoints [cornerPoints.Count]);
		cornerPoints [cornerPoints.Count].AddNeighbour (cornerPoints [0]);
	}


	// Connects 2 cells (connects the corner points each other and the cores)
	public void ConnectToOtherGraphVertex(GraphVertex other) {
		RegularCell otherCell = (RegularCell)other;

		// We select the 2 pairs of corner points which are the closest between the graph vertices
		float currentMin1 = cellSize;
		float currentMin2 = cellSize;
		Vertex currentVertex1_1 = cornerPoints [0];
		Vertex currentVertex1_2 = otherCell.cornerPoints [0];
		Vertex currentVertex2_1 = cornerPoints [0];
		Vertex currentVertex2_2 = otherCell.cornerPoints [0];

		// First pair
		foreach (Vertex v1 in cornerPoints) {
			foreach (Vertex v2 in otherCell.cornerPoints) {
				if (Vector2.Distance (v1.Coo, v2.Coo) < currentMin1) {
					currentMin1 = Vector2.Distance (v1.Coo, v2.Coo);
					currentVertex1_1 = v1;
					currentVertex1_2 = v2;
				}
			}
		}

		// Second pair
		foreach (Vertex v1 in cornerPoints) {
			foreach (Vertex v2 in otherCell.cornerPoints) {
				if (Vector2.Distance (v1.Coo, v2.Coo) < currentMin1 && v1 != currentVertex1_1 && v2 != currentVertex1_2) {
					currentMin1 = Vector2.Distance (v1.Coo, v2.Coo);
					currentVertex2_1 = v1;
					currentVertex2_2 = v2;
				}
			}
		}

		// Fusion of the pair elements and their respective neighborhood
		foreach (Vertex v in currentVertex1_2.Neighbours) {
			// We add the neighbours of the other corner point to our corner point
			currentVertex1_1.AddNeighbour (v);
		}
		// Replacement of the corner in the other cell
		otherCell.cornerPoints.Remove (currentVertex1_2);
		otherCell.cornerPoints.Add (currentVertex1_1);


		// Same with the other pair
		foreach (Vertex v in currentVertex2_2.Neighbours) {
			currentVertex2_1.AddNeighbour (v);
		}
		otherCell.cornerPoints.Remove (currentVertex2_2);
		otherCell.cornerPoints.Add (currentVertex2_1);


		// Connection of the cores of each cells
		core.AddNeighbour (otherCell.core);
		otherCell.core.AddNeighbour (core);

		// Add to the connected cells lists
		this.connectedCells.Add (otherCell);
		otherCell.connectedCells.Add (this);
	}


	public void RemoveBorderBetweenCells (GraphVertex otherGraphVertex) {
		RegularCell otherCell = (RegularCell)otherGraphVertex;

		foreach (RegularCell c in this.connectedCells) {
			// We look for the adjacent cell to remove the border
			if (c.Equals (otherCell)) {
				// We look for the corner points which are common to the two cells
				List<Vertex> commonVertices = new List<Vertex> ();
				foreach (Vertex v1 in this.cornerPoints) {
					foreach (Vertex v2 in otherCell.cornerPoints) {
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
			foreach (RegularCell cell in connectedCells) {
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
			foreach (RegularCell c in this.connectedCells) {
				l.Add (c.CoreCoordinates);
			}

			return l;
		}
	}

	public List<Wall> Walls {
		get {
			List<Wall> list = new List<Wall> ();

			foreach (Vertex cp1 in cornerPoints) {
				foreach (Vertex cp2 in cornerPoints) {
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
		foreach (RegularCell c in connectedCells) {
			if (c.core.Mark == m) {
				markedNeighbours.Add ((GraphVertex)c);
			}
		}
		return markedNeighbours;
	}
}
