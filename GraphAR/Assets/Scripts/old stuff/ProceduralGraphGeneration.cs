using System.Collections;
using UnityEngine;
using org.mariuszgromada.math.mxparser;


[RequireComponent (typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralGraphGeneration : MonoBehaviour {
	public string funcString = "f(x, z) = sin(x) + cos(z)";
	Function f;
	Mesh mesh;
	Vector3[] vertices;
	int[] triangles;


	// grid settings
	public float cellSize = 1;
	public Vector3 gridOffset;
	public int gridSize = 10;

	// Use this for initialization
	void Awake () {
		mesh = GetComponent<MeshFilter> ().mesh;
	}

	void Start () {
		f = new Function (funcString);
		MakeContinuousProceduralGrid ();
		UpdateMesh ();
	}

	void UpdateMesh() {
		mesh.Clear ();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.RecalculateNormals ();
	}

	float customFunc3D (float x, float y) {
		Expression e = new Expression ("f(" + x + "," + y + ")", f);
		return (float)e.calculate();
	}

	void MakeContinuousProceduralGrid() {
		//set array sizes
		vertices = new Vector3[(gridSize + 1) * (gridSize + 1)];
		triangles = new int[gridSize * gridSize * 6];

		//set tracker integers
		int v = 0;
		int t = 0;

		float vertexOffset = gridSize * 0.5f;

		// create vertex grid
		for (int x = 0; x <= gridSize; x++) {
			for (int y = 0; y <= gridSize; y++) {
				vertices [v] = new Vector3 ((x * cellSize) - vertexOffset,
					customFunc3D((x * cellSize) - vertexOffset, (y * cellSize - vertexOffset)), (y * cellSize) - vertexOffset);
				v += 1;
			}
		}

		// reset vertex tracker
		v = 0;

		// setting each cell's triangles
		for (int x = 0; x < gridSize; x++) {
			for (int y = 0; y < gridSize; y++) {
				triangles [t] = v;
				triangles [t + 1] = triangles [t + 4] = v + 1;
				triangles [t + 2] = triangles [t + 3] = v + (gridSize + 1);
				triangles [t + 5] = v + (gridSize + 1) + 1;
				v++;
				t += 6;
			}
			v++;
		}
	}
}

