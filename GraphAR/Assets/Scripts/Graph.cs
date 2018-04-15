using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using org.mariuszgromada.math.mxparser; //@Source: http://mathparser.org/

public class Graph : MonoBehaviour {

	public Transform pointPrefab;
	public GameObject graph;
	public float resolution;
	public string funcString; //pre-defined function choices: Parabola, Bumps, Boobs
	public bool customFuncEnabled;

	private int twoDorThreeD; //0 = 2d function, 1 = 3d function
	private Function f;
	private bool hasNegativeValues; //used to determine whether there is a need to take out part of y axis

	void awake() {
		run ();
	}

//	public Graph(string funcString, bool customFuncEnabled) {
//		this.funcString = funcString;
//		this.customFuncEnabled = customFuncEnabled;
//		run ();
//	}


	public void scaleGraph(float scale) {
		graph.transform.localScale = graph.transform.localScale * scale;
	}

	void run () {
		hasNegativeValues = false;
		Vector3 scale = Vector3.one / (resolution * 6f);
		Vector3 position;

		if (customFuncEnabled) {
			f = generateFunction (funcString);
			if (twoDorThreeD == 0) { //only dependent on x
				for (float x = -10f; x <= 10f; x = x + 1f / resolution) {
					Transform point = Instantiate (pointPrefab);
					point.parent = graph.transform;
					position.x = x / 8f;
					position.z = 0;
					position.y = customFunc2D (position.x);
					checkPosNeg (position.y);
					point.localPosition = position;
					point.localScale = scale;
				}
			} else {
				for (float x = -10f; x <= 10f; x = x + 1f / resolution) {
					for (float z = -10f; z <= 10f; z = z + 1f / resolution) {
						Transform point = Instantiate (pointPrefab);
						point.parent = graph.transform;
						position.x = x / 8f;
						position.z = z / 8f;
						position.y = customFunc3D (position.x, position.z);
						checkPosNeg (position.y);
						point.localPosition = position;
						point.localScale = scale;
					}
				}
			}
		} else {

			//for pre-defined functions
			if (funcString == "Parabola") {
				for (float x = -5f; x < 5f; x = x + 1f / resolution) {
					Transform point = Instantiate (pointPrefab);
					point.parent = graph.transform;
					position.x = (x) / 5f;
					position.z = 0;
					position.y = funcParabola (position.x);
					checkPosNeg (position.y);
					point.localPosition = position;
					point.localScale = scale;
				}
			} else if (funcString == "Bumps") {
				for (float x = -5f; x < 5f; x = x + 1f / resolution) {
					for (float z = -5f; z < 5f; z = z + 1f / resolution) {
						Transform point = Instantiate (pointPrefab);
						point.parent = graph.transform;
						position.x = (x + 0.5f) / 5f - 1f;
						position.z = (z + 0.5f) / 5f - 1f;
						position.y = funcBumps (position.x, position.z);
						checkPosNeg (position.y);
						point.localPosition = position;
						point.localScale = scale;
					}
				}
			} else if (funcString == "Boobs") {
				for (float x = 0f; x < 50f; x = x + 1f / resolution) {
					for (float z = -20f; z < 20f; z = z + 1f / resolution) {
						Transform point = Instantiate (pointPrefab);
						point.parent = graph.transform;
						position.x = (x + 0.5f) / 5f - 1f;
						position.z = (z + 0.5f) / 5f - 1f;
						position.y = funcBoobs (position.x, position.z);
						checkPosNeg (position.y);
						point.localPosition = position;
						point.localScale = scale;
					}
				}
			}
		}

		if (!hasNegativeValues) {
			//DrawAxis.negative = false
			Debug.Log ("rip");
			graph.transform.position = new Vector3(transform.position.x, graph.transform.position.y - 10f/8f, transform.position.z);
			//scaleGraph (3f);
		}
	
	}

	Function generateFunction(string func) {
		//decide whether to draw 2d or 3d function
		if (func.IndexOf ("z") == -1) {
			twoDorThreeD = 0;
		} else {
			twoDorThreeD = 1;
		}
		return new Function(func);
	}

	void checkPosNeg(float pos) {
		if (pos < 0) {
			hasNegativeValues = true;
		}
	}
		

	float funcBumps(float x, float z) {
		float y = (Mathf.Sin(5f * x) * Mathf.Cos(5f * z)) / 5f;
		return y;
	}

	float funcParabola(float x) {
		float y = Mathf.Pow(x, 2f);
		return y;
	}

    float funcBoobs(float x, float z)
    {
        float y = Mathf.Pow(25f - (20f * Mathf.Cos(x)) - Mathf.Pow(z, 3) - Mathf.Pow(x, 2), (0.3333333333f));
        Debug.Log(y);
        return y;
    }

	float customFunc2D (float x) {
		Expression e = new Expression ("f(" + x + ")", f);
		return (float)e.calculate();
	}

	float customFunc3D (float x, float y) {
		Expression e = new Expression ("f(" + x + "," + y + ")", f);
		return (float)e.calculate ();
	}



}
