using System.Collections;
using System.Collections.Generic;
using org.mariuszgromada.math.mxparser;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public static string function = "f(x, z) = x + z";
    public static int resolution = 3;
    public static int gridSize = 10;
    public Generator g;
    public InputField input;
    public Text textField;

    public void updateText()
    {
        textField.text = input.text;
        function = input.text;
    }

    public void delete()
    {
        g.Delete();
    }

    public void begin()
    {
        function = input.text;
        g.render();
    }
}
