using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Vertex
{
	private Vector2 coordinates;
	private List<Vertex> neighbours;


	// CONSTRUCTORS
	// Create a vertex at a point, without initial neighbour
	public Vertex (Vector2 coo) {
		this.coordinates = coo;
		neighbours = new List<Vertex> ();
	}



	// METHODS
	// Add a vertex to the neighbours
	// Does not do anything if it is already a neighbour
	public void AddNeighbour(Vertex v) {
		if (!neighbours.Contains (v)) {
			neighbours.Add (v);
		}
	}

	// Removes a given vertex from the neighbours
	// Does not do anything if it is not a neighbour
	public void RemoveNeighbour(Vertex v) {
		if (neighbours.Contains (v)) {
			neighbours.Remove (v);
		}
	}



	// GETTERS AND SETTERS
	public Vector2 Coo {
		get {
			return this.coordinates;
		}
	}

	public List<Vertex> Neighbours {
		get {
			return this.neighbours;
		}
	}
}


