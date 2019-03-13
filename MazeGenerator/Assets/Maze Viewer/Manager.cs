using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

	public GameObject cellPrefab;
	public GameObject wallPrefab;
	public float cellSize;
	private IEnumerable<GraphVertex> maze;

	void Start () {
		maze = UndirectedGraph.GridCellUndirectedGraph (cellSize, 20, 30);

		//PrimGenerator.Generate (maze);
		//MazeViewer.DisplayGrid (maze, cellPrefab);

		StartCoroutine (WilsonGenerator.AnimatedGeneration (maze, cellPrefab, 0f));
	}


	public static void ClearMazeObjects() {
		GameObject[] array = GameObject.FindGameObjectsWithTag ("MazeElement");
		foreach (GameObject obj in array) {
			GameObject.Destroy (obj);
		}
	}

}
