using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using org.mariuszgromada.math.mxparser;

class GameObjectGenerator : MonoBehaviour {

    public Transform p; //Point transform
	private Mesh mesh;
    private List<Graph> graphs;

    //private Transform[] pointTransforms;
    private string funcString;
    private int points; //number of points
    private int length; //length of grid
    private float gridOffset;
    private float inv_resolution;
    private Quaternion rotation;
    private int gridSize;
    private int yRange;
    
    public float scale;


    /* Represents a mathematical graph and contains transforms of all the points
     * of the graph. Making a subclass could be useful in the future when we want
     * to add new features to individual graphs and generate multiple ones 
     * (the latter is supported right now). */
    class Graph {
        Transform[] pointTransforms;

        public Graph(Transform[] points){
            pointTransforms = points;
        }

        public Transform[] getPointTransforms() {
            return pointTransforms;
        }
    }


	private void Start()
	{
        gridSize = GUIManager.gridSize;
        yRange = gridSize;
        length = GUIManager.resolution * gridSize;
        gridOffset = length / 2;
        inv_resolution = 1 / (float) GUIManager.resolution;
        Debug.Log(inv_resolution);
        points = (length + 1) * (length + 1);
        Debug.Log("Begin initialization");
        graphs = new List<Graph>();
        //pointTransforms = new Transform[points];
        //initialize();

	}
    //initial rectangle
    /*private void initialize() {

        for (int x = 0, i = 0; x <= length; x++)
        {
            for (int z = 0; z <= length; z++)
            {
                float xval = (x - gridOffset) * inv_resolution;
                float zval = (z - gridOffset) * inv_resolution;

                Transform point = Instantiate(p);
                point.parent = this.GetComponent<Transform>();
                point.localPosition = new Vector3(xval, 0, zval);
                point.localScale = new Vector3(scale, scale, scale);
                point.gameObject.layer = point.parent.gameObject.layer; // Set layer to scaled content layer
                pointTransforms[i] = point;
                i++;
            }
        }
    }*/

    public void generateGraph(string function) {
        Function f = new Function(function);
        //Function f = normalizeFunc(function);

        Transform[] pointTransforms = new Transform[points];

        for (int x = 0, i = 0; x <= length; x++)
        {
            for (int z = 0; z <= length; z++)
            {
                float xPos = (x - gridOffset) * inv_resolution;
                float zPos = (z - gridOffset) * inv_resolution;

                Transform point = Instantiate(p);
                point.parent = this.GetComponent<Transform>();
                point.localScale = new Vector3(scale, scale, scale);
                point.gameObject.layer = point.parent.gameObject.layer; // Set layer to scaled content layer
                pointTransforms[i] = point;

                if (is3DFunc(function))
                {
                    float yPos = (float)f.calculate(xPos, zPos);
                    if (checkRange(yPos))
                    {
                        //Debug.Log(xPos + " " + yPos + " " + zPos);
                        pointTransforms[i].localPosition = new Vector3(xPos, yPos, zPos);
                    }
                    else
                    {
                        setVisibility(pointTransforms[i], false);
                    }
                }
                else
                {
                    float yPos = (float)f.calculate(xPos);
                    if (Mathf.Approximately(zPos, 0) && checkRange(yPos))
                    {
                        pointTransforms[i].localPosition = new Vector3(xPos, yPos, zPos);
                    }
                    else
                    {
                        setVisibility(pointTransforms[i], false);
                    }
                }

                i++;
            }
        }
        GUIManager.graphGenerated = true; // This is to get the content scaler camera aligned to the AR camera
        graphs.Add(new Graph(pointTransforms));
    }

    /* Toggles visibility of transforms by manipulating their scale */
    private void setVisibility(Transform t, bool visible) {
        if (visible) {
            t.localScale = new Vector3(scale, scale, scale);
        } else {
            t.localScale = new Vector3(0, 0, 0);
        }
    }
    private bool checkRange(float y) {
        return (y <= yRange && y >= -yRange);
    }
    private float normalize(float y) {
        return (y - (-yRange)) / (gridSize);
    }
	public void Reset()
	{
        /*for (int x = 0, i = 0; x <= length; x++)
        {
            for (int z = 0; z <= length; z++)
            {
                float xval = (x - gridOffset) * inv_resolution;
                float zval = (z - gridOffset) * inv_resolution;
                pointTransforms[i].localPosition = new Vector3(xval, 0, zval);
                setVisibility(pointTransforms[i], true);
                i++;
            }
        }*/

        // For now, loop through all the graphs and delete them. Later on, we can enable deleting them one at a time.
        foreach(Graph graph in graphs){
            foreach(Transform point in graph.getPointTransforms()) {
                Destroy(point.gameObject);
            }
        }
	}

	public static bool is3DFunc(string func)
    {
        return (func.Contains("x") && func.Contains("z") ||
                func.Contains("x") && func.Contains("y") ||
                func.Contains("y") && func.Contains("z"));
    }

    /* TODO: Make this work!!!
     * Rotates the graph and sets variables to x and z 
     * based on which variables are inputed. Does not alter funcString. */
    public Function normalizeFunc(string func)
    {
        Transform parent = this.GetComponent<Transform>();;
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