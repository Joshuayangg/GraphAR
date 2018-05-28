using System.Collections;
using UnityEngine;
using org.mariuszgromada.math.mxparser;


[RequireComponent (typeof(MeshFilter), typeof(MeshRenderer))]
public class Generator : MonoBehaviour {
	public Mesh mesh;
    public Transform pointPrefab; //used for pointRender
    private bool meshInstantiated = false;
	private Vector3[] vertices;
	private int[] triangles;
    private Transform parent; //used for graph rotation
    private string funcString;
    private Vector3 scale;
//	void UpdateMesh() {
//		mesh.Clear ();
//		mesh.vertices = vertices;
//		mesh.triangles = triangles;
//		mesh.RecalculateNormals ();
//	}
	public void Delete() {
        scale.Set(0.04f, 0.04f, 0.04f);
        parent.localScale = scale;
        if (!is3DFunc(funcString))
        {
            foreach (Transform child in parent)
            {
                Destroy(child.gameObject);
            }
        }
        if (meshInstantiated) {
            mesh.Clear();
            mesh = null;
            vertices = null;
            triangles = null;
            meshInstantiated = false;
        }
	}

	public void render()
	{
        parent = GameObject.Find("mesh").transform;
        funcString = GUIManager.function;
        if (is3DFunc(funcString))
        {
            PointRender();
        }
        else
        {
            PointRender();
        }
	}

	public void PointRender() {
        // vertices = Parser.getVertices ();
        // float size = (float)GUIManager.resolution;
        // Transform parent = GameObject.FindWithTag("pointParent").transform;
        // scale.Set(size/10f, size/10f, size/10f);
        // parent.localScale = scale;
        // for (int i = 0; i < vertices.Length; i++) {
        // 	//for each point, generate a gameobject
        //     Transform point = Instantiate(pointPrefab);
        //     point.SetParent(parent);
        //     point.localPosition = vertices[i];
        //     point.localScale = scale / 9f;
        // }
        mesh = new Mesh();
        int numPoints = Parser.numPoints();
        GetComponent<MeshFilter>().mesh = mesh;

        Vector3[] points = Parser.getVertices();
        int[] indices = Parser.getIndices();
        mesh.vertices = points;
        mesh.SetIndices(indices, MeshTopology.Points, 0);
	}
	public void MeshRender() {
        //setup
        GetComponent<MeshFilter>().transform.parent = parent; //setting parent to mesh??
		mesh = GetComponent<MeshFilter> ().mesh;
		mesh.vertices = Parser.getVertices ();
		int points = Parser.numPoints ();
        meshInstantiated = true;
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

    public static bool is3DFunc(string func)
    {
        return (func.Contains("x") && func.Contains("z") ||
                func.Contains("x") && func.Contains("y") ||
                func.Contains("y") && func.Contains("z"));
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

