using System.Collections;
using UnityEngine;
using org.mariuszgromada.math.mxparser;

class GameObjectGenerator : MonoBehaviour {

    public Transform p; //Point transform
	private Mesh mesh;
    private Transform[] pointTransforms;
    private string funcString;
    private int points; //number of points
    private int length; //length of grid
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
        pointTransforms = new Transform[points];
        initialize();

	}
    //initial rectangle
    private void initialize() {

        for (int x = 0, i = 0; x <= length; x++)
        {
            for (int z = 0; z <= length; z++)
            {
                float xval = (x - gridOffset) * inv_resolution;
                float zval = (z - gridOffset) * inv_resolution;

                Transform point = Instantiate(p);
                point.parent = this.GetComponent<Transform>();
                point.localPosition = new Vector3(xval, 0, zval);
                pointTransforms[i] = point;
                i++;
            }
        }
    }

    public void update(string function) {
        Function f = new Function(function);
        //Function f = normalizeFunc(function);

        if (is3DFunc(function))
        {
            for (int i = 0; i < pointTransforms.Length; i++)
            {
                float xPos = pointTransforms[i].localPosition.x;
                float zPos = pointTransforms[i].localPosition.z;
                float yPos = (float)f.calculate(xPos, zPos);
                if (checkRange(yPos))
                {
                    Debug.Log(xPos + " " + yPos + " " + zPos);
                    pointTransforms[i].localPosition = new Vector3(xPos, yPos, zPos);
                } else {
                    //pointTransforms[i].localScale = new Vector3(0, 0, 0); //will have to figure out how to reset
                }
            }
        } else {
            for (int i = 0; i < pointTransforms.Length; i++) {
                float xPos = pointTransforms[i].localPosition.x;
                float zPos = pointTransforms[i].localPosition.z;
                float yPos = (float)f.calculate(xPos);
                if (Mathf.Approximately(zPos, 0) && checkRange(yPos))
                {
                    pointTransforms[i].localPosition = new Vector3(xPos, yPos, zPos);
                } else {
                    //pointTransforms[i].localScale = new Vector3(0, 0, 0); //will have to figure out how to reset
                }
            }
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