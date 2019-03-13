using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

	public GameObject vertexPrefab;
	public float cellSize;
	public int nbLines, nbColumns;
	public bool animated;
	public GeneratorType genType;
	public int nbBorders;

	private IEnumerator currentCoroutine;
	private IEnumerable<GraphVertex> maze;
	private bool sizeChanged;


	void Start () {
		currentCoroutine = null;
		GenerateMaze ();
	}


	public static void ClearMazeObjects() {
		GameObject[] array = GameObject.FindGameObjectsWithTag ("MazeElement");
		foreach (GameObject obj in array) {
			GameObject.Destroy (obj);
		}
	}


	public void GenerateMaze(){
		// Center the camera on the maze
		SetupCamera();

		// Delete the previous maze (if any)
		Manager.ClearMazeObjects();

		// Construction of the initial maze, with walls everywhere
		IEnumerable<GraphVertex> maze = UndirectedGraph.GridCellUndirectedGraph (1, this.nbLines, this.nbColumns, this.nbBorders);

		// Selection of the right generator
		MazeGenerator generator = null;
		switch (this.genType) {
		case GeneratorType.DFS:
			{
				generator = new DFSGenerator ();
				break;
			}
		case GeneratorType.Prim:
			{
				generator = new PrimGenerator ();
				break;
			}
		case GeneratorType.Wilson:
			{
				generator = new WilsonGenerator ();
				break;
			}
		}

		// Generation of the maze
		if (this.animated) {
			currentCoroutine = generator.AnimatedGeneration (maze, vertexPrefab, 0);
			StartCoroutine(currentCoroutine);
		} else {
			generator.Generate (maze);
			MazeViewer.DisplayGrid (maze, vertexPrefab);
		}
	}

	public void CancelAnimation() {
		if (currentCoroutine != null) {
			StopCoroutine (currentCoroutine);
			Manager.ClearMazeObjects ();
		}
	}


	private void SetupCamera() {
		Vector2 mazeCenter = new Vector2(nbColumns*cellSize/2, nbLines*cellSize/2);
		Camera.main.transform.position = new Vector3 (mazeCenter.x - cellSize/2, mazeCenter.y - cellSize/2, Camera.main.transform.position.z);

		float cameraHeight = Camera.main.pixelHeight;
		float cameraWidth = Camera.main.pixelWidth;
		if (nbLines > nbColumns * cameraHeight / cameraWidth) {
			Camera.main.orthographicSize = nbLines * cellSize / 2;
		} else {
			Camera.main.orthographicSize = (cameraHeight/cameraWidth) * nbColumns * cellSize / 2;
		}
	}



}


