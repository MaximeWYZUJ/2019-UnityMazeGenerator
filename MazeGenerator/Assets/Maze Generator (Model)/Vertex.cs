using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Vertex
{
	private Vector2 coordinates;
	private List<Vertex> neighbours;
	private MarkType mark;


	// CONSTRUCTORS
	// Create a vertex at a point, without initial neighbour
	public Vertex (Vector2 coo) {
		this.coordinates = coo;
		neighbours = new List<Vertex> ();
		this.mark = MarkType.Unvisited;
	}



	// METHODS
	// Add a vertex to the neighbours
	// Does not do anything if it is already a neighbour
	public void AddNeighbour(Vertex v) {
		bool alreadyIn = false;
		float delta = 0.1f;
		foreach(Vertex v1 in neighbours) {
			if (Vector2.Distance(v1.coordinates, v.coordinates) < delta) {
				alreadyIn = true;
				break;
			}
		}

		if (!alreadyIn) {
			neighbours.Add (v);
		}
	}

	// Removes a given vertex from the neighbours
	// Does not do anything if it is not a neighbour
	public void RemoveNeighbour(Vertex v) {
		float delta = 0.1f;
		foreach(Vertex v1 in neighbours.ToArray()) {
			if (Vector2.Distance(v1.coordinates, v.coordinates) < delta) {
				neighbours.Remove (v1);
			}
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

	public MarkType Mark {
		get {
			return mark;
		}
		set {
			this.mark = value;
		}
	}
}


