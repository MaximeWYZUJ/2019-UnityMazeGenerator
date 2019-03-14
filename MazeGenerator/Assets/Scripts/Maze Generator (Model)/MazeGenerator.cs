using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface MazeGenerator
{
	// Method to generate a maze
	void Generate (IEnumerable<GraphVertex> maze);

	// Method to generate a maze and display every step of it
	IEnumerator AnimatedGeneration (IEnumerable<GraphVertex> maze, GameObject vertexPrefab, float deltaTime);
}


public enum GeneratorType { DFS, Prim, Wilson }