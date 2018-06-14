using System.Collections;
using UnityEngine;
using org.mariuszgromada.math.mxparser;


[RequireComponent (typeof(MeshFilter), typeof(MeshRenderer))]
class Generator : MonoBehaviour {
	private Mesh mesh;
	private Vector3[] vertices;
	private int[] indices;
    private Color[] colors;
    private string funcString;
    private int points;
    private int length;
    private float gridOffset;
    private float inv_resolution;
    private Quaternion rotation;
    private int gridSize;
    private int yRange;

	private void Start()
	{
        gridSize = GUIManager.gridSize;
        yRange = gridSize / 2;
        length = GUIManager.resolution * gridSize;
        gridOffset = length / 2;
        inv_resolution = 1 / (float) GUIManager.resolution;
        Debug.Log(inv_resolution);
        points = (length + 1) * (length + 1);
        Debug.Log("Begin initialization");
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        vertices = new Vector3[points];
        indices = new int[points];
        colors = new Color[points];
        initialize();

	}
    //initial rectangle
    private void initialize() {

        for (int x = 0, v = 0; x <= length; x++)
        {
            for (int z = 0; z <= length; z++)
            {
                float xval = (x - gridOffset) * inv_resolution;
                float zval = (z - gridOffset) * inv_resolution;

                vertices[v] = new Vector3(xval, 0, zval);
                indices[v] = v;
                colors[v] = new Color(0.8f, 0, 0.8f);
                v++;
            }
        }

        mesh.vertices = vertices;
        mesh.colors = colors;
        mesh.SetIndices(indices, MeshTopology.Points, 0);
    }

    public void updateMesh(string function) {
        Function f = new Function(function);
        //Function f = normalizeFunc(function);

        if (is3DFunc(function))
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                float yval = (float)f.calculate(vertices[i].x, vertices[i].z);
                if (checkRange(yval))
                {
                    vertices[i].y = yval;
                } else {
                    vertices[i] = new Vector3(0,0,0); //fix
                    yval = 0;
                }
                colors[i] = new Color(0.8f, normalize(yval), 0.8f);
            }
        } else {
            for (int i = 0; i < vertices.Length; i++) {
                float yval = (float)f.calculate(vertices[i].x);
                if (Mathf.Approximately(vertices[i].z, 0) && checkRange(yval))
                {
                    vertices[i].y = yval;
                } else {
                    vertices[i] = new Vector3(0, 0, 0); // fix
                    yval = 0;
                }
                colors[i] = new Color(0.8f, normalize(yval), 0.8f);
            }
        }

        mesh.vertices = vertices;
        mesh.colors = colors;
        mesh.SetIndices(indices, MeshTopology.Points, 0);
    }
    private bool checkRange(float y) {
        return (y <= yRange && y >= -yRange);
    }
    private float normalize(float y) {
        return (y - (-yRange)) / (gridSize);
    }
	public void Reset()
	{
        initialize();
	}

	public static bool is3DFunc(string func)
    {
        return (func.Contains("x") && func.Contains("z") ||
                func.Contains("x") && func.Contains("y") ||
                func.Contains("y") && func.Contains("z"));
    }

    /* Rotates the graph and sets variables to x and z 
     * based on which variables are inputed. Does not alter funcString. */
    public Function normalizeFunc(string func)
    {
        Transform parent = GameObject.FindWithTag("pointParent").transform;
        //reset rotation (and position)
        parent.SetPositionAndRotation(Vector3.zero,
                                      Quaternion.Euler(0, 0, 0));
        if (!is3DFunc(func))
        {
            if (func.Contains("y"))
            {
                //rotate around Z axis
                rotation = Quaternion.Euler(0, 90, 0);
                func = func.Replace('y', 'x');
            }
            else if (func.Contains("z"))
            {
                //rotate around Y axis
                rotation = Quaternion.Euler(0, 90, 0);
                func = func.Replace('y', 'x');
            }
        }
        else
        {
            if (func.Contains("y") && func.Contains("z"))
            {
                //rotate around Z axis
                rotation = Quaternion.Euler(0, 0, 90);
                func = func.Replace('y', 'x');
            }
            else if (func.Contains("y"))
            {
                //rotate around X axis
                rotation = Quaternion.Euler(90, 0, 0);
                func = func.Replace('y', 'z');
            }
        }
        return new Function(func);
    }
}

