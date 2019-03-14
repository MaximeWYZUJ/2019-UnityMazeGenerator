using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface MazeGenerator
{
	void Generate (IEnumerable<GraphVertex> maze);
	IEnumerator AnimatedGeneration (IEnumerable<GraphVertex> maze, GameObject vertexPrefab, float deltaTime);
}


public enum GeneratorType { DFS, Prim, Wilson }