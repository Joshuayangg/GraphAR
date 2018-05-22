using UnityEngine;
using org.mariuszgromada.math.mxparser;

public class APIManager {
	public static string funcString = "f(x, z) = sin(x) + cos(z)";
	public static int resolution = 1;
	public static int gridSize = 10;

	public static Function getFunction() {
		return new Function (funcString);
	}


}
