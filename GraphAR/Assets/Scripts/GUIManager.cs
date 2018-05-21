using System.Collections;
using System.Collections.Generic;
using org.mariuszgromada.math.mxparser;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {
	public static string function= "f(x, z) = x + z";
	public static int resolution = 1;
	public static int gridSize = 10;
	public Generator g;
	public InputField input;
	public Text textField;

	public void updateText() {
		textField.text = input.text;
		function = input.text;
	}

	public void delete() {
		g.Delete ();
	}

	public void begin() {
		function = input.text;

        //"normalize" variable input by rotating entire graph based on var inputs
        if (function.IndexOf('x') != -1 && function.IndexOf('y') != -1)
        {
            //rotate graph z by 90 deg (done by rotating parent of meshes)
            function.Replace('y', 'z');

        } else if (function.IndexOf('y') != -1 && function.IndexOf('z') != -1) {
            //rotate graph x by 90 deg
            function.Replace('y', 'x');

        }
		g.MeshRender ();
	}
}
