using System.Collections;
using UnityEngine;
using org.mariuszgromada.math.mxparser;


[RequireComponent (typeof(MeshFilter), typeof(MeshRenderer))]
public class Generator : MonoBehaviour {
	public Mesh mesh;
	private Vector3[] vertices;
	private int[] triangles;

//	void UpdateMesh() {
//		mesh.Clear ();
//		mesh.vertices = vertices;
//		mesh.triangles = triangles;
//		mesh.RecalculateNormals ();
//	}
	public void Delete() {
		mesh.Clear ();
		mesh = null;
		vertices = null;
		triangles = null;
	}

	public void PointRender() {
		vertices = Parser.getVertices ();
		for (int i = 0; i < vertices.Length; i++) {
			//for each point, generate a gameobject? 
		}
	}

	public void MeshRender() {
		//setup
		mesh = GetComponent<MeshFilter> ().mesh;
		mesh.vertices = Parser.getVertices ();
		int points = Parser.numPoints ();
		//set array sizes
		triangles = new int[points * points * 6];
		// reset vertex tracker
		int v = 0;
		int t = 0;

		// setting each cell's triangles
		for (int x = 0; x < points; x++) {
			for (int y = 0; y < points; y++) {
				triangles [t] = v;
				triangles [t + 1] = triangles [t + 4] = v + 1;
				triangles [t + 2] = triangles [t + 3] = v + (points + 1);
				triangles [t + 5] = v + (points + 1) + 1;
				v++;
				t += 6;
			}
			v++;
		}
		mesh.triangles = triangles;
		mesh.RecalculateNormals ();
	}

//	private void OnDrawGizmos () {
//		if (vertices == null) {
//			return;
//		}
//		Gizmos.color = Color.black;
//		for (int i = 0; i < vertices.Length; i++) {
//			Gizmos.DrawSphere (vertices [i], 0.1f);
//		}
//	}
}

