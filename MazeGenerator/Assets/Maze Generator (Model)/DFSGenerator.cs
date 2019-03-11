using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DFSGenerator
{

	public static void Generate(IEnumerable<GraphVertex> maze) {
		// Initialization
		Stack<GraphVertex> stack = new Stack<GraphVertex>();

		IEnumerator<GraphVertex> enumerator = maze.GetEnumerator ();
		enumerator.MoveNext ();
		GraphVertex current = enumerator.Current;

		stack.Push (current);

		GraphVertex next;
		while (!(stack.Count == 0)) {
			current = stack.Peek ();
			Debug.Log (current.CoreCoordinates.ToString() + "                                " + Random.Range(0, 10000));
			current.SetVisited ();
			next = current.GetRandomUnvisitedNeighbour ();

			if (next == null) { // dead end, every neighbour of the current cell has been visited
				Debug.Log("Dead end");
				stack.Pop ();
			
			} else {
				current.RemoveBorderBetweenCells (next);
				stack.Push (next);
			}
		}
	}
}
