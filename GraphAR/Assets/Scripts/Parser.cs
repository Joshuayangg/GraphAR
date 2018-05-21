using System.Collections;
using org.mariuszgromada.math.mxparser;
using UnityEngine;

public class Parser {
	static Vector3[] vertices;
	static Function f;
	static float resolution = GUIManager.resolution;
	static int gridSize = GUIManager.gridSize;
	static int points = gridSize * (int) resolution;
	static float gridOffset = points / 2;
	static float inv_resolution = 1 / resolution;


	private static float y_calculator (float x, float z) {
		Expression e = new Expression ("f(" + x + "," + z + ")", f);
		return (float)e.calculate();
	}

	static void generateVertices() {
		f = new Function (GUIManager.function);
		vertices = new Vector3[(points + 1) * (points + 1)];
		for (int x = 0, v = 0; x <= points; x++) {
			for (int z = 0; z <= points; z++) {
                float y = y_calculator((x - gridOffset) * inv_resolution, (z - gridOffset) * inv_resolution);
                if (double.IsNaN(y)) {
                    Debug.Log("The value at (" + x + ", " + z + ")" + " is undefined.");
                } else {
                    vertices[v] = new Vector3((x - gridOffset) * inv_resolution,
                                              y, (z - gridOffset) * inv_resolution);  
                }
				v += 1;
			}
		}
	}

	public static int numPoints() {
		return points;
	}
	public static Vector3[] getVertices() {
		generateVertices ();
		return vertices;
	}
}
