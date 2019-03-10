using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

	public GameObject cellPrefab;
	private QuadCell[,] maze;

	void Start () {
		maze = UndirectedGraph.GridCellUndirectedGraph (1, 10, 15);
		MazeViewer.DisplayGrid (maze, cellPrefab);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
