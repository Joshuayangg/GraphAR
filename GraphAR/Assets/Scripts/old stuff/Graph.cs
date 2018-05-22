using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using org.mariuszgromada.math.mxparser; //@Source: http://mathparser.org/

[RequireComponent (typeof(MeshFilter), typeof(MeshRenderer))]
public class Graph : MonoBehaviour {

	public float resolution;
	public Transform axis;
	public string funcString; //pre-defined function choices: Parabola, Bumps, Boobs

	private Mesh mesh;
	private Vector3[] vertices;
	private int[] triangles;
	public float cellSize = 1;
	public Vector3 gridOffset;
	public int gridSize = 10;



	private int twoDorThreeD; //0 = 2d function, 1 = 3d function
	private Function f;
	private bool hasNegativeValues; //used to determine whether there is a need to take out part of y axis

	Function generateFunction(string func) {
		//decide whether 2d or 3d function
		if (func.IndexOf ("z") == -1) {
			twoDorThreeD = 0;
		} else {
			twoDorThreeD = 1;
		}
		return new Function(func);
	}

	void checkPosNeg(float pos) {
		if (pos < 0) {
			hasNegativeValues = true;
		}
	}

	public void delete() {
		mesh.Clear ();
	}

	public void run() {
		mesh = GetComponent<MeshFilter> ().mesh;
		f = generateFunction (funcString);

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

		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.RecalculateNormals ();

	}

	float customFunc2D (float x) {
		Expression e = new Expression ("f(" + x + ")", f);
		return (float)e.calculate();
	}

	float customFunc3D (float x, float y) {
		Expression e = new Expression ("f(" + x + "," + y + ")", f);
		return (float)e.calculate();
	}



}