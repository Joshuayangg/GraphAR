using System.Collections;
using org.mariuszgromada.math.mxparser;
using UnityEngine;

public class Parser
{
    static Vector3[] vertices;
    static int[] indices;
    static Function f;
    static string funcString;
    static float resolution = GUIManager.resolution;
    static int gridSize = GUIManager.gridSize;
    static int points = gridSize * (int)resolution;
    static float gridOffset = points / 2;
    static float inv_resolution = 1 / resolution;

    /* Calculates y for 3D graphs */
    private static float y_calculator(float x, float z)
    {
        Expression e = new Expression("f(" + x + "," + z + ")", f);
        if (!e.checkSyntax())
        {
            Debug.Log("Input is invalid.");
            return 0f;
        }
        return (float)e.calculate();
    }

    /* Calculates y for 2D graphs */
    private static float y_calculator(float x)
    {
        Expression e = new Expression("f(" + x + ")", f);
        if (!e.checkSyntax())
        {
            Debug.Log("Input is invalid.");
            return 0f;
        }
        return (float)e.calculate();
    }

    static void generateVertices()
    {
        funcString = GUIManager.function;
        f = normalizeFunc(funcString);
        if (Generator.is3DFunc(funcString))
        {
            vertices = new Vector3[(points + 1) * (points + 1)];
            indices = new int[(points + 1) * (points + 1)];
            for (int x = 0, v = 0; x <= points; x++)
            {
                for (int z = 0; z <= points; z++)
                {
                    float y = y_calculator((x - gridOffset) * inv_resolution, (z - gridOffset) * inv_resolution);
                    if (double.IsNaN(y))
                    {
                        Debug.Log("The value at (" + x + ", " + z + ") is undefined");
                    }
                    else
                    {
                        vertices[v] = new Vector3((x - gridOffset) * inv_resolution, y, (z - gridOffset) * inv_resolution);
                        indices[v] = v;
                        
                    }
                    v += 1;
                }
            }
        }
        else
        {
            vertices = new Vector3[(points + 1)];
            indices = new int[(points + 1)];
            for (int x = 0; x < points + 1; x++)
            {
                float y = y_calculator((x - gridOffset) * inv_resolution) / 2f;
                if (double.IsNaN(y))
                {
                    Debug.Log("The value at (" + x + ") is undefined");
                }
                else
                {
                    vertices[x] = new Vector3((x - gridOffset) * inv_resolution, y, 0);
                    indices[x] = x;
                    
                }
            }

        }
    }

    public static int numPoints()
    {
        return points;
    }
    public static Vector3[] getVertices()
    {
        generateVertices();
        return vertices;
    }

    public static int[] getIndices() {
        if (vertices == null) {
            generateVertices();
        }
        return indices;
    }

    /* Rotates the graph and sets variables to x and z 
     * based on which variables are inputed. Does not alter funcString. */
    public static Function normalizeFunc(string func)
    {
        Transform parent = GameObject.FindWithTag("pointParent").transform;
        //reset rotation (and position)
        parent.SetPositionAndRotation(Vector3.zero,
                                      Quaternion.Euler(0, 0, 0));
        if (!Generator.is3DFunc(func))
        {
            if (func.Contains("y"))
            {
                //rotate around Z axis
                parent.transform.Rotate(Vector3.forward * 90);
                func = func.Replace('y', 'x');
            }
            else if (func.Contains("z"))
            {
                //rotate around Y axis
                parent.transform.Rotate(Vector3.up * 90);
                func = func.Replace('y', 'x');
            }
        }
        else
        {
            if (func.Contains("y") && func.Contains("z"))
            {
                //rotate around Z axis
                parent.Rotate(Vector3.forward * 90);
                func = func.Replace('y', 'x');
            }
            else if (func.Contains("y"))
            {
                //rotate around X axis
                parent.Rotate(Vector3.right * 90);
                func = func.Replace('y', 'z');
            }
        }
        return new Function(func);
    }
}
