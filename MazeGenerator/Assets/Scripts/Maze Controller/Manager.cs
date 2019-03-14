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
	public bool allowTeleporters;

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
		// Delete the previous maze (if any)
		Manager.ClearMazeObjects();

		// Construction of the initial maze, with walls everywhere
		IEnumerable<GraphVertex> maze = UndirectedGraph.GridCellUndirectedGraph (1, this.nbLines, this.nbColumns, this.nbBorders, this.allowTeleporters);

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

		// Center the camera on the maze
		SetupCamera(maze);
	}


	public void CancelAnimation() {
		if (currentCoroutine != null) {
			StopCoroutine (currentCoroutine);
			Manager.ClearMazeObjects ();
		}
	}


	private void SetupCamera(IEnumerable<GraphVertex> maze) {
		// Find the corner coordinates of the maze
		// Could be calculated with nbLines and nbColumns but depends on the border size and not so easy
		float xmin = 0;
		float xmax = 0;
		float ymin = 0;
		float ymax = 0;
		foreach (GraphVertex gv in maze) {
			Vector2 coo = gv.CoreCoordinates;
			xmin = Mathf.Min (xmin, coo.x);
			xmax = Mathf.Max (xmax, coo.x);
			ymin = Mathf.Min (ymin, coo.y);
			ymax = Mathf.Max (ymax, coo.y);
		}

		// We add cellSize twice in order :
		//     1) not to crop the cells on the border
		//     2) to have a bit of padding on the screen border
		float w = xmax - xmin + 2 * cellSize;
		float h = ymax - ymin + 2 * cellSize;

		Vector2 mazeCenter = new Vector2 ((xmax + xmin) / 2, (ymax + ymin) / 2);
		Camera.main.transform.position = new Vector3 (mazeCenter.x, mazeCenter.y, Camera.main.transform.position.z);

		float cameraHeight = Camera.main.pixelHeight;
		float cameraWidth = Camera.main.pixelWidth;
		if (h > w * cameraHeight / cameraWidth) {
			Camera.main.orthographicSize = h / 2;
		} else {
			Camera.main.orthographicSize = (cameraHeight/cameraWidth) * w / 2;
		}
	}



}


