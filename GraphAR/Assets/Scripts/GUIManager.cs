using System.Collections;
using System.Collections.Generic;
using org.mariuszgromada.math.mxparser;
using UnityEngine;
using UnityEngine.UI;

class GUIManager : MonoBehaviour
{
    static string function = "f(x, z) = sin(x) + cos(z)";
    public static int resolution = 10;
    public static int gridSize = 20;
    public static bool graphGenerated = false;
    public MidAirPlaneManager planeManager; 
    public static int middle;
    public VuforiaCameraScaler scaler;
    public Generator g;
    public GameObjectGenerator og;
    public InputField input;
    public Text textField;
    public Slider scale;

    public Text tapScreenText;

    private bool firstGeneration;

	private void Awake()
	{
        scale.maxValue = 1f;
        scale.minValue = 0;
        firstGeneration = true;
        //float middle = (scale.maxValue - scale.minValue) / 2;
        //scaler.cameraScale = middle;
        //scale.value = middle;
	}
	public void updateText()
    {
        textField.text = input.text;
        function = input.text;

        if (firstGeneration) {
            tapScreenText.enabled = true;
            firstGeneration = false;
        }
    }

    public void disableTapScreenText() {
        if (tapScreenText.enabled == true) {
            tapScreenText.enabled = false;
        }
    }

    public void delete()
    {
        //g.Reset();
        og.reset();
    }

    public void begin()
    {
        function = input.text;
        //g.updateMesh(function);
        //planeManager.ResetScene();
        og.generateGraph(function);
    }

    public void updateScale()
    {
        scaler.cameraScale = scale.value; 
    }

    void Update() {
        if (graphGenerated) {
            // Puts scaler camera behind main camera everytime graph as finished generating
            updateScale(); 
            graphGenerated = false;
        }
    }
}
