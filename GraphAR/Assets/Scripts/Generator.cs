using System.Collections;
using UnityEngine;
using org.mariuszgromada.math.mxparser;


[RequireComponent (typeof(MeshFilter), typeof(MeshRenderer))]
class Generator : MonoBehaviour {
	private Mesh mesh;
	private Vector3[] vertices;
	private int[] indices;
    private string funcString;
    private int points;
    private int length;
    private float gridOffset;
    private float inv_resolution;
    private Quaternion rotation;

	private void Start()
	{
        length = GUIManager.resolution * GUIManager.gridSize;
        gridOffset = length / 2;
        inv_resolution = 1 / (float) GUIManager.resolution;
        Debug.Log(inv_resolution);
        points = (length + 1) * (length + 1);
        Debug.Log("Begin initialization");
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        generateVertices();
        mesh.vertices = vertices;
        mesh.SetIndices(indices, MeshTopology.Points, 0);

	}

    private static float y_calculator(float x, float z, Function f)
    {
        Expression e = new Expression("f(" + x + "," + z + ")", f);
        if (!e.checkSyntax())
        {
            Debug.Log("Input is invalid.");
            return 0f;
        }
        return (float)e.calculate();
    }

    private void generateVertices() {
        vertices = new Vector3[points];
        indices = new int[points];

        for (int x = 0, v = 0; x <= length; x++)
        {
            for (int z = 0; z <= length; z++)
            {
                float xval = (x - gridOffset) * inv_resolution;
                float zval = (z - gridOffset) * inv_resolution;

                vertices[v] = new Vector3(xval, 0, zval);
                indices[v] = v;
                v++;
            }
        }
    }

    public void updateMesh(string function) {
        Function f = new Function(function);
        for (int i = 0; i < vertices.Length; i++)
        {
            float yval = y_calculator(vertices[i].x, vertices[i].z, f);
            vertices[i].y = yval;
        }
        mesh.vertices = vertices;
        mesh.SetIndices(indices, MeshTopology.Points, 0);
    }

	public void Reset()
	{
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i].y = 0;
        }
        mesh.vertices = vertices;
        mesh.SetIndices(indices, MeshTopology.Points, 0);
	}

	public static bool is3DFunc(string func)
    {
        return (func.Contains("x") && func.Contains("z") ||
                func.Contains("x") && func.Contains("y") ||
                func.Contains("y") && func.Contains("z"));
    }
}

