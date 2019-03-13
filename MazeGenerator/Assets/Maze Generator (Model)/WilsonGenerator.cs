using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WilsonGenerator
{
	public static void Generate(IEnumerable<GraphVertex> maze) {
		// Initialization
		Stack<GraphVertex> currentPath = new Stack<GraphVertex>();

		IEnumerator<GraphVertex> enumerator = maze.GetEnumerator ();
		enumerator.MoveNext ();
		GraphVertex first = enumerator.Current;
		first.SetVisited ();

		List<GraphVertex> processedGV = new List<GraphVertex> ();
		processedGV.Add (first);


		// Process
		GraphVertex previous = first;
		GraphVertex next = first.GetRandomMarkedNeighbour(MarkType.Unvisited);
		next.Mark = MarkType.TemporaryVisited;
		currentPath.Push(next);
		while (currentPath.Count != 0) {
			List<GraphVertex> list = currentPath.Peek ().Neighbours;
			list.Remove (previous); // we don't want to go back on our step (yet)
			previous = next;

			// Update of the next cell to check
			int randomIndex = Random.Range (0, list.Count);
			next = list [randomIndex];


			if (next.Mark == MarkType.Visited) {
				// We have reached the maze so we backtrack the path to process it
				while (currentPath.Count > 1) {
					GraphVertex gv = currentPath.Pop ();
					gv.RemoveBorderBetweenCells (currentPath.Peek ());
					gv.SetVisited ();
					processedGV.Add (gv);
				}
				GraphVertex lastGv = currentPath.Pop();
				lastGv.RemoveBorderBetweenCells (first);
				lastGv.SetVisited ();
				processedGV.Add (lastGv);

				// Setup for the next path, we choose a new starting point
				foreach (GraphVertex gv in processedGV) {
					GraphVertex tempSecond = gv.GetRandomMarkedNeighbour (MarkType.Unvisited);
					if (tempSecond != null) {
						first = gv;
						previous = first;
						next = tempSecond;
						next.Mark = MarkType.TemporaryVisited;
						currentPath.Push (next);
						break;
					}
				}


			} else if (currentPath.Contains (next)) {
				// There is a loop in the path so we restart the path
				while (currentPath.Count != 0) {
					currentPath.Pop ().Mark = MarkType.Unvisited;
				}
				// Setup for the rebooted path
				previous = first;
				next = first.GetRandomMarkedNeighbour(MarkType.Unvisited);
				next.Mark = MarkType.TemporaryVisited;
				currentPath.Push(next);


			} else {
				// We add "next" to the current path
				next.Mark = MarkType.TemporaryVisited;
				currentPath.Push (next);
			}
		}
	}



	public static IEnumerator AnimatedGeneration(IEnumerable<GraphVertex> maze, GameObject vertexPrefab, float deltaTime) {
		// Initialization
		Stack<GraphVertex> currentPath = new Stack<GraphVertex>();

		IEnumerator<GraphVertex> enumerator = maze.GetEnumerator ();
		enumerator.MoveNext ();
		GraphVertex first = enumerator.Current;
		first.SetVisited ();

		List<GraphVertex> processedGV = new List<GraphVertex> ();
		processedGV.Add (first);


		// Process
		GraphVertex previous = first;
		GraphVertex next = first.GetRandomMarkedNeighbour(MarkType.Unvisited);
		next.Mark = MarkType.TemporaryVisited;
		currentPath.Push(next);
		while (currentPath.Count != 0) {
			List<GraphVertex> list = currentPath.Peek ().Neighbours;
			list.Remove (previous); // we don't want to go back on our step (yet)
			previous = next;

			// Update of the next cell to check
			int randomIndex = Random.Range (0, list.Count);
			next = list [randomIndex];


			if (next.Mark == MarkType.Visited) {
				// We have reached the maze so we backtrack the path to process it
				while (currentPath.Count > 1) {
					GraphVertex gv = currentPath.Pop ();
					gv.RemoveBorderBetweenCells (currentPath.Peek ());
					gv.SetVisited ();
					processedGV.Add (gv);
				}
				GraphVertex lastGv = currentPath.Pop();
				lastGv.RemoveBorderBetweenCells (first);
				lastGv.SetVisited ();
				processedGV.Add (lastGv);

				// Setup for the next path, we choose a new starting point
				foreach (GraphVertex gv in processedGV) {
					GraphVertex tempSecond = gv.GetRandomMarkedNeighbour (MarkType.Unvisited);
					if (tempSecond != null) {
						first = gv;
						previous = first;
						next = tempSecond;
						next.Mark = MarkType.TemporaryVisited;
						currentPath.Push (next);
						break;
					}
				}

				// Visualization
				Manager.ClearMazeObjects();
				MazeViewer.DisplayGrid(maze, vertexPrefab);
				yield return new WaitForSecondsRealtime (deltaTime);


			} else if (currentPath.Contains (next)) {
				// There is a loop in the path so we restart the path
				while (currentPath.Count != 0) {
					currentPath.Pop ().Mark = MarkType.Unvisited;
				}
				// Setup for the rebooted path
				previous = first;
				next = first.GetRandomMarkedNeighbour(MarkType.Unvisited);
				next.Mark = MarkType.TemporaryVisited;
				currentPath.Push(next);


			} else {
				// We add "next" to the current path
				next.Mark = MarkType.TemporaryVisited;
				currentPath.Push (next);
			}
		}
		MazeViewer.DisplayGrid (maze, vertexPrefab);
	}
}
