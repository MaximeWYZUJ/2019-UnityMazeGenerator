using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RegularCell : GraphVertex
{
	private Vertex core; // center of the cell
	private float cellSize; // radius of the cell
	private List<Vertex> cornerPoints; // list of points which are on the border
	private List<RegularCell> connectedCells; // list of the surrounding cells (regardless of the walls)
	private int nbBorders; // number of points on the border (4 or 6)
	private float angleOffset; // offset to display the first corner
	private RegularCell teleportConnexion; // cell connected by a teleport (null if no teleport)
	private Color teleportColor; // color of the teleport (if any)


	// CONSTRUCTOR
	// Creates a cell without connected cell nor teleport connexion
	public RegularCell(Vector2 coreCoo, float cellSize, int nbBorders, float angleOffset) {
		this.core = new Vertex (coreCoo);
		this.cellSize = cellSize;
		this.nbBorders = nbBorders;
		this.angleOffset = angleOffset;

		this.cornerPoints = new List<Vertex> ();
		for (int i = 0; i < nbBorders; i++) {
			float angle = i * 2 * Mathf.PI / nbBorders + angleOffset;
			cornerPoints.Add (new Vertex (coreCoo + cellSize * new Vector2 (Mathf.Cos (angle), Mathf.Sin (angle))));
		}
		ConnectCorners ();

		this.connectedCells = new List<RegularCell> ();
		this.teleportConnexion = null;
	}


	// Creates a cell with a teleport connexion as first connected cell
	public RegularCell(Vector2 coreCoo, float cellSize, int nbBorders, float angleOffset, RegularCell teleportCell) {
		this.core = new Vertex (coreCoo);
		this.cellSize = cellSize;
		this.nbBorders = nbBorders;
		this.angleOffset = angleOffset;

		this.cornerPoints = new List<Vertex> ();
		for (int i = 0; i < nbBorders; i++) {
			float angle = i * 2 * Mathf.PI / nbBorders + angleOffset;
			cornerPoints.Add (new Vertex (coreCoo + cellSize * new Vector2 (Mathf.Cos (angle), Mathf.Sin (angle))));
		}
		ConnectCorners ();

		this.connectedCells = new List<RegularCell> ();
		this.teleportConnexion = teleportCell;
		this.connectedCells.Add (teleportCell);
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
		cornerPoints [0].AddNeighbour (cornerPoints [cornerPoints.Count-1]);
		cornerPoints [cornerPoints.Count-1].AddNeighbour (cornerPoints [0]);
	}


	// Connects 2 cells (connects the corner points each other and the cores)
	public void ConnectToOtherGraphVertex(GraphVertex other) {
		RegularCell otherCell = (RegularCell)other;

		List<Pair<Vertex>> pairList = new List<Pair<Vertex>> ();
		float delta = 0.2f;

		foreach (Vertex v1 in this.cornerPoints) {
			foreach (Vertex v2 in otherCell.cornerPoints) {
				if (Vector2.Distance (v1.Coo, v2.Coo) < delta) {
					Pair<Vertex> pair = new Pair<Vertex> (v1, v2);
					if (!pair.AlreadyExists (pairList)) {
						pairList.Add (pair);
					}
				}
			}
		}

		Debug.Assert (pairList.Count == 2);

		foreach (Pair<Vertex> p in pairList) {
			// Fusion of the pair elements and their respective neighborhood
			foreach (Vertex v in p.Ext1.Neighbours.ToArray()) {
				// We add our neighbours to the other corner point
				p.Ext2.AddNeighbour (v);
			}
			// Replacement of the corner in our cell
			this.cornerPoints.Remove (p.Ext1);
			this.cornerPoints.Add (p.Ext2);
		}


		// Connection of the cores of each cells
		core.AddNeighbour (otherCell.core);
		otherCell.core.AddNeighbour (core);

		// Add to the connected cells lists
		if (!this.connectedCells.Contains (otherCell)) {
			this.connectedCells.Add (otherCell);
		}
		if (!otherCell.connectedCells.Contains (this)) {
			otherCell.connectedCells.Add (this);
		}
	}


	// Removes the wall between 2 cells by looking a their cornerPoints
	// Once the common corner points are found, we set them as "non-neighbours"
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
	// List of the surrounding cells
	public List<GraphVertex> Neighbours {
		get {
			List<GraphVertex> list = new List<GraphVertex> ();
			foreach (RegularCell cell in connectedCells) {
				list.Add ((GraphVertex)cell);
			}
			return list;
		}
	}

	// Coordinates of the center of the cell
	public Vector2 CoreCoordinates{
		get {
			return this.core.Coo;
		}
	}

	// Coordinates of the centers of the surrounding cells
	public List<Vector2> ConnectedGraphVerticesCoordinates{
		get {
			List<Vector2> l = new List<Vector2> ();
			foreach (RegularCell c in this.connectedCells) {
				l.Add (c.CoreCoordinates);
			}

			return l;
		}
	}

	// List of the walls of this cell
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

	// Marks that cell as visited
	public void SetVisited() {
		this.core.Mark = MarkType.Visited;
	}

	// Gets or sets the mark of this cell
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

	// Returns the cell connected by the teleport, or null if there is no teleport
	public GraphVertex TeleportCell {
		get {
			return teleportConnexion;
		}
		set {
			connectedCells.Add ((RegularCell)value);
			teleportConnexion = (RegularCell)value;
		}
	}

	// Gets or sets the color of the teleport (if any)
	public Color TeleportColor {
		get {
			return teleportColor;
		}
		set {
			teleportColor = value;
		}
	}
}
