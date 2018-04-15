using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour {

	public Transform pointPrefab;
	public float resolution;
	public enum String {
		Parabola,
		Bumps,
        Boobs
	}
	public String function;

	void Awake () {
		Vector3 scale = Vector3.one / (resolution * 8f);
		Vector3 position;
        if ((function.ToString() == "Parabola"))
        {
            Debug.Log("hi");
            for (float x = -5f; x < 5f; x = x + 1f / resolution)
            {
                Transform point = Instantiate(pointPrefab);
                position.x = (x) / 5f;
                position.z = 0;
                position.y = funcParabola(position.x);
                point.localPosition = position;
                point.localScale = scale;
            }
        }
        else if (function.ToString() == "Bumps")
        {
            for (float x = -5f; x < 5f; x = x + 1f / resolution)
            {
                for (float z = 0f; z < 10f; z = z + 1f / resolution)
                {
                    Transform point = Instantiate(pointPrefab);
                    position.x = (x + 0.5f) / 5f - 1f;
                    position.z = (z + 0.5f) / 5f - 1f;
                    position.y = funcBumps(position.x, position.z);
                    point.localPosition = position;
                    point.localScale = scale;
                }
            }
        }
        else if (function.ToString() == "Boobs")
        {
            for (float x = -100f; x < 100f; x = x + 1f / resolution)
            {
                for (float z = 0f; z < 50f; z = z + 1f / resolution)
                {
                    Transform point = Instantiate(pointPrefab);
                    position.x = (x + 0.5f) / 5f - 1f;
                    position.z = (z + 0.5f) / 5f - 1f;
                    position.y = funcBoobs(position.x, position.z);
                    point.localPosition = position;
                    point.localScale = scale;
                }
            }
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



}
