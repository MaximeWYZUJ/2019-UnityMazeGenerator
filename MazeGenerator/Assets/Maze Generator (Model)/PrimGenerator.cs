using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PrimGenerator : MazeGenerator
{
	public void Generate(IEnumerable<GraphVertex> maze) {
		// Initialization
		List<GraphVertex> elementsToProcess = new List<GraphVertex>();

		IEnumerator<GraphVertex> enumerator = maze.GetEnumerator ();
		enumerator.MoveNext ();
		GraphVertex first = enumerator.Current;

		first.SetVisited ();
		foreach (GraphVertex gv in first.GetMarkedNeighbours(MarkType.Unvisited)) {
			elementsToProcess.Add (gv);
		}


		// Process
		GraphVertex gvToProcess;
		GraphVertex gvToLinkTo;
		while (elementsToProcess.Count != 0) {
			// Get the cells to link
			gvToProcess = elementsToProcess [Random.Range (0, elementsToProcess.Count)];
			gvToLinkTo = gvToProcess.GetRandomMarkedNeighbour (MarkType.Visited);

			// Link the cells
			gvToProcess.RemoveBorderBetweenCells(gvToLinkTo);

			// Setup the next iteration
			gvToProcess.SetVisited();
			elementsToProcess.Remove (gvToProcess);
			foreach (GraphVertex gv in gvToProcess.GetMarkedNeighbours(MarkType.Unvisited)) {
				// We add each unmarked adjacent cell as long as it is not already in the list
				if (!elementsToProcess.Contains (gv)) {
					elementsToProcess.Add (gv);
				}
			}
		}
	}



	public IEnumerator AnimatedGeneration(IEnumerable<GraphVertex> maze, GameObject vertexPrefab, float deltaTime) {
		// Initialization
		List<GraphVertex> elementsToProcess = new List<GraphVertex>();

		IEnumerator<GraphVertex> enumerator = maze.GetEnumerator ();
		enumerator.MoveNext ();
		GraphVertex first = enumerator.Current;

		first.SetVisited ();
		foreach (GraphVertex gv in first.GetMarkedNeighbours(MarkType.Unvisited)) {
			elementsToProcess.Add (gv);
		}


		// Process
		GraphVertex gvToProcess;
		GraphVertex gvToLinkTo;
		while (elementsToProcess.Count != 0) {
			// Get the cells to link
			gvToProcess = elementsToProcess [Random.Range (0, elementsToProcess.Count)];
			gvToLinkTo = gvToProcess.GetRandomMarkedNeighbour (MarkType.Visited);

			// Link the cells
			gvToProcess.RemoveBorderBetweenCells(gvToLinkTo);

			// Setup the next iteration
			gvToProcess.SetVisited();
			elementsToProcess.Remove (gvToProcess);
			foreach (GraphVertex gv in gvToProcess.GetMarkedNeighbours(MarkType.Unvisited)) {
				// We add each unmarked adjacent cell as long as it is not already in the list
				if (!elementsToProcess.Contains (gv)) {
					elementsToProcess.Add (gv);
				}
			}


			// Visualization
			Manager.ClearMazeObjects();
			MazeViewer.DisplayGrid(maze, vertexPrefab);
			yield return new WaitForSecondsRealtime (deltaTime);
		}
		Manager.ClearMazeObjects ();
		MazeViewer.DisplayGrid (maze, vertexPrefab);
	}
}


