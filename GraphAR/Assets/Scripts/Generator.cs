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
    private Vector3 initialScale;
    private Vector3 initialPos;
    private Quaternion rotation;
//	void UpdateMesh() {
//		mesh.Clear ();
//		mesh.vertices = vertices;
//		mesh.triangles = triangles;
//		mesh.RecalculateNormals ();
//	}
	public void Delete() {
        parent.localScale = initialScale;
        parent.localPosition = initialPos;
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
        parent = GameObject.FindWithTag("pointParent").transform;
        initialScale.Set(1.0f, 1.0f, 1.0f);
        initialPos.Set(0f, 0f, 0f);

        funcString = GUIManager.function;
        PointRender(funcString);
        parent.localScale = initialScale;
        parent.localPosition = initialPos;
        parent.localRotation = Parser.normalizationOffset;
        Parser.normalizationOffset = Quaternion.Euler(0, 0, 0);
	}

	public void PointRender(string function) {
        mesh = new Mesh();
        int numPoints = Parser.numPoints();
        GetComponent<MeshFilter>().mesh = mesh;

        Vector3[] points = Parser.getVertices(function);
        int[] indices = Parser.getIndices(function);
        mesh.vertices = points;
        mesh.colors = Parser.getColors();
        mesh.SetIndices(indices, MeshTopology.Points, 0);
	}

    public static bool is3DFunc(string func)
    {
        return (func.Contains("x") && func.Contains("z") ||
                func.Contains("x") && func.Contains("y") ||
                func.Contains("y") && func.Contains("z"));
    }
}

