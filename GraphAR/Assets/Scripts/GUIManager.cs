using System.Collections;
using System.Collections.Generic;
using org.mariuszgromada.math.mxparser;
using UnityEngine;
using UnityEngine.UI;

class GUIManager : MonoBehaviour
{
    static string function = "f(x, z) = sin(x) + cos(z)";
    public static int resolution = 10;
    public static int gridSize = 10;
    public static int middle;
    public VuforiaCameraScaler scaler;
    public Generator g;
    public InputField input;
    public Text textField;
    public Slider scale;

	private void Awake()
	{
        scale.maxValue = 0.01f;
        scale.minValue = 0;
        //float middle = (scale.maxValue - scale.minValue) / 2;
        //scaler.cameraScale = middle;
        //scale.value = middle;
	}
	public void updateText()
    {
        textField.text = input.text;
        function = input.text;
    }

    public void delete()
    {
        g.Reset();
    }

    public void begin()
    {
        function = input.text;
        g.updateMesh(function);
    }

    public void updateScale()
    {
        scaler.cameraScale = scale.value; 
    }
}
