using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

	public GameObject cellPrefab;
	private IEnumerable<GraphVertex> maze;

	void Start () {
		maze = UndirectedGraph.GridCellUndirectedGraph (1.5f, 10, 15);

		/*PrimGenerator.Generate (maze);
		MazeViewer.DisplayGrid (maze, cellPrefab, 2000);*/

		StartCoroutine (PrimGenerator.AnimatedGeneration (maze, cellPrefab, 0.2f));
	}

}
