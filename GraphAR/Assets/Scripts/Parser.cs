//using System.Collections;
//using org.mariuszgromada.math.mxparser;
//using UnityEngine;

//public class Parser
//{
//    static Vector3[] vertices;
//    static int[] indices;
//    static Color[] colors;
//    static Function f;
//    static float resolution = GUIManager.resolution;
//    static int gridSize = GUIManager.gridSize;
//    static int points = gridSize * (int)resolution;
//    static int totalpoints = (points + 1) * (points + 1);
//    static float gridOffset = points / 2;
//    static float inv_resolution = 1 / resolution;
//    public static Quaternion normalizationOffset;

//    static float maxY;
//    static float minY;

//    /* Calculates y for 3D graphs */
//    private static float y_calculator(float x, float z)
//    {
//        Expression e = new Expression("f(" + x + "," + z + ")", f);
//        if (!e.checkSyntax())
//        {
//            Debug.Log("Input is invalid.");
//            return 0f;
//        }
//        return (float)e.calculate();
//    }

//    /* Calculates y for 2D graphs */
//    private static float y_calculator(float x)
//    {
//        Expression e = new Expression("f(" + x + ")", f);
//        if (!e.checkSyntax())
//        {
//            Debug.Log("Input is invalid.");
//            return 0f;
//        }
//        return (float)e.calculate();
//    }

//    public static void generateVertices()
//    {
//        vertices = new Vector3[totalpoints];
//        indices = new int[totalpoints];
//        colors = new Color[totalpoints];

//        for (int x = 0, v = 0; x <= points; x++)
//        {
//            for (int z = 0; z <= points; z++)
//            {
//                float xval = (x - gridOffset) * inv_resolution;
//                float zval = (z - gridOffset) * inv_resolution;

//                vertices[v] = new Vector3(xval, 0, zval);
//                indices[v] = v;
//                v++;
//            }
//        }
//        Debug.Log("Initialization Complete");
//    }

//    static void generateVertices(string function)
//    {
//        //f = normalizeFunc(function);
//        maxY = Mathf.NegativeInfinity;
//        minY = Mathf.Infinity;

//        if (Generator.is3DFunc(function))
//        {
//        vertices = new Vector3[totalpoints];
//        indices = new int[totalpoints];
//        colors = new Color[totalpoints];
//        for (int i = 0; i < vertices.Length; i++)
//        {
//            float yval = y_calculator(vertices[i].x, vertices[i].z);
//            if (yval != vertices[i].y)
//            {
//                vertices[i].y = yval;
//            }
//            i++;
//        }
//    }
//        for (int x = 0, v = 0; x <= points; x++)
//        {
//            for (int z = 0; z <= points; z++)
//            {
//                float yval = y_calculator(vertices[v].x, vertices[v].z);
//                if (double.IsNaN(y))
//                {
//                    Debug.Log("The value at (" + x + ", " + z + ") is undefined");
//                }
//                else
//                {
//                if (yval != vertices[v].y)
//                {
//                    vertices[v].y = yval;
//                }
//                v++;
//                    vertices[v] = new Vector3((x - gridOffset) * inv_resolution, y, (z - gridOffset) * inv_resolution);
//                    indices[v] = v;
//                    updateMaxMinY(y);

//                }
//                v += 1;
//            }
//        }
//        Debug.Log("render complete");
//        }
//        else
//        {
//            vertices = new Vector3[(points + 1)];
//            indices = new int[(points + 1)];
//            for (int x = 0; x < points + 1; x++)
//            {
//                float y = y_calculator((x - gridOffset) * inv_resolution) / 2f;
//                if (double.IsNaN(y))
//                {
//                    Debug.Log("The value at (" + x + ") is undefined");
//                }
//                else
//                {
//                    vertices[x] = new Vector3((x - gridOffset) * inv_resolution, y, 0);
//                    indices[x] = x;
//                    updateMaxMinY(y);

//                }
//            }

//        }
//        setColors();

//    static void updateMaxMinY(float y) {
//        if (y > maxY) {
//            maxY = y;
//        }
//        if (y < minY) {
//            minY = y;
//        }
//    }

//    static void setColors() {
//        for (int x = 0; x < indices.Length; x++) {
//            colors[x] = new Color(0.8f, normalize(vertices[x].y), 0.8f);
//        }
//    }
//    public static float normalize(float num) {
//        return (num - minY) / (maxY - minY);
//    }

//    public static int numPoints()
//    {
//        return points;
//    }
//    public static Vector3[] getVertices()
//    {
//        return vertices;
//    }
//    public static Vector3[] getVertices(string function)
//    {
//        f = new Function(function);
//        generateVertices(function);
//        return vertices;
//    }

//    public static int[] getIndices()
//    {
//        return indices;
//    }
//}

//    public static Color[] getColors()
//    {
//        if (colors == null)
//        {
//            setColors();
//        }
//        return colors;
//    }

//    /* Rotates the graph and sets variables to x and z 
//     * based on which variables are inputed. Does not alter funcString. */
//    public static Function normalizeFunc(string func)
//    {
//        Transform parent = GameObject.FindWithTag("pointParent").transform;
//        //reset rotation (and position)
//        parent.SetPositionAndRotation(Vector3.zero,
//                                      Quaternion.Euler(0, 0, 0));
//        if (!Generator.is3DFunc(func))
//        {
//            if (func.Contains("y"))
//            {
//                //rotate around Z axis
//                normalizationOffset = Quaternion.Euler(0, 90, 0);
//                func = func.Replace('y', 'x');
//            }
//            else if (func.Contains("z"))
//            {
//                //rotate around Y axis
//                normalizationOffset = Quaternion.Euler(0, 90, 0);
//                func = func.Replace('y', 'x');
//            }
//        }
//        else
//        {
//            if (func.Contains("y") && func.Contains("z"))
//            {
//                //rotate around Z axis
//                normalizationOffset = Quaternion.Euler(0, 0, 90);
//                func = func.Replace('y', 'x');
//            }
//            else if (func.Contains("y"))
//            {
//                //rotate around X axis
//                normalizationOffset = Quaternion.Euler(90, 0, 0);
//                func = func.Replace('y', 'z');
//            }
//        }
//        return new Function(func);
//    }
//}
